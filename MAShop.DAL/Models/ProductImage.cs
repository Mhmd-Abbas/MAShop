using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.DAL.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public int ProdductId { get; set; }
        public Product Product { get; set; }
    }
}
