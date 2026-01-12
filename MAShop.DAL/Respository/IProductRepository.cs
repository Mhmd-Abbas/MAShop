using MAShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.DAL.Respository
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product req);
        Task<List<Product>> GetAllAsync();

        Task<Product> FindByIdAsync(int id);
    }
}
