using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using MAShop.DAL.Models;
using MAShop.DAL.Respository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace MAShop.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IOrderItemRepository _orderItemRepo;
        private readonly IProductRepository _productRepo;
        public CheckoutService(
            ICartRepository cartRepo,
            IOrderRepository orderRepo,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IOrderItemRepository orderItemRepo,
            IProductRepository productRepo
            )
        {
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
            _userManager = userManager;
            _emailSender = emailSender;
            _orderItemRepo = orderItemRepo;
            _productRepo = productRepo;
        }

        public async Task<CheckoutResposne> ProccessPaymentAsync(CheckoutRequest req, string userId)
        {
            var cartItems = await _cartRepo.GetUserCartItems(userId);

            if (!cartItems.Any())
            {

                return new CheckoutResposne
                {
                    Success = false,
                    Message = "Cart is empty."
                };
            }


            decimal totalAmount = 0;

            foreach (var item in cartItems)
            {
                if (item.Product.Quantity < item.Count)
                {
                    return new CheckoutResposne
                    {
                        Success = false,
                        Message = "Not enough stock"
                    };
                }

                totalAmount += item.Product.Price;
            }

            Order order = new Order
            {
                UserId = userId,
                PaymentMethod = req.PaymentMethod,
                AmountPaid = totalAmount,
            };

            if (req.PaymentMethod == PaymentTypeEnum.Cash)
            {
                return new CheckoutResposne
                {
                    Success = true,
                    Message = "Order placed successfully. Pay with cash on delivery."
                };
            }
            else if(req.PaymentMethod == PaymentTypeEnum.Visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions> (),

                    Mode = "payment",
                    SuccessUrl = $"http://localhost:5257/api/checkouts/success?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"http://localhost:5257/checkout/cancel",
                    Metadata = new Dictionary<string, string>
                    {
                        { "userId", userId }
                    }
                };

                foreach(var item in cartItems)
                {
                    options.LineItems.Add(new SessionLineItemOptions {

                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Translations.FirstOrDefault(t => t.Language == "en").Name,
                            },
                            UnitAmount = (long)item.Product.Price * 100,
                        },
                        Quantity = item.Count,

                    });
                }

                var service = new SessionService();
                var session = service.Create(options);
                order.SessionId = session.Id;

                await _orderRepo.CreateAsync(order);
                
                return new CheckoutResposne
                {
                    Success = true,
                    Url = session.Url,
                    Message = "Payment initiated successfully."
                };
            }

            else
            {
                return new CheckoutResposne
                {
                    Success = false,
                    Message = "Invalid payment method."
                };
            }

        }


        public async Task<CheckoutResposne> HandleSuccessAsync(string session_id)
        {
            var service = new SessionService();
            var session = await service.GetAsync(session_id);
            var userId = session.Metadata["userId"];


            var order = await _orderRepo.GetBySessionIdAsync(session_id);

            order.PaymentId = session.PaymentIntentId;

            order.OrderStatus = OrederStatusEnum.Approved;

            await _orderRepo.UpdateAsync(order);

            var user = await _userManager.FindByIdAsync(userId);

            var cartItems = await _cartRepo.GetUserCartItems(userId);
            var orderItems = new List<OrderItem>();
            var updatedProduct = new List<(int productId, int quantity)>();

            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.Product.Id,
                    Quantity = item.Count,
                    UnitPrice = item.Product.Price,
                };
                orderItems.Add(orderItem);
                updatedProduct.Add((item.Product.Id, item.Count)); 
                
            }
            await _orderItemRepo.CreateRangeAsync(orderItems);
            await _cartRepo.ClearCartAsync(userId);
            await _productRepo.DecreaseQuantityAsync(updatedProduct);
            await _emailSender.SendEmailAsync(user.Email, "Order Confirmation", $"Your order with ID {order.Id} has been successfully placed.");

            return new CheckoutResposne
            {
                Success = true,
                Message = "Payment successful and order approved."
            };
        }
    }
}
