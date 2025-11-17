using ReservationSystem.Exceptions;
using ReservationSystem.Interface;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationSystem.Model
{
    public class Admin : IAdmin
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Username {  get; set; }
        public string Fin { get; private set; }

        public Admin(string username,string fin)
        {
            Username = username;
            Fin = fin;
        }

        public void AddCategory(Menu menu, Category category)
        {
            if (menu.Categories.Any(c =>
                c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
                throw new CategoryAlreadyExistsException($"Bu kateqoriya artıq mövcuddur!!! ({category.Name})");

            menu.Categories.Add(category);
        }
        public void RemoveCategory(Menu menu, string categoryName)
        {
            var category = menu.Categories
             .FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (category == null)
                throw new CategoryNotFoundException($"Katoqoriya tapılmadı!!! ({categoryName})");

            menu.Categories.Remove(category);
        }
        public void AddMealToCategory(Menu menu, string categoryName, Meal meal)
        {
            var category = menu.Categories
             .FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (category == null)
                throw new CategoryNotFoundException(categoryName);

            if (category.Meals.Any(m =>
                m.Name.Equals(meal.Name, StringComparison.OrdinalIgnoreCase)))
                throw new MealAlreadyExistsException($"Məhsul artıq mövcuddur!!! ({meal.Name})");

            category.Meals.Add(meal);
        }
        public void RemoveMealFromCategory(Menu menu, string categoryName, string mealName)
        {
            var category = menu.Categories
            .FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (category == null)
                throw new CategoryNotFoundException(categoryName);

            var meal = category.Meals
                .FirstOrDefault(m => m.Name.Equals(mealName, StringComparison.OrdinalIgnoreCase));

            if (meal == null)
                throw new MealNotFoundException($"Məhsul tapılmadı!!! ({mealName})");

            category.Meals.Remove(meal);
        }
        public void ShowAllReservations(Restaurant restaurant)
        {
            if (!restaurant.Reservations.Any())
            {
                AnsiConsole.MarkupLine("[red]Heç bir rezervasiya yoxdur![/]");
                return;
            }

            foreach (var r in restaurant.Reservations)
            {
                AnsiConsole.MarkupLine(
                    $"[cyan]ID:[/] {r.Id}\n" +
                    $"[yellow]Müştəri:[/] {r.Customer?.FullName}\n" +
                    $"[yellow]Masa:[/] {r.Table?.Number}\n" +
                    $"[yellow]Vaxt:[/] {r.ReservationTime}\n" +
                    $"[yellow]Status:[/] [bold]{r.Status}[/]\n" +
                    $"------------------------------------------"
                );
            }
        }


    }
}
