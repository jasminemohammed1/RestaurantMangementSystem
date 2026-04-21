using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class Cashier : Employee
    {
        public Cashier() => Position = "Cashier";
        public override string GetRole()
        {
            return "Cashier";
        }
    }
}
