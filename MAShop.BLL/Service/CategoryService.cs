using Mapster;
using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using MAShop.DAL.Models;
using MAShop.DAL.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository CategoryRepository)
        {
            _categoryRepository = CategoryRepository;
        }
        public async Task<CategoryResponse> CreateCategory(CategoryRequest Request)
        {
            var category = Request.Adapt<Category>();
            await _categoryRepository.CreateAsync(category);

            return category.Adapt<CategoryResponse>();
        }

        public async Task<List<CategoryResponse>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var response = categories.Adapt<List<CategoryResponse>>();
            return response;

        }

        public async Task<BaseResponse> ToggleStatus(int Id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(Id);

                if (category is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Category not found"
                    };
                }

                category.Status = category.Status == Status.Active ? Status.InActive : Status.Active;
                await _categoryRepository.UpdateAsync(category);
                return new BaseResponse
                {
                    Success = true,
                    Message = "Category Updated Successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Cant Delete Category",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<BaseResponse> DeleteCategoryAsync(int id)
        {

            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);

                if (category is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Category not found"
                    };
                }
                await _categoryRepository.DeleteAsync(category);

                return new BaseResponse
                {
                    Success = true,
                    Message = "Category deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Cant Delete Category",
                    Errors = new List<string> { ex.Message }
                };
            }
        }


        public async Task<BaseResponse> UpdateCategoryAsync(int Id, CategoryRequest request)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(Id);

                if (category is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Category not found"
                    };
                }

                if (category.Translations != null)
                {
                    foreach (var translation in request.Translations)
                    {
                        var existing = category.Translations.FirstOrDefault(t => t.Language == translation.Language);

                        if (existing is not null)
                        {
                            existing.Name = translation.Name;
                        }
                        else
                        {
                            category.Translations.Add(new CategoryTranslation
                            {
                                Name = translation.Name,
                                Language = translation.Language
                            });
                        }
                    }
                }
                await _categoryRepository.UpdateAsync(category);

                return new BaseResponse
                {
                    Success = true,
                    Message = "Category updated successfully"
                };


            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Cant Delete Category",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

    }
}
