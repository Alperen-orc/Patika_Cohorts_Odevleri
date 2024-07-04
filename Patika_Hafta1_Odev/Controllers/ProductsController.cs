using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Patika_Hafta1_Odev.Models;

namespace Patika_Hafta1_Odev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly Context _context;
        public ProductsController(Context context)
        {
            _context = context;
        }

        //Tüm ürünleri listeleyen Endpoint
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>>GetProducts()
        {
            return await _context.Products.ToListAsync();
        }
        //Belirli bir ürünü getiren Endpoint
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>>GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                throw new KeyNotFoundException("Product Not Found");
            }
            return product;
        }

        //Ürün ekleme Endpoint
        [HttpPost]
        public async Task<ActionResult<Product>>PostProduct([FromBody]Product product)
        {
            if (!ModelState.IsValid)
            {
                throw new ApplicationException("Invalid model state");
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            //201 Http kodunun işlenmesi
            return CreatedAtAction("GetProduct", new { id = product.Id }, new { message = "Product Created." });
        }
        //Ürün silme Endpoint
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>>DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product Not Found");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

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

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { Message = "Product Updated." });
        }
        //Ürünün "name" değişkenini güncelleyen Endpoint
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProduct(int id, [FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required.");
            }

            var product = await _context.Products.FindAsync(id);
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
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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
            var products = from p in _context.Products
                           select p;

            if (!string.IsNullOrEmpty(name))
            {
                products = products.Where(p => p.Name.Contains(name));
            }

            return await products.ToListAsync();
        }
        //Ürünün "price" değerine göre sıralama yapan Endpoint
        [HttpGet("sorted-by-price")]
        public IActionResult GetProductsSortedByPrice()
        {
            // Price'a göre sıralama yap
            var sortedProducts = _context.Products.OrderBy(p => p.Price).ToList();

            return Ok(sortedProducts);
        }

        //Ürünün geçerliliğini kontrol eden fonksiyon
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
