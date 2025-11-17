using ReservationSystem.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class Reservation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Customer? Customer { get; set; }
        public Table? Table { get; set; }
        public Order? Order { get; set; }
        public DateTime ReservationTime { get; set; }
        public Status Status { get; set; } = Status.Pending;
    }

}
