using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class RecipeItem
    {
        public int IngredientId { get; set; }
        public int MenueItemId { get; set; }
        public decimal RequiredQuantity { get; set; }
    }
}
