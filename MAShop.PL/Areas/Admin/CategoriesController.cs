using MAShop.BLL.Service;
using MAShop.DAL.DTO.Request;
using MAShop.PL.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Create([FromBody] CategoryRequest request)
        {

            var response = await _category.CreateCategory(request);
            return Ok(new { message = _localizer["Success"].Value });
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCategoty([FromRoute]int Id)
        {
            var result = await _category.DeleteCategoryAsync(Id);
            if (!result.Success)
            {
                if(result.Message.Contains("not found"))
                {
                    return NotFound(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            return Ok(result);
        }

        [HttpPatch("{Id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute]int Id, [FromBody] CategoryRequest request)
        {
            var result = await _category.UpdateCategoryAsync(Id, request);

            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                {
                    return NotFound(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }

            return Ok(result);
        }


        [HttpPatch("/toggle-status/{Id}")]
        public async Task<IActionResult> ToggleStatus(int Id)
        {
            var result = await _category.ToggleStatus(Id);

            if (!result.Success)
            {
                if (result.Message.Contains("not found"))
                {
                    return NotFound(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }

            return Ok(result);
        }


        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _category.GetAllCategoriesForAdmin();
            return Ok(new { message = _localizer["Success"].Value, response });
        }
    }
} 
