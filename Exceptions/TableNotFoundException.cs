using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Exceptions
{
    public class TableNotFoundException : Exception
    {
        public TableNotFoundException()
          : base("Masa tapılmadı!!!") { }

        public TableNotFoundException(string message)
            : base(message) { }

        public TableNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
