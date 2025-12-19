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


            Services.AddScoped<ICategoryRepository, CategoryRepository>();
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IAuthenicationService, AuthenticationService>();

            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<ISeedData, UserSeedData>();
            Services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}
