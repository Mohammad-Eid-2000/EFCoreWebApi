using EFCoreWebApi.Data;
using EFCoreWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreWebApi.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false; // Product not found
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> Search(string propertyName, string searchText)
        {
            IQueryable<Product> query = _context.Products;

            switch (propertyName)
            {
                case nameof(Product.Name):
                    query = query.Where(x => x.Name == searchText);
                    break;
                case nameof(Product.Price):
                    query = query.Where(x => x.Price == decimal.Parse(searchText));
                    break;
                default:
                    throw new ArgumentException($"Property '{propertyName}' is not supported for searching.");
            }
            var result = await query.ToListAsync();
            return result;
        }


        public Task<Product> SearchSingle(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}