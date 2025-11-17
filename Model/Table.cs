using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class Table
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Number { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; } = true;
        public Restaurant Restaurant { get; set; }
        public List<Reservation> Reservations { get; set; } = new();
    }
}
