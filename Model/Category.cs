using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();   
        public string Name { get; set; }

        public Category(string categoryName)
        {
            Name = categoryName;
        }
        public List<Meal> Meals { get; set; } = new();
    }
}
