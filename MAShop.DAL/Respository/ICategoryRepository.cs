using MAShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.DAL.Respository
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category Create(Category category);
    }
}
