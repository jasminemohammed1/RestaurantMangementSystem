using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string Comments { get; set; } = "";
        public int Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
