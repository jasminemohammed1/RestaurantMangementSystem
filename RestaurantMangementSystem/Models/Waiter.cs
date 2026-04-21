using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class Waiter : Employee
    {
        public Waiter() => Position = "Waiter";
        public override string GetRole()
        {
            return "Waiter";
        }
    }
}
