using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Pentago.GUI;
using Pentago.GameCore;
using Pentago.AI;
using Pentago;


namespace Pentago.GUI
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void QuickMatch_Click(object sender, RoutedEventArgs e)
        {
            QuickMatchMenuScroll.Visibility = Visibility.Visible;
            PlayerVsGroupBox.Visibility = Visibility.Visible;
            PlayerVsStackPanel.Visibility = Visibility.Visible;
            PlayerVsPlayer.Visibility = Visibility.Visible;
            PlayerVsComputer.Visibility = Visibility.Visible;
            MoveFirst.Visibility = Visibility.Visible;
            MoveGroupBox.Visibility = Visibility.Visible;
            MoveStackPanel.Visibility = Visibility.Visible;
            Player1.Visibility = Visibility.Visible;
            Player2.Visibility = Visibility.Visible;
            Player1Name.Visibility = Visibility.Visible;
            Player1NameTextBox.Visibility = Visibility.Visible;
            Player2Name.Visibility = Visibility.Visible;
            Player2NameTextBox.Visibility = Visibility.Visible;
            Battle.Visibility = Visibility.Visible;
            ReHideMenues("Quick");
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ReHideMenues(string MenuName)
        {
            if (MenuName == "Story")
            {
                QuickMatchMenuScroll.Visibility = Visibility.Visible;
                PlayerVsGroupBox.Visibility = Visibility.Visible;
                PlayerVsStackPanel.Visibility = Visibility.Visible;
                PlayerVsPlayer.Visibility = Visibility.Visible;
                PlayerVsComputer.Visibility = Visibility.Visible;
                MoveFirst.Visibility = Visibility.Visible;
                MoveGroupBox.Visibility = Visibility.Visible;
                MoveStackPanel.Visibility = Visibility.Visible;
                Player1.Visibility = Visibility.Visible;
                Player2.Visibility = Visibility.Visible;
                Player1Name.Visibility = Visibility.Visible;
                Player1NameTextBox.Visibility = Visibility.Visible;
                Player2Name.Visibility = Visibility.Visible;
                Player2NameTextBox.Visibility = Visibility.Visible;
                Battle.Visibility = Visibility.Visible;
            }
            else if (MenuName == "Quick")
            {
                StoryModeMenuScroll.Visibility = Visibility.Visible;
                NewProfileTextBox.Visibility = Visibility.Visible;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PlayerVsComputer_Checked(object sender, RoutedEventArgs e)
        {
            Player2Name.Visibility = Visibility.Visible;
            Player2NameTextBox.Visibility = Visibility.Visible;
            Player2.Content = "Computer";
            ComputerLevel.Visibility = Visibility.Visible;
            ComputerHardLevel.Visibility = Visibility.Visible;
            ComputerEasyLevel.Visibility = Visibility.Visible;
        }

        private void PlayerVsComputer_Unchecked(object sender, RoutedEventArgs e)
        {
            Player2Name.Visibility = Visibility.Visible;
            Player2NameTextBox.Visibility = Visibility.Visible;
            Player2.Content = "Player 2";
            ComputerLevel.Visibility = Visibility.Visible;
            ComputerHardLevel.Visibility = Visibility.Visible;
            ComputerEasyLevel.Visibility = Visibility.Visible;
        }

        private void Battle_Click(object sender, RoutedEventArgs e)
        {
            /******************************DATA VALIDATION HERE BEFORE CALLING INITIALIZE******************************/
            if (PlayerVsPlayer.IsChecked == true)
                InitializePlayerVsPlayerGame();
            else if (PlayerVsComputer.IsChecked == true)
                InitializePlayerVsComputerGame();
            else
                Console.WriteLine("There is something wrong!");
        }

        private void InitializePlayerVsPlayerGame()
        {
            string player1Name = Player1NameTextBox.Text;

            bool isPlayer1Active;
            if (Player1.IsChecked == true)
                isPlayer1Active = true;
            else
                isPlayer1Active = false;

            ImageBrush player1Image = new ImageBrush();
            player1Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/dragon1.jpg", UriKind.Absolute));
            Player player1 = new Player(player1Name, isPlayer1Active, player1Image);

            string player2Name = Player2NameTextBox.Text;
            ImageBrush player2Image = new ImageBrush();
            player2Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/dragon2.jpg", UriKind.Absolute));
            Player player2 = new Player(player2Name, !isPlayer1Active, player2Image);

            GameOptions gameOptions = new GameOptions(GameOptions.TypeOfGame.QuickMatch, player1, player2);
            Window gameWindow = new GameWindow(gameOptions);
            App.Current.MainWindow = gameWindow;
            gameWindow.Show();
            this.Hide();

        }

        private void InitializePlayerVsComputerGame()
        {
            string player1Name = Player1NameTextBox.Text;

            bool isPlayer1Active;
            if (Player1.IsChecked == true)
                isPlayer1Active = true;
            else
                isPlayer1Active = false;

            ImageBrush player1Image = new ImageBrush();
            player1Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/dragon1.jpg", UriKind.Absolute));
            Player player1 = new Player(player1Name, isPlayer1Active, player1Image);

            string computerPlayerName = "Miley Twerk";
            ImageBrush computerPlayerImage = new ImageBrush();
            computerPlayerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/dragon2.jpg", UriKind.Absolute));

            computerAI.Difficulty difficulty;
            if (ComputerEasyLevel.IsChecked == true)
                difficulty = computerAI.Difficulty.Easy;
            else
                difficulty = computerAI.Difficulty.Hard;

            computerAI computerPlayer = new computerAI(computerPlayerName, !isPlayer1Active, computerPlayerImage, difficulty);
            GameOptions gameOptions = new GameOptions(GameOptions.TypeOfGame.AI, player1, computerPlayer);
            Window gameWindow = new GameWindow(gameOptions);
            App.Current.MainWindow = gameWindow;
            gameWindow.Show();
            this.Hide();
        }
    }
}
