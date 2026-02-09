using MAShop.BLL.Service;
using MAShop.DAL.DTO.Request;
using MAShop.PL.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace MAShop.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cart;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CartController(ICartService cart, IStringLocalizer<SharedResource> Localizer)
        {
            _cart = cart;
            _localizer = Localizer;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest req)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _cart.AddToCartAsync(userId, req);

            return Ok(response);
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _cart.GetUserCartAsync(userId);
            return Ok(response);
        }


        [HttpDelete("")]
        public async Task<IActionResult> ClearCart()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _cart.clearCartAsync(userId);
            return Ok(response);

        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart([FromRoute] int productId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _cart.RemoveFromCartAsync(userId, productId);
            return Ok(response);

        }

        [HttpPatch("{productId}")]
        public async Task<IActionResult> UpdateQuantity([FromRoute] int productId, [FromBody] UpdateQuantityRequest req)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _cart.UpdateQuantityAsync(userId, productId, req.Count);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
