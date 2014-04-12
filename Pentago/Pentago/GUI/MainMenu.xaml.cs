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
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;


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


        private System.Windows.Point vikingArmPivot;
        private System.Windows.Point giantArmPivot;
        private System.Windows.Point zero;
        private System.Windows.Point topRight;


        public int unMuteMusicVol = 6;
        public int currentMusicVol = 6;
        public int unMuteSoundVol = 6;
        public int currentSoundVol = 6;

        private string currentPant = "Base";
        private string currentArmor = "Base";
        private string currentBeard = "Base";
        private string currentPant1 = "Base";
        private string currentArmor1 = "Base";
        private string currentBeard1 = "Base";

        public MainMenu()
        {
            InitializeComponent();
            SoundManager.sfxVolume = Properties.Settings.Default.SFXVolume;
            SoundManager.musicVolume = Properties.Settings.Default.MusicVolume;
            unMuteMusicVol = SoundManager.musicVolume / 16;
            unMuteSoundVol = SoundManager.sfxVolume / 16;
            currentMusicVol = SoundManager.musicVolume / 16;
            currentSoundVol = SoundManager.sfxVolume / 16;
            SoundManager.backgroundMusicPlayer.Open(new Uri("GUI/Sounds/Intro.mp3", UriKind.Relative));
            SoundManager.backgroundMusicPlayer.Play();
            //Initialize profile manager
            profileManager = ProfileManager.InstanceCreator();
            //MainMenuWindow = this;


            vikingArmPivot = new System.Windows.Point(167 + 40, this.Height - 420 + 121);
            zero = new System.Windows.Point(0, 0);
            topRight = new System.Windows.Point(Width, 0);

            Stream cur = File.OpenRead("GUI/images/MouseArrow.cur");
            this.Cursor = new Cursor(cur);
        }
        
        private void QuickMatch_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.Click);
            if (QuickMatchMenuScroll.Visibility != Visibility.Visible)
            {
                ReHideMenues();
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

                
            }
            Player1NameTextBox.Focusable = true;
            Player1NameTextBox.Focus();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.Click);
            Application.Current.Shutdown();
        }

        private void ReHideMenues()
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
            GameDifficultyEasyOn.Visibility = Visibility.Hidden;
            GameDifficultyHardOff.Visibility = Visibility.Hidden;
            Player1MoveFirstOn.Visibility = Visibility.Hidden;
            Player1MoveFirstOff.Visibility = Visibility.Hidden;
            Player2MoveFirstOn.Visibility = Visibility.Hidden;
            Player2MoveFirstOff.Visibility = Visibility.Hidden;
            GameDifficultyEasyOn.Visibility = Visibility.Hidden;
            GameDifficultyHardOn.Visibility = Visibility.Hidden;
            GameDifficultyEasyOff.Visibility = Visibility.Hidden;
            GameDifficultyHardOff.Visibility = Visibility.Hidden;
            HighScorePanel.Visibility = Visibility.Hidden;
            StoryModePanel.Visibility = Visibility.Hidden;
            NewProfilePanel.Visibility = Visibility.Hidden;
            PlayerProfilePanel.Visibility = Visibility.Hidden;
            GameDifficultyBeginnerOff.Visibility = Visibility.Hidden;
            GameDifficultyBeginnerOn.Visibility = Visibility.Hidden;
            GameDifficultyEasyOff.Visibility = Visibility.Hidden;
            GameDifficultyEasyOn.Visibility = Visibility.Hidden;
            GameDifficultyMediumOff.Visibility = Visibility.Hidden;
            GameDifficultyMediumOn.Visibility = Visibility.Hidden;
            GameDifficultyHardOff.Visibility = Visibility.Hidden;
            GameDifficultyHardOn.Visibility = Visibility.Hidden;
            GameDifficultyExpertOff.Visibility = Visibility.Hidden;
            GameDifficultyExpertOn.Visibility = Visibility.Hidden;
            DifficultyIndicator.Visibility = Visibility.Hidden;
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
                const string message = "Please, verify names are alphanumeric, longer than 1 character and less than 15.";
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
            GameDifficultyBeginnerOff.Visibility = Visibility.Hidden;
            GameDifficultyBeginnerOn.Visibility = Visibility.Hidden;
            GameDifficultyEasyOff.Visibility = Visibility.Hidden;
            GameDifficultyEasyOn.Visibility = Visibility.Hidden;
            GameDifficultyMediumOff.Visibility = Visibility.Hidden;
            GameDifficultyMediumOn.Visibility = Visibility.Hidden;
            GameDifficultyHardOff.Visibility = Visibility.Hidden;
            GameDifficultyHardOn.Visibility = Visibility.Hidden;
            GameDifficultyExpertOff.Visibility = Visibility.Hidden;
            GameDifficultyExpertOn.Visibility = Visibility.Hidden;
            DifficultyIndicator.Visibility = Visibility.Hidden;
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
            GameDifficultyBeginnerOff.Visibility = Visibility.Visible;
            GameDifficultyBeginnerOn.Visibility = Visibility.Hidden;
            GameDifficultyEasyOff.Visibility = Visibility.Visible;
            GameDifficultyEasyOn.Visibility = Visibility.Hidden;
            GameDifficultyMediumOff.Visibility = Visibility.Hidden;
            GameDifficultyMediumOn.Visibility = Visibility.Visible;
            GameDifficultyHardOff.Visibility = Visibility.Visible;
            GameDifficultyHardOn.Visibility = Visibility.Hidden;
            GameDifficultyExpertOff.Visibility = Visibility.Visible;
            GameDifficultyExpertOn.Visibility = Visibility.Hidden;
            DifficultyIndicator.Visibility = Visibility.Visible;
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

            //Window loadingWindow = new LoadingScreen(gameOptions);
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

            computerAI.Difficulty difficulty;
            if (GameDifficultyBeginnerOn.Visibility == Visibility.Visible)
                difficulty = computerAI.Difficulty.Beginner;
            else if (GameDifficultyEasyOn.Visibility == Visibility.Visible)
                difficulty = computerAI.Difficulty.Easy;
            else if (GameDifficultyMediumOn.Visibility == Visibility.Visible)
                difficulty = computerAI.Difficulty.Medium;
            else if (GameDifficultyHardOn.Visibility == Visibility.Visible)
                difficulty = computerAI.Difficulty.Medium;
            else
                difficulty = computerAI.Difficulty.Hard;

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
                ReHideMenues();
                OptionsPanel.Visibility = Visibility.Visible;
                
            }
        }

        private void StoryMode_Click(object sender, RoutedEventArgs e)
        {
            //if it is already visible, dont show it again!
            if (StoryModePanel.Visibility != Visibility.Visible)
            {
                ReHideMenues();
                StoryModePanel.Visibility = Visibility.Visible;
                NewProfilePanel.Visibility = Visibility.Hidden;
                ProfileList.Visibility = Visibility.Visible;
                ContineAdventure.Visibility = Visibility.Visible;
                LoadProfilesList();
                try
                {
                    ProfileList.SelectedIndex = 0;
                }
                catch { } 
            }
        }

        private void LoadProfilesList()
        {
            ProfileList.Items.Clear();
            List<ProfileManager.Profile> profiles = profileManager.GetAllProfiles();
            foreach (ProfileManager.Profile profile in profiles)
                ProfileList.Items.Add(profile.ProfileName);
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
            try
            {
                ProfileList.SelectedIndex = 0;
            }
            catch { } 
            
        }

        private void QuitToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            OnlineMenuPanel.Visibility = Visibility.Hidden;
        }

        private void OnlineGame_Click(object sender, RoutedEventArgs e)
        {
            ReHideMenues();
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
                networkUtil.Discovered -= new peerDiscoveredHandler(PeerListChanged);
                networkUtil.ConnectionRequest -= new peerConnectionRequestHandler(ConnectionRequest);
                networkUtil.Connected -= new peerConnectedHandler(PeerConnected);
                networkUtil.Disconnected -= new peerDisconnectedHancler(PeerDisconnected);
                networkUtil.PlayerRemoved -= new playerRemovedHandler(PeerListChanged);
                networkUtil.stop();
                networkUtil = new PentagoNetwork(NameBox.Text);
            }
            networkUtil.Discovered += new peerDiscoveredHandler(PeerListChanged);
            networkUtil.ConnectionRequest += new peerConnectionRequestHandler(ConnectionRequest);
            networkUtil.Connected += new peerConnectedHandler(PeerConnected);
            networkUtil.Disconnected += new peerDisconnectedHancler(PeerDisconnected);
            networkUtil.PlayerRemoved += new playerRemovedHandler(PeerListChanged);
            Searching_for_Opponents.Visibility = Visibility.Visible;
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

        private void PeerListChanged(object msg, EventArgs e)
        {
            AvailableLobbies.Dispatcher.BeginInvoke(new Action(delegate() { UpdateLobbyList(); }), null);            
        }

        private void UpdateLobbyList()
        {
            AvailableLobbies.Items.Clear();
            for (int i = 0; i < networkUtil.availablePeers.Count; i++)
            {
                PentagoNetwork.peerType p = networkUtil.availablePeers[i];
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
            
        }

        private bool IsProfileNameValid(string profileName)
        {
            profileName = profileName.Trim();
            Regex regex = new Regex("^[a-zA-Z0-9 ]*$");
            if (profileName == "" || profileName.Length < 1 ||
                profileName.Length > 15 || !regex.IsMatch(profileName))
                return false;
            return true;
        }

        private bool ValidateNames()
        {
            string player1Name = Player1NameTextBox.Text;
            string player2Name = Player2NameTextBox.Text;
            player1Name = player1Name.Trim();
            player2Name = player2Name.Trim();

            Regex regex = new Regex("^[a-zA-Z0-9 ]*$");

            if (player1Name == "" || player1Name.Length < 1 || 
                player1Name.Length > 15 || !regex.IsMatch(player1Name))
                return false;

            if (PlayerVsPlayerOn.Visibility == Visibility.Visible)
                if (player2Name == "" || player2Name.Length < 1 || 
                    player2Name.Length > 15 || !regex.IsMatch(player2Name))
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
            ReHideMenues();
            HighScorePanel.Visibility = Visibility.Visible;
        }

        private void Menu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != OnlineMenuPanel)
            {
                OnlineMenuPanel.Visibility = Visibility.Hidden;
            }
        }

        public void MusicToggle_Click(object sender, RoutedEventArgs e)
        {
            if (currentMusicVol == 0)
            {
                restoreMusicVol(unMuteMusicVol);
                currentMusicVol = unMuteMusicVol;
            }

            else if (currentMusicVol > 0)
            {
                MusicOn1.Visibility = Visibility.Hidden;
                MusicOn2.Visibility = Visibility.Hidden;
                MusicOn3.Visibility = Visibility.Hidden;
                MusicOn4.Visibility = Visibility.Hidden;
                MusicOn5.Visibility = Visibility.Hidden;
                MusicOn6.Visibility = Visibility.Hidden;
                MusicOff1.Visibility = Visibility.Visible;
                MusicOff2.Visibility = Visibility.Visible;
                MusicOff3.Visibility = Visibility.Visible;
                MusicOff4.Visibility = Visibility.Visible;
                MusicOff5.Visibility = Visibility.Visible;
                MusicOff6.Visibility = Visibility.Visible;
                currentMusicVol = 0;
                SoundManager.musicVolume = 0;
            }
        }

        public void SoundToggle_Click(object sender, RoutedEventArgs e)
        {
            if (currentSoundVol == 0)
            {
                restoreSoundVol(unMuteSoundVol);
                currentSoundVol = unMuteSoundVol;
            }

            else if (currentSoundVol > 0)
            {
                SoundOn1.Visibility = Visibility.Hidden;
                SoundOn2.Visibility = Visibility.Hidden;
                SoundOn3.Visibility = Visibility.Hidden;
                SoundOn4.Visibility = Visibility.Hidden;
                SoundOn5.Visibility = Visibility.Hidden;
                SoundOn6.Visibility = Visibility.Hidden;
                SoundOff1.Visibility = Visibility.Visible;
                SoundOff2.Visibility = Visibility.Visible;
                SoundOff3.Visibility = Visibility.Visible;
                SoundOff4.Visibility = Visibility.Visible;
                SoundOff5.Visibility = Visibility.Visible;
                SoundOff6.Visibility = Visibility.Visible;
                currentSoundVol = 0;
                SoundManager.sfxVolume = 0;
            }
        }
        private void restoreMusicVol(int i)
        {
            switch(i)
            {
                case 1: MusicOn1.Visibility = Visibility.Visible;
                     MusicOn2.Visibility = Visibility.Hidden;
                    MusicOn3.Visibility = Visibility.Hidden;
                    MusicOn4.Visibility = Visibility.Hidden;
                    MusicOn5.Visibility = Visibility.Hidden;
                    MusicOn6.Visibility = Visibility.Hidden;
                    MusicOff1.Visibility = Visibility.Hidden;
                    MusicOff2.Visibility = Visibility.Visible;
                    MusicOff3.Visibility = Visibility.Visible;
                    MusicOff4.Visibility = Visibility.Visible;
                    MusicOff5.Visibility = Visibility.Visible;
                    MusicOff6.Visibility = Visibility.Visible;
                    break;
                case 2: MusicOn1.Visibility = Visibility.Visible;
                    MusicOn2.Visibility = Visibility.Visible;
                    MusicOn3.Visibility = Visibility.Hidden;
                    MusicOn4.Visibility = Visibility.Hidden;
                    MusicOn5.Visibility = Visibility.Hidden;
                    MusicOn6.Visibility = Visibility.Hidden;
                    MusicOff1.Visibility = Visibility.Hidden;
                    MusicOff2.Visibility = Visibility.Hidden;
                    MusicOff3.Visibility = Visibility.Visible;
                    MusicOff4.Visibility = Visibility.Visible;
                    MusicOff5.Visibility = Visibility.Visible;
                    MusicOff6.Visibility = Visibility.Visible;
                    break;
                case 3: MusicOn1.Visibility = Visibility.Visible;
                    MusicOn2.Visibility = Visibility.Visible;
                    MusicOn3.Visibility = Visibility.Visible;
                    MusicOn4.Visibility = Visibility.Hidden;
                    MusicOn5.Visibility = Visibility.Hidden;
                    MusicOn6.Visibility = Visibility.Hidden;
                    MusicOff1.Visibility = Visibility.Hidden;
                    MusicOff2.Visibility = Visibility.Hidden;
                    MusicOff3.Visibility = Visibility.Hidden;
                    MusicOff4.Visibility = Visibility.Visible;
                    MusicOff5.Visibility = Visibility.Visible;
                    MusicOff6.Visibility = Visibility.Visible;
                    break;
                case 4: MusicOn1.Visibility = Visibility.Visible;
                    MusicOn2.Visibility = Visibility.Visible;
                    MusicOn3.Visibility = Visibility.Visible;
                    MusicOn4.Visibility = Visibility.Visible;
                    MusicOn5.Visibility = Visibility.Hidden;
                    MusicOn6.Visibility = Visibility.Hidden;
                    MusicOff1.Visibility = Visibility.Hidden;
                    MusicOff2.Visibility = Visibility.Hidden;
                    MusicOff3.Visibility = Visibility.Hidden;
                    MusicOff4.Visibility = Visibility.Hidden;
                    MusicOff5.Visibility = Visibility.Visible;
                    MusicOff6.Visibility = Visibility.Visible;
                    break;
                case 5: MusicOn1.Visibility = Visibility.Visible;
                    MusicOn2.Visibility = Visibility.Visible;
                    MusicOn3.Visibility = Visibility.Visible;
                    MusicOn4.Visibility = Visibility.Visible;
                    MusicOn5.Visibility = Visibility.Visible;
                    MusicOn6.Visibility = Visibility.Hidden;
                    MusicOff1.Visibility = Visibility.Hidden;
                    MusicOff2.Visibility = Visibility.Hidden;
                    MusicOff3.Visibility = Visibility.Hidden;
                    MusicOff4.Visibility = Visibility.Hidden;
                    MusicOff5.Visibility = Visibility.Hidden;
                    MusicOff6.Visibility = Visibility.Visible;
                    break;
                case 6: MusicOn1.Visibility = Visibility.Visible;
                    MusicOn2.Visibility = Visibility.Visible;
                    MusicOn3.Visibility = Visibility.Visible;
                    MusicOn4.Visibility = Visibility.Visible;
                    MusicOn5.Visibility = Visibility.Visible;
                    MusicOn6.Visibility = Visibility.Visible;
                    MusicOff1.Visibility = Visibility.Hidden;
                    MusicOff2.Visibility = Visibility.Hidden;
                    MusicOff3.Visibility = Visibility.Hidden;
                    MusicOff4.Visibility = Visibility.Hidden;
                    MusicOff5.Visibility = Visibility.Hidden;
                    MusicOff6.Visibility = Visibility.Hidden;
                    break;
            }
            SoundManager.musicVolume = 16 * i;
        }

        private void restoreSoundVol(int i)
        {
            switch (i)
            {
                case 1: SoundOn1.Visibility = Visibility.Visible;
                    SoundOn2.Visibility = Visibility.Hidden;
                    SoundOn3.Visibility = Visibility.Hidden;
                    SoundOn4.Visibility = Visibility.Hidden;
                    SoundOn5.Visibility = Visibility.Hidden;
                    SoundOn6.Visibility = Visibility.Hidden;
                    SoundOff1.Visibility = Visibility.Hidden;
                    SoundOff2.Visibility = Visibility.Visible;
                    SoundOff3.Visibility = Visibility.Visible;
                    SoundOff4.Visibility = Visibility.Visible;
                    SoundOff5.Visibility = Visibility.Visible;
                    SoundOff6.Visibility = Visibility.Visible;
                    break;
                case 2: SoundOn1.Visibility = Visibility.Visible;
                    SoundOn2.Visibility = Visibility.Visible;
                    SoundOn3.Visibility = Visibility.Hidden;
                    SoundOn4.Visibility = Visibility.Hidden;
                    SoundOn5.Visibility = Visibility.Hidden;
                    SoundOn6.Visibility = Visibility.Hidden;
                    SoundOff1.Visibility = Visibility.Hidden;
                    SoundOff2.Visibility = Visibility.Hidden;
                    SoundOff3.Visibility = Visibility.Visible;
                    SoundOff4.Visibility = Visibility.Visible;
                    SoundOff5.Visibility = Visibility.Visible;
                    SoundOff6.Visibility = Visibility.Visible;
                    break;
                case 3: SoundOn1.Visibility = Visibility.Visible;
                    SoundOn2.Visibility = Visibility.Visible;
                    SoundOn3.Visibility = Visibility.Visible;
                    SoundOn4.Visibility = Visibility.Hidden;
                    SoundOn5.Visibility = Visibility.Hidden;
                    SoundOn6.Visibility = Visibility.Hidden;
                    SoundOff1.Visibility = Visibility.Hidden;
                    SoundOff2.Visibility = Visibility.Hidden;
                    SoundOff3.Visibility = Visibility.Hidden;
                    SoundOff4.Visibility = Visibility.Visible;
                    SoundOff5.Visibility = Visibility.Visible;
                    SoundOff6.Visibility = Visibility.Visible;
                    break;
                case 4: SoundOn1.Visibility = Visibility.Visible;
                    SoundOn2.Visibility = Visibility.Visible;
                    SoundOn3.Visibility = Visibility.Visible;
                    SoundOn4.Visibility = Visibility.Visible;
                    SoundOn5.Visibility = Visibility.Hidden;
                    SoundOn6.Visibility = Visibility.Hidden;
                    SoundOff1.Visibility = Visibility.Hidden;
                    SoundOff2.Visibility = Visibility.Hidden;
                    SoundOff3.Visibility = Visibility.Hidden;
                    SoundOff4.Visibility = Visibility.Hidden;
                    SoundOff5.Visibility = Visibility.Visible;
                    SoundOff6.Visibility = Visibility.Visible;
                    break;
                case 5: SoundOn1.Visibility = Visibility.Visible;
                    SoundOn2.Visibility = Visibility.Visible;
                    SoundOn3.Visibility = Visibility.Visible;
                    SoundOn4.Visibility = Visibility.Visible;
                    SoundOn5.Visibility = Visibility.Visible;
                    SoundOn6.Visibility = Visibility.Hidden;
                    SoundOff1.Visibility = Visibility.Hidden;
                    SoundOff2.Visibility = Visibility.Hidden;
                    SoundOff3.Visibility = Visibility.Hidden;
                    SoundOff4.Visibility = Visibility.Hidden;
                    SoundOff5.Visibility = Visibility.Hidden;
                    SoundOff6.Visibility = Visibility.Visible;
                    break;
                case 6: SoundOn1.Visibility = Visibility.Visible;
                    SoundOn2.Visibility = Visibility.Visible;
                    SoundOn3.Visibility = Visibility.Visible;
                    SoundOn4.Visibility = Visibility.Visible;
                    SoundOn5.Visibility = Visibility.Visible;
                    SoundOn6.Visibility = Visibility.Visible;
                    SoundOff1.Visibility = Visibility.Hidden;
                    SoundOff2.Visibility = Visibility.Hidden;
                    SoundOff3.Visibility = Visibility.Hidden;
                    SoundOff4.Visibility = Visibility.Hidden;
                    SoundOff5.Visibility = Visibility.Hidden;
                    SoundOff6.Visibility = Visibility.Hidden;
                    break;
            }
            SoundManager.sfxVolume = 16 * i;
        }

        private void MusicOff1_Click(object sender, RoutedEventArgs e)
        {
            currentMusicVol = 1;
            unMuteMusicVol = 1;
            restoreMusicVol(currentMusicVol);
        }

        private void MusicOff2_Click(object sender, RoutedEventArgs e)
        {
            currentMusicVol = 2;
            unMuteMusicVol = 2;
            restoreMusicVol(currentMusicVol);
        }

        private void MusicOff3_Click(object sender, RoutedEventArgs e)
        {
            currentMusicVol = 3;
            unMuteMusicVol = 3;
            restoreMusicVol(currentMusicVol);
        }

        private void MusicOff4_Click(object sender, RoutedEventArgs e)
        {
            currentMusicVol = 4;
            unMuteMusicVol = 4;
            restoreMusicVol(currentMusicVol);
        }

        private void MusicOff5_Click(object sender, RoutedEventArgs e)
        {
            currentMusicVol = 5;
            unMuteMusicVol = 5;
            restoreMusicVol(currentMusicVol);
        }

        private void MusicOff6_Click(object sender, RoutedEventArgs e)
        {
            currentMusicVol = 6;
            unMuteMusicVol = 6;
            restoreMusicVol(currentMusicVol);
        }

        private void MusicOn1_Click(object sender, RoutedEventArgs e)
        {
            currentMusicVol = 1;
            unMuteMusicVol = 1;
            restoreMusicVol(currentMusicVol);
        }

        private void MusicOn2_Click(object sender, RoutedEventArgs e)
        {
            currentMusicVol = 2;
            unMuteMusicVol = 2;
            restoreMusicVol(currentMusicVol);
        }

        private void MusicOn3_Click(object sender, RoutedEventArgs e)
        {
            currentMusicVol = 3;
            unMuteMusicVol = 3;
            restoreMusicVol(currentMusicVol);
        }

        private void MusicOn4_Click(object sender, RoutedEventArgs e)
        {
            currentMusicVol = 4;
            unMuteMusicVol = 4;
            restoreMusicVol(currentMusicVol);
        }

        private void MusicOn5_Click(object sender, RoutedEventArgs e)
        {
            currentMusicVol = 5;
            unMuteMusicVol = 5;
            restoreMusicVol(currentMusicVol);
        }

        private void MusicOn6_Click(object sender, RoutedEventArgs e)
        {
            currentMusicVol = 6;
            unMuteMusicVol = 6;
            restoreMusicVol(currentMusicVol);
        }

        private void SoundOff1_Click(object sender, RoutedEventArgs e)
        {
            currentSoundVol = 1;
            unMuteSoundVol = 1;
            restoreSoundVol(currentSoundVol);
        }

        private void SoundOff2_Click(object sender, RoutedEventArgs e)
        {
            currentSoundVol = 2;
            unMuteSoundVol = 2;
            restoreSoundVol(currentSoundVol);
        }

        private void SoundOff3_Click(object sender, RoutedEventArgs e)
        {
            currentSoundVol = 3;
            unMuteSoundVol = 3;
            restoreSoundVol(currentSoundVol);
        }

        private void SoundOff4_Click(object sender, RoutedEventArgs e)
        {
            currentSoundVol = 4;
            unMuteSoundVol = 4;
            restoreSoundVol(currentSoundVol);
        }

        private void SoundOff5_Click(object sender, RoutedEventArgs e)
        {
            currentSoundVol = 5;
            unMuteSoundVol = 5;
            restoreSoundVol(currentSoundVol);
        }

        private void SoundOff6_Click(object sender, RoutedEventArgs e)
        {
            currentSoundVol = 6;
            unMuteSoundVol = 6;
            restoreSoundVol(currentSoundVol);
        }

        private void SoundOn1_Click(object sender, RoutedEventArgs e)
        {
            currentSoundVol = 1;
            unMuteSoundVol = 1;
            restoreSoundVol(currentSoundVol);
        }

        private void SoundOn2_Click(object sender, RoutedEventArgs e)
        {
            currentSoundVol = 2;
            unMuteSoundVol = 2;
            restoreSoundVol(currentSoundVol);
        }

        private void SoundOn3_Click(object sender, RoutedEventArgs e)
        {
            currentSoundVol = 3;
            unMuteSoundVol = 3;
            restoreSoundVol(currentSoundVol);
        }

        private void SoundOn4_Click(object sender, RoutedEventArgs e)
        {
            currentSoundVol = 4;
            unMuteSoundVol = 4;
            restoreSoundVol(currentSoundVol);
        }

        private void SoundOn5_Click(object sender, RoutedEventArgs e)
        {
            currentSoundVol = 5;
            unMuteSoundVol = 5;
            restoreSoundVol(currentSoundVol);
        }

        private void SoundOn6_Click(object sender, RoutedEventArgs e)
        {
            currentSoundVol = 6;
            unMuteSoundVol = 6;
            restoreSoundVol(currentSoundVol);
        }

        int BeardCount = 1;
        int ArmorCount = 1;
        int PantsCount = 1;

        private void ArmorCycleRight_Click(object sender, RoutedEventArgs e)
        {
            ArmorCount++;
            if (ArmorCount == 6)
            {
                ArmorCount = 1;
            }
            ArmorChange(ArmorCount);
        }

        private void ArmorCycleLeft_Click(object sender, RoutedEventArgs e)
        {
            ArmorCount--;
            if (ArmorCount == 0)
            {
                ArmorCount = 5;
            }
            ArmorChange(ArmorCount);
        }

        private void ArmorChange(int i)
        {
            switch (i)
            {
                case 1: RedArmor.Visibility = Visibility.Hidden;
                    BlackArmor.Visibility = Visibility.Hidden;
                    SilverArmor.Visibility = Visibility.Hidden;
                    GoldArmor.Visibility = Visibility.Hidden;
                    currentArmor = "Base";
                    break;
                case 2: RedArmor.Visibility = Visibility.Visible;
                    BlackArmor.Visibility = Visibility.Hidden;
                    SilverArmor.Visibility = Visibility.Hidden;
                    GoldArmor.Visibility = Visibility.Hidden;
                    currentArmor = @"Red.png";
                    break;
                case 3: RedArmor.Visibility = Visibility.Hidden;
                    BlackArmor.Visibility = Visibility.Visible;
                    SilverArmor.Visibility = Visibility.Hidden;
                    GoldArmor.Visibility = Visibility.Hidden;
                    currentArmor = @"Black.png";
                    break;
                case 4: RedArmor.Visibility = Visibility.Hidden;
                    BlackArmor.Visibility = Visibility.Hidden;
                    SilverArmor.Visibility = Visibility.Visible;
                    GoldArmor.Visibility = Visibility.Hidden;
                    currentArmor = @"Silver.png";
                    break;
                case 5: RedArmor.Visibility = Visibility.Hidden;
                    BlackArmor.Visibility = Visibility.Hidden;
                    SilverArmor.Visibility = Visibility.Hidden;
                    GoldArmor.Visibility = Visibility.Visible;
                    currentArmor = @"Gold.png";
                    break;
            }
        }

        private void BeardChange(int i)
        {
            switch (i)
            {
                case 1: RedBeard.Visibility = Visibility.Hidden;
                    BlackBeard.Visibility = Visibility.Hidden;
                    GrayBeard.Visibility = Visibility.Hidden;
                    BrownBeard.Visibility = Visibility.Hidden;
                    currentBeard = "Base";
                    break;
                case 2: RedBeard.Visibility = Visibility.Visible;
                    BlackBeard.Visibility = Visibility.Hidden;
                    GrayBeard.Visibility = Visibility.Hidden;
                    BrownBeard.Visibility = Visibility.Hidden;
                    currentBeard = @"RedBeard.png";
                    break;
                case 3: RedBeard.Visibility = Visibility.Hidden;
                    BlackBeard.Visibility = Visibility.Visible;
                    GrayBeard.Visibility = Visibility.Hidden;
                    BrownBeard.Visibility = Visibility.Hidden;
                    currentBeard = @"BlackBeard.png";
                    break;
                case 4: RedBeard.Visibility = Visibility.Hidden;
                    BlackBeard.Visibility = Visibility.Hidden;
                    GrayBeard.Visibility = Visibility.Visible;
                    BrownBeard.Visibility = Visibility.Hidden;
                    currentBeard = @"Gray.png";
                    break;
                case 5: RedBeard.Visibility = Visibility.Hidden;
                    BlackBeard.Visibility = Visibility.Hidden;
                    GrayBeard.Visibility = Visibility.Hidden;
                    BrownBeard.Visibility = Visibility.Visible;
                    currentBeard = @"Brown.png";
                    break;
            }
        }

        private void PantsChange(int i)
        {
            switch (i)
            {
                case 1: RedPants.Visibility = Visibility.Hidden;
                    PurplePants.Visibility = Visibility.Hidden;
                    PinkPants.Visibility = Visibility.Hidden;
                    GrayPants.Visibility = Visibility.Hidden;
                    GoldPants.Visibility = Visibility.Hidden;
                    BrownPants.Visibility = Visibility.Hidden;
                    currentPant = "Base";
                    break;
                case 2: RedPants.Visibility = Visibility.Visible;
                    PurplePants.Visibility = Visibility.Hidden;
                    PinkPants.Visibility = Visibility.Hidden;
                    GrayPants.Visibility = Visibility.Hidden;
                    GoldPants.Visibility = Visibility.Hidden;
                    BrownPants.Visibility = Visibility.Hidden;
                    currentPant = @"RedPants.png";                    
                    break;
                case 3: RedPants.Visibility = Visibility.Hidden;
                    PurplePants.Visibility = Visibility.Visible;
                    PinkPants.Visibility = Visibility.Hidden;
                    GrayPants.Visibility = Visibility.Hidden;
                    GoldPants.Visibility = Visibility.Hidden;
                    BrownPants.Visibility = Visibility.Hidden;
                    currentPant = @"PurplePants.png";
                    break;
                case 4: RedPants.Visibility = Visibility.Hidden;
                    PurplePants.Visibility = Visibility.Hidden;
                    PinkPants.Visibility = Visibility.Visible;
                    GrayPants.Visibility = Visibility.Hidden;
                    GoldPants.Visibility = Visibility.Hidden;
                    BrownPants.Visibility = Visibility.Hidden;
                    currentPant = @"PinkPants.png";
                    break;
                case 5: RedPants.Visibility = Visibility.Hidden;
                    PurplePants.Visibility = Visibility.Hidden;
                    PinkPants.Visibility = Visibility.Hidden;
                    GrayPants.Visibility = Visibility.Visible;
                    GoldPants.Visibility = Visibility.Hidden;
                    BrownPants.Visibility = Visibility.Hidden;
                    currentPant = @"GrayPants.png";
                    break;
                case 6: RedPants.Visibility = Visibility.Hidden;
                    PurplePants.Visibility = Visibility.Hidden;
                    PinkPants.Visibility = Visibility.Hidden;
                    GrayPants.Visibility = Visibility.Hidden;
                    GoldPants.Visibility = Visibility.Visible;
                    BrownPants.Visibility = Visibility.Hidden;
                    currentPant = @"GoldPants.png";
                    break;
                case 7: RedPants.Visibility = Visibility.Hidden;
                    PurplePants.Visibility = Visibility.Hidden;
                    PinkPants.Visibility = Visibility.Hidden;
                    GrayPants.Visibility = Visibility.Hidden;
                    GoldPants.Visibility = Visibility.Hidden;
                    BrownPants.Visibility = Visibility.Visible;
                    currentPant = @"BrownPants.png";
                    break;
            }
        }

        private void BeardCycleRight_Click(object sender, RoutedEventArgs e)
        {
            BeardCount++;
            if (BeardCount == 6)
            {
                BeardCount = 1;
            }
            BeardChange(BeardCount);
        }

        private void BeardCycleLeft_Click(object sender, RoutedEventArgs e)
        {
            BeardCount--;
            if (BeardCount == 0)
            {
                BeardCount = 5;
            }
            BeardChange(BeardCount);
        }

        private void PantsCycleRight_Click(object sender, RoutedEventArgs e)
        {
            PantsCount++;
            if (PantsCount == 8)
            {
                PantsCount = 1;
            }
            PantsChange(PantsCount);
        }

        private void PantsCycleLeft_Click(object sender, RoutedEventArgs e)
        {
            PantsCount--;
            if (PantsCount == 0)
            {
                PantsCount = 7;
            }
            PantsChange(PantsCount);
        }

        private void AvailableLobbies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void SaveProfile_Click(object sender, RoutedEventArgs e)
        {

            string newProfileName = NewProfileName.Text;
            newProfileName = newProfileName.Trim();
            if (IsProfileNameValid(newProfileName))
            {
                if (profileManager.IsProfileValid(newProfileName))
                {
                    List<Bitmap> images = new List<Bitmap>();
                    if (currentArmor1 != "Base")
                        images.Add(new Bitmap(@"GUI\Images\" + currentArmor1));
                    if (currentBeard1 != "Base")
                        images.Add(new Bitmap(@"GUI\Images\" + currentBeard1));
                    if (currentPant1 != "Base")
                        images.Add(new Bitmap(@"GUI\Images\" + currentPant1));

                    var target = new Bitmap(@"GUI\Images\Armless.png");
                    var graphics = Graphics.FromImage(target);
                    graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

                    foreach (Bitmap image in images)
                        graphics.DrawImage(image, 0, 0);

                    if (!Directory.Exists(@"GUI\Images\CustomVikings"))
                        Directory.CreateDirectory(@"GUI\Images\CustomVikings");
                    target.Save(@"GUI\Images\CustomVikings\" + newProfileName + ".png", System.Drawing.Imaging.ImageFormat.Png);

                    //append to file 
                    profileManager.CreateNewProfile(newProfileName);
                    const string message = "Your profile has been set up.";
                    MessageWindow messageWindow = new MessageWindow(message, MessageBoxButton.OK);
                    ExistingProfile_Click(sender, e);
                    messageWindow.ShowDialog();
                }
                else
                {
                    const string message = "This profile name already exists, please create a new one.";
                    MessageWindow messageWindow = new MessageWindow(message, MessageBoxButton.OK);
                    messageWindow.ShowDialog();
                }
            }
            else
            {
                const string message = "Please, verify name is alphanumeric, longer than 1 character and less than 15.";
                MessageWindow messageWindow = new MessageWindow(message, MessageBoxButton.OK);
                messageWindow.ShowDialog();
            }

        }

        private void Menu_MouseMove(object sender, MouseEventArgs e)
        {
            //System.Windows.Point mousePositionRelativeToWindow = e.GetPosition(this);
            //TransformGroup t = new TransformGroup();
            //t.Children.Add(new TranslateTransform(mousePositionRelativeToWindow.X + 1, mousePositionRelativeToWindow.Y + 1));
            //Pointer.RenderTransform = t;
        }

        private void PlayerProfile_Click(object sender, RoutedEventArgs e)
        {
            ReHideMenues();
            PlayerProfilePanel.Visibility = Visibility.Visible;
        }

        private void GameDifficultyBeginnerOff_Click(object sender, RoutedEventArgs e)
        {
            GameDifficultyBeginnerOff.Visibility = Visibility.Hidden;
            GameDifficultyBeginnerOn.Visibility = Visibility.Visible;

            GameDifficultyEasyOff.Visibility = Visibility.Visible;
            GameDifficultyEasyOn.Visibility = Visibility.Hidden;

            GameDifficultyMediumOff.Visibility = Visibility.Visible;
            GameDifficultyMediumOn.Visibility = Visibility.Hidden;

            GameDifficultyHardOff.Visibility = Visibility.Visible;
            GameDifficultyHardOn.Visibility = Visibility.Hidden;

            GameDifficultyExpertOff.Visibility = Visibility.Visible;
            GameDifficultyExpertOn.Visibility = Visibility.Hidden;
            DifficultyIndicator.Text = "Beginner";
        }

        private void GameDifficultyEasyOff_Click(object sender, RoutedEventArgs e)
        {
            GameDifficultyBeginnerOff.Visibility = Visibility.Visible;
            GameDifficultyBeginnerOn.Visibility = Visibility.Hidden;

            GameDifficultyEasyOff.Visibility = Visibility.Hidden;
            GameDifficultyEasyOn.Visibility = Visibility.Visible;

            GameDifficultyMediumOff.Visibility = Visibility.Visible;
            GameDifficultyMediumOn.Visibility = Visibility.Hidden;

            GameDifficultyHardOff.Visibility = Visibility.Visible;
            GameDifficultyHardOn.Visibility = Visibility.Hidden;

            GameDifficultyExpertOff.Visibility = Visibility.Visible;
            GameDifficultyExpertOn.Visibility = Visibility.Hidden;
            DifficultyIndicator.Text = "Easy";
        }

        private void GameDifficultyMediumOff_Click(object sender, RoutedEventArgs e)
        {
            GameDifficultyBeginnerOff.Visibility = Visibility.Visible;
            GameDifficultyBeginnerOn.Visibility = Visibility.Hidden;

            GameDifficultyEasyOff.Visibility = Visibility.Visible;
            GameDifficultyEasyOn.Visibility = Visibility.Hidden;

            GameDifficultyMediumOff.Visibility = Visibility.Hidden;
            GameDifficultyMediumOn.Visibility = Visibility.Visible;

            GameDifficultyHardOff.Visibility = Visibility.Visible;
            GameDifficultyHardOn.Visibility = Visibility.Hidden;

            GameDifficultyExpertOff.Visibility = Visibility.Visible;
            GameDifficultyExpertOn.Visibility = Visibility.Hidden;
            DifficultyIndicator.Text = "Medium";
        }

        private void GameDifficultyHardOff_Click(object sender, RoutedEventArgs e)
        {
            GameDifficultyBeginnerOff.Visibility = Visibility.Visible;
            GameDifficultyBeginnerOn.Visibility = Visibility.Hidden;
            GameDifficultyEasyOff.Visibility = Visibility.Visible;
            GameDifficultyEasyOn.Visibility = Visibility.Hidden;
            GameDifficultyMediumOff.Visibility = Visibility.Visible;
            GameDifficultyMediumOn.Visibility = Visibility.Hidden;
            GameDifficultyHardOff.Visibility = Visibility.Hidden;
            GameDifficultyHardOn.Visibility = Visibility.Visible;
            GameDifficultyExpertOff.Visibility = Visibility.Visible;
            GameDifficultyExpertOn.Visibility = Visibility.Hidden;
            DifficultyIndicator.Text = "Hard";
        }

        private void GameDifficultyExpertOff_Click(object sender, RoutedEventArgs e)
        {
            GameDifficultyBeginnerOff.Visibility = Visibility.Visible;
            GameDifficultyBeginnerOn.Visibility = Visibility.Hidden;
            GameDifficultyEasyOff.Visibility = Visibility.Visible;
            GameDifficultyEasyOn.Visibility = Visibility.Hidden;
            GameDifficultyMediumOff.Visibility = Visibility.Visible;
            GameDifficultyMediumOn.Visibility = Visibility.Hidden;
            GameDifficultyHardOff.Visibility = Visibility.Visible;
            GameDifficultyHardOn.Visibility = Visibility.Hidden;
            GameDifficultyExpertOff.Visibility = Visibility.Hidden;
            GameDifficultyExpertOn.Visibility = Visibility.Visible;
            DifficultyIndicator.Text = "Expert";
        }

        int BeardCount1 = 1;
        int ArmorCount1 = 1;
        int PantsCount1 = 1;

        private void BeardCycleRight1_Click(object sender, RoutedEventArgs e)
        {
            BeardCount1++;
            if (BeardCount1 == 6)
            {
                BeardCount1 = 1;
            }
            BeardChange1(BeardCount1);
        }

        private void BeardCycleLeft1_Click(object sender, RoutedEventArgs e)
        {
            BeardCount1--;
            if (BeardCount1 == 0)
            {
                BeardCount1 = 5;
            }
            BeardChange1(BeardCount1);
        }

        private void ArmorCycleRight1_Click(object sender, RoutedEventArgs e)
        {
            ArmorCount1++;
            if (ArmorCount1 == 6)
            {
                ArmorCount1 = 1;
            }
            ArmorChange1(ArmorCount1);
        }

        private void ArmorCycleLeft1_Click(object sender, RoutedEventArgs e)
        {
            ArmorCount1--;
            if (ArmorCount1 == 0)
            {
                ArmorCount1 = 5;
            }
            ArmorChange1(ArmorCount1);
        }

        private void PantsCycleRight1_Click(object sender, RoutedEventArgs e)
        {
            PantsCount1++;
            if (PantsCount1 == 8)
            {
                PantsCount1 = 1;
            }
            PantsChange1(PantsCount1);
        }

        private void PantsCycleLeft1_Click(object sender, RoutedEventArgs e)
        {
            PantsCount1--;
            if (PantsCount1 == 0)
            {
                PantsCount1 = 7;
            }
            PantsChange1(PantsCount1);
        }

        private void ArmorChange1(int i)
        {
            switch (i)
            {
                case 1: RedArmor1.Visibility = Visibility.Hidden;
                    BlackArmor1.Visibility = Visibility.Hidden;
                    SilverArmor1.Visibility = Visibility.Hidden;
                    GoldArmor1.Visibility = Visibility.Hidden;
                    currentArmor1 = "Base";
                    break;
                case 2: RedArmor1.Visibility = Visibility.Visible;
                    BlackArmor1.Visibility = Visibility.Hidden;
                    SilverArmor1.Visibility = Visibility.Hidden;
                    GoldArmor1.Visibility = Visibility.Hidden;
                    currentArmor1 = @"Red.png";
                    break;
                case 3: RedArmor1.Visibility = Visibility.Hidden;
                    BlackArmor1.Visibility = Visibility.Visible;
                    SilverArmor1.Visibility = Visibility.Hidden;
                    GoldArmor1.Visibility = Visibility.Hidden;
                    currentArmor1 = @"Black.png";
                    break;
                case 4: RedArmor1.Visibility = Visibility.Hidden;
                    BlackArmor1.Visibility = Visibility.Hidden;
                    SilverArmor1.Visibility = Visibility.Visible;
                    GoldArmor1.Visibility = Visibility.Hidden;
                    currentArmor1 = @"Silver.png";
                    break;
                case 5: RedArmor1.Visibility = Visibility.Hidden;
                    BlackArmor1.Visibility = Visibility.Hidden;
                    SilverArmor1.Visibility = Visibility.Hidden;
                    GoldArmor1.Visibility = Visibility.Visible;
                    currentArmor1 = @"Gold.png";
                    break;
            }
        }

        private void BeardChange1(int i)
        {
            switch (i)
            {
                case 1: RedBeard1.Visibility = Visibility.Hidden;
                    BlackBeard1.Visibility = Visibility.Hidden;
                    GrayBeard1.Visibility = Visibility.Hidden;
                    BrownBeard1.Visibility = Visibility.Hidden;
                    currentBeard1 = "Base";
                    break;
                case 2: RedBeard1.Visibility = Visibility.Visible;
                    BlackBeard1.Visibility = Visibility.Hidden;
                    GrayBeard1.Visibility = Visibility.Hidden;
                    BrownBeard1.Visibility = Visibility.Hidden;
                    currentBeard1 = @"RedBeard.png";
                    break;
                case 3: RedBeard1.Visibility = Visibility.Hidden;
                    BlackBeard1.Visibility = Visibility.Visible;
                    GrayBeard1.Visibility = Visibility.Hidden;
                    BrownBeard1.Visibility = Visibility.Hidden;
                    currentBeard1 = @"BlackBeard.png";
                    break;
                case 4: RedBeard1.Visibility = Visibility.Hidden;
                    BlackBeard1.Visibility = Visibility.Hidden;
                    GrayBeard1.Visibility = Visibility.Visible;
                    BrownBeard1.Visibility = Visibility.Hidden;
                    currentBeard1 = @"Gray.png";
                    break;
                case 5: RedBeard1.Visibility = Visibility.Hidden;
                    BlackBeard1.Visibility = Visibility.Hidden;
                    GrayBeard1.Visibility = Visibility.Hidden;
                    BrownBeard1.Visibility = Visibility.Visible;
                    currentBeard1 = @"Brown.png";
                    break;
            }
        }

        private void PantsChange1(int i)
        {
            switch (i)
            {
                case 1: RedPants1.Visibility = Visibility.Hidden;
                    PurplePants1.Visibility = Visibility.Hidden;
                    PinkPants1.Visibility = Visibility.Hidden;
                    GrayPants1.Visibility = Visibility.Hidden;
                    GoldPants1.Visibility = Visibility.Hidden;
                    BrownPants1.Visibility = Visibility.Hidden;
                    currentPant1 = "Base";
                    break;
                case 2: RedPants1.Visibility = Visibility.Visible;
                    PurplePants1.Visibility = Visibility.Hidden;
                    PinkPants1.Visibility = Visibility.Hidden;
                    GrayPants1.Visibility = Visibility.Hidden;
                    GoldPants1.Visibility = Visibility.Hidden;
                    BrownPants1.Visibility = Visibility.Hidden;
                    currentPant1 = @"RedPants.png";
                    break;
                case 3: RedPants1.Visibility = Visibility.Hidden;
                    PurplePants1.Visibility = Visibility.Visible;
                    PinkPants1.Visibility = Visibility.Hidden;
                    GrayPants1.Visibility = Visibility.Hidden;
                    GoldPants1.Visibility = Visibility.Hidden;
                    BrownPants1.Visibility = Visibility.Hidden;
                    currentPant1 = @"PurplePants.png";
                    break;
                case 4: RedPants1.Visibility = Visibility.Hidden;
                    PurplePants1.Visibility = Visibility.Hidden;
                    PinkPants1.Visibility = Visibility.Visible;
                    GrayPants1.Visibility = Visibility.Hidden;
                    GoldPants1.Visibility = Visibility.Hidden;
                    BrownPants1.Visibility = Visibility.Hidden;
                    currentPant1 = @"PinkPants.png";
                    break;
                case 5: RedPants1.Visibility = Visibility.Hidden;
                    PurplePants1.Visibility = Visibility.Hidden;
                    PinkPants1.Visibility = Visibility.Hidden;
                    GrayPants1.Visibility = Visibility.Visible;
                    GoldPants1.Visibility = Visibility.Hidden;
                    BrownPants1.Visibility = Visibility.Hidden;
                    currentPant1 = @"GrayPants.png";
                    break;
                case 6: RedPants1.Visibility = Visibility.Hidden;
                    PurplePants1.Visibility = Visibility.Hidden;
                    PinkPants1.Visibility = Visibility.Hidden;
                    GrayPants1.Visibility = Visibility.Hidden;
                    GoldPants1.Visibility = Visibility.Visible;
                    BrownPants1.Visibility = Visibility.Hidden;
                    currentPant1 = @"GoldPants.png";
                    break;
                case 7: RedPants1.Visibility = Visibility.Hidden;
                    PurplePants1.Visibility = Visibility.Hidden;
                    PinkPants1.Visibility = Visibility.Hidden;
                    GrayPants1.Visibility = Visibility.Hidden;
                    GoldPants1.Visibility = Visibility.Hidden;
                    BrownPants1.Visibility = Visibility.Visible;
                    currentPant1 = @"BrownPants.png";
                    break;
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            ReHideMenues();
            Help help = new Help();
            help.Show();
        }

        private void ContineAdventure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string profileName = ProfileList.SelectedValue.ToString().Trim();
                if (profileName != "")
                {
                    Pentago.GameCore.ProfileManager.Profile playerProfile = profileManager.SearchProfile(profileName);
                    string player1Name = playerProfile.ProfileName;

                    bool isPlayer1Active = true;

                    ImageBrush player1Image = new ImageBrush();
                    player1Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/RedPup.png", UriKind.Absolute));
                    ImageBrush player1ImageHover = new ImageBrush();
                    player1ImageHover.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/RedPupHover.png", UriKind.Absolute));

                    string computerPlayerName = "Computer";
                    ImageBrush computerPlayerImage = new ImageBrush();
                    computerPlayerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/BluePup.png", UriKind.Absolute));

                    ImageBrush computerPlayerImageHover = new ImageBrush();
                    computerPlayerImageHover.ImageSource = new BitmapImage(new Uri("pack://application:,,,/GUI/images/BluePupHover.png", UriKind.Absolute));


                    computerAI.Difficulty difficulty;
                    if (playerProfile.CampaignProgress == 0)
                        difficulty = computerAI.Difficulty.Beginner;
                    else if (playerProfile.CampaignProgress == 1)
                        difficulty = computerAI.Difficulty.Easy;
                    else if (playerProfile.CampaignProgress == 2)
                        difficulty = computerAI.Difficulty.Medium;
                    else if (playerProfile.CampaignProgress == 3)
                        difficulty = computerAI.Difficulty.Medium;
                    else
                        difficulty = computerAI.Difficulty.Hard;
                    Player player1 = new Player(player1Name.Trim(), isPlayer1Active, player1Image, player1ImageHover);
                    computerAI computerPlayer = new computerAI(computerPlayerName.Trim(), !isPlayer1Active, computerPlayerImage, computerPlayerImageHover, difficulty);

                    GameOptions gameOptions = new GameOptions(GameOptions.TypeOfGame.AI, player1, computerPlayer);
                    Window mapWindow = new MapWindow(gameOptions);
                    App.Current.MainWindow = mapWindow;
                    mapWindow.Show();
                    this.Hide();
                }
            }
            catch 
            {
                const string message = "Please, select a profile.";
                MessageWindow messageWindow = new MessageWindow(message, MessageBoxButton.OK);
                messageWindow.ShowDialog();
            }
        }

        private void NewProfileName_KeyDown(object sender, KeyEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.KeyDown);
        }
    }
}
