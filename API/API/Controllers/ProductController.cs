using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Dtos;
using API.Errors;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {

        private readonly IProductRepository repo;

        public ProductController(IProductRepository repo)
        {
            this.repo = repo;
        }


        // get : api/products

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await repo.GetAllProductAsync();
            var productsToReturn = products.Select(p => new ProductToReturnDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description
            }).ToList();
            return Ok(productsToReturn);
        }


        [HttpGet("{id}")]

        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await repo.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(new ApiResponse(404));

            var productToReturn = new ProductToReturnDto
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description
            };
            return Ok(productToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductToReturnDto productDto)
        {
            if (productDto == null)
                return BadRequest(new ApiResponse(400, "Product data is required"));

            var product = new Product
            {
                Title = productDto.Title,
                Description = productDto.Description
            };

            var newProduct = await repo.CreateProductAsync(product);

            if (newProduct == null)
                return BadRequest(new ApiResponse(400, "Problem creating product"));

            var productToReturn = new ProductToReturnDto
            {
                Id = newProduct.Id,
                Title = newProduct.Title,
                Description = newProduct.Description
            };
            return Ok(productToReturn);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductToReturnDto productDto)
        {
            try
            {
                if (productDto == null)
                    return BadRequest(new ApiResponse(400, "Product data is required"));

                if (id != productDto.Id)
                    return BadRequest(new ApiResponse(400, "Id mismatch between route and product data"));

                var existingProduct = await repo.GetProductByIdAsync(id);

                if (existingProduct == null)
                    return NotFound(new ApiResponse(404, $"Product with id {id} not found"));

                existingProduct.Title = productDto.Title;
                existingProduct.Description = productDto.Description;

                Product updatedProduct = await repo.UpdateProductAsync(id, existingProduct);
                if (updatedProduct == null)
                    return BadRequest(new ApiResponse(400, "Problem updating product"));

                var productToReturn = new ProductToReturnDto
                {
                    Id = updatedProduct.Id,
                    Title = updatedProduct.Title,
                    Description = updatedProduct.Description
                };
                return Ok(productToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while updating the product: {ex.Message}"));
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productExists = await repo.GetProductByIdAsync(id);
            if (productExists == null)
                return NotFound(new ApiResponse(404, $"Product with id {id} not found"));

            await repo.DeleteProductByIdAsync(id);
            return Ok(new ApiResponse(200, "Product deleted successfully"));
        }








    }
}
