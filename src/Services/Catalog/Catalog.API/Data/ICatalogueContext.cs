using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public interface ICatalogueContext
    {
        IMongoCollection<Product> Products { get; }
    }
}