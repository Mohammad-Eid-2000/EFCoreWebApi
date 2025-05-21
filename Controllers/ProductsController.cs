using EFCoreWebApi.DTO;
using EFCoreWebApi.Models;
using EFCoreWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;

        public ProductsController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productRepository.GetAllAsync());
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            await _productRepository.AddAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            await _productRepository.UpdateAsync(product);
            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            bool isDeleted = await _productRepository.DeleteAsync(id);

            if (isDeleted)
            {
                return Ok(new { success = true, message = "Product deleted successfully." });
            }
            else
            {
                return NotFound(new { success = false, message = "Product not found or could not be deleted." });
            }
        }
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProduct([FromBody] searchDTO search)
        {
            if (search.SearchText == null || string.IsNullOrWhiteSpace(search.SearchText))
            {
                return BadRequest(new { message = "Search text cannot be empty." });
            }

            var products = await _productRepository.Search(search.PropertyName, search.SearchText);

            if (products == null || !products.Any())
            {
                return NotFound(new { message = "No products found." });
            }

            return Ok(products);
        }


    }
}