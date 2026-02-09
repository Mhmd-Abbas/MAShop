using MAShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.DAL.Respository
{
    public interface ICartRepository
    {
        Task<Cart> addAsync(Cart req);
        Task<List<Cart>> GetUserCartItems(string userId);
        Task<Cart?> GetCartItemAsync(string userId, int ProductId);
        Task<Cart> updateAsync(Cart req);
        Task ClearCartAsync(string userId);

        Task DeleteAsync(Cart cart);
    }
}
