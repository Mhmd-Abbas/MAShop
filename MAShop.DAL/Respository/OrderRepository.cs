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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Order> CreateAsync(Order req)
        {
            await _context.Orders.AddAsync(req);
            await _context.SaveChangesAsync();
            return req;
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> GetBySessionIdAsync(string session_id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.SessionId == session_id);
            
        }
    }
}
