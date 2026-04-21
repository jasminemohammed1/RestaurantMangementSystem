using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class BranchManager : Employee
    {
        public BranchManager() => Position = "Branch Manager";
      
        public override string GetRole()
        {
            return "BranchManager";
        }
    }
}
