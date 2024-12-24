using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace SudokuApp
{
    public partial class ContinuingGameWindow : Window
    {
        private int[,] currentGrid = new int[9, 9];
        private string saveFileName;
        private int lives;
        private int filledCells;
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

        public ContinuingGameWindow(int[,] loadedGrid, int lives, int filledCells, string saveFileName)
        {
            InitializeComponent();
            this.saveFileName = saveFileName;
            this.lives = lives;
            this.filledCells = filledCells;
            LoadGame(loadedGrid);
            DisplayGrid();
        }

        private void LoadGame(int[,] loadedGrid)
        {
            if (loadedGrid == null)
            {
                MessageBox.Show("Ошибка загрузки игры! Недействительные данные.");
                return;
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    currentGrid[i, j] = loadedGrid[i, j] >= 0 && loadedGrid[i, j] <= 9 ? loadedGrid[i, j] : 0;
                }
            }
        }

        private void DisplayGrid()
        {
            SudokuGrid.Children.Clear();
            SudokuGrid.RowDefinitions.Clear();
            SudokuGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < 9; i++)
            {
                SudokuGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                SudokuGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var textBox = new TextBox
                    {
                        Width = 40,
                        Height = 40,
                        Text = currentGrid[i, j] == 0 ? "" : currentGrid[i, j].ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 18,
                        MaxLength = 1,
                        IsReadOnly = currentGrid[i, j] != 0,
                        Background = System.Windows.Media.Brushes.White,
                        BorderThickness = new System.Windows.Thickness(0),
                        TextAlignment = TextAlignment.Center
                    };

                    int row = i;
                    int col = j;

                    textBox.TextChanged += (sender, e) =>
                    {
                        HandleTextChanged(row, col, textBox);
                    };

                    var border = new Border
                    {
                        BorderBrush = System.Windows.Media.Brushes.Black,
                        BorderThickness = GetBorderThickness(row, col),
                        Child = textBox
                    };

                    SudokuGrid.Children.Add(border);
                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                }
            }
        }


        private System.Windows.Thickness GetBorderThickness(int row, int col)
        {
            return new System.Windows.Thickness(
                col % 3 == 0 ? 2 : 1,
                row % 3 == 0 ? 2 : 1,
                (col + 1) % 3 == 0 ? 2 : 1,
                (row + 1) % 3 == 0 ? 2 : 1
            );
        }

        private System.Windows.Media.Brush GetBorderBrush(int row, int col)
        {
            return System.Windows.Media.Brushes.Black;
        }

        private void HandleTextChanged(int x, int y, TextBox textBox)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    currentGrid[x, y] = 0;
                }
                else if (int.TryParse(textBox.Text, out int value) && value >= 1 && value <= 9)
                {
                    if (value == correctSolution[x, y])
                    {
                        currentGrid[x, y] = value;
                        CheckGameCompletion();
                    }
                    else
                    {
                        MessageBox.Show("Неправильное значение! Попробуйте снова.");
                        textBox.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Введите число от 1 до 9.");
                    textBox.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обработке ввода: {ex.Message}");
            }
        }

        private void CheckGameCompletion()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (currentGrid[i, j] != correctSolution[i, j])
                        return;
                }
            }

            try
            {
                if (File.Exists(saveFileName))
                    File.Delete(saveFileName);
                MessageBox.Show("Поздравляем! Вы успешно завершили игру!");
                StartWindow startWindow = new StartWindow();
                startWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при завершении игры: {ex.Message}");
            }
        }

        private void SaveGame()
        {
            try
            {
                var gameData = new
                {
                    Grid = currentGrid,
                    Lives = lives,
                    FilledCells = filledCells
                };

                string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
                File.WriteAllText(saveFileName, json);
                MessageBox.Show("Игра сохранена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении игры: {ex.Message}");
            }
        }

        private void SaveAndExit_Click(object sender, RoutedEventArgs e)
        {
            SaveGame();
            StartWindow startWindow = new StartWindow();
            startWindow.Show();
            this.Close();
        }
    }
}
