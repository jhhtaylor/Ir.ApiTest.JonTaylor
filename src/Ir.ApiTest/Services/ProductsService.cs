using Ir.IntegrationTest.Contracts;
using Ir.IntegrationTest.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq;

public class ProductsService
{
    private readonly Context _context;

    public ProductsService(Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> CreateProduct(Ir.IntegrationTest.Contracts.Product dtoProduct)
    {
        // Task: Fail if the product already exists
        if (_context.Products.Any(p => p.Id == dtoProduct.Id))
        {
            return new BadRequestObjectResult("Product already exists.");
        }

        // Task: Set Created and LastUpdated properties to Now regardless of what is passed in
        var entityProduct = new Ir.IntegrationTest.Entity.Models.Product
        {
            Id = dtoProduct.Id,
            Name = dtoProduct.Name,
            Size = dtoProduct.Size,
            Colour = dtoProduct.Colour,
            Price = dtoProduct.Price,
            LastUpdated = DateTimeOffset.UtcNow, // Set LastUpdated to current time
            Created = DateTimeOffset.UtcNow, // Set Created to current time
            Hash = GenerateHash(dtoProduct) // Generate a hash from the product data
        };

        _context.Products.Add(entityProduct);
        await _context.SaveChangesAsync();

        return new OkObjectResult(entityProduct);
    }

    public async Task<IEnumerable<Ir.IntegrationTest.Contracts.Product>> GetAllProductsAsync(int page = 1, int pageSize = 10, DateTimeOffset? lastUpdatedAfter = null)
    {
        IQueryable<Ir.IntegrationTest.Entity.Models.Product> query = _context.Products;

        if (lastUpdatedAfter.HasValue)
        {
            query = query.Where(p => p.LastUpdated > lastUpdatedAfter.Value);
        }

        var products = await query.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

        return products.Select(p => new Ir.IntegrationTest.Contracts.Product
        {
            Id = p.Id,
            Name = p.Name,
            Size = p.Size,
            Colour = p.Colour,
            Price = p.Price,
            LastUpdated = p.LastUpdated,
            Created = p.Created,
            Hash = p.Hash
        }).ToList();
    }

    public async Task<Ir.IntegrationTest.Contracts.Product> GetProductAsync(string id)
    {
        // Task: Return the product with the matching Id or a 404 NotFound Result when there is no matching Id
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return null;

        return new Ir.IntegrationTest.Contracts.Product
        {
            Id = product.Id,
            Name = product.Name,
            Size = product.Size,
            Colour = product.Colour,
            Price = product.Price,
            LastUpdated = product.LastUpdated,
            Created = product.Created,
            Hash = product.Hash
        };
    }

    public async Task<IActionResult> UpdateProduct(string id, JsonPatchDocument<Ir.IntegrationTest.Contracts.Product> patchDoc)
    {
        var entityProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (entityProduct == null)
        {
            return new NotFoundObjectResult("Product not found.");
        }

        var dtoProduct = new Ir.IntegrationTest.Contracts.Product
        {
            Id = entityProduct.Id, // Ensure Id is not modified
            Name = entityProduct.Name,
            Size = entityProduct.Size,
            Colour = entityProduct.Colour,
            Price = entityProduct.Price,
            LastUpdated = entityProduct.LastUpdated,
            Created = entityProduct.Created, // Ensure Created is not modified
            Hash = entityProduct.Hash // Ensure Hash is not modified
        };
        patchDoc.ApplyTo(dtoProduct);

        entityProduct.Name = dtoProduct.Name;
        entityProduct.Size = dtoProduct.Size;
        entityProduct.Colour = dtoProduct.Colour;
        entityProduct.Price = dtoProduct.Price;
        // Task: Set LastUpdated to Now if a change is made
        entityProduct.LastUpdated = DateTimeOffset.UtcNow;
        entityProduct.Hash = GenerateHash(dtoProduct);
        await _context.SaveChangesAsync();
        return new OkObjectResult(dtoProduct);
    }

    private string GenerateHash(Ir.IntegrationTest.Contracts.Product product)
    {
        var stringToHash = $"{product.Name}{product.Size}{product.Colour}{product.Price}";
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(stringToHash));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
