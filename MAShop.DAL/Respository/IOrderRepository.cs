using MAShop.DAL.Migrations;
using MAShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.DAL.Respository
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order req);
        Task<Order> UpdateAsync(Order order);
        Task<Order> GetBySessionIdAsync(string session_id);

        Task<List<Order>> GetOrdersByStatusAsync(OrderStatusEnum status);

        Task<Order?> GetOrderByIdAsync(int id);
        Task<bool> HasUserDeliverdOrderForProductAsync(string userId, int productId);
    }
}
