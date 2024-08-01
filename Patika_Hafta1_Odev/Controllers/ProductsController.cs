using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patika_Hafta1_Odev.Models;
using Patika_Hafta1_Odev.Services;

namespace Patika_Hafta1_Odev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        //Tüm ürünleri listeleyen Endpoint
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>>GetProducts()
        {
            return Ok(await _productService.GetProducts());
        }
        //Belirli bir ürünü getiren Endpoint
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>>GetProduct(int id)
        {
            var product = await _productService.GetProductById(id);
            if(product == null)
            {
                throw new KeyNotFoundException("Product Not Found");
            }
            return Ok(product);
        }

        //Ürün ekleme Endpoint
        [HttpPost]
        public async Task<ActionResult<Product>>CreateProduct([FromBody]Product product)
        {
            if (!ModelState.IsValid)
            {
                throw new ApplicationException("Invalid model state");
            }
            var createdProduct=await _productService.CreateProduct(product);

            //201 Http kodunun işlenmesi
            return CreatedAtAction("GetProduct", new { id = createdProduct.Id }, new { message = "Product Created" });
        }
        //Ürün silme Endpoint
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>>DeleteProduct(int id)
        {
            await _productService.DeleteProduct(id);

            //204 Http kodunun işlenmesi
            return NoContent();
        }
        //Ürün güncelleme Endpoint
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            await _productService.UpdateProduct(product);
            return Ok(new { Message = "Product Update" });

        }
        //Ürünün "name" değişkenini güncelleyen Endpoint
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProduct(int id, [FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required.");
            }

            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = name;

            if (!TryValidateModel(product))
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                await _productService.UpdateProduct(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (id==null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //200 Http kodunun işlenmesi
            return Ok(new { Message = "Product Name Updated." });
        }

        //Ürünün "name" değişkenine göre listeleme yapan Endpoint
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Product>>> ListProducts([FromQuery] string name)
        {
            var products = await _productService.GetProducts();
            if(string.IsNullOrEmpty(name))
            {
                products=products.Where(p=>p.Name.Contains(name));
            }
            return Ok(products);
        }
        //Ürünün "price" değerine göre sıralama yapan Endpoint
        [HttpGet("sorted-by-price")]
        public IActionResult GetProductsSortedByPrice()
        {
            // Price'a göre sıralama yap
            var sortedProducts = _productService.GetProducts().Result.OrderBy(p => p.Price);

            return Ok(sortedProducts);
        }

    }
}
