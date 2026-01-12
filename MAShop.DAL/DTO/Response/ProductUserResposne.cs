using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAShop.DAL.Models;

namespace MAShop.DAL.DTO.Response
{
    public class ProductUserResposne
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
        //public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public string MainImage { get; set; }

    }
}
