using MAShop.BLL.Service;
using MAShop.DAL.DTO.Request;
using MAShop.PL.Resource;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace MAShop.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly IReviewService _reviewService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ProductsController(IProductService productService, IReviewService reviewService, IStringLocalizer<SharedResource> Localizer)
        {
            _productService = productService;
            _reviewService = reviewService;
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

        [HttpPost("{productId}/reviews")]
        public async Task<IActionResult> AddReview([FromRoute] int productId, [FromBody] CreateReviewRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _reviewService.AddReviewAsync(userId, productId, req);

            if (!response.Success)
            {
                return BadRequest(new { message = response.Message });
            }

            return Ok(new { message = response.Message });
        }
    }
}
