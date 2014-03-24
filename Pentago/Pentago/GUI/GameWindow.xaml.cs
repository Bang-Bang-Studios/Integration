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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Pentago.GUI;
using Pentago.GameCore;
using Pentago.AI;
using Pentago;
using Pentago.GUI.Classes;

//just for visual help
using System.Threading;
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace Pentago
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private GameBrain gameBrain = null;

        private const int BOARDSIZE = 36;
        private const int MAXCOLUMNS = 6;
        private const int MAXROWS = 6;

        private bool userMadeRotation = true;

        //These are the players for the GUI
        private Player player1 = null;
        private Player player2 = null;
        private computerAI computerPlayer = null;
        GameOptions gameOptions = null;

        public GameWindow(GameOptions options)
        {
            InitializeComponent();
            SoundManager.backgroundMusicPlayer.Open(new Uri("GUI/Sounds/Gameplay.mp3", UriKind.Relative));
            SoundManager.backgroundMusicPlayer.Play();
            gameOptions = options;
            PaintBoard();
            switch (gameOptions._TypeOfGame)
            {
                case GameOptions.TypeOfGame.QuickMatch:
                    player1 = options._Player1;
                    player2 = options._Player2;
                    gameBrain = new GameBrain(player1);
                    Player1NameText.Text = player1.Name;
                    Player2NameText.Text = player2.Name;
                    break;
                case GameOptions.TypeOfGame.AI:
                    player1 = options._Player1;
                    computerPlayer = options._ComputerPlayer;
                    gameBrain = new GameBrain(player1, computerPlayer);
                    Player1NameText.Text = player1.Name;
                    Player2NameText.Text = computerPlayer.Name;
                    if (!player1.ActivePlayer)
                        GetComputerMoveAsynchronously();
                    break;
                default:
                    break;
            }
            ShowActivePlayer();
        }

        public void PaintBoard()
        {
            Board.Rows = MAXROWS;
            Board.Columns = MAXCOLUMNS;
            for (int i = 0; i < BOARDSIZE; i++)
            {
                Rectangle rec = new Rectangle();
                rec.Fill = Brushes.Transparent;
                rec.Stroke = Brushes.Gray;
                rec.Opacity = 1;
                Board.Children.Add(rec);
            }
        }

        private void Board_MouseMove(object sender, MouseEventArgs e)
        {
            if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch || player1.ActivePlayer)
            {
                RePaintBoard();
                int rectSize = (int)Board.Width / MAXCOLUMNS;
                Point mousePosition = e.GetPosition(Board);
                short row = (short)(mousePosition.Y / rectSize);
                if (row == 6)
                    row--;
                short col = (short)(mousePosition.X / rectSize);
                if (col == 6)
                    col--;
                int winner = gameBrain.CheckForWin();
                int[] tempBoard = gameBrain.GetBoard;
                if (userMadeRotation && winner == 0 && tempBoard[MAXCOLUMNS * row + col] == 0)
                {
                    Rectangle rec = (Rectangle)Board.Children[MAXCOLUMNS * row + col];
                    if (gameBrain.isPlayer1Turn())
                        rec.Fill = player1.ImageHover;
                    else
                        rec.Fill = player2.ImageHover;
                }
            }
        }

        private void Board_MouseLeave(object sender, MouseEventArgs e)
        {
            RePaintBoard();
        }

        private void Board_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch || player1.ActivePlayer)
            {
                int rectSize = (int)Board.Width / MAXCOLUMNS;

                Point mousePosition = e.GetPosition(Board);
                short row = (short)(mousePosition.Y / rectSize);
                if (row == 6)
                    row--;
                short col = (short)(mousePosition.X / rectSize);
                if (col == 6)
                    col--;
                int winner = gameBrain.CheckForWin();
                if (userMadeRotation && winner == 0 && gameBrain.PlacePiece(row, col))
                {
                    SoundManager.playSFX(SoundManager.SoundType.Click);
                    userMadeRotation = false;
                    Rectangle rec = (Rectangle)Board.Children[MAXCOLUMNS * row + col];
                    if (gameBrain.isPlayer1Turn())
                        rec.Fill = player1.Image;
                    else
                        rec.Fill = player2.Image;
                    RePaintBoard();
                    winner = gameBrain.CheckForWin();
                    if (winner != 0)
                        ShowWinner(winner);
                    else
                        MakeRotationsVisible();
                }
                else if (winner != 0)
                    ShowWinner(winner);
            }

        }

        private void ShowWinner(int winner)
        {
            string winnerText = "";
            switch (winner)
            {
                case 1:
                    winnerText = "Congratulations " + player1.Name + " you have won!";
                    break;
                case 2:
                    if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch)
                        winnerText = "Congratulations " + player2.Name + " you have won!";
                    else if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.AI)
                        winnerText = "The " + computerPlayer.Name + " has won!";
                    break;
                case 3:
                    winnerText = "It is a tie.";
                    break;
                default:
                    break;
            }

            winnerAnnoucement.Text = winnerText;
        }

        private void ShowActivePlayer()
        {
            if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch)
            {
                if (player1.ActivePlayer)
                    ActivePlayer.Fill = player1.Image;
                else
                    ActivePlayer.Fill = player2.Image;
            }
            else if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.AI)
            {
                if (player1.ActivePlayer)
                    ActivePlayer.Fill = player1.Image;
                else
                    ActivePlayer.Fill = computerPlayer.Image;
            }
        }

        private void RePaintBoard()
        {
            int[] tempBoard = gameBrain.GetBoard;
            for (int i = 0; i < BOARDSIZE; i++)
            {
                Rectangle rec = (Rectangle)Board.Children[i];
                if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch)
                {
                    if (tempBoard[i] == 1)
                        rec.Fill = player1.Image;
                    else if (tempBoard[i] == 2)
                        rec.Fill = player2.Image;
                    else
                        rec.Fill = Brushes.Transparent;
                }
                else if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.AI)
                {
                    if (tempBoard[i] == 1)
                        rec.Fill = player1.Image;
                    else if (tempBoard[i] == 2)
                        rec.Fill = computerPlayer.Image;
                    else
                        rec.Fill = Brushes.Transparent;
                }
                rec.Opacity = 1;
            }
        }
        /*
        private void RotateRectangle()
        {
            Rectangle rec = (Rectangle)Board.Children[0];
            var da = new DoubleAnimation(360, 0, new Duration(TimeSpan.FromSeconds(1)));
            var rt = new RotateTransform();
            rec.RenderTransform = rt;
            rec.RenderTransformOrigin = new Point(0.5, 0.5);
            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }
        */

        private void MakeRotationsVisible() 
        {
            btnClockWise1.Visibility = Visibility.Visible;
            btnCounterClockWise1.Visibility = Visibility.Visible;
            btnClockWise2.Visibility = Visibility.Visible;
            btnCounterClockWise2.Visibility = Visibility.Visible;
            btnClockWise3.Visibility = Visibility.Visible;
            btnCounterClockWise3.Visibility = Visibility.Visible;
            btnClockWise4.Visibility = Visibility.Visible;
            btnCounterClockWise4.Visibility = Visibility.Visible;
        }

        //Hide all rotations and show which player turn is it
        private void MakeRotationsHidden()
        {
            userMadeRotation = true;
            btnClockWise1.Visibility = Visibility.Hidden;
            btnCounterClockWise1.Visibility = Visibility.Hidden;
            btnClockWise2.Visibility = Visibility.Hidden;
            btnCounterClockWise2.Visibility = Visibility.Hidden;
            btnClockWise3.Visibility = Visibility.Hidden;
            btnCounterClockWise3.Visibility = Visibility.Hidden;
            btnClockWise4.Visibility = Visibility.Hidden;
            btnCounterClockWise4.Visibility = Visibility.Hidden;

            //Changes turn of player in GUI
            player1.ActivePlayer = gameBrain.isPlayer1Turn();

            int winner = gameBrain.CheckForWin();
            if (winner != 0)
                ShowWinner(winner);
            else
                ShowActivePlayer();

            //if this is a human vs computer game
            if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.AI)
            {
                if (!player1.ActivePlayer && winner == 0)
                    GetComputerMoveAsynchronously();
            }
        }

        private void GetComputerMoveAsynchronously()
        {
            BackgroundWorker AIbackgroundWorker = new BackgroundWorker();
            // define the event handlers
            AIbackgroundWorker.DoWork += (sender, args) =>
            {
                //Console.WriteLine("Started AI thread.");
                int winner = gameBrain.CheckForWin();
                if (winner != 0 || !gameBrain.MakeComputerMove())
                {
                    //Console.WriteLine("Cancelled AI thread.");
                    AIbackgroundWorker.CancelAsync();
                    if (winner != 0)
                        ShowWinner(winner);
                }
            };
            AIbackgroundWorker.RunWorkerCompleted += (sender, args) =>
            {
                if (args.Error != null)  // if an exception occurred during DoWork,
                {
                    //Console.WriteLine("Something went wrong with AI thread.");
                    MessageBox.Show(args.Error.ToString());  // do your error handling here
                }

                //Console.WriteLine("Good with AI thread.");
                int winner = gameBrain.CheckForWin();
                if (winner == 0)
                {
                    //Update GUI player
                    int computerMove = gameBrain.GetComputerMove();
                    Rectangle rec = (Rectangle)Board.Children[computerMove];
                    rec.Fill = computerPlayer.Image;
                    winner = gameBrain.CheckForWin();
                    if (winner != 0)
                        ShowWinner(winner);
                    else
                        GetComputerRotation();
                }
                else if (winner != 0)
                {
                    RePaintBoard();
                    ShowWinner(winner);
                }

            };
            AIbackgroundWorker.RunWorkerAsync();
        }

        private void GetComputerRotation()
        {
            for (int i = 1; i <= 2; i ++)
            {
                gameBrain.MakeComputerRotation(i);
                RePaintBoard();
            }

            MakeRotationsHidden();
        }

        private void InitiateRotation(bool rotateClockwise, short quad)
        {
            SoundManager.playSFX(SoundManager.SoundType.Rotate);

                gameBrain.RotateBoard(rotateClockwise, quad, 1);
                RotateAnimation(quad, rotateClockwise);
                //RePaintBoard();
                //Thread.Sleep(1000);
                gameBrain.RotateBoard(rotateClockwise, quad, 2);
                //RePaintBoard();

            MakeRotationsHidden();
        }

        private void RotateAnimation(short quad, bool rotateClockwise) 
        {
            var da = new DoubleAnimation(360, 0, new Duration(TimeSpan.FromSeconds(1)));
            var rt = new RotateTransform();
            Rectangle rect0 = (Rectangle)Board.Children[0];
            rect0.RenderTransform = rt;
            rect0.RenderTransformOrigin = new Point(0.5, 0.5);
            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        //private void InitiateRotation(bool rotateClockwise, short quad)
        //{

        //    SoundManager.playSFX(SoundManager.SoundType.Rotate);
        //    gameBrain.RotateBoard(rotateClockwise, quad);
        //    RePaintBoard();
        //    MakeRotationsHidden();
        //}

        private void btnCounterClockWise2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(false, 2);
        }

        private void btnClockWise1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(true, 1);
        }

        private void btnCounterClockWise1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(false, 1);
        }

        private void btnClockWise2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(true, 2);
        }

        private void btnClockWise3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(true, 3);
        }

        private void btnCounterClockWise3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(false, 3);
        }

        private void btnClockWise4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(true, 4);
        }

        private void btnCounterClockWise4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(false, 4);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Window mainWindow = new MainMenu();
            App.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Hide();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            //Storyboard sb = new Storyboard();
            //DoubleAnimation da = new DoubleAnimation(-100, 100, new Duration(new TimeSpan(0, 0, 1)));
            //Storyboard.SetTargetProperty(da, new PropertyPath("(Canvas.Top)")); //Do not miss the '(' and ')'
            //sb.Children.Add(da);

            //loop to check each spot in quad for image
            //get board location for each image
            //slide each image
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.Click);
            Window mainWindow = new MainMenu();
            App.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Hide();
        }

        private void ExitButton_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }




    }
}
