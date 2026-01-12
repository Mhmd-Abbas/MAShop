using MAShop.BLL.Service;
using MAShop.DAL.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MAShop.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        public CheckoutController( ICheckoutService checkoutService )
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Payment([FromBody] CheckoutRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _checkoutService.ProccessPaymentAsync(req, userId);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
    }
}
