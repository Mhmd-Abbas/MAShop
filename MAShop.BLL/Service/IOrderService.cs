using MAShop.DAL.DTO.Response;
using MAShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.BLL.Service
{
    public interface IOrderService
    {
        Task<List<OrderResponse?>> GetOrdersAsync(OrderStatusEnum status);
        Task<BaseResponse> UpdateOrderStatusAsync(int orderId, OrderStatusEnum newStatus);
        Task<Order> GetOrderByIdAsync(int orderId);
    }
}
