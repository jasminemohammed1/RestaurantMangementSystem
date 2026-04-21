using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class BranchMenueItem
    {
        public int BranchId { get; set; }
        public int ItemId { get; set; }
        public decimal? PriceOverride { get; set; }
        public bool IsAvailable { get; set; } = true;

    }
}
