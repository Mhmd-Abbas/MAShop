using Mapster;
using MAShop.BLL.Service;
using MAShop.DAL.Data;
using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using MAShop.DAL.Models;
using MAShop.DAL.Respository;
using MAShop.PL.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace MAShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly ICategoryService _categoryService;

        public ICategoryService CategoryService { get; }

        public CategoriesController(IStringLocalizer<SharedResource> localizer, ICategoryService categoryService) 
        {
            this.localizer = localizer;
            _categoryService = categoryService;
        }

        [HttpGet("")]
        public IActionResult Index() 
        {
            var response = _categoryService.GetAllCategories();
            return Ok(new { message = localizer["Success"].Value , response});
        }

        [HttpPost("")]

        public IActionResult Create(CategoryRequest request)
        {
            var response = _categoryService.CreateCategory(request);
            return Ok(new { message = localizer["Success"].Value  }  );
        }

    }
}
