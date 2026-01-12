using MAShop.BLL.Service;
using MAShop.DAL.Respository;
using MAShop.DAL.Utils;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MAShop.PL
{
    public class AppConfig
    {
        public static void config(IServiceCollection Services)
        {

            //Repos
            Services.AddScoped<ICategoryRepository, CategoryRepository>();
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<ICartRepository, CartRepository>();


            //Services
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IAuthenicationService, AuthenticationService>();
            Services.AddScoped<IEmailSender, EmailSender>();
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<IFileService, FileService>();
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddScoped<ICartService, CartService>();
            Services.AddScoped<ICheckoutService, CheckoutService>();


            //Utils
            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<ISeedData, UserSeedData>();
            Services.AddTransient<IEmailSender, EmailSender>();

        }
    }
}
