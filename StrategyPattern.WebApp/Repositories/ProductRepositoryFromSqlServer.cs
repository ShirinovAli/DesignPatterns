using Microsoft.EntityFrameworkCore;
using StrategyPattern.WebApp.DataAccess;
using StrategyPattern.WebApp.Models;

namespace StrategyPattern.WebApp.Repositories
{
    public class ProductRepositoryFromSqlServer : IProductRepository
    {
        private readonly ApplicationContext _context;

        public ProductRepositoryFromSqlServer(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Delete(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllByUserId(string userId)
        {
            return await _context.Products.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Product> GetById(string id)
        {

            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> Add(Product product)
        {
            product.Id = Guid.NewGuid().ToString();
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task Update(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
