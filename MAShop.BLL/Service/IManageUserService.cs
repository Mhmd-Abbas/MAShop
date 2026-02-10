using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.BLL.Service
{
    public interface IManageUserService
    {
        Task<List<UserLIstResposne>> GetUserAsync();
        Task<UserDetails> GetUserDetailsAsync();
        Task<BaseResponse> BlockedUserAsync(string userId);
        Task<BaseResponse> UnBlockedUserAsync(string userId);
        Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest req);
    }
}
