using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.IRepository;


namespace API
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand,string? type,string? sort)
        {
            var products = await _productRepository.GetProductsAsync(brand,type,sort);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async  Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _productRepository.AddProductAsync(product);

            if (await _productRepository.SaveAllAsync())
                if (await _productRepository.SaveAllAsync())
                {
                    return CreatedAtAction("GetProduct", new { id = product.Id }, product);
                }
                 return BadRequest("Failed to save product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id) return BadRequest("Product ID mismatch");

            if (!_productRepository.ProductExists(id))
            {
                return NotFound();
            }
             _productRepository.UpdateProduct(product);
            if (await _productRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Failed to update product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            if (! _productRepository.ProductExists(id))
            {
                return NotFound();
            }
           var Product= await _productRepository.GetProductByIdAsync(id);
            if (Product == null) return NotFound();
            _productRepository.DeleteProductAsync(Product);
            if (await _productRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return NoContent();
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok(await _productRepository.GetBrandAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await _productRepository.GetTypeAsync());
        }
        private bool ProductExists(int id)
        {
            return _productRepository.ProductExists(id);
        }
    }
}