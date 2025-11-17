using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Customer Customer { get; set; }         
        public string Comment { get; set; }            
        public int Rating { get; set; }                
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Review(Customer customer, string comment, int rating)
        {
            Customer = customer;
            Comment = comment;
            Rating = Math.Clamp(rating, 1, 5); 
        }
    }
}
