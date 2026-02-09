using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.BLL.Service
{
    public interface ICartService
    {
        Task<BaseResponse> AddToCartAsync(string userId, AddToCartRequest req);
        Task<CartSummaryResposne> GetUserCartAsync(string userId, string lang = "en") ;
        Task<BaseResponse> clearCartAsync(string userId);
        Task<BaseResponse> RemoveFromCartAsync(string userId, int productId);
        Task<BaseResponse> UpdateQuantityAsync(string userId, int productId, int count);
    }
}
