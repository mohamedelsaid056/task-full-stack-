using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductAsync();

        Task<Product> GetProductByIdAsync(int id);

        Task<Product> CreateProductAsync(Product product);

        Task<Product> UpdateProductAsync(int id, Product product);

        Task<bool> DeleteProductByIdAsync(int id);

    }
}
