using Mapster;
using MAShop.DAL.Data;
using MAShop.DAL.DTO.Request;
using MAShop.DAL.DTO.Response;
using MAShop.DAL.Models;
using MAShop.DAL.Respository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IFileService _fileService;
        public ProductService( IProductRepository repo, IFileService fileService)
        {
            _repo = repo;
            _fileService = fileService;
        }


        public async Task<ProductResponse> CreateProduct(ProductRequest req)
        {
            var product = req.Adapt<Product>();

            if(req.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(req.MainImage);
                product.MainImage = imagePath;
            }

            if (req.SubImages!= null)
            {
                product.SubImages = new List<ProductImage>();

                foreach (var img in req.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(img);
                    product.SubImages.Add(new ProductImage
                    {
                        ImageName = imagePath
                    });
                }
            }

            await _repo.AddAsync(product);

            var response =  product.Adapt<ProductResponse>();
            //response.SubImages = product.SubImages.Select(i => i.ImageName).ToList();

            return response;
        }

        public async Task<List<ProductUserResposne>> GetAllProductsForUser
            (string lang = "en", int page = 1, int limit = 3, string? search = null )
        {
            var products = _repo.Query();

            if(search is not null)
            {
                products = products
                    .Where(p => p.Translations.Any(t => t.Language == lang && t.Name.Contains(search) || t.Description.Contains(search)));
            }

            var totalCount = await products.CountAsync();

            products = products.Skip( (page - 1) * limit).Take(limit);

            var Listproducts = products.ToList();

            var response = Listproducts.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<ProductUserResposne>>();
            return response;
        }

        public async Task<List<ProductResponse>> GetAllProductsForAdmin()
        {
            var products = await _repo.GetAllAsync();
            var response = products.Adapt<List<ProductResponse>>();
            return response;
        }

        public async Task<ProductUserDetails> GetAllProductsDetailsForUser(int id, string lang = "en")
        {
            var product = await _repo.FindByIdAsync(id);
            var response = product.BuildAdapter().AddParameters("lang", lang).AdaptToType<ProductUserDetails>();
            return response;
        }
    }
}
