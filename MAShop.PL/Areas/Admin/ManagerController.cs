using MAShop.BLL.Service;
using MAShop.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MAShop.PL.Areas.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ManagerController : ControllerBase
    {
        private readonly IManageUserService _ManageUser;
        public ManagerController(IManageUserService manageUser)
        {
            _ManageUser = manageUser;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _ManageUser.GetUserAsync();
            return Ok(users);
        }

        [HttpPatch("block/{id}")]
        public async Task<IActionResult> BlockUser([FromRoute]string id)
            => Ok(await _ManageUser.BlockedUserAsync(id));


        [HttpPatch("unblock/{id}")]
        public async Task<IActionResult> UnblockUser([FromRoute] string id)
            => Ok(await _ManageUser.UnBlockedUserAsync(id));


        [HttpPatch("change-role")]
        public async Task<IActionResult> ChangeUserRole(ChangeUserRoleRequest req)
            => Ok(await _ManageUser.ChangeUserRoleAsync(req));

    }
}
