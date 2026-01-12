using Azure.Core;
using MAShop.DAL.Data;
using MAShop.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.DAL.Respository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> addAsync(Cart req)
        {
            await _context.Carts.AddAsync(req);
            await _context.SaveChangesAsync();
            return req;
        }

        public async Task<List<Cart>> GetUserCartItems(string userId)
        {
            return await _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ThenInclude(p => p.Translations)
                .ToListAsync();
        }


        public async Task<Cart?> GetCartItemAsync(string userId, int ProductId)
        {
            return await _context.Carts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == ProductId);
        } 

        public async Task<Cart> updateAsync(Cart req)
        {
            _context.Carts.Update(req);
            await _context.SaveChangesAsync();
            return req;
        }

        public async Task ClearCartAsync(string userId)
        {
            var items = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();
            _context.Carts.RemoveRange(items);

            await _context.SaveChangesAsync();
        }
    }
}
