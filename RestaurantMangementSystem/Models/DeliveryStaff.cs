using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class DeliveryStaff
    {
        public int StaffId { get; set; }
       public string FullName { get; set; } = "";
        public string VehicleType { get; set; } = "";
        public string LicenseNumber { get; set; } = "";
        public string AssignedArea { get; set; } = "";
        public int BranchId { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
