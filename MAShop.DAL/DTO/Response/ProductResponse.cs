using MAShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAShop.DAL.DTO.Response
{
    public class ProductResponse
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }

        public string MainImage { get; set; }

        public string CreatedByUser { get; set; }

        //public List<string> SubImages { get; set; }

        public List<CategoryTranslationResponse> Translations { get; set; }
    }
}
