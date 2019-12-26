using OnlineShopCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopCore.Models.AccountViewModels
{
    public class ResponseUser
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

        public ResponseUser(AppUser u, string r, string token)
        {
            Name = u.FullName;
            Role = r;
            Token = token;
        }
    }
}
