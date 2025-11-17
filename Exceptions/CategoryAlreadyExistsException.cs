using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Exceptions
{
    public class CategoryAlreadyExistsException : Exception
    {
        public CategoryAlreadyExistsException()
           : base("Bu kateqoriya artıq mövcuddur!!!") { }

        public CategoryAlreadyExistsException(string message)
            : base(message) { }

        public CategoryAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
