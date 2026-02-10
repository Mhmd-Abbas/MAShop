using Mapster;
using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using MAShop.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.BLL.Service
{
    public class ManageUserService : IManageUserService
    {
        private readonly UserManager<ApplicationUser> _UserManager;

        public ManageUserService(UserManager<ApplicationUser> userManager) 
        {
            _UserManager = userManager;
        }
        public async Task<List<UserLIstResposne>> GetUserAsync()
        {
            var user = await _UserManager.Users.ToListAsync();

            var result = user.Adapt<List<UserLIstResposne>>();

            for (int i=0; i<user.Count; i++)
            {
                var roles = await _UserManager.GetRolesAsync(user[i]);
                result[i].Roles = roles.ToList();
            }

            return result;
        }
        public async Task<UserDetails> GetUserDetailsAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest req)
        {
            var user = await _UserManager.FindByIdAsync(req.UserId);

            var currentRoles = await _UserManager.GetRolesAsync(user);

            await _UserManager.RemoveFromRolesAsync(user, currentRoles);

            await _UserManager.AddToRoleAsync(user, req.Roles);

            return new BaseResponse
            {
                Success = true,
                Message = "User Role Changed Successfully"
            };
        }

        public async Task<BaseResponse> BlockedUserAsync(string userId)
        {
            var user = await _UserManager.FindByIdAsync(userId);

            await _UserManager.SetLockoutEnabledAsync(user, true);
            await _UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

            await _UserManager.UpdateAsync(user);

            return new BaseResponse
            {
                Success = true,
                Message = "User Blocked Successfully"
            };
        }


        public async Task<BaseResponse> UnBlockedUserAsync(string userId)
        {
            var user = await _UserManager.FindByIdAsync(userId);

            await _UserManager.SetLockoutEnabledAsync(user, false);
            await _UserManager.SetLockoutEndDateAsync(user, null);

            await _UserManager.UpdateAsync(user);

            return new BaseResponse
            {
                Success = true,
                Message = "User UnBlocked Successfully"
            };
        }
    }
}
