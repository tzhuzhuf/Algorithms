using System.Windows;
using System.Windows.Controls;

namespace SudokuApp
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void LevelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Если выбран элемент, включить кнопку "Играть"
            if (LevelSelector.SelectedItem != null)
            {
                PlayButton.IsEnabled = true;
            }
            else
            {
                PlayButton.IsEnabled = false;
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedLevel = (LevelSelector.SelectedItem as ComboBoxItem)?.Content.ToString();
            double difficulty = 0.8;
            switch (selectedLevel)
            {
                case "Средний":
                    difficulty = 0.5;
                    break;
                case "Трудный":
                    difficulty = 0.3;
                    break;
                case "Лёгкий":
                default:
                    difficulty = 0.8;
                    break;
            }

            MainWindow mainWindow = new MainWindow(difficulty);
            mainWindow.Show();
            this.Close();
        }

        private void SavedGamesButton_Click(object sender, RoutedEventArgs e)
        {
            SavedGamesWindow savedGamesWindow = new SavedGamesWindow();
            savedGamesWindow.Show();
            this.Close();
        }
    }
}
