using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.BLL.Service
{
    public interface IProductService
    {
        Task<ProductResponse> CreateProduct(ProductRequest req);
        Task<PagenatedResposne<ProductUserResposne>> GetAllProductsForUser(
            string lang = "en", int page = 1, int limit = 3, string? search = null,
            int? categoryId = null, decimal? maxPrice = null, decimal? minPrice = null,
            string? sortBy = null, bool asc = true);
        Task<List<ProductResponse>> GetAllProductsForAdmin();
        Task<ProductUserDetails> GetAllProductsDetailsForUser(int id, string lang = "en");
    }
}
