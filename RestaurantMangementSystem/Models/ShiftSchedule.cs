using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class ShiftSchedule
    {
        public int ShiftScheduleId { get; set; }
        public int BranchId { get; set; }
        public int EmployeeId { get; set; }
        public string TimeSlot { get; set; } = "";
        public DateTime Date { get; set; }

    }
}
