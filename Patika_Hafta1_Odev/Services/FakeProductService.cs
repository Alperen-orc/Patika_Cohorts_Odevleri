using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Patika_Hafta1_Odev.Models;

namespace Patika_Hafta1_Odev.Services
{
    public class FakeProductService : IProductService
    {
        private readonly Context _context;
        public FakeProductService(Context context)
        {
            _context = context;
        }
        //Product Create Metodu
        public async Task<Product> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        //Product Delete Metodu
        public async Task DeleteProduct(int id)
        {
            var product=await _context.Products.FindAsync(id);
            if(product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        //id değişkenine göre Product getirme
        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        //Tüm Product listesinin dönülmesi
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        //Product Güncelleme metodu
        public async Task<Product> UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
