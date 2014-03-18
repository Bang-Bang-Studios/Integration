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
using Pentago.GUI.Classes;
using System.Windows.Media.Animation;


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
            SoundManager.backgroundMusicPlayer.Open(new Uri("GUI/Sounds/Intro.mp3", UriKind.Relative));
            SoundManager.backgroundMusicPlayer.Play();
        }
        
        private void QuickMatch_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.Click);
            Player1NameTextBox.Focusable = true;
            Player1NameTextBox.Focus();
            if (QuickMatchMenuScroll.Visibility != Visibility.Visible)
            {
                
                //Button b = (Button)sender;
               // DoubleAnimation animation = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(1000)));
                //DoubleAnimation reverseAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(100)));
                //b.BeginAnimation(Image.OpacityProperty, animation);
                //QuickMatchMenuScroll.BeginAnimation(Image.OpacityProperty, reverseAnimation);
                QuickMatchMenuScroll.Visibility = Visibility.Hidden;
                QuickMatchMenuScroll.Visibility = Visibility.Visible;

                //animation = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(1000)));
                //reverseAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(100)));
               // b.BeginAnimation(GroupBox.OpacityProperty, animation);
                //PlayerVsGroupBox.BeginAnimation(GroupBox.OpacityProperty, reverseAnimation);
                PlayerVsGroupBox.Visibility = Visibility.Visible;

                //animation = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(1000)));
                //reverseAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(100)));
                //b.BeginAnimation(StackPanel.OpacityProperty, animation);
                //PlayerVsStackPanel.BeginAnimation(StackPanel.OpacityProperty, reverseAnimation);
                PlayerVsStackPanel.Visibility = Visibility.Visible;

                //animation = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(1000)));
                //reverseAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(100)));
                //b.BeginAnimation(Button.OpacityProperty, animation);
                //PlayerVsPlayer.BeginAnimation(Button.OpacityProperty, reverseAnimation);
                PlayerVsPlayer.Visibility = Visibility.Visible;

                //animation = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(1000)));
                //reverseAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(100)));
                //b.BeginAnimation(Button.OpacityProperty, animation);
                //PlayerVsComputer.BeginAnimation(Button.OpacityProperty, reverseAnimation);
                PlayerVsComputer.Visibility = Visibility.Visible;
                
                MoveFirst.Visibility = Visibility.Visible;
                MoveGroupBox.Visibility = Visibility.Visible;
                MoveStackPanel.Visibility = Visibility.Visible;
                Player1.Visibility = Visibility.Visible;
                Player2.Visibility = Visibility.Visible;
                Player1Name.Visibility = Visibility.Visible;
                Player1NameTextBox.Visibility = Visibility.Visible;
                Player1NameTextBox.Focusable = true;
                Player1NameTextBox.Focus();
                Player2Name.Visibility = Visibility.Visible;
                Player2NameTextBox.Visibility = Visibility.Visible;
                Battle.Visibility = Visibility.Visible;
                PlayerVsPlayerOn.Visibility = Visibility.Visible;
                PlayerVsComputerOff.Visibility = Visibility.Visible;
                Player1MoveFirstOn.Visibility = Visibility.Visible;
                Player2MoveFirstOff.Visibility = Visibility.Visible;
                ReHideMenues("Quick");
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.Click);
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
                OptionsPanel.Visibility = Visibility.Hidden;
            }
            else if(MenuName == "Options")
            {

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Battle_Click(object sender, RoutedEventArgs e)
        {
            /******************************DATA VALIDATION HERE BEFORE CALLING INITIALIZE******************************/
            SoundManager.playSFX(SoundManager.SoundType.Click);
            if (PlayerVsPlayerOn.Visibility == Visibility.Visible)
                InitializePlayerVsPlayerGame();
            else if (PlayerVsPlayerOff.Visibility == Visibility.Visible)
                InitializePlayerVsComputerGame();
            else
                Console.WriteLine("There is something wrong!");
        }

        private void PlayerVsPlayer_Click(object sender, RoutedEventArgs e)
        {
            PlayerVsPlayerOn.Visibility = Visibility.Visible;
            PlayerVsPlayerOff.Visibility = Visibility.Hidden;
            PlayerVsComputerOn.Visibility = Visibility.Hidden;
            PlayerVsComputerOff.Visibility = Visibility.Visible;
            Player2Name.Visibility = Visibility.Visible;
            Player2NameTextBox.Visibility = Visibility.Visible;
            Player1NameTextBox.Focusable = true;
            Player1NameTextBox.Focus();
            Player2.Content = "Player 2";
            ComputerLevel.Visibility = Visibility.Hidden;
            ComputerHardLevel.Visibility = Visibility.Hidden;
            ComputerEasyLevel.Visibility = Visibility.Hidden;
            GameDifficultyEasyOn.Visibility = Visibility.Hidden;
            GameDifficultyHardOn.Visibility = Visibility.Hidden;
            GameDifficultyEasyOff.Visibility = Visibility.Hidden;
            GameDifficultyHardOff.Visibility = Visibility.Hidden;
            SoundManager.playSFX(SoundManager.SoundType.Click);
        }

        private void PlayerVsComputer_Click(object sender, RoutedEventArgs e)
        {
            PlayerVsPlayerOn.Visibility = Visibility.Hidden;
            PlayerVsPlayerOff.Visibility = Visibility.Visible;
            PlayerVsComputerOn.Visibility = Visibility.Visible;
            PlayerVsComputerOff.Visibility = Visibility.Hidden;
            Player2Name.Visibility = Visibility.Hidden;
            Player2NameTextBox.Visibility = Visibility.Hidden;
            Player1NameTextBox.Focusable = true;
            Player1NameTextBox.Focus();
            Player2.Content = "Computer";
            ComputerLevel.Visibility = Visibility.Visible;
            ComputerHardLevel.Visibility = Visibility.Visible;
            ComputerEasyLevel.Visibility = Visibility.Visible;
            GameDifficultyEasyOn.Visibility = Visibility.Visible;
            GameDifficultyHardOn.Visibility = Visibility.Hidden;
            GameDifficultyEasyOff.Visibility = Visibility.Hidden;
            GameDifficultyHardOff.Visibility = Visibility.Visible;
            SoundManager.playSFX(SoundManager.SoundType.Click);            
        }

        private void Player1_Click(object sender, RoutedEventArgs e)
        {
            Player1NameTextBox.Focusable = true;
            Player1NameTextBox.Focus();
            Player1MoveFirstOn.Visibility = Visibility.Visible;
            Player1MoveFirstOff.Visibility = Visibility.Hidden;
            Player2MoveFirstOn.Visibility = Visibility.Hidden;
            Player2MoveFirstOff.Visibility = Visibility.Visible;
            SoundManager.playSFX(SoundManager.SoundType.Click);
        }

        private void Player2_Click(object sender, RoutedEventArgs e)
        {
            Player1NameTextBox.Focusable = true;
            Player1NameTextBox.Focus();
            Player1MoveFirstOn.Visibility = Visibility.Hidden;
            Player1MoveFirstOff.Visibility = Visibility.Visible;
            Player2MoveFirstOn.Visibility = Visibility.Visible;
            Player2MoveFirstOff.Visibility = Visibility.Hidden;
            SoundManager.playSFX(SoundManager.SoundType.Click);
        }

        private void StoryMode_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }

        private void QuickMatch_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }

        private void OnlineGame_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }

        private void PlayerProfile_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }

        private void Highscores_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }

        private void GameOptionsButton_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }

        private void Quit_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }

        private void PlayerVsPlayer_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }


        private void InitializePlayerVsPlayerGame()
        {
            string player1Name = Player1NameTextBox.Text;

            bool isPlayer1Active;
            if (Player1MoveFirstOn.Visibility == Visibility.Visible)
                isPlayer1Active = true;
            else
                isPlayer1Active = false;

            ImageBrush player1Image = new ImageBrush();
            player1Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/RedPup.png", UriKind.Absolute));
            ImageBrush player1ImageHover = new ImageBrush();
            player1ImageHover.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/RedPupHover.png", UriKind.Absolute));
            Player player1 = new Player(player1Name, isPlayer1Active, player1Image, player1ImageHover);

            string player2Name = Player2NameTextBox.Text;
            ImageBrush player2Image = new ImageBrush();
            player2Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/BluePup.png", UriKind.Absolute));
            ImageBrush player2ImageHover = new ImageBrush();
            player2ImageHover.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/BluePupHover.png", UriKind.Absolute));
            Player player2 = new Player(player2Name, !isPlayer1Active, player2Image, player2ImageHover);

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
            if (Player1MoveFirstOn.Visibility == Visibility.Visible)
                isPlayer1Active = true;
            else
                isPlayer1Active = false;

            ImageBrush player1Image = new ImageBrush();
            player1Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/RedPup.png", UriKind.Absolute));
            ImageBrush player1ImageHover = new ImageBrush();
            player1ImageHover.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/RedPupHover.png", UriKind.Absolute));
            Player player1 = new Player(player1Name, isPlayer1Active, player1Image, player1ImageHover);

            string computerPlayerName = "Computer";
            ImageBrush computerPlayerImage = new ImageBrush();
            computerPlayerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/BluePup.png", UriKind.Absolute));

            ImageBrush computerPlayerImageHover = new ImageBrush();
            computerPlayerImageHover.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/BluePupHover.png", UriKind.Absolute));

            computerAI.Difficulty difficulty;
            if (GameDifficultyEasyOn.Visibility == Visibility.Visible)
                difficulty = computerAI.Difficulty.Easy;
            else
                difficulty = computerAI.Difficulty.Hard;

            computerAI computerPlayer = new computerAI(computerPlayerName, !isPlayer1Active, computerPlayerImage, computerPlayerImageHover, difficulty);
            GameOptions gameOptions = new GameOptions(GameOptions.TypeOfGame.AI, player1, computerPlayer);
            Window gameWindow = new GameWindow(gameOptions);
            App.Current.MainWindow = gameWindow;
            gameWindow.Show();
            this.Hide();
        }

        private void ComputerEasyLevel_Click(object sender, RoutedEventArgs e)
        {
            Player1NameTextBox.Focusable = true;
            Player1NameTextBox.Focus();
            GameDifficultyEasyOn.Visibility = Visibility.Visible;
            GameDifficultyHardOn.Visibility = Visibility.Hidden;
            GameDifficultyEasyOff.Visibility = Visibility.Hidden;
            GameDifficultyHardOff.Visibility = Visibility.Visible;
            SoundManager.playSFX(SoundManager.SoundType.Click);
        }

        private void ComputerHardLevel_Click(object sender, RoutedEventArgs e)
        {
            Player1NameTextBox.Focusable = true;
            Player1NameTextBox.Focus();
            GameDifficultyEasyOn.Visibility = Visibility.Hidden;
            GameDifficultyHardOn.Visibility = Visibility.Visible;
            GameDifficultyEasyOff.Visibility = Visibility.Visible;
            GameDifficultyHardOff.Visibility = Visibility.Hidden;
            SoundManager.playSFX(SoundManager.SoundType.Click);
        }

        private void Player1_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }

        private void Player2_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }

        private void PlayerVsComputer_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }

        private void Battle_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }

        private void ComputerEasyLevel_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }

        private void ComputerHardLevel_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }

        private void Player1NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.KeyDown);
        }

        private void Player2NameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.KeyDown);
        }

        private void GameOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            OptionsPanel.Visibility = Visibility.Visible;
            ReHideMenues("Options");
        }
    }
}
