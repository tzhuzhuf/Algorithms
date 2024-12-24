using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;



namespace SudokuApp
{
    public partial class MainWindow : Window
    {
        private string saveDirectory = "SavedGames"; private int[,] sudokuGrid = new int[9, 9];
        private int[,] correctSolution = new int[9, 9]
        {
            { 5, 3, 4, 6, 7, 8, 9, 1, 2 },
            { 6, 7, 2, 1, 9, 5, 3, 4, 8 },
            { 1, 9, 8, 3, 4, 2, 5, 6, 7 },
            { 8, 5, 9, 7, 6, 1, 4, 2, 3 },
            { 4, 2, 6, 8, 5, 3, 7, 9, 1 },
            { 7, 1, 3, 9, 2, 4, 8, 5, 6 },
            { 9, 6, 1, 5, 3, 7, 2, 8, 4 },
            { 2, 8, 7, 4, 1, 9, 6, 3, 5 },
            { 3, 4, 5, 2, 8, 6, 1, 7, 9 }
        };
        private int lives = 3;
        private int filledCells = 0;

        private double currentDifficulty;

        public MainWindow(double difficulty)
        {
            InitializeComponent();
            currentDifficulty = difficulty;

            if (!Directory.Exists(saveDirectory))
                Directory.CreateDirectory(saveDirectory);

            filledCells = 0; InitializeSudokuGrid();
            GeneratePuzzle(currentDifficulty);
        }


        private void InitializeSudokuGrid()
        {
            SudokuGrid.Children.Clear();
            SudokuGrid.RowDefinitions.Clear();
            SudokuGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < 9; i++)
            {
                SudokuGrid.RowDefinitions.Add(new RowDefinition());
                SudokuGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBox textBox = new TextBox
                    {
                        Width = 40,
                        Height = 40,
                        MaxLength = 1,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 18,
                        TextAlignment = TextAlignment.Center,
                        IsReadOnly = false
                    };

                    textBox.Tag = new Tuple<int, int>(i, j);

                    textBox.TextChanged += (sender, e) =>
                    {
                        var tag = (Tuple<int, int>)((TextBox)sender).Tag;
                        int x = tag.Item1;
                        int y = tag.Item2;

                        HandleTextChanged(x, y, (TextBox)sender);
                    };

                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, j);
                    SudokuGrid.Children.Add(textBox);

                    if ((i % 3 == 0) && (j % 3 == 0))
                    {
                        var border = new Border
                        {
                            BorderBrush = System.Windows.Media.Brushes.Black,
                            BorderThickness = new System.Windows.Thickness(2),
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch
                        };

                        SudokuGrid.Children.Add(border);
                        Grid.SetRowSpan(border, 3);
                        Grid.SetColumnSpan(border, 3);
                        Grid.SetRow(border, i);
                        Grid.SetColumn(border, j);
                    }
                }
            }
        }





        private void GeneratePuzzle(double difficulty)
        {
            Random rand = new Random();

            filledCells = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    sudokuGrid[i, j] = correctSolution[i, j];
                }
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (rand.NextDouble() > difficulty)
                    {
                        sudokuGrid[i, j] = 0;
                    }

                    TextBox textBox = GetTextBoxFromGrid(i, j);
                    if (textBox != null)
                    {
                        if (sudokuGrid[i, j] != 0)
                        {
                            textBox.Text = sudokuGrid[i, j].ToString();
                            textBox.IsReadOnly = true;
                            filledCells++;
                        }
                        else
                        {
                            textBox.Text = "";
                            textBox.IsReadOnly = false;
                        }
                    }
                }
            }
        }



        private TextBox GetTextBoxFromGrid(int row, int column)
        {
            foreach (var child in SudokuGrid.Children)
            {
                if (child is TextBox textBox && Grid.GetRow(textBox) == row && Grid.GetColumn(textBox) == column)
                {
                    return textBox;
                }
            }
            return null;
        }

        private void HandleTextChanged(int x, int y, TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                sudokuGrid[x, y] = 0;
                return;
            }

            if (int.TryParse(textBox.Text, out int value) && value >= 1 && value <= 9)
            {
                if (value == correctSolution[x, y])
                {
                    if (sudokuGrid[x, y] == 0)
                    {
                        filledCells++;
                    }

                    sudokuGrid[x, y] = value;

                    if (filledCells == 81)
                    {
                        MessageBox.Show("Поздравляем, вы выиграли!");
                        ResetGame();
                    }
                }
                else
                {
                    lives--;
                    MessageBox.Show($"Неправильное число! Осталось {lives} жизней.");
                    textBox.Text = "";
                    if (lives == 0)
                    {
                        MessageBox.Show("Игра окончена! Вы проиграли.");
                        ResetGame();
                    }
                }
            }
            else
            {
                MessageBox.Show("Введите число от 1 до 9.");
                textBox.Text = "";
            }
        }


        private void ResetGame()
        {
            lives = 3;
            filledCells = 0;
            GeneratePuzzle(currentDifficulty);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Сохранить игру перед выходом?", "Выход", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SaveGame();
                ReturnToStartWindow();
            }
            else if (result == MessageBoxResult.No)
            {
                ReturnToStartWindow();
            }
        }

        private void SaveGame()
        {
            string saveFileName = $"{DateTime.Now:yyyyMMdd_HHmmss}.json"; string savePath = Path.Combine(saveDirectory, saveFileName);

            var gameData = new
            {
                Grid = sudokuGrid,
                Lives = lives,
                FilledCells = filledCells
            };

            string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);

            File.WriteAllText(savePath, json);

            MessageBox.Show("Игра сохранена в формате JSON!");
        }


        private void ReturnToStartWindow()
        {
            StartWindow startWindow = new StartWindow();
            startWindow.Show();
            this.Close();
        }
    }
}
