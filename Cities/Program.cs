using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static HashSet<string> usedCities = new HashSet<string>();
    static List<string> cities = new List<string>();

    static void Main()
    {
        string filePath = System.IO.Path.GetFullPath(@"russian_cities.txt");
        cities = ToList(filePath);

        Console.WriteLine("Выберите режим игры: \n1 - Игрок против игрока \n2 - PlayerVsBot");
        int mode = int.Parse(Console.ReadLine());

        if (mode == 1)
            PvP();
        else if (mode == 2)
            PlayerVsBot();
        else
            Console.WriteLine("Неверный режим.");
    }
    static List<string> ToList(string filePath)
    {
        List<string> cities = new List<string>();
        using (StreamReader reader = new StreamReader(filePath))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                cities.Add(line.Trim().ToLower());
            }
        }
        return cities;
    }

    static void PvP()
    {
        string lastCity = "";
        int currentPlayer = 1;

        while (true)
        {
            Console.WriteLine($"Игрок {currentPlayer}, назовите город (или введите 'сдаться' для завершения):");
            string input = Console.ReadLine().Trim();

            if (input.ToLower() == "сдаться")
            {
                Console.WriteLine($"Игрок {3 - currentPlayer} победил!");
                break;
            }

            while (!IsValidCity(input, lastCity, currentPlayer))
            {
                Console.WriteLine($"Игрок {currentPlayer}, попробуйте еще раз:");
                input = Console.ReadLine().Trim();

                if (input.ToLower() == "сдаться")
                {
                    Console.WriteLine($"Игрок {currentPlayer} победил!");
                    return;
                }
            }

            usedCities.Add(input.ToLower());
            lastCity = input;

            if (!IsCitieAvailable(lastCity))
            {
                Console.WriteLine($"Игрок {currentPlayer} проиграл! Все города на букву '{char.ToUpper(lastCity.Last())}' закончились.");
                break;
            }

            currentPlayer = 3 - currentPlayer;
        }
    }

    static void PlayerVsBot()
    {
        string lastCity = "";
        int currentPlayer = 1;

        while (true)
        {
            if (currentPlayer == 1)
            {
                Console.WriteLine("Игрок, назовите город (или введите 'сдаться' для завершения):");
                string input = Console.ReadLine().Trim();

                if (input.ToLower() == "сдаться")
                {
                    Console.WriteLine("Компьютер победил!");
                    break;
                }

                while (!IsValidCity(input, lastCity, currentPlayer))
                {
                    Console.WriteLine("Попробуйте еще раз:");
                    input = Console.ReadLine().Trim();

                    if (input.ToLower() == "сдаться")
                    {
                        Console.WriteLine("Компьютер победил!");
                        return;
                    }
                }

                usedCities.Add(input.ToLower());
                lastCity = input;

                if (!IsCitieAvailable(lastCity))
                {
                    Console.WriteLine("Все города на букву '" + char.ToUpper(lastCity.Last()) + "' кончились. Ничья!");
                    break;
                }
            }
            else
            {
                char nextLetter = GetNextLetter(lastCity);
                var availableCities = cities
                    .Where(city => !usedCities.Contains(city.ToLower()) && city.StartsWith(nextLetter.ToString(), StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (availableCities.Count > 0)
                {
                    string computerCity = availableCities[new Random().Next(availableCities.Count)];
                    Console.WriteLine($"Компьютер называет: {computerCity}");
                    usedCities.Add(computerCity.ToLower());
                    lastCity = computerCity;

                    if (!IsCitieAvailable(lastCity))
                    {
                        Console.WriteLine("Все города на букву '" + char.ToUpper(lastCity.Last()) + "' кончились. Ничья!");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Игрок победил! Компьютер не может назвать город.");
                    break;
                }
            }
            currentPlayer = 3 - currentPlayer;
        }
    }

    static char GetNextLetter(string city)
    {
        char lastChar = city[city.Length - 1];


        while (lastChar == 'ь' || lastChar == 'ъ' || lastChar == 'ы') 
        {
            if (city.Length > 1)
            {
                city = city.Substring(0, city.Length - 1);
                lastChar = city[city.Length - 1];
            }
            else
            {
                return 'а';
            }
        }

        return char.ToLower(lastChar);
    }

    static bool IsValidCity(string city, string lastCity, int currentPlayer)
    {
        if (usedCities.Contains(city.ToLower()))
        {
            Console.WriteLine($"Город \"{city}\" уже был использован.");
            return false;
        }

        if (!cities.Contains(city, StringComparer.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Город \"{city}\" не существует.");
            return false;
        }

        if (!string.IsNullOrEmpty(lastCity))
        {
            char expectedFirstChar = GetNextLetter(lastCity);
            if (char.ToLower(city[0]) != expectedFirstChar)
            {
                Console.WriteLine($"Город \"{city}\" не начинается на букву \"{expectedFirstChar}\".");
                return false;
            }
        }

        return true;
    }

    static bool IsCitieAvailable(string lastCity)
    {
        if (string.IsNullOrEmpty(lastCity)) return false;

        char lastChar = char.ToLower(lastCity.Last());

        if (lastChar == 'ь' || lastChar == 'ъ' || lastChar == 'ы') 
        {
            if (lastCity.Length > 1)
            {
                lastCity = lastCity.Substring(0, lastCity.Length - 1);
                lastChar = char.ToLower(lastCity.Last());
            }
            else
            {
                return false;
            }
        }

        return cities.Any(city => !usedCities.Contains(city.ToLower()) && char.ToLower(city[0]) == lastChar);
    }

}