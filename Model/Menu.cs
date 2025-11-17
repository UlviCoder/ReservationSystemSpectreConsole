using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class Menu
    {
        public List<Category> Categories { get; set; } = new();
        public List<Set> Sets { get; set; } = new();
    }
}
