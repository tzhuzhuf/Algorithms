using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    public class MenuItem
    {
        public string Name { get; set; }
        public List<string> Variations { get; set; }
        public decimal Price { get; set; }
        public int Calories { get; set; }
        public int EnergyValue { get; set; }
        public List<string> Allergens { get; set; }

        public MenuItem(string name, List<string> variations, decimal price, int calories, int energyValue, List<string> allergens)
        {
            Name = name;
            Variations = variations;
            Price = price;
            Calories = calories;
            EnergyValue = energyValue;
            Allergens = allergens;
        }
    }

    static void Main(string[] args)
    {
        decimal exchangeRate = 100m;

        List<MenuItem> menu = new List<MenuItem>
    {
        new MenuItem("Салат Цезарь", new List<string> { "Классический с курицей", "Вегетарианский с тофу" }, 10.99m * exchangeRate, 350, 15, new List<string> { "Глютен", "Молоко" }),
        new MenuItem("Паста Альфредо", new List<string> { "С курицей", "С грибами" }, 12.99m * exchangeRate, 700, 25, new List<string> { "Глютен", "Молоко" }),
        new MenuItem("Бургер с говядиной", new List<string> { "Классический", "С сыром и беконом" }, 9.99m * exchangeRate, 600, 20, new List<string> { "Глютен", "Молоко" }),
        new MenuItem("Суп Минестроне", new List<string> { "Овощной", "С курицей" }, 7.99m * exchangeRate, 200, 10, new List<string> { }),
        new MenuItem("Ризотто с грибами", new List<string> { "Классическое", "С трюфелем" }, 14.99m * exchangeRate, 500, 30, new List<string> { "Молоко" }),
        new MenuItem("Тайский карри", new List<string> { "С курицей", "Вегетарианский" }, 11.99m * exchangeRate, 600, 20, new List<string> { "Орехи" }),
        new MenuItem("Пицца Маргарита", new List<string> { "Классическая", "С добавлением пепперони" }, 8.99m * exchangeRate, 400, 15, new List<string> { "Глютен", "Молоко" }),
        new MenuItem("Десерт Тирамису", new List<string> { "Классический", "Безглютеновый" }, 5.99m * exchangeRate, 300, 12, new List<string> { "Глютен", "Молоко", "Яйца" }),
        new MenuItem("Сэндвич с тунцом", new List<string> { "Классический", "С авокадо" }, 6.99m * exchangeRate, 400, 18, new List<string> { "Глютен", "Рыба" }),
        new MenuItem("Фрукты в шоколаде", new List<string> { "С клубникой", "С бананом" }, 4.99m * exchangeRate, 250, 8, new List<string> { "Шоколад" })
    };

        Console.WriteLine("Меню:");
        foreach (var item in menu)
        {
            Console.WriteLine($"{item.Name} - {item.Price:C} ({item.Calories} ккал, Энергетическая ценность: {item.EnergyValue}) | Аллергены: {string.Join(", ", item.Allergens)}");
            foreach (var variation in item.Variations)
            {
                Console.WriteLine($"  - {variation}");
            }
        }

        Console.WriteLine("\nФильтрация меню:");

        Console.WriteLine("Введите максимальную цену:");
        decimal maxPrice = Convert.ToDecimal(Console.ReadLine());

        Console.WriteLine("Введите максимальное количество калорий:");
        int maxCalories = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Введите максимальную энергетическую ценность:");
        int maxEnergyValue = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Введите аллерген, который нужно исключить (или оставьте пустым для пропуска):");
        string excludedAllergen = Console.ReadLine();

        var filteredMenu = menu.Where(m => m.Price <= maxPrice &&
                                           m.Calories <= maxCalories &&
                                           m.EnergyValue <= maxEnergyValue &&
                                           (string.IsNullOrEmpty(excludedAllergen) || !m.Allergens.Contains(excludedAllergen))).ToList();

        Console.WriteLine("\nОтфильтрованное меню:");
        if (filteredMenu.Count > 0)
        {
            foreach (var item in filteredMenu)
            {
                Console.WriteLine($"{item.Name} - {item.Price:C} ({item.Calories} ккал, Энергетическая ценность: {item.EnergyValue}) | Аллергены: {string.Join(", ", item.Allergens)}");
                foreach (var variation in item.Variations)
                {
                    Console.WriteLine($"  - {variation}");
                }
            }
        }
        else
        {
            Console.WriteLine("Нет доступных позиций, соответствующих заданным критериям.");
        }
    }
}