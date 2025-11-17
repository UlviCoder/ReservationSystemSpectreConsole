using ReservationSystem.Interface;
using ReservationSystem.Model;
using Spectre.Console;
using System.Reflection;
using System.Text;
using Table = Spectre.Console.Table;
public class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var restaurant = new Restaurant();

        ShowWelcomeScreen();

        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]🍽️ Xoş gəldiniz! Davam etmək üçün seçim edin:[/]")
                    .AddChoices("🧾 Qeydiyyatdan keç (Register)", "🔑 Daxil ol (Login)", "🚪 Çıxış"));

            switch (choice)
            {
                case "🧾 Qeydiyyatdan keç (Register)":
                    RegisterFlow(restaurant);
                    break;

                case "🔑 Daxil ol (Login)":
                    LoginFlow(restaurant);
                    break;

                case "🚪 Çıxış":
                    AnsiConsole.MarkupLine("[red]Programdan çıxılır...[/]");
                    return;
            }
        }
    }

    static void ShowWelcomeScreen()
    {
        AnsiConsole.Clear();
        var rule = new Rule("[bold cyan]🍴 RESTORAN İDARƏ SİSTEMİ[/]");
        rule.RuleStyle("grey");
        AnsiConsole.Write(rule);

        AnsiConsole.MarkupLine("\n[yellow]Hazırlayan: Ülvi[/]");
        AnsiConsole.MarkupLine("[grey]Versiya 1.0.0 | Spectre.Console Interface[/]\n");

        AnsiConsole.Status()
            .Start("[green]Yüklənir...[/]", ctx =>
            {
                Thread.Sleep(800);
            });

        AnsiConsole.Clear();
    }

    static void RegisterFlow(Restaurant restaurant)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold green]🧾 Qeydiyyat Mərhələsi[/]\n");

        var name = AnsiConsole.Ask<string>("👤 [yellow]Ad Soyad:[/]");
        var phone = AnsiConsole.Ask<string>("📞 [yellow]Telefon nömrəsi:[/]");
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("🔒 [yellow]Şifrə:[/]").PromptStyle("red").Secret());
        var confirm = AnsiConsole.Prompt(
            new TextPrompt<string>("🔁 [yellow]Təkrar şifrə:[/]").PromptStyle("red").Secret());

        restaurant.RegisterCustomer(name, phone, password, confirm);

        AnsiConsole.MarkupLine("\n[grey]Əsas menyuya qayıtmaq üçün hər hansı düyməyə bas...[/]");
        Console.ReadKey();
        AnsiConsole.Clear();
    }

    static void LoginFlow(Restaurant restaurant)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold yellow]🔑 Daxil ol[/]\n");

        var name = AnsiConsole.Ask<string>("👤 [yellow]Ad Soyad və ya İstifadəçi adı:[/]");
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("🔒 [yellow]Şifrə:[/]").PromptStyle("red").Secret());

        var (user, role) = restaurant.Login(name, password);

        if (user == null)
        {
            AnsiConsole.MarkupLine("[red]❌ İstifadəçi adı və ya şifrə yalnışdır.[/]");
            Console.ReadKey();
            AnsiConsole.Clear();
            return;
        }

        switch (role)
        {
            case "Admin":
                AnsiConsole.MarkupLine($"[bold red]👑 Admin xoş gəldiniz, [underline]{((Admin)user).Username}[/]![/]");
                AdminMenu((Admin)user, restaurant);
                break;

            case "Manager":
                AnsiConsole.MarkupLine($"[bold yellow]🧑‍💼 Manager xoş gəldiniz, [underline]{((Manager)user).Username}[/]![/]");
                ManagerMenu((Manager)user, restaurant);
                break;

            case "Customer":
                var customer = (Customer)user;
                AnsiConsole.MarkupLine($"[bold green]👋 Xoş gəldiniz, {customer.FullName}![/]");
                CustomerFlow(restaurant, customer);
                break;
        }

        AnsiConsole.MarkupLine("\n[grey]Əsas menyuya qayıtmaq üçün hər hansı düyməyə bas...[/]");
        Console.ReadKey();
        AnsiConsole.Clear();
    }
    static void AdminMenu(Admin admin, Restaurant restaurant)
    {
        while (true)
        {
            Console.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold red]👑 Admin Paneli[/]")
                    .AddChoices(
                        "📂 Kateqoriya əlavə et",
                        "🍔 Yemək əlavə et",
                        "🗑️ Kateqoriya sil",
                        "🍽️ Bütün menyuya bax",
                        "⭐ Rəylərə bax",
                        "📅 Bütün rezervasiyalara bax",
                        "↩️ Əsas menyuya qayıt"
                    )
            );

            try
            {
                switch (choice)
                {
                    case "📂 Kateqoriya əlavə et":
                        var categoryName = AnsiConsole.Ask<string>("[yellow]Yeni kateqoriyanın adını daxil edin:[/]");
                        admin.AddCategory(restaurant.Menu, new Category(categoryName));
                        AnsiConsole.MarkupLine($"[green]✅ {categoryName} kateqoriyası əlavə edildi![/]");
                        break;

                    case "🍔 Yemək əlavə et":
                        if (restaurant.Menu.Categories.Count == 0)
                        {
                            AnsiConsole.MarkupLine("[red]⚠️ Əvvəlcə kateqoriya əlavə edin![/]");
                            break;
                        }

                        var categoryChoice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[yellow]Kateqoriya seçin:[/]")
                                .AddChoices(restaurant.Menu.Categories.Select(c => c.Name))
                        );

                        var mealName = AnsiConsole.Ask<string>("[yellow]Yeməyin adını daxil edin:[/]");
                        var mealPrice = AnsiConsole.Ask<decimal>("[yellow]Yeməyin qiymətini daxil edin:[/]");

                        admin.AddMealToCategory(restaurant.Menu, categoryChoice, new Meal { Name = mealName, Price = mealPrice });

                        AnsiConsole.MarkupLine($"[green]✅ {mealName} adlı yemək {categoryChoice} kateqoriyasına əlavə edildi![/]");
                        break;

                    case "🗑️ Kateqoriya sil":
                        if (restaurant.Menu.Categories.Count == 0)
                        {
                            AnsiConsole.MarkupLine("[red]⚠️ Silinəcək kateqoriya yoxdur![/]");
                            break;
                        }

                        var deleteChoice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[yellow]Silinəcək kateqoriyanı seçin:[/]")
                                .AddChoices(restaurant.Menu.Categories.Select(c => c.Name))
                        );

                        admin.RemoveCategory(restaurant.Menu, deleteChoice);
                        AnsiConsole.MarkupLine($"[red]🗑️ {deleteChoice} kateqoriyası silindi.[/]");
                        break;

                    case "🍽️ Bütün menyuya bax":
                        restaurant.ShowAllMeals();
                        break;

                    case "⭐ Rəylərə bax":
                        restaurant.ShowReviews();
                        break;

                    case "📅 Bütün rezervasiyalara bax":
                        admin.ShowAllReservations(restaurant);
                        break;

                    case "↩️ Əsas menyuya qayıt":
                        return;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Xəta: {ex.Message}[/]");
            }

            AnsiConsole.MarkupLine("\n[grey]Davam etmək üçün hər hansı düyməyə bas...[/]");
            Console.ReadKey();
        }
    }


    static void CustomerFlow(Restaurant restaurant, Customer customer)
    {
        while (true)
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"👋 [green]{customer.FullName}[/], seçim edin:")
                    .AddChoices(
                        "📋 Menyunu göstər",
                        "🪑 Rezervasiya et",
                        "💬 Rəy əlavə et",
                        "🚪 Çıxış"
                    ));

            switch (choice)
            {
                case "📋 Menyunu göstər":
                    restaurant.ShowAllMeals();
                    Console.ReadKey();
                    break;

                case "🪑 Rezervasiya et":
                    CreateReservationFlow(restaurant, customer); 
                    break;

                case "💬 Rəy əlavə et":
                    CreateReviewFlow(restaurant, customer);
                    break;

                case "🚪 Çıxış":
                    return;
            }
        }
    }
    public static void CreateReservationFlow(Restaurant restaurant, Customer customer)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[yellow]📅 Rezervasiya yarat[/]");

        if (restaurant.Tables == null || restaurant.Tables.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]❌ Hazırda heç bir masa mövcud deyil![/]");
            AnsiConsole.MarkupLine("[grey]Geri dönmək üçün bir düymə bas...[/]");
            Console.ReadKey();
            return;
        }

        var table = AnsiConsole.Prompt(
            new SelectionPrompt<ReservationSystem.Model.Table>()
                .Title("[cyan]Masa seçin:[/]")
                .AddChoices(restaurant.Tables)
                .UseConverter(t => $"Masa {t.Number} (Capacity: {t.Capacity})")
        );

        var dateTime = AnsiConsole.Ask<DateTime>("[green]Tarix (dd.MM.yyyy HH:mm):[/]");

        var orderItems = SelectProductsFromMenu(restaurant.Menu);

        restaurant.CreateReservation(customer, table, dateTime, orderItems);
    }

    public static List<OrderItem> SelectProductsFromMenu(Menu menu)
    {
        var orderItems = new List<OrderItem>();

        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Məhsul növü seçin:[/]")
                    .AddChoices("🍽 Yeməklər", "🍱 Setlər")
            );

            if (choice == "🍽 Yeməklər")
            {
                var category = AnsiConsole.Prompt(
                    new SelectionPrompt<Category>()
                        .Title("[green]Kateqoriya seçin:[/]")
                        .AddChoices(menu.Categories)
                        .UseConverter(c => c.Name)
                );

                var meal = AnsiConsole.Prompt(
                    new SelectionPrompt<Meal>()
                        .Title($"[yellow]{category.Name}[/] kateqoriyasından yemək seç:")
                        .AddChoices(category.Meals)
                        .UseConverter(m => $"{m.Name} — {m.Price} AZN")
                );

                int quantity = AnsiConsole.Ask<int>("[blue]Neçə ədəd olsun?[/]");

                orderItems.Add(new OrderItem
                {
                    Meal = meal,
                    Quantity = quantity
                });
            }
            else
            {
                if (menu.Sets == null || menu.Sets.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]❌ Hazırda set mövcud deyil![/]");
                    continue;
                }
                var set = AnsiConsole.Prompt(
                    new SelectionPrompt<Set>()
                        .Title("[green]Set seçin:[/]")
                        .AddChoices(menu.Sets)
                        .UseConverter(s => $"{s.Name} — {s.TotalPrice} AZN")
                );

                int quantity = AnsiConsole.Ask<int>("[blue]Neçə ədəd olsun?[/]");

                orderItems.Add(new OrderItem
                {
                    Set = set,
                    Quantity = quantity
                });
            }

            bool more = AnsiConsole.Confirm("[grey]Başqa məhsul əlavə edək?[/]");
            if (!more) break;
        }

        return orderItems;
    }

    private static void CreateReviewFlow(Restaurant restaurant, Customer customer)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold yellow]💬 Yeni rəy əlavə et[/]\n");

        int rating = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
                .Title("[green]Rəy üçün bal seç (1-5):[/]")
                .AddChoices(1, 2, 3, 4, 5)
        );

        string comment = AnsiConsole.Ask<string>("Şərhinizi yazın:");

        Review review = new Review(customer, comment, rating);

        restaurant.AddReview(review);

        AnsiConsole.MarkupLine("\n[green]✔ Rəy uğurla əlavə olundu![/]");
        AnsiConsole.MarkupLine("[grey]Devam etmək üçün Enter basın...[/]");
        Console.ReadLine();
    }



    static void ManagerMenu(Manager manager, Restaurant restaurant)
    {
        while (true)
        {
            Console.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]🧑‍💼 Manager Paneli[/]")
                    .AddChoices(
                        "➕ Masa əlavə et",
                        "➖ Masa sil",
                        "🍱 Yemək seti əlavə et",
                        "🗑️ Yemək seti sil",
                        "📋 Bütün setlərə bax",
                        "💰 Ümumi gəliri göstər",
                        "↩️ Əsas menyuya qayıt"
                    )
            );

            try
            {
                switch (choice)
                {
                    case "➕ Masa əlavə et":
                        var tableNumber = AnsiConsole.Ask<int>("[yellow]Yeni masa nömrəsini daxil edin:[/]");
                        manager.AddTable(restaurant, new ReservationSystem.Model.Table
                        {
                            Number = tableNumber,
                            Capacity = AnsiConsole.Ask<int>("💺 [yellow]Masada neçə nəfərlik yer olacaq?[/]"),
                            Restaurant = restaurant
                        });
                        AnsiConsole.MarkupLine($"[green]✅ Masa {tableNumber} əlavə edildi.[/]");
                        break;

                    case "➖ Masa sil":
                        if (restaurant.Tables.Count == 0)
                        {
                            AnsiConsole.MarkupLine("[red]⚠️ Silinəcək masa yoxdur![/]");
                            break;
                        }

                        var deleteNumber = AnsiConsole.Prompt(
                            new SelectionPrompt<int>()
                                .Title("[yellow]Silinəcək masanı seçin:[/]")
                                .AddChoices(restaurant.Tables.Select(t => t.Number))
                        );

                        manager.RemoveTable(restaurant, deleteNumber);
                        AnsiConsole.MarkupLine($"[red]🗑️ Masa {deleteNumber} silindi.[/]");
                        break;

                    case "🍱 Yemək seti əlavə et":
                        var setName = AnsiConsole.Ask<string>("[yellow]Setin adını daxil edin:[/]");
                        var set = new Set { Name = setName, Meals = new List<Meal>() };

                        while (true)
                        {
                            var mealName = AnsiConsole.Ask<string>("[yellow]Yemək adı (dayandırmaq üçün 'bitir' yaz):[/]");
                            if (mealName.Equals("bitir", StringComparison.OrdinalIgnoreCase)) break;

                            var price = AnsiConsole.Ask<decimal>("[yellow]Qiymət (AZN):[/]");
                            var count = AnsiConsole.Ask<int>("[yellow]Say (ədəd):[/]");

                            set.Meals.Add(new Meal { Name = mealName, Price = price, Count = count });
                        }

                        restaurant.Menu.Sets.Add(set); 
                        break;

                    case "🗑️ Yemək seti sil":
                        if (!restaurant.Menu.Sets.Any())
                        {
                            AnsiConsole.MarkupLine("[red]⚠️ Silinəcək set yoxdur![/]");
                            break;
                        }

                        var setChoice = AnsiConsole.Prompt(
                            new SelectionPrompt<Set>()
                                .Title("[yellow]Silinəcək seti seçin:[/]")
                                .AddChoices(restaurant.Menu.Sets)
                                .UseConverter(s => $"{s.Name} — {s.TotalPrice} AZN")
                        );

                        restaurant.Menu.Sets.Remove(setChoice);

                        AnsiConsole.MarkupLine($"[green]🗑️ '{setChoice.Name}' seti silindi![/]");
                        break;

                    case "📋 Bütün setlərə bax":
                        manager.ShowAllMealSets(restaurant.Menu);
                        break;

                    case "💰 Ümumi gəliri göstər":
                        manager.CalculateTotalIncome(restaurant.Payments);
                        break;

                    case "↩️ Əsas menyuya qayıt":
                        return;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]❌ Xəta: {ex.Message}[/]");
            }

            AnsiConsole.MarkupLine("\n[grey]Davam etmək üçün hər hansı düyməyə bas...[/]");
            Console.ReadKey();
        }
    }


}
