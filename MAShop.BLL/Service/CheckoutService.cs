using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using MAShop.DAL.Respository;
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
        public CheckoutService( ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
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

            if(req.PaymentMethod == "cash")
            {
                return new CheckoutResposne
                {
                    Success = true,
                    Message = "Order placed successfully. Pay with cash on delivery."
                };
            }
            else if(req.PaymentMethod == "visa")
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions> (),

                    Mode = "payment",
                    SuccessUrl = $"https://localhost:5257/checkout/success",
                    CancelUrl = $"https://localhost:5257/checkout/cancel",
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
                            UnitAmount = (long)item.Product.Price,
                        },
                        Quantity = item.Count,

                    });
                }

                var service = new SessionService();
                var session = service.Create(options);

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
    }
}
