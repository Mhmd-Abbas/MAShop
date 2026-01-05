using MAShop.BLL.Service;
using MAShop.DAL.DTO.Request;
using MAShop.PL.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace MAShop.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles ="SuperAdmin,Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ProductsController(IProductService productService, IStringLocalizer<SharedResource> localizer)
        {
            _productService = productService;
            _localizer = localizer;
        }


        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm]ProductRequest req)
        {
            var response = await _productService.CreateProduct(req);

            return Ok(new { message = _localizer["Success"].Value, response });
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _productService.GetAllProductsForUser();
            return Ok(new { message = _localizer["Success"].Value, response });
        }
    }
}
