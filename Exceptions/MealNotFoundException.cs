using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Exceptions
{
    public class MealNotFoundException : Exception
    {
        public MealNotFoundException()
           : base("Məhsul tapılmadı!!!") { }

        public MealNotFoundException(string message)
            : base(message) { }

        public MealNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
