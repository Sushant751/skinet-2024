using Core.Entities;
using Core.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;

        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product= await _context.Products.FindAsync(id);

                return product;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type,string? sort)
        {
            var query = _context.Products.AsQueryable();
            if(!string.IsNullOrWhiteSpace(brand))
            {
                query = query.Where(x => x.Brand == brand);
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(x => x.Type == type);
            }
            if (!string.IsNullOrWhiteSpace(sort))
            {
                query = sort switch
                {
                    "priceAsc" => query.OrderBy(x => x.Price),
                    "priceDesc" => query.OrderByDescending(x => x.Price),
                    _ => query.OrderBy(x => x.Name)
                };
            }
            return await query.ToListAsync();
        }
        public void AddProductAsync(Product product)
        {
            _context.Products.Add(product);
        }
        public void UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
        public void DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
        }
        public bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IReadOnlyList<string>> GetBrandAsync()
        {
            return await _context.Products.Select(x => x.Brand).Distinct().ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetTypeAsync()
        {
            return await _context.Products.Select(x => x.Type).Distinct().ToListAsync();
        }
    }
}
