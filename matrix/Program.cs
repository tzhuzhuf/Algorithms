using System;
class Program
{
    static void Main()
    {
        Random random = new Random();
        Console.WriteLine("Введите количество строк первой матрицы (A):");
        int rows1 = int.Parse(Console.ReadLine());
        Console.WriteLine("Введите количество столбцов первой матрицы (A):");
        int cols1 = int.Parse(Console.ReadLine());
        int[,] A = new int[rows1, cols1];

        Console.WriteLine("Ваша матрица A: ");
        for (int i = 0; i < rows1; i++)
        {
            for (int j = 0; j < cols1; j++)
            {
                A[i, j] = random.Next(-100, 100);
                Console.Write("{0}\t", A[i, j]);
            }
            Console.WriteLine();
        }

        Console.WriteLine("Введите количество строк второй матрицы (B):");
        int rows2 = int.Parse(Console.ReadLine());
        Console.WriteLine("Введите количество столбцов второй матрицы (B):");
        int cols2 = int.Parse(Console.ReadLine());

        if (cols1 != rows2)
        {
            Console.WriteLine("Ошибка: количество столбцов первой матрицы должно быть равно количеству строк второй матрицы.");
            return;
        }

        Console.WriteLine("Ваша матрица B: ");
        int[,] B = new int[rows2, cols2];
        for (int i = 0; i < rows2; i++)
        {
            for (int j = 0; j < cols2; j++)
            {
                B[i, j] = random.Next(-100, 100);
                Console.Write("{0}\t", B[i, j]);
            }
            Console.WriteLine();
        }
        int[,] C = new int[rows1, cols2];

        for (int i = 0; i < rows1; i++)
        {
            for (int j = 0; j < cols2; j++)
            {
                C[i, j] = 0;
                for (int k = 0; k < cols1; k++)
                {
                    C[i, j] += A[i, k] * B[k, j];

                }
            }
        }

        Console.WriteLine("Финальная матрица C (до сортировки):");
        PrintMatrix(C);

        int totalSize = rows1 * cols2;
        int[] flatArray = new int[totalSize];
        int index = 0;
        for (int i = 0; i < rows1; i++)
        {
            for (int j = 0; j < cols2; j++)
            {
                flatArray[index++] = C[i, j];
            }
        }


        Array.Sort(flatArray);

        index = 0;
        for (int i = 0; i < rows1; i++)
        {
            for (int j = 0; j < cols2; j++)
            {
                C[i, j] = flatArray[index++];
            }
        }

        Console.WriteLine("Финальная матрица C (после сортировки):");
        PrintMatrix(C);
    }

    static void PrintMatrix(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }
}