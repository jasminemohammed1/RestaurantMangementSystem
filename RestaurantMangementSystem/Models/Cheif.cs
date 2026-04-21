using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class Cheif : Employee
    {
        public Cheif() => Position = "Cheif";
        public override string GetRole()
        {
            return "Cheif";
        }
    }
}
