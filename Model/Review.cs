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

        public override string ToString()
        {
            return
                $"👤 Müştəri: {Customer.FullName}\n" +
                $"⭐ Qiymət: {Rating}/5\n" +
                $"💬 Şərh: {Comment}\n" +
                $"📅 Tarix: {CreatedAt:dd.MM.yyyy HH:mm}\n";
        }
    }

}
