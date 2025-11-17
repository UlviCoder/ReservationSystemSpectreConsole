using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class Meal
    {
        public Guid Id { get; set; } = Guid.NewGuid();   
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; } = 1;
        public Category Category { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
