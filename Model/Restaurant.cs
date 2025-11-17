using ReservationSystem.Enum;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class Restaurant
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double AverageRating
        {
            get
            {
                if (Reviews == null || Reviews.Count == 0)
                    return 0;

                return Reviews.Average(r => r.Rating);
            }
        }
        public List<Payment> Payments { get; set; } = new();
        public Menu Menu { get; set; }
        public Admin Admin { get; set; }
        public Manager Manager { get; set; }
        public List<Review> Reviews { get; set; } = new();

        public Restaurant()
        {
            Menu = new Menu();
            Admin = new Admin("admin", "12345");
            Manager = new Manager("manager", "12345");
        }
        public void RegisterCustomer(string fullName, string phoneNumber, string password, string confirmPassword)
        {
            if (Customers.Any(c => c.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("⚠️ Bu istifadəçi adı artıq mövcuddur.");
                return;
            }

            var newCustomer = new Customer(fullName, phoneNumber, password, confirmPassword);
            Customers.Add(newCustomer);
            Console.WriteLine($"✅ Qeydiyyat uğurlu! Xoş gəlmisiniz, {fullName}!");
        }

        public (object User, string Role) Login(string name, string password)
        {
            if (Admin != null && Admin.Username == name && Admin.Fin == password)
            {
                return (Admin, "Admin");
            }

            if (Manager != null && Manager.Username == name && Manager.Fin == password)
            {
                return (Manager, "Manager");
            }

            var customer = Customers
                .FirstOrDefault(c => c.FullName.Equals(name, StringComparison.OrdinalIgnoreCase)
                                  && c.Password == password);

            if (customer != null)
            {
                return (customer, "Customer");
            }

            return (null, null);
        }
        public void CreateReservation(Customer customer, Table table, DateTime dateTime, List<OrderItem> orderItems = null)
        {
            bool isTableBusy = Reservations.Any(r =>
                r.Table == table &&
                r.ReservationTime.Date == dateTime.Date &&
                r.Status != ReservationSystem.Enum.Status.Canceled);

            if (isTableBusy)
            {
                AnsiConsole.MarkupLine($"[red]❌ Masa {table.Number} bu tarixdə artıq rezerv olunub![/]");
                return;
            }

            Order order = null;
            Payment payment = null;

            if (orderItems != null && orderItems.Count > 0)
            {
                order = new Order
                {
                    Customer = customer,
                    Items = orderItems,
                    Status = ReservationSystem.Enum.Status.Pending
                };

                payment = new Payment
                {
                    Customer = customer,
                    Amount = order.TotalAmount,
                    Status = PaymentStatus.Completed
                };

                Payments.Add(payment);
            }

            var reservation = new Reservation
            {
                Customer = customer,
                Table = table,
                ReservationTime = dateTime,
                Status = ReservationSystem.Enum.Status.Pending,
                Order = order
            };

            Reservations.Add(reservation);

            var panel = new Panel($@"
                    [green]✅ Rezervasiya yaradıldı![/]
                    📅 Tarix: [yellow]{dateTime:dd.MM.yyyy HH:mm}[/]
                    🪑 Masa: [cyan]{table.Number}[/]
                    👤 Müştəri: [bold]{customer.FullName}[/]
                    💵 Ödəniş: [bold green]{payment?.Amount ?? 0} AZN[/]
                    🔖 Status: [grey]{reservation.Status}[/]")
                                {
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(Color.Green)
            };

            AnsiConsole.Write(panel);
        }

        public void AddReview(Review review)
        {
            Reviews.Add(review);

            var borderColor = review.Rating switch
            {
                >= 4 => Color.Green,
                3 => Color.Yellow,
                _ => Color.Red
            };

            var panel = new Panel($@"
                    ⭐ Qiymət: [bold]{review.Rating}[/] / 5
                    💬 [italic]{review.Comment}[/]
                    👤 Müştəri: [bold]{review.Customer.FullName}[/]")
            {
                Header = new PanelHeader("Yeni rəy əlavə olundu"),
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(borderColor)
            };

            AnsiConsole.Write(panel);
        }

        public void ShowReviews()
        {
            if (Reviews.Count == 0)
            {
                Console.WriteLine("📭 Bu restoran üçün hələ heç bir rəy yoxdur.");
                return;
            }

            Console.WriteLine($"\n💬 {Name} restoranının rəyləri (Ortalama: {AverageRating:F1}⭐)\n");
            foreach (var review in Reviews)
            {
                Console.WriteLine(review);
            }
        }
        public void ShowAllMeals()
        {
            var menu = Menu;

            if ((menu.Categories == null || menu.Categories.Count == 0) &&
                (menu.Sets == null || menu.Sets.Count == 0))
            {
                AnsiConsole.MarkupLine("[red]❌ Menyuda hələ heç bir kateqoriya və ya set yoxdur.[/]");
                return;
            }

            AnsiConsole.MarkupLine("\n🍽️ [bold yellow]MENYU[/]");
            AnsiConsole.Write(new Rule());

            if (menu.Categories != null && menu.Categories.Count > 0)
            {
                foreach (var category in menu.Categories)
                {
                    var panel = new Panel("")
                    {
                        Header = new PanelHeader($"📂 {category.Name}"),
                        Border = BoxBorder.Rounded,
                        BorderStyle = new Style(Color.Yellow)
                    };

                    if (category.Meals == null || category.Meals.Count == 0)
                    {
                        panel = new Panel(" → Bu kateqoriyada hələ yemək yoxdur")
                        {
                            Header = new PanelHeader($"📂 {category.Name}"),
                            Border = BoxBorder.Rounded,
                            BorderStyle = new Style(Color.Grey)
                        };
                    }
                    else
                    {
                        var items = string.Join("\n", category.Meals
                            .Select(m => $"🍴 {m.Name} — {m.Price} AZN"));

                        panel = new Panel(items)
                        {
                            Header = new PanelHeader($"📂 {category.Name}"),
                            Border = BoxBorder.Rounded,
                            BorderStyle = new Style(Color.Yellow)
                        };
                    }

                    AnsiConsole.Write(panel);
                }
            }

            if (menu.Sets != null && menu.Sets.Count > 0)
            {
                AnsiConsole.Write(new Rule("[bold cyan]🍱 YEMƏK SETLƏRİ[/]"));

                foreach (var set in menu.Sets)
                {
                    var mealsText = string.Join("\n",
                        set.Meals.Select(m =>
                            $"• {m.Name} ({m.Count} ədəd × {m.Price} AZN)"));

                    var panel = new Panel("Hello")
                    {
                        Header = new PanelHeader("Test Header"),
                        Border = BoxBorder.Rounded,
                        BorderStyle = new Style(Color.Aqua),
                    };

                    AnsiConsole.Write(panel);

                }
            }
        }


        public List<Table> Tables { get; set; } = new();
        public List<Employee> Employees { get; set; } = new();
        public List<Reservation> Reservations { get; set; } = new();
        public List<Customer> Customers { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
    }
}
