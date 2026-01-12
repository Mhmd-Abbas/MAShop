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
    public class CartService : ICartService
    {
        private readonly IProductRepository _ProductRepo;
        private readonly ICartRepository _cartRepo;
        public CartService(IProductRepository productRepo, ICartRepository cartRepo)
        {
            _ProductRepo = productRepo;
            _cartRepo = cartRepo;
        }


        public async Task<BaseResponse> AddToCartAsync(string userId, AddToCartRequest req)
        {
            var product = await _ProductRepo.FindByIdAsync(req.ProductId);

            if (product == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Product not found",
                    Errors = new List<string> { "The product you are trying to add to the cart does not exist." }
                };
            }

            if( product.Quantity < req.Count)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Not Enough in Stock",
                };
            }

            var cartItem = await _cartRepo.GetCartItemAsync(userId, req.ProductId);

            if(cartItem is not null)
            {
                cartItem.Count += req.Count;
                await _cartRepo.updateAsync(cartItem);
                return new BaseResponse
                {
                    Success = true,
                    Message = "Product added to cart successfully",
                    Errors = null
                };
            }
            else
            {
                var cart = req.Adapt<Cart>();
                cart.UserId = userId;

                await _cartRepo.addAsync(cart);
            }


            return new BaseResponse
            {
                Success = true,
                Message = "Product added to cart successfully",
                Errors = null
            };

        }

        public async Task<CartSummaryResposne> GetUserCartAsync(string userId, string lang = "en")
        {
            var cartItems = await _cartRepo.GetUserCartItems(userId);

            var resposne = cartItems.Adapt<CartResponse>();

            var items = cartItems.Select(c=> new CartResponse
            {
                ProductId = c.ProductId,
                ProductName = c.Product.Translations.FirstOrDefault(t => t.Language == lang).Name,
                Count = c.Count,
                Price = c.Product.Price,
            }).ToList();

            return new CartSummaryResposne
            {
                Items = items
            };

        }

        public async Task<BaseResponse> clearCartAsync(string userId)
        {
            await _cartRepo.ClearCartAsync(userId);

            return new BaseResponse
            {
                Success = true,
                Message = "Cart Cleared successfully"
            };
        }
    }
}
