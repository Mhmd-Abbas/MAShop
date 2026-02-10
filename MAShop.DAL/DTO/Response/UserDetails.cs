using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.DAL.DTO.Response
{
    public class UserDetails
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public bool EmailConfirmed { get; set; }
        public string IsBlocked { get; set; }
        public List<string> Roles { get; set; }
    }
}
