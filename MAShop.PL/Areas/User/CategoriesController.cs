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
        public IActionResult Index()
        {
            var response = _category.GetAllCategories();
            return Ok(new { message = _localizer["Success"].Value, response });
        }
    }
}
