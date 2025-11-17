using ReservationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Interface
{
    public interface IRestaurant
    {
        public void RegisterCustomer(string fullName, string phoneNumber, string password, string confirmPassword);
        public (object User, string Role) Login(string name, string password);
        public void CreateReservation(Customer customer, Table table, DateTime dateTime, List<OrderItem> orderItems = null);
        public void AddReview(Review review);
        public void ShowReviews();
        public void ShowAllMeals();
    }
}
