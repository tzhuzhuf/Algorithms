using System;
using System.Collections.Generic;
using System.Text;

namespace MeatRecipe
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding(1251);

            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            System.Console.InputEncoding = enc1251;
            Console.WriteLine("Введите количество мяса в кг или граммах:");
            string input = Console.ReadLine();
            double meatQuantity;

            if (input.Contains("кг"))
            {
                meatQuantity = double.Parse(input.Replace("кг", ""));
            }
            else if (input.Contains("г"))
            {
                meatQuantity = double.Parse(input.Replace("г", "")) / 1000;
            }
            else
            {
                Console.WriteLine("Неправильный формат ввода. Пожалуйста, введите количество мяса в кг или граммах.");
                return;
            }

            Console.WriteLine("Введите время приготовления в минутах (опционально):");
            string timeInput = Console.ReadLine();
            int cookingTime = 0;

            if (!string.IsNullOrEmpty(timeInput))
            {
                cookingTime = int.Parse(timeInput);
            }

            Console.WriteLine("Введите ингредиенты (опционально), разделенные запятыми:");
            string ingredientsInput = Console.ReadLine();
            List<string> ingredients = new List<string>();

            if (!string.IsNullOrEmpty(ingredientsInput))
            {
                ingredients = ingredientsInput.Split(',').Select(i => i.Trim()).ToList();
            }

            Recipe recipe = GetOptimalRecipe(meatQuantity, cookingTime, ingredients);

            if (recipe != null)
            {
                Console.WriteLine("Оптимальный рецепт:");
                Console.WriteLine($"Блюдо: {recipe.Dish}");
                Console.WriteLine($"Ингредиенты: {string.Join(", ", recipe.Ingredients)}");
                Console.WriteLine($"Время приготовления: {recipe.CookingTime} минут");
                Console.WriteLine($"Инструкции: {recipe.Instructions}");
            }
            else
            {
                Console.WriteLine("Не удалось найти подходящий рецепт.");
            }
        }

        static Recipe GetOptimalRecipe(double meatQuantity, int cookingTime, List<string> ingredients)
        {

            if (meatQuantity < 0.5)
            {
                return new Recipe
                {
                    Dish = "Мини-бургер",
                    Ingredients = new List<string> { "мясо", "хлеб", "сыр" },
                    CookingTime = 10,
                    Instructions = "Обжарьте мясо, положите на хлеб, добавьте сыр и подайте."
                };
            }
            else if (meatQuantity < 1.0)
            {
                return new Recipe
                {
                    Dish = "Бургер",
                    Ingredients = new List<string> { "мясо", "хлеб", "сыр", "овощи" },
                    CookingTime = 20,
                    Instructions = "Обжарьте мясо, положите на хлеб, добавьте сыр и овощи, подайте."
                };
            }
            else
            {
                return new Recipe
                {
                    Dish = "Мясной стейк",
                    Ingredients = new List<string> { "мясо", "овощи" },
                    CookingTime = 30,
                    Instructions = "Обжарьте мясо, подайте с овощами."
                };
            }
        }
    }

    public class Recipe
    {
        public string Dish { get; set; }
        public List<string> Ingredients { get; set; }
        public int CookingTime { get; set; }
        public string Instructions { get; set; }
    }
}