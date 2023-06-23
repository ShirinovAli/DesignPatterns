using MongoDB.Driver;
using StrategyPattern.WebApp.Models;

namespace StrategyPattern.WebApp.Repositories
{
    public class ProductRepositoryFromMongoDb : IProductRepository
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductRepositoryFromMongoDb(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb"); // connectionString

            var client = new MongoClient(connectionString);  // create mongo client
            var database = client.GetDatabase("StrategyDb"); // get database

            _productCollection = database.GetCollection<Product>("Products");
        }

        public async Task<Product> Add(Product product)
        {
            await _productCollection.InsertOneAsync(product);

            return product;
        }

        public async Task Delete(Product product)
        {
            await _productCollection.FindOneAndDeleteAsync(x => x.Id == product.Id);
        }

        public async Task<List<Product>> GetAllByUserId(string userId)
        {
            return await _productCollection.Find(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Product> GetById(string id)
        {
            return await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(Product product)
        {
            await _productCollection.FindOneAndReplaceAsync(x => x.Id == product.Id, product);
        }
    }
}
