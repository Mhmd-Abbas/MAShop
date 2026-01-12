using Mapster;
using MAShop.DAL.DTO.Response;
using MAShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.BLL.MapsterConfigurations
{
    public class MapsterConfig
    {
        public static void MapsterConfRegister()
        {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.CreatedByUser, source => source.User.UserName);

            TypeAdapterConfig<Category, CategoryUserResponse>.NewConfig()
                .Map(dest => dest.Name,
                source => source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault());

            //TypeAdapterConfig<Product, ProductResponse>.NewConfig()
            //    .Map(dest => dest.CreatedByUser, source => source.User.UserName);

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest => dest.CreatedByUser, source => source.User.UserName)
                .Map(dest => dest.MainImage, source => $"http://localhost:5257/Images/{source.MainImage}");

            TypeAdapterConfig<Product, ProductUserResposne>.NewConfig()
                .Map(dest => dest.MainImage, source => $"http://localhost:5257/Images/{source.MainImage}")
                .Map(dest => dest.Name,
                source => source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Product, ProductUserDetails>.NewConfig()
                .Map(dest => dest.MainImage, source => $"http://localhost:5257/Images/{source.MainImage}")
                .Map(dest => dest.Name,
                source => source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault())
                .Map(dest => dest.Description,
                source => source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Description).FirstOrDefault());


        }
    }
}
