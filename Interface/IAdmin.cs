using ReservationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Interface
{
    public interface IAdmin
    {
        public void AddCategory(Menu menu, Category category);
        public void RemoveCategory(Menu menu, string categoryName);
        public void AddMealToCategory(Menu menu, string categoryName, Meal meal);
        public void RemoveMealFromCategory(Menu menu, string categoryName, string mealName);
        public void ShowAllReservations(Restaurant restaurant);
    }
}
