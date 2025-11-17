using ReservationSystem.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Customer Customer { get; set; }
        public Order Order { get; set; }         

        public decimal Amount { get; set; }       
        public DateTime PaymentDate { get; set; } 

        public PaymentMethod Method { get; set; } 
        public PaymentStatus Status { get; set; } 
    }
}
