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
        if (_context.Products.Any(p => p.Id == dtoProduct.Id))
        {
            return new BadRequestObjectResult("Product already exists.");
        }

        var entityProduct = new Ir.IntegrationTest.Entity.Models.Product
        {
            Id = dtoProduct.Id,
            Name = dtoProduct.Name,
            Size = dtoProduct.Size,
            Colour = dtoProduct.Colour,
            Price = dtoProduct.Price,
            LastUpdated = DateTimeOffset.UtcNow,
            Created = DateTimeOffset.UtcNow,
            Hash = dtoProduct.Hash
        };

        _context.Products.Add(entityProduct);
        await _context.SaveChangesAsync();

        return new OkObjectResult(dtoProduct);
    }

    public async Task<IEnumerable<Ir.IntegrationTest.Contracts.Product>> GetAllProductsAsync()
    {
        var products = await _context.Products.ToListAsync();
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

        // Convert entity to DTO to apply the patch
        var dtoProduct = new Ir.IntegrationTest.Contracts.Product
        {
            Id = entityProduct.Id,
            Name = entityProduct.Name,
            Size = entityProduct.Size,
            Colour = entityProduct.Colour,
            Price = entityProduct.Price,
            LastUpdated = entityProduct.LastUpdated,
            Created = entityProduct.Created,
            Hash = entityProduct.Hash
        };

        // Apply the patch to the DTO
        patchDoc.ApplyTo(dtoProduct);

        // Map the DTO back to the entity while ensuring protected fields are not changed
        entityProduct.Name = dtoProduct.Name;
        entityProduct.Size = dtoProduct.Size;
        entityProduct.Colour = dtoProduct.Colour;
        entityProduct.Price = dtoProduct.Price;
        // Ensure Id, Created, and Hash are not modified
        entityProduct.LastUpdated = DateTimeOffset.UtcNow; // Update LastUpdated to Now

        await _context.SaveChangesAsync();
        return new OkObjectResult(dtoProduct);
    }
}
