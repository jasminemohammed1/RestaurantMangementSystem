using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class MenueItem
    {
        public int ItemId { get; set; }
        public decimal BasePrice { get; set; }
        public string Name { get; set; } = "";
        public string Decription { get; set; } = "";

        public string Category { get; set; } = "";
        public List<AddOn> Addons { get; set; } = new ();


    }
}
