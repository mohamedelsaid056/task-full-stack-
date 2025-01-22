using Core.Interfaces;
using Core.Models;
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
        // inject the database context

        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        //create a new product
        public async Task<Product> CreateProductAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }


        //delete a product by id
        public async Task<bool> DeleteProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false; // Indicate that the product was not found
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true; // Indicate successful deletion
        }


        //get all products
        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
            return await _context.Products.ToListAsync();
        }

        //get a product by id
        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }
            return product;
        }

        //update a product
        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            if (id != product.Id)
            {
                throw new ArgumentException("Product ID mismatch.");
            }
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }


    }

}

