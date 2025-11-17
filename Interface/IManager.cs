using ReservationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Interface
{
    public interface IManager
    {
        public void AddTable(Restaurant restaurant, Table table);
        public void RemoveTable(Restaurant restaurant, int tableNumber);
        public decimal CalculateTotalIncome(List<Payment> payments);
    }
}
