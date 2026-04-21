using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class AddOn
    {
        public int AddOnId { get; set; }
        public string Name { get; set; } = "";
        public decimal ExtraPrice { get; set; }
    }
}
