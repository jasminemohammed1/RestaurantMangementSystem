using RestaurantMangementSystem.Data;
using RestaurantMangementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Services
{
    public static class InventoryService
    {
        private static  Dictionary<int,double> AggregateRequired(List<OrderItem> items)
        {
            var required = new Dictionary<int,double>();
            foreach(var item in items)
            {
                foreach(var recipe in DataBase.RecipeItems.Where(r => r.MenuItemId == item.ItemId))
                {
                    if(!required.ContainsKey(recipe.IngredientId))
                    {
                        required[recipe.IngredientId] = 0;
                    }
                    required[recipe.IngredientId] += recipe.QuantityRequired * item.Quantity;
                }
            }
            return required;
        }
        public static Dictionary<int,double> GetShortFalls(int BranchId , List<OrderItem> items) 
        {
            var required = AggregateRequired(items);
            Dictionary<int, double> ShortFalls = new Dictionary<int, double>();

            foreach( var (ingredientId,  qnt) in required)
            {
                var inv = DataBase.BranchInventories.FirstOrDefault(i => i.BranchId == BranchId && i.IngredientId == ingredientId);
               double available = inv?.CurrentQuantity ?? 0;
                if(available< qnt)
                ShortFalls[ingredientId] = qnt - available;

            }
            return ShortFalls;
        }

        public static void Dedcut(int BranchId , List<OrderItem> items)
        {
            var required = AggregateRequired(items);
            foreach(var (ingredientId,qnt) in required)
            {
                var inv = DataBase.BranchInventories.FirstOrDefault(x => x.BranchId == BranchId && x.IngredientId == ingredientId);
                if(inv != null)
                {
                    inv.CurrentQuantity = Math.Max(0 , inv.CurrentQuantity - qnt);

                }
            }
        }
        public static bool IsSufficient(int BranchId, List<OrderItem> items) => GetShortFalls(BranchId, items).Count == 0;
            
    }

}
