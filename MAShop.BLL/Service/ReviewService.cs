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
    public class ReviewService : IReviewService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IReviewRepository _reviewRepo;

        public ReviewService(IOrderRepository orderRepo, IReviewRepository reviewRepo)
        {
            _orderRepo = orderRepo;
            _reviewRepo = reviewRepo;
        }

        public async Task<BaseResponse> AddReviewAsync(string userId, int productId, CreateReviewRequest req)
        {
            var hasPurchased = await _orderRepo.HasUserDeliverdOrderForProductAsync(userId, productId);
            if (!hasPurchased)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "You can only review products you have purchased.",
                };
            }

            var alreadyReviewed = await _reviewRepo.HasUserReviewedProductAsync(userId, productId);
            if (alreadyReviewed)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "You have already reviewed this product.",
                };
            }

            var review = req.Adapt<Review>();
            review.UserId = userId;
            review.ProductId = productId;
            await _reviewRepo.CreateAsyc(review);

            return new BaseResponse
            {
                Success = true,
                Message = "Review added successfully.",
            };
    } }
}
