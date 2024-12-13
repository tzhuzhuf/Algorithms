using System;
using System.Collections.Generic;

class Akinator
{
    static string[] characters = { "Путин", "Наруто", "Блум", "Барби", "Железный человек", "Даша путешественница", "Башмачок", "Карлсон", "Серега пират", "Баба Яга" };
    static Dictionary<string, List<string>> characterTraits = new Dictionary<string, List<string>>
        {
            { "Путин", new List<string> { "да", "да", "нет", "да", "нет", "да"} },
            { "Наруто", new List<string> { "нет", "да", "нет", "да", "нет", "нет"} },
            { "Блум", new List<string> { "нет", "нет", "да", "нет", "нет", "нет"} },
            { "Барби", new List<string> { "нет", "нет", "нет", "нет", "нет", "нет"} },
            { "Железный человек", new List<string> { "нет", "да", "да", "нет", "нет", "нет" } },
            { "Даша путешественница", new List<string> { "нет", "нет", "нет", "нет", "нет", "нет"} },
            { "Башмачок", new List<string> { "нет", "да", "нет", "нет", "нет", "нет"} },
            { "Карлсон", new List<string> { "нет", "да", "да", "нет", "да", "нет"} },
            { "Серега пират", new List<string> { "да", "да", "нет", "нет", "нет", "нет"} },
            { "Баба Яга", new List<string> { "нет", "нет", "да", "нет", "нет", "да"} }
        };

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Добро пожаловать в игру Akinator!");
            Console.WriteLine("Вот доступные персонажи:");
            foreach (string character in characters)
            {
                Console.WriteLine($"- {character}");
            }
            Console.WriteLine("\nВыберите режим игры (для выхода введите \"exit\"):");
            Console.WriteLine("1. Вы загадываете, компьютер угадывает.");
            Console.WriteLine("2. Компьютер загадывает, вы угадываете.");


            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Mode1();
            }
            else if (choice == "2")
            {
                Mode2();
            }
            else if(choice == "exit")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Неправильный выбор. Пожалуйста, выберите 1 или 2.");
            }
        }
    }

    static void Mode1()
    {
        Console.WriteLine("Загадайте персонажа, и я постараюсь его угадать!");
        string[] questions = {
            "Ваш персонаж существует в реальности?",
            "Ваш персонаж мужского пола?",
            "Ваш персонаж умеет летать?",
            "Ваш персонаж занимается политикой?",
            "Ваш персонаж отыгрывал летающее кресло?",
            "В возрасте ли ваш персонаж?"
        };

        List<string> answers = new List<string>();

        int i = 0;
        while(i < questions.Length)
        {
            Console.WriteLine(questions[i] + " (да/нет)");
            string answer = Console.ReadLine().ToLower().Trim();
            if (answer != "да" && answer != "нет")
            {
                Console.WriteLine("Неверный ввод, пожалуйста, ответьте 'да' или 'нет'.");
                continue;
            }
            answers.Add(answer);
            i++;
        }

        foreach (var character in characters)
        {
            bool match = true;
            for (int j = 0; j < questions.Length; j++)
            {
                if (answers[j] != characterTraits[character][j])
                {
                    match = false;
                    break;
                }
            }
            if (match)
            {
                Console.WriteLine($"Ваш персонаж — это {character}!");
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey(true);
                return;
            }
        }

        Console.WriteLine("К сожалению, я не смог угадать вашего персонажа.");
    }

    static void Mode2()
    {
        Random random = new Random();
        string secretCharacter = characters[random.Next(characters.Length)];
        Console.WriteLine("Я загадал персонажа. Попробуйте его угадать, задавая вопросы (ответы только 'да' или 'нет').\n ");

        Dictionary<string, int> questionMapping = new Dictionary<string, int>
        {
            { "реальност", 0 },
            { "мужчина", 1 },
            { "летать", 2 },
            { "политик", 3 },
            { "возраст", 4 }
        };

        bool guessed = false;
        int remainingAttemps = 20;

        while (!guessed && remainingAttemps > 0)
        {
            Console.WriteLine($"\nУ вас осталось {remainingAttemps} попыток. Задайте вопрос или попробуйте угадать персонажа (если захотите угадать персонажа то введите \"это {{имя персонажа}}\" ):");
            string input = Console.ReadLine().ToLower();

            if (input.Contains(secretCharacter.ToLower()))
            {
                Console.WriteLine($"Поздравляю! Вы угадали: {secretCharacter}.");
                guessed = true;
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey(true);
            }

            bool recognized = false;
            foreach (var pair in questionMapping)
            {
                if (input.Contains(pair.Key))
                {
                    Console.WriteLine(characterTraits[secretCharacter][pair.Value]);
                    recognized = true;
                    break;
                }
            }

            if (!recognized)
            {
                Console.WriteLine("Я не могу ответить на этот вопрос. Попробуйте переформулировать.");
            }

            remainingAttemps--;
        }

        if (!guessed)
        {
            Console.WriteLine($"Вы проиграли! Я загадал: {secretCharacter}.");
        }
    }
}