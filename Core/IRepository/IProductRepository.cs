using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository
{
    public interface IProductRepository
    {
        // Define method signatures for the repository
        Task<Product> GetProductByIdAsync(int id);
        Task<IReadOnlyList<Product>> GetProductsAsync(string? brand,string? type,string? sort);
        Task<IReadOnlyList<string>> GetBrandAsync();
        Task<IReadOnlyList<string>> GetTypeAsync();
        void AddProductAsync(Product product);
        void UpdateProduct(Product product);
        void DeleteProductAsync(Product product);
        bool ProductExists(int id);
        Task<bool> SaveAllAsync();
    }
}
