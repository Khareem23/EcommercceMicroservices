using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogueContext _context;

        public ProductRepository(ICatalogueContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public  async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.Find(p => true).ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name,name); 
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Category,categoryName); 
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
             await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product updateProduct)
        {
            var updatedResult = await _context.Products
                .ReplaceOneAsync(filter: g => g.Id == updateProduct.Id, replacement: updateProduct);
            return updatedResult.IsAcknowledged && updatedResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        { 
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Id,id); 
            
            var updatedResult = await _context.Products
                .DeleteOneAsync(filter);
            return updatedResult.IsAcknowledged && updatedResult.DeletedCount   > 0;
        }
    }
}