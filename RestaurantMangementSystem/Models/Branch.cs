using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class Branch
    {
        public int BranchId { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string OpeningHours { get; set; } = "";

        public int ManagerId { get; set; }


    }
}
