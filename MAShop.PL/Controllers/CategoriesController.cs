using Mapster;
using MAShop.DAL.Data;
using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using MAShop.DAL.Models;
using MAShop.DAL.Respository;
using MAShop.PL.Resource;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace MAShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(IStringLocalizer<SharedResource> localizer, ICategoryRepository categoryRepository) 
        {
            this.localizer = localizer;
            _categoryRepository = categoryRepository;
        }

        [HttpGet("")]
        public IActionResult Index() 
        {
            var categories = _categoryRepository.GetAll();
            var response = categories.Adapt<List<CategoryResponse>>();
            return Ok(new { message = localizer["Success"].Value , response});
        }

        [HttpPost("")]

        public IActionResult Create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            _categoryRepository.Create(category);
            return Ok(new { message = localizer["Success"].Value  }  );
        }

    }
}
