using System;
using System.Collections.Generic;
using System.Text;

class WordGame
{
    static void Main()
    {
        Console.WriteLine("ИГРА В СЛОВА!");
        Console.WriteLine("Для завершения игры введите 'выход'.");

        string filePath = System.IO.Path.GetFullPath(@"summary.txt");


        List<string> availableWords = ToList(filePath);
        List<string> usedWords = new List<string>();
        string lastLetter = "";
        bool isGameRunning = true;
        int player = 1;

        while (isGameRunning)
        {
            Console.WriteLine($"Игрок {player}, введите слово:");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (input == "выход")
            {
                Console.WriteLine("Игра завершена.");
                break;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Слово не может быть пустым. Попробуйте снова.");
                continue;
            }

            if (usedWords.Contains(input))
            {
                Console.WriteLine("Это слово уже использовано. Попробуйте снова.");
                continue;
            }

            if (!string.IsNullOrEmpty(lastLetter) && input[0] != lastLetter[0])
            {
                Console.WriteLine($"Слово должно начинаться с буквы '{lastLetter}'. Попробуйте снова.");
                continue;
            }

            if (!availableWords.Contains(input))
            {
                Console.WriteLine("Такого слова не существует!");
                continue;
            }

            usedWords.Add(input);
            lastLetter = input[^1].ToString(); 

            if (lastLetter == "ь" || lastLetter == "ъ" || lastLetter =="ы")
            {
                lastLetter = input[^2].ToString();
            }

            player = player == 1 ? 2 : 1;
        }
    }
    static List<string> ToList(string filePath)
    {
        List<string> words = new List<string>();
        using(StreamReader reader = new StreamReader(filePath))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    words.Add(line.Trim().ToLower());
                }
            }
        }
        return words;
    }
}
