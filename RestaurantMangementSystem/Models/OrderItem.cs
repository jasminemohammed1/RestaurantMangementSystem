using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
   public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public string ?SpecialNotes { get; set; }
        public int Amount { get; set; }
        public decimal UnitPrice { get; set; }
        public List<int> SelectedAddonIDs { get; set; } = new ();

    }
}
