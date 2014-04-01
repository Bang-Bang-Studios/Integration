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
        ProfileManager profileManager = null;

        public PentagoNetwork networkUtil;

        Window MainMenuWindow;

        public MainMenu()
        {
            InitializeComponent();
            SoundManager.backgroundMusicPlayer.Open(new Uri("GUI/Sounds/Intro.mp3", UriKind.Relative));
            SoundManager.backgroundMusicPlayer.Play();
            //Initialize profile manager
            profileManager = ProfileManager.InstanceCreator();
            MainMenuWindow = this;
        }
        
        private void QuickMatch_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.Click);
            if (QuickMatchMenuScroll.Visibility != Visibility.Visible)
            {
                QuickMatchMenuScroll.Visibility = Visibility.Hidden;
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
            Player1NameTextBox.Focusable = true;
            Player1NameTextBox.Focus();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.Click);
            Application.Current.Shutdown();
        }

        private void ReHideMenues(string MenuName)
        {
            if (MenuName == "Quick")
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
           
                StoryModeMenuScroll.Visibility = Visibility.Visible;
                OptionsPanel.Visibility = Visibility.Hidden;
                StoryModePanel.Visibility = Visibility.Hidden;
                NewProfilePanel.Visibility = Visibility.Hidden;
                HighScorePanel.Visibility = Visibility.Hidden;
            }
            else if (MenuName == "Options")
            {
                QuickMatchMenuScroll.Visibility = Visibility.Hidden;
                StoryModePanel.Visibility = Visibility.Hidden;
                NewProfilePanel.Visibility = Visibility.Hidden;
                HighScorePanel.Visibility = Visibility.Hidden;
            } 
            else if (MenuName == "StoryMode")
            {
                OptionsPanel.Visibility = Visibility.Hidden;
                QuickMatchMenuScroll.Visibility = Visibility.Hidden;
                PlayerVsGroupBox.Visibility = Visibility.Hidden;
                PlayerVsStackPanel.Visibility = Visibility.Hidden;
                PlayerVsPlayer.Visibility = Visibility.Hidden;
                PlayerVsComputer.Visibility = Visibility.Hidden;
                MoveFirst.Visibility = Visibility.Hidden;
                MoveGroupBox.Visibility = Visibility.Hidden;
                MoveStackPanel.Visibility = Visibility.Hidden;
                Player1.Visibility = Visibility.Hidden;
                Player2.Visibility = Visibility.Hidden;
                Player1Name.Visibility = Visibility.Hidden;
                Player1NameTextBox.Visibility = Visibility.Hidden;
                Player2Name.Visibility = Visibility.Hidden;
                Player2NameTextBox.Visibility = Visibility.Hidden;
                Battle.Visibility = Visibility.Hidden;
                PlayerVsPlayerOn.Visibility = Visibility.Hidden;
                PlayerVsComputerOff.Visibility = Visibility.Hidden;
                Player1MoveFirstOn.Visibility = Visibility.Hidden;
                Player2MoveFirstOff.Visibility = Visibility.Hidden;
                StoryModeMenuScroll.Visibility = Visibility.Hidden;
                PlayerVsPlayerOff.Visibility = Visibility.Hidden;
                PlayerVsComputerOn.Visibility = Visibility.Hidden;
                ComputerLevel.Visibility = Visibility.Hidden;
                ComputerHardLevel.Visibility = Visibility.Hidden;
                ComputerEasyLevel.Visibility = Visibility.Hidden;
                GameDifficultyEasyOn.Visibility = Visibility.Hidden;
                GameDifficultyHardOff.Visibility = Visibility.Hidden;
                Player1MoveFirstOn.Visibility = Visibility.Hidden;
                Player1MoveFirstOff.Visibility = Visibility.Hidden;
                Player2MoveFirstOn.Visibility = Visibility.Hidden;
                Player2MoveFirstOff.Visibility = Visibility.Hidden;
                ComputerHardLevel.Visibility = Visibility.Hidden;
                ComputerEasyLevel.Visibility = Visibility.Hidden;
                GameDifficultyEasyOn.Visibility = Visibility.Hidden;
                GameDifficultyHardOn.Visibility = Visibility.Hidden;
                GameDifficultyEasyOff.Visibility = Visibility.Hidden;
                GameDifficultyHardOff.Visibility = Visibility.Hidden;
                HighScorePanel.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Battle_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.Click);
            if (ValidateNames()) {
                if (PlayerVsPlayerOn.Visibility == Visibility.Visible)
                    InitializePlayerVsPlayerGame();
                else if (PlayerVsPlayerOff.Visibility == Visibility.Visible)
                    InitializePlayerVsComputerGame();
                else
                    Console.WriteLine("There is something wrong!");
            } else {
                const string message = "Please, verify names are longer than 1 character and less than 15.";
                const string caption = "Dragon Horde";
                //MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
                MessageWindow messageWindow = new MessageWindow(message, MessageBoxButton.OK);
                messageWindow.ShowDialog();
            }

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
            Player player1 = new Player(player1Name.Trim(), isPlayer1Active, player1Image, player1ImageHover);

            string player2Name = Player2NameTextBox.Text;
            ImageBrush player2Image = new ImageBrush();
            player2Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/BluePup.png", UriKind.Absolute));
            ImageBrush player2ImageHover = new ImageBrush();
            player2ImageHover.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/BluePupHover.png", UriKind.Absolute));
            Player player2 = new Player(player2Name.Trim(), !isPlayer1Active, player2Image, player2ImageHover);

            GameOptions gameOptions = new GameOptions(GameOptions.TypeOfGame.QuickMatch, player1, player2);
            Window gameWindow = new GameWindow(gameOptions);
            App.Current.MainWindow = gameWindow;
            gameWindow.Show();
            this.Hide();

        }

        private void InitializeNetworkGame()
        {
            string player1Name = networkUtil.peerName;

            bool isPlayer1Active = networkUtil.iAmPlayer1;

            ImageBrush player1Image = new ImageBrush();
            player1Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/RedPup.png", UriKind.Absolute));
            ImageBrush player1ImageHover = new ImageBrush();
            player1ImageHover.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/RedPupHover.png", UriKind.Absolute));
            Player player1 = new Player(player1Name.Trim(), isPlayer1Active, player1Image, player1ImageHover);

            string player2Name = networkUtil.clientName;
            ImageBrush player2Image = new ImageBrush();
            player2Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/BluePup.png", UriKind.Absolute));
            ImageBrush player2ImageHover = new ImageBrush();
            player2ImageHover.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/BluePupHover.png", UriKind.Absolute));
            Player player2 = new Player(player2Name.Trim(), !isPlayer1Active, player2Image, player2ImageHover);

            GameOptions gameOptions = new GameOptions(GameOptions.TypeOfGame.Network, player1, player2, networkUtil);
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
            Player player1 = new Player(player1Name.Trim(), isPlayer1Active, player1Image, player1ImageHover);

            string computerPlayerName = "Computer";
            ImageBrush computerPlayerImage = new ImageBrush();
            computerPlayerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/BluePup.png", UriKind.Absolute));

            ImageBrush computerPlayerImageHover = new ImageBrush();
            computerPlayerImageHover.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/BluePupHover.png", UriKind.Absolute));

            computerAI.Difficulty difficulty = computerAI.Difficulty.Hard;
            /*
            if (GameDifficultyEasyOn.Visibility == Visibility.Visible)
                difficulty = computerAI.Difficulty.Easy;
            else
                difficulty = computerAI.Difficulty.Hard;
            */
            computerAI computerPlayer = new computerAI(computerPlayerName.Trim(), !isPlayer1Active, computerPlayerImage, computerPlayerImageHover, difficulty);
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
            if (OptionsPanel.Visibility != Visibility.Visible)
            {
                OptionsPanel.Visibility = Visibility.Visible;
                ReHideMenues("Options");
            }
        }

        private void StoryMode_Click(object sender, RoutedEventArgs e)
        {
            //if it is already visible, dont show it again!
            if (StoryModePanel.Visibility != Visibility.Visible)
            {
                StoryModePanel.Visibility = Visibility.Visible;
                NewProfilePanel.Visibility = Visibility.Hidden;
                LoadProfilesList();
                ProfileList.Visibility = Visibility.Visible;
                ContineAdventure.Visibility = Visibility.Visible;
                ReHideMenues("StoryMode");
            }
        }

        private void LoadProfilesList()
        {
            ProfileList.Items.Clear();
            List<ProfileManager.Profile> profiles = profileManager.GetAllProfiles();
            foreach (ProfileManager.Profile profile in profiles)
            {
                string name = profile.ProfileName;
                ProfileList.Items.Add(name);
            }
        }

        private void NewProfile_Click(object sender, RoutedEventArgs e)
        {
            if (NewProfilePanel.Visibility != Visibility.Visible)
            {
                ProfileList.Visibility = Visibility.Hidden;
                ContineAdventure.Visibility = Visibility.Hidden;
                NewProfilePanel.Visibility = Visibility.Visible;
                NewProfileName.Clear();
            }
            NewProfileName.Focusable = true;
            NewProfileName.Focus();
        }

        private void ExistingProfile_Click(object sender, RoutedEventArgs e)
        {

            NewProfilePanel.Visibility = Visibility.Hidden;
            ProfileList.Visibility = Visibility.Visible;
            ContineAdventure.Visibility = Visibility.Visible;
            LoadProfilesList();
            
        }

        private void QuitToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            OnlineMenuPanel.Visibility = Visibility.Hidden;
        }

        private void OnlineGame_Click(object sender, RoutedEventArgs e)
        {
            OnlineMenuPanel.Visibility = Visibility.Visible;
        }
        
        private void FindOpponent_Click(object sender, RoutedEventArgs e)
        {
            OpponentsTag.Visibility = Visibility.Visible;
            if (networkUtil == null)
            {
                networkUtil = new PentagoNetwork(NameBox.Text);
            }
            else
            {
                networkUtil.Discovered -= new peerDiscoveredHandler(PeerDiscovered);
                networkUtil.ConnectionRequest -= new peerConnectionRequestHandler(ConnectionRequest);
                networkUtil.Connected -= new peerConnectedHandler(PeerConnected);
                networkUtil.Disconnected -= new peerDisconnectedHancler(PeerDisconnected);
                networkUtil.stop();
                networkUtil = new PentagoNetwork(NameBox.Text);
            }
            networkUtil.Discovered += new peerDiscoveredHandler(PeerDiscovered);
            networkUtil.ConnectionRequest += new peerConnectionRequestHandler(ConnectionRequest);
            networkUtil.Connected += new peerConnectedHandler(PeerConnected);
            networkUtil.Disconnected += new peerDisconnectedHancler(PeerDisconnected);
        }

        private void PeerConnected(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate() { InitializeNetworkGame(); }), null);
        }

        private void PeerDisconnected(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate() {
                MessageWindow message = new MessageWindow("Disconnected");
                message.ShowDialog();
                Window mainWindow = new MainMenu();
                App.Current.MainWindow = mainWindow;
                mainWindow.Show();
                this.Hide();
                networkUtil.stop();
            }));
        }

        private void PeerDiscovered(object msg, EventArgs e)
        {
            AvailableLobbies.Dispatcher.BeginInvoke(new Action(delegate() { UpdateLobbyList(); }), null);            
        }

        private void UpdateLobbyList()
        {
            AvailableLobbies.Items.Clear();
            foreach (PentagoNetwork.peerType p in networkUtil.availablePeers)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = p.name;
                AvailableLobbies.Items.Add(item);
            }
        }

        private void ConnectionRequest(object msg, EventArgs e)
        {
            string requesterName = (string)msg;
            AcceptGameText.Dispatcher.BeginInvoke(new Action(delegate() { IncomingRequest(requesterName); }), null); 
        }

        private void IncomingRequest(string requesterName)
        {
            if (OnlineMenuPanel.Visibility == Visibility.Visible)
            {
                AcceptGameText.Text = "You have been challenged by " + requesterName + ". Do you accept?";
                AcceptGameText.Visibility = Visibility.Visible;
                AcceptChallenge.Visibility = Visibility.Visible;
                DenyChallenge.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AvailableLobbies.Items.Clear();
            foreach (PentagoNetwork.peerType p in networkUtil.availablePeers)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = p.name;
                AvailableLobbies.Items.Add(item);
            }
        }

        

        private void ChallengeOpponent_Click(object sender, RoutedEventArgs e)
        {
            networkUtil.ConnectUsingIndex(AvailableLobbies.SelectedIndex);
        }

        private void ChallengeAccept_Click(object sender, RoutedEventArgs e)
        {
            networkUtil.AcceptConnection();
        }

        private void ChallengeDecline_Click(object sender, RoutedEventArgs e)
        {
            AcceptGameText.Visibility = Visibility.Hidden;
            AcceptChallenge.Visibility = Visibility.Hidden;
            DenyChallenge.Visibility = Visibility.Hidden;
        }

        private void CreateNewProfile_Click(object sender, RoutedEventArgs e)
        {
            string newProfileName = NewProfileName.Text;
            if (IsProfileNameValid(newProfileName.Trim()))
            {
                if (profileManager.IsProfileValid(newProfileName.Trim()))
                {
                    //append to file 
                    profileManager.CreateNewProfile(newProfileName.Trim());
                    ExistingProfile_Click(sender, e);
                }
                else
                {
                    const string message = "This profile name already exists, please create a new one.";
                    const string caption = "Dragon Horde";
                    //MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
                    MessageWindow messageWindow = new MessageWindow(message, MessageBoxButton.OK);
                    messageWindow.ShowDialog();
                }
            }
        }

        private bool IsProfileNameValid(string profileName)
        {
            if (profileName.Trim() == "" || profileName.Trim().Length < 1 || profileName.Trim().Length > 15)
                return false;
            return true;
        }

        private bool ValidateNames()
        {
            string player1Name = Player1NameTextBox.Text;
            string player2Name = Player2NameTextBox.Text;

            if (player1Name.Trim() == "" || player1Name.Trim().Length < 1 || player1Name.Trim().Length > 15)
                return false;

            if (PlayerVsPlayerOn.Visibility == Visibility.Visible)
                if (player2Name.Trim() == "" || player2Name.Trim().Length < 1 || player2Name.Trim().Length > 15)
                    return false;

            return true;
        }

        private bool ValidateName()
        {
            string onlineName = NameBox.Text;
            if (onlineName.Trim() == "" || onlineName.Trim().Length < 1 || onlineName.Trim().Length > 15)
                return false;

            return true;
        }

        private void Highscores_Click(object sender, RoutedEventArgs e)
        {
            HighScorePanel.Visibility = Visibility.Visible;
        }

    }
}
