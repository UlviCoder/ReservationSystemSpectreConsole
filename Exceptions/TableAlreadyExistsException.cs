using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Exceptions
{
    public class TableAlreadyExistsException : Exception
    {
        public TableAlreadyExistsException()
         : base("Bu masa artıq mövcuddur!!!") { }

        public TableAlreadyExistsException(string message)
            : base(message) { }

        public TableAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
