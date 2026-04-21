using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public abstract  class  Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = "";
        public decimal Salary { get; set; }
        public DateTime DateOfHire { get; set; }
        public string ContactInformation { get; set; } = "";
        public string Position { get; set; } = "";

        public int PrimaryBranchId { get; set; }
        public List<int> AssignedBranchIds { get; set; } = new ();
        public abstract  string GetRole();

    }
}
