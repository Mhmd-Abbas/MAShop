using MAShop.BLL.Service;
using MAShop.DAL.DTO.Request;
using MAShop.PL.Resource;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace MAShop.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _category;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService category, IStringLocalizer<SharedResource> Localizer)
        {
            _category = category;
            _localizer = Localizer;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string lang = "en")
        {
            var response = _category.GetAllCategoriesForUser(lang);
            return Ok(new { message = _localizer["Success"].Value, response });
        }
    }
}
