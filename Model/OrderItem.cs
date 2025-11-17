using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class OrderItem
    {
        public Guid Id { get; }
        public Meal Meal { get; set; }
        public Set Set { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice =>
            Meal != null
                ? Meal.Price * Quantity
                : Set.TotalPrice * Quantity;
    }
}
