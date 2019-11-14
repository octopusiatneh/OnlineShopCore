using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopCore.Models.ProductViewModels
{
    public class VoteViewModel
    {
        public int Id { get; set; }

        public int VoteForId { get; set; }

        public float Vote { get; set; }
    }
}
