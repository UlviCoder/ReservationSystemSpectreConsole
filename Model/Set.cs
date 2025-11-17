using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class Set
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }              
        public List<Meal> Meals { get; set; } = new();    
        public decimal TotalPrice => Meals.Sum(m => m.Price * m.Count);
          

        
    }
}
