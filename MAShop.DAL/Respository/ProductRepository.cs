using MAShop.DAL.Data;
using MAShop.DAL.DTO.Response;
using MAShop.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.DAL.Respository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Product> AddAsync(Product req)
        {
            await _context.Products.AddAsync(req);
            await _context.SaveChangesAsync();
            return req;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.Include(c => c.Translations).Include(c => c.User).ToListAsync();
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            return await _context.Products
                .Include(c => c.Translations)
                .Include(c => c.SubImages)
                .Include(c => c.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public IQueryable<Product> Query()
        {
            return _context.Products.Include(p=> p.Translations).AsQueryable();
        }

        public async Task<bool> DecreaseQuantityAsync(List<( int productId, int quantity )> items)
        {
            var producId = items.Select(i => i.productId).ToList();

            var products = await _context.Products.Where(p => producId.Contains(p.Id)).ToListAsync();

            foreach (var product in products)
            {
                var item = items.First(i => i.productId == product.Id);
                if (product.Quantity < item.quantity)
                {
                    return false;
                }

                product.Quantity -= item.quantity;
            }


            await _context.SaveChangesAsync();
            return true;
        }
    }
}
