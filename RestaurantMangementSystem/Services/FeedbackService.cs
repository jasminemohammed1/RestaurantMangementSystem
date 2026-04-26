using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.Services
{
    public static class FeedbackService
    {
        public static  (bool success , string message) Submit(int CustomerId , int OrderId , int Rate , string comment)
        {

            var order = Data.DataBase.Orders.FirstOrDefault(x => x.OrderId == OrderId);
            if (order is null)
                return (false, "Order is not found");
            if (order.OrderStatus != Enums.OrderStatus.Completed)
                return (false, "You can only submit feedback for completed orders");
            if (order.CustomerId != CustomerId)
                return (false, "You can only submit Feedbacks for you Orders");
            bool IsSubmittedBefore = Data.DataBase.Feedbacks.Any(x => x.CustomerId == CustomerId && x.OrderId ==OrderId);
            if (IsSubmittedBefore)
                return (false, "You have already submitted feedback for this order.");
            if (Rate < 1 || Rate > 5)
                return (false, "Rating must be between 1 & 5");
            Data.DataBase.Feedbacks.Add(new Models.Feedback()
            {
                FeedbackId = Data.DataBase.NextFeedbackId++,
                CustomerId = CustomerId,
                OrderId = OrderId,
                Rate = Rate,
                Comments = comment,
                Date = DateTime.Now
            });
            return (true, "FeedBack Submitted Sucessfully");
        }
    }
}
