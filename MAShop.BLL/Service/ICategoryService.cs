using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using MAShop.DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.BLL.Service
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllCategories();

        Task<CategoryResponse> CreateCategory(CategoryRequest Request);
        Task<BaseResponse> DeleteCategoryAsync(int id);

        Task<BaseResponse> UpdateCategoryAsync(int Id, CategoryRequest request);

        Task<BaseResponse> ToggleStatus(int Id);
    }
}
