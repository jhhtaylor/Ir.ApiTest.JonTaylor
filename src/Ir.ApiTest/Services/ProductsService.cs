using Ir.IntegrationTest.Contracts;
using Ir.IntegrationTest.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
}
