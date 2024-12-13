using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите слово: ");
        string input = Console.ReadLine();

        input = new string(input.Where(c => !Char.IsDigit(c)).ToArray());
        Dictionary<char, int> lettersCount = new Dictionary<char, int>();

        foreach (char c in input)
        {
            if (!lettersCount.ContainsKey(c))
                lettersCount[c] = 0;

            lettersCount[c]++;
        }

        List<string> words = new List<string>();
        GenerateAllWords(input, "", 0, lettersCount, words, 3);

        Console.WriteLine("Получившиеся 'слова':");

        if (words.Count > 0)
        {
            foreach (var word in words)
            {
                Console.WriteLine(word);
            }
        }
        else
        {
            Console.WriteLine("Из этих букв невозможно составить слова.");
        }
    }

    private static void GenerateAllWords(string input, string currentWord, int length, Dictionary<char, int> lettersCount, List<string> words, int minLength)
    {

        if (length >= minLength)
        {
            words.Add(currentWord);
        }

        for (int i = 0; i < input.Length; ++i)
        {
            char currentChar = input[i];

            if (lettersCount.TryGetValue(currentChar, out int count) && count > 0)
            {
                lettersCount[currentChar]--;

                        
                string newCurrentWord = currentWord + currentChar;


                GenerateAllWords(input, newCurrentWord, length + 1, lettersCount, words, minLength);

                lettersCount[currentChar]++;
            }
        }
    }
}