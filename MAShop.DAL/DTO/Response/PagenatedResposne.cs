using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.DAL.DTO.Response
{
    public class PagenatedResposne<T>
    {
        public int TotalCount { get; set; } 
        public int Page { get; set; }
        public int limit { get; set; }
        public List<T> Data { get; set; }
    }   
}
