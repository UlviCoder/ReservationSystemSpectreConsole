using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class Customer
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FullName { get; set; }          
        public string PhoneNumber { get; set; }      
        public string Password { get; set; }
        public Customer(string fullName, string phoneNumber, string password, string confirmPassword)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
            if(password == confirmPassword)
            {
                Password = password;
            }
        }
        public List<Reservation> Reservations { get; set; } = new();  
        public List<Order> Orders { get; set; } = new();             
        public List<Payment> Payments { get; set; } = new();
    }
}
