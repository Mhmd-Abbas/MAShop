using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.DAL.Models
{
    public enum OrederStatusEnum {
    
        Pending = 1,
        Cancelled = 2,
        Approved = 3,
        Shipped  =4,
        Delivered = 5
    }

    public enum PaymentTypeEnum
    {
        Cash = 1,
        Visa = 2
    }

    public class Order
    {
        public int Id { get; set; }
        public OrederStatusEnum OrderStatus { get; set; } = OrederStatusEnum.Pending;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;  
        public DateTime? ShippedDate { get; set; }
        public PaymentTypeEnum PaymentMethod { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentId { get; set; }
        public decimal? AmountPaid { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
