using ReservationSystem.Enum;
using ReservationSystem.Exceptions;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class Manager
    {
        public Guid Id { get;} = Guid.NewGuid();
        public string Username { get; set; }
        public string Fin {  get; private set; }
        public Manager(string userName,string fin)
        {
                Username = userName;
                Fin = fin;
        }


        public void AddTable(Restaurant restaurant, Table table)
        {
            table.Restaurant = restaurant; 

            if (!restaurant.Tables.Any(t => t.Number == table.Number))
            {
                restaurant.Tables.Add(table);
                Console.WriteLine($"✅ Masa {table.Number} əlavə olundu.");
            }
            else
            {
                throw new TableAlreadyExistsException($"Bu masa artıq mövcuddur!!! ({table.Number})");
            }
        }


        public void RemoveTable(Restaurant restaurant, int tableNumber)
        {
            var table = restaurant.Tables.FirstOrDefault(t => t.Number == tableNumber);
            if (table != null)
            {
                restaurant.Tables.Remove(table);
                Console.WriteLine($"✅ Masa {tableNumber} silindi.");
            }
            else
            {
                throw new TableNotFoundException($"Masa tapılmadı: ({tableNumber})");
            }
        }
        public void AddMealSet(Menu menu, Set set)
        {
            if (menu.Sets.Any(s => s.Name.Equals(set.Name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("⚠️ Bu adda set artıq mövcuddur.");
                return;
            }

            menu.Sets.Add(set);
            Console.WriteLine($"✅ '{set.Name}' adlı set əlavə olundu.");
        }

        public void RemoveMealSet(Menu menu, string setName)
        {
            var set = menu.Sets.FirstOrDefault(s =>
                s.Name.Equals(setName, StringComparison.OrdinalIgnoreCase));

            if (set == null)
            {
                Console.WriteLine("❌ Belə bir set tapılmadı.");
                return;
            }

            menu.Sets.Remove(set);
            Console.WriteLine($"🗑️ '{set.Name}' adlı set silindi.");
        }

        public void ShowAllMealSets(Menu menu)
        {
            if (menu.Sets == null || menu.Sets.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]🍽️ Hal-hazırda menyuda heç bir set yoxdur.[/]");
                return;
            }

            AnsiConsole.MarkupLine("\n[bold yellow]📋 Mövcud yemək setləri:[/]\n");

            foreach (var set in menu.Sets)
            {
                var panel = new Panel($@"
                [yellow]{set.Name}[/]
                Qiymət: [green]{set.TotalPrice} AZN[/]
                Yeməklər:
                {string.Join("\n", set.Meals.Select(m => $"  • {m.Name} ({m.Price} AZN × {m.Count})"))}
                ")
                {
                    Border = BoxBorder.Rounded,
                    BorderStyle = new Style(Color.Blue),
                    Header = new PanelHeader(set.Name)
                };

                AnsiConsole.Write(panel);
            }
        }


        public decimal CalculateTotalIncome(List<Payment> payments)
        {
            var total = payments
                .Where(p => p.Status == PaymentStatus.Completed)
                .Sum(p => p.Amount);

            Console.WriteLine($"💰 Ümumi gəlir: {total} AZN");
            return total;
        }
    }

}
