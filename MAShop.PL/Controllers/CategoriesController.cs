using Mapster;
using MAShop.DAL.Data;
using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using MAShop.DAL.Models;
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
        private readonly ApplicationDbContext context;
        private readonly IStringLocalizer<SharedResource> localizer;

        public CategoriesController(ApplicationDbContext context, IStringLocalizer<SharedResource> localizer) 
        {
            this.context = context;
            this.localizer = localizer;
        }

        [HttpGet("")]
        public IActionResult Index() 
        {
            var categories = context.Categories.Include(c=>c.Translations).ToList() ;
            var response = categories.Adapt<List<CategoryResponse>>();
            return Ok(new { message = localizer["Success"].Value , response});
        }

        [HttpPost("")]

        public IActionResult Create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            context.Add(category);
            context.SaveChanges(); 
            return Ok(new { message = localizer["Success"].Value  }  );
        }

    }
}
