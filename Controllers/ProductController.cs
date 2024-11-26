using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bobs_Racing.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProductController : ControllerBase
    {
        private static List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Bike", Price = 10.1m, Description = "To ride" },
            new Product { Id = 2, Name = "cake", Price = 2.2m, Description = "To eat" }
        };

        //get: /product
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(products);
        }

        //get: /product/{id}
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
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
            newProduct.Id = products.Max(p => p.Id) + 1;
            products.Add(newProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }
    }
}