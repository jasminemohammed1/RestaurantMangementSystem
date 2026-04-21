using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; } = "";
        public string Unit { get; set; } = "";
    }
}
