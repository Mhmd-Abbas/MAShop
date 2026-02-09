using MAShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAShop.DAL.DTO.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }


        public PaymentStatusEnum PaymentStatus { get; set; }
        public decimal? AmountPaid { get; set; }
        public string? userName { get; set; }

        //public List<OrderItemResponse> OrderItems { get; set; }
    }
}
