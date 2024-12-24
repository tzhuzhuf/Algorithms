using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace SudokuApp
{
    public partial class SavedGamesWindow : Window
    {
        private string saveDirectory = "SavedGames";
        public event Action<int[,]> OnGameLoaded;

        public SavedGamesWindow()
        {
            InitializeComponent();
            LoadSavedGames();
        }

        private void LoadSavedGames()
        {
            if (Directory.Exists(saveDirectory))
            {
                string[] files = Directory.GetFiles(saveDirectory, "*.json"); SavedGamesList.Items.Clear();

                foreach (var file in files)
                {
                    SavedGamesList.Items.Add(Path.GetFileName(file));
                }
            }
        }

        private void SavedGamesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SavedGamesList.SelectedItem != null)
            {
                MessageBox.Show($"Вы выбрали: {SavedGamesList.SelectedItem.ToString()}");
            }
        }

        private void LoadGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (SavedGamesList.SelectedItem != null)
            {
                string selectedFile = SavedGamesList.SelectedItem.ToString();
                LoadGame(selectedFile);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите игру из списка.");
            }
        }

        private void LoadGame(string fileName)
        {
            string filePath = Path.Combine(saveDirectory, fileName);

            string json = File.ReadAllText(filePath);

            var gameData = JsonConvert.DeserializeObject<GameData>(json);

            ContinuingGameWindow continuingGameWindow = new ContinuingGameWindow(gameData.Grid, gameData.Lives, gameData.FilledCells, filePath);

            continuingGameWindow.Show();
            this.Close();
        }


        private void BackToMain_Click(object sender, RoutedEventArgs e)
        {
            StartWindow startWindow = new StartWindow();
            startWindow.Show();
            this.Close();
        }
    }
    public class GameData
    {
        public int[,] Grid { get; set; }
        public int Lives { get; set; }
        public int FilledCells { get; set; }
    }

}
