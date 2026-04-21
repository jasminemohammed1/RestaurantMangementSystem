using RestaurantMangementSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; } = DeliveryStatus.AwaitingAssignment;
        public DateTime? DeliveryTime {  get; set; }
        public int? DeliveryStaffId { get; set; }
        public string? FailureReason { get; set; }
        public string DeliveryAddress { get; set; } = "";

    }
}
