using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    public class City
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public List<string> FamousFoods { get; set; }
        public List<string> Restaurants { get; set; }

        public City(string name, string country, List<string> famousFoods, List<string> restaurants)
        {
            Name = name;
            Country = country;
            FamousFoods = famousFoods;
            Restaurants = restaurants;
        }
    }

    static void Main(string[] args)
    {
        List<City> cities = new List<City>
        {
            new City("Москва", "Россия", new List<string> { "Борщ", "Пельмени", "Селёдка под шубой" }, new List<string> { "Dr. Живаго", "Белый Кролик", "Чайка" }),
            new City("Санкт-Петербург", "Россия", new List<string> { "Блины", "Пирожки", "Кулебяка" }, new List<string> { "Палкин", "Мансарда", "КоКоКо" }),
            new City("Казань", "Россия", new List<string> { "Чак-чак", "Эчпочмак" }, new List<string> { "Дом татарской кулинарии", "Тюбетей" }),
            new City("Екатеринбург", "Россия", new List<string> { "Пельмени", "Щи" }, new List<string> { "Шалимар", "Свердловская" }),
            new City("Сочи", "Россия", new List<string> { "Шашлык", "Хачапури" }, new List<string> { "Гагринский дворик", "Кавказский аул" }),
            new City("Владивосток", "Россия", new List<string> { "Краб", "Раки" }, new List<string> { "Ресторан Супра", "Зума" }),

            new City("Минск", "Беларусь", new List<string> { "Драники", "Клёцки", "Сбитень" }, new List<string> { "ГастроДом", "Кухмистерская", "Белорусская хата" }),
            new City("Гродно", "Беларусь", new List<string> { "Мачанка", "Зубровка", "Холодник" }, new List<string> { "Кронон", "Королевская охота" }),
            new City("Брест", "Беларусь", new List<string> { "Колдуны", "Сырники" }, new List<string> { "Жюль Верн", "Южный парк" }),
            new City("Гомель", "Беларусь", new List<string> { "Камай", "Вареники" }, new List<string> { "София", "Полесье" }),

            new City("Торонто", "Канада", new List<string> { "Путин", "Кленовый сироп" }, new List<string> { "Alo", "Richmond Station", "Canoe" }),
            new City("Монреаль", "Канада", new List<string> { "Багель", "Смоук мит" }, new List<string> { "Joe Beef", "Le Filet", "Au Pied de Cochon" }),
            new City("Ванкувер", "Канада", new List<string> { "Лосось", "Чаудер" }, new List<string> { "Blue Water Cafe", "Hawksworth Restaurant" }),
            new City("Квебек", "Канада", new List<string> { "Фуа-гра", "Крем брюле" }, new List<string> { "Le Saint-Amour", "Chez Muffy" }),

            new City("Пекин", "Китай", new List<string> { "Утка по-пекински", "Димсам", "Лапша" }, new List<string> { "Quanjude", "TRB Hutong", "Duck de Chine" }),
            new City("Шанхай", "Китай", new List<string> { "Шанхайская лапша", "Баоцзы" }, new List<string> { "Ultraviolet", "Fu He Hui", "Jin Xuan" }),
            new City("Гуанчжоу", "Китай", new List<string> { "Кантонская утка", "Конджи" }, new List<string> { "Summer Palace", "Social & Co" }),

            new City("Астана", "Казахстан", new List<string> { "Бешбармак", "Кумыс", "Казы" }, new List<string> { "Qazaq Gourmet", "Rafe", "Традиции" }),
            new City("Алматы", "Казахстан", new List<string> { "Лагман", "Наурыз коже" }, new List<string> { "Kishlak", "Alasha", "Green House" }),
            new City("Шымкент", "Казахстан", new List<string> { "Плов", "Кебаб" }, new List<string> { "Гуляй", "Бархан" }),
        };

        List<(string From, string To)> routes = new List<(string, string)>
        {
            ("Москва", "Минск"), ("Минск", "Монреаль"), ("Монреаль", "Торонто"),
            ("Торонто", "Пекин"), ("Пекин", "Шанхай"), ("Шанхай", "Астана"),
            ("Астана", "Алматы"), ("Москва", "Санкт-Петербург"), ("Минск", "Гродно"),
            ("Москва", "Гуанчжоу"), ("Гуанчжоу", "Шанхай"), ("Шанхай", "Пекин")
        };

        bool HasDirectRoute(string from, string to) => routes.Any(r => (r.From == from && r.To == to));

        Console.WriteLine("Введите начальный город:");
        string startCity = Console.ReadLine().Trim();

        Console.WriteLine("Введите пункт назначения:");
        string destinationCity = Console.ReadLine().Trim();

        var start = cities.FirstOrDefault(c => c.Name.Equals(startCity, StringComparison.OrdinalIgnoreCase));
        var destination = cities.FirstOrDefault(c => c.Name.Equals(destinationCity, StringComparison.OrdinalIgnoreCase));

        if (start == null || destination == null)
        {
            Console.WriteLine("Один или оба города не найдены в базе данных.");
            return;
        }

        Console.WriteLine($"Билет: {start.Name} ({start.Country}) -> {destination.Name} ({destination.Country})");

        Console.WriteLine("\nРекомендации для путешественников:");
        Console.WriteLine($"Местная кухня в {destination.Name}: {string.Join(", ", destination.FamousFoods)}");
        Console.WriteLine($"Рестораны, которые стоит посетить: {string.Join(", ", destination.Restaurants)}");

        // Поиск маршрутов с пересадками с использованием очереди
        var transitRoutes = FindRoutesWithQueue(start.Name, destination.Name, routes);

        if (transitRoutes.Any())
        {
            Console.WriteLine("\nМаршруты с пересадками:");
            foreach (var route in transitRoutes)
            {
                Console.WriteLine($"Маршрут: {string.Join(" -> ", route)}");
                foreach (var cityName in route)
                {
                    var city = cities.FirstOrDefault(c => c.Name == cityName);
                    Console.WriteLine($"Город: {city.Name} ({city.Country})");
                    Console.WriteLine($"Рекомендуемые блюда: {string.Join(", ", city.FamousFoods)}");
                    Console.WriteLine($"Рестораны: {string.Join(", ", city.Restaurants)}");
                }
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("Прямой маршрут или маршруты с пересадками через другие города не найдены.");
        }
    }

    // Функция поиска маршрутов с пересадками с использованием очереди
    static List<List<string>> FindRoutesWithQueue(string start, string destination, List<(string From, string To)> routes)
    {
        var result = new List<List<string>>();
        var queue = new Queue<List<string>>();

        // Добавляем начальный город в очередь
        queue.Enqueue(new List<string> { start });

        while (queue.Any())
        {
            var currentRoute = queue.Dequeue();
            var lastCity = currentRoute.Last();

            // Если достигнут пункт назначения, добавляем маршрут в результат
            if (lastCity == destination)
            {
                result.Add(currentRoute);
                continue;
            }

            // Добавляем все возможные маршруты из последнего города
            foreach (var route in routes.Where(r => r.From == lastCity))
            {
                if (!currentRoute.Contains(route.To)) // Не добавляем города, которые уже есть в маршруте
                {
                    var newRoute = new List<string>(currentRoute) { route.To };
                    queue.Enqueue(newRoute);
                }
            }
        }

        return result;
    }
}
