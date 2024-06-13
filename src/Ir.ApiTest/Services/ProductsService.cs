using Ir.IntegrationTest.Contracts;
using System;
using System.Collections.Generic;

public class ProductsService
{
    private List<Product> _products;

    public ProductsService()
    {
        // Initialize with more detailed dummy data
        _products = new List<Product>
        {
            new Product 
            {
                Id = "1",
                Name = "Classic Tee",
                Size = "M",
                Colour = "Red",
                Price = 15.99,
                LastUpdated = DateTimeOffset.UtcNow,
                Created = DateTimeOffset.UtcNow.AddDays(-10),
                Hash = Guid.NewGuid().ToString()
            },
            new Product 
            {
                Id = "2",
                Name = "Vintage Jeans",
                Size = "L",
                Colour = "Blue",
                Price = 45.50,
                LastUpdated = DateTimeOffset.UtcNow,
                Created = DateTimeOffset.UtcNow.AddDays(-20),
                Hash = Guid.NewGuid().ToString()
            }
        };
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _products;
    }
}
