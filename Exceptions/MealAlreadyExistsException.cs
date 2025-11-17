using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Exceptions
{
    public class MealAlreadyExistsException : Exception
    {
        public MealAlreadyExistsException()
           : base("Məhsul artıq mövcuddur!!!") { }

        public MealAlreadyExistsException(string message)
            : base(message) { }

        public MealAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
