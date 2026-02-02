using MAShop.BLL.Service;
using MAShop.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace MAShop.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        public CheckoutsController( ICheckoutService checkoutService )
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

        [HttpGet("success")]
        [AllowAnonymous]
        public async Task<IActionResult> Success([FromQuery] string session_id)
        {
            var response = await _checkoutService.HandleSuccessAsync(session_id);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
