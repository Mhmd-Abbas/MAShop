using MAShop.BLL.Service;
using MAShop.DAL.DTO.Request;
using MAShop.DAL.Models;
using MAShop.PL.Resource;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace MAShop.PL.Areas.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public OrdersController(IOrderService orderService, IStringLocalizer<SharedResource> Localizer)
        {
            _orderService = orderService;
            _localizer = Localizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] OrderStatusEnum status = OrderStatusEnum.Pending)
        {
            var orders = await _orderService.GetOrdersAsync(status);
            return Ok(orders);
        }


        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateStatus(int orderId, [FromBody] UpdateOrderStatusRequest req)
        {
            var response = await _orderService.UpdateOrderStatusAsync(orderId, req.Status);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);



        }
    }
}
