using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bobs_Racing.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        //get: /product
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _repository.GetAll();
            return Ok(products);
        }

        //get: /product/{id}
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
            {
                return NotFound("id not found");
            }
            return Ok(product);
        }

        //post: /product
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product newProduct)
        {
            _repository.Add(newProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            var existingProduct = _repository.GetById(id);
            if (existingProduct == null)
            {
                return NotFound($"Product with Id = {id} not found.");
            }

            updatedProduct.Id = id; // Ensure the ID matches
            _repository.Update(updatedProduct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var existingProduct = _repository.GetById(id);
            if (existingProduct == null)
            {
                return NotFound($"Product with Id = {id} not found.");
            }

            _repository.Delete(id);
            return NoContent();
        }
    }
}