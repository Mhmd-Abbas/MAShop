using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.BLL.Service
{
    public interface IReviewService
    {
        Task<BaseResponse> AddReviewAsync(string userId, int productId, CreateReviewRequest req);
    }
}
