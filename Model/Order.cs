using ReservationSystem.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class Order
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public Customer? Customer { get; set; }
        public Table? Table { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
        public Status Status { get; set; } = Status.Pending;
    }


}
