using MAShop.BLL.Service;
using MAShop.DAL.DTO.Request;
using MAShop.PL.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace MAShop.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _category;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService category, IStringLocalizer<SharedResource> Localizer)
        {
            _category = category;
            _localizer = Localizer;
        }

        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var createdBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Console.WriteLine("User id is :");
            Console.WriteLine(createdBy);

            var response = _category.CreateCategory(request);
            return Ok(new { message = _localizer["Success"].Value });
        }
    }
} 
