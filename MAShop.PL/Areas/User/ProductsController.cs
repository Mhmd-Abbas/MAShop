using MAShop.BLL.Service;
using MAShop.PL.Resource;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace MAShop.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ProductsController(IProductService productService, IStringLocalizer<SharedResource> Localizer)
        {
            _productService = productService;
            _localizer = Localizer;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index
            ([FromQuery] string lang = "en", [FromQuery] int page = 1, [FromQuery] int limit = 3,
            [FromQuery] string? search = null,
            [FromQuery] int? categorId = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool asc = true)
        {
            var response = await _productService.GetAllProductsForUser(
                lang, page, limit, search, categorId, maxPrice, minPrice,
                sortBy, asc
                );

            return Ok(new { message = _localizer["Success"].Value, response });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index([FromRoute] int id, string lang = "en")
        {
            var response = await _productService.GetAllProductsDetailsForUser(id, lang);
            return Ok(new { message = _localizer["Success"].Value, response });
        }
    }
}
