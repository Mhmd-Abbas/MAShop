using MAShop.DAL.Data;
using MAShop.DAL.Models;
using MAShop.DAL.Respository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OrderItemRepository : IOrderItemRepository
{

    private readonly ApplicationDbContext _context;

    public OrderItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task CreateRangeAsync(List<OrderItem> req)
    {
        await _context.OrderItems.AddRangeAsync(req);
        await _context.SaveChangesAsync();
    }

}
