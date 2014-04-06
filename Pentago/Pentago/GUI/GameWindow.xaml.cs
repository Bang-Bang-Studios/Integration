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
using System.Timers;
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
        private PentagoNetwork networkUtil;

        private bool isNetwork = false;
        private short movePos;


        public GameWindow(GameOptions options)
        {
            InitializeComponent();
            //SoundManager.backgroundMusicPlayer.Open(new Uri("GUI/Sounds/Gameplay.mp3", UriKind.Relative));
            //SoundManager.backgroundMusicPlayer.Play();
            gameOptions = options;
            CreateChildrenList();
            switch (gameOptions._TypeOfGame)
            {
                case GameOptions.TypeOfGame.QuickMatch:
                    player1 = options._Player1;
                    player2 = options._Player2;
                    gameBrain = new GameBrain(player1);
                    Player1NameText.Text = player1.Name;
                    Player2NameText.Text = player2.Name;
                    isNetwork = false;
                    break;
                case GameOptions.TypeOfGame.Network:
                    player1 = options._Player1;
                    player2 = options._Player2;
                    gameBrain = new GameBrain(player1);
                    Player1NameText.Text = player1.Name;
                    Player2NameText.Text = player2.Name;
                    networkUtil = options._NetworkUtil;
                    networkUtil.MoveReceived += new moveReceivedHandler(NetworkMoveReceived);
                    isNetwork = true;
                    break;
                case GameOptions.TypeOfGame.AI:
                    player1 = options._Player1;
                    computerPlayer = options._ComputerPlayer;
                    gameBrain = new GameBrain(player1, computerPlayer);
                    Player1NameText.Text = player1.Name;
                    Player2NameText.Text = computerPlayer.Name;
                    if (!player1.ActivePlayer)
                        GetComputerMoveAsynchronously();
                    isNetwork = false;
                    break;
                default:
                    break;
            }
            ShowActivePlayer();
        }

        private List<Rectangle> rectangleChildren = null;
        private void CreateChildrenList()
        {
            rectangleChildren = new List<Rectangle>();
            var rectangles = Board.Children;
            foreach (Rectangle element in rectangles)
            {
                rectangleChildren.Add(element);
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
                    Rectangle rec = rectangleChildren.ElementAt(MAXCOLUMNS * row + col);
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
                movePos = 0;
                movePos += col;
                movePos += (short)(row * 6);
                if (userMadeRotation && winner == 0 && gameBrain.PlacePiece(row, col))
                {
                    SoundManager.playSFX(SoundManager.SoundType.Click);
                    userMadeRotation = false;
                    Rectangle rec = rectangleChildren.ElementAt(MAXCOLUMNS * row + col);
                    if (gameBrain.isPlayer1Turn())
                        rec.Fill = player1.Image;
                    else
                        rec.Fill = player2.Image;
                    RePaintBoard();
                    winner = gameBrain.CheckForWin();
                    if (winner != 0)
                    {
                        ShowWinner(winner);
                        if (isNetwork)
                        {
                            networkUtil.SendMove(-1, movePos, true);
                        }
                    }
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
                    else if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.Network)
                        winnerText = player2.Name + " has defeated you!";
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
            if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch || gameOptions._TypeOfGame == GameOptions.TypeOfGame.Network)
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
            var rectangleChildren = Board.Children;
            int slot = 0;
            foreach (Rectangle element in rectangleChildren)
            {
                if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch ||
                                gameOptions._TypeOfGame == GameOptions.TypeOfGame.Network)
                {
                    if (tempBoard[slot] == 1)
                        element.Fill = player1.Image;
                    else if (tempBoard[slot] == 2)
                        element.Fill = player2.Image;
                    else
                        element.Fill = Brushes.Transparent;
                }
                else if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.AI)
                {
                    if (tempBoard[slot] == 1)
                        element.Fill = player1.Image;
                    else if (tempBoard[slot] == 2)
                        element.Fill = computerPlayer.Image;
                    else
                        element.Fill = Brushes.Transparent;
                }
                element.Opacity = 1;
                slot++;
            }


        }

        private void RemoveGrid()
        {
            Board.Children.Clear();
        }

        private void RecreateGrid()
        {
            int[] tempBoard = gameBrain.GetBoard;
            for (int i = 0; i < BOARDSIZE; i++)
            {
                Rectangle rec = new Rectangle();
                rec.Stroke = Brushes.Gray;
                if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch ||
                                gameOptions._TypeOfGame == GameOptions.TypeOfGame.Network)
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
                rec.Width = 83;
                rec.Height = 83;
                Board.Children.Add(rec);
            }
            CreateChildrenList();
        }

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
            for (int i = 1; i <= 2; i++)
            {
                gameBrain.MakeComputerRotation(i);
                RePaintBoard();
            }

            MakeRotationsHidden();
        }

        private void NetworkMoveReceived(object move, EventArgs e)
        {
            PentagoNetwork.moveType mov = (PentagoNetwork.moveType)move;
            gameBrain.PlacePieceByPos(mov.position);
            this.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    InitiateRotation(mov.isClockwise, mov.quad);
                    RePaintBoard();
                    int winner = gameBrain.CheckForWin();
                    if (winner != 0)
                    {
                        ShowWinner(winner);
                    }
                }), null);

        }

        private void InitiateRotation(bool rotateClockwise, short quad)
        {
            if (quad != -1)
            {
                SoundManager.playSFX(SoundManager.SoundType.Rotate);

                gameBrain.RotateBoard(rotateClockwise, quad, 1);
                gameBrain.RotateBoard(rotateClockwise, quad, 2);

                RotateAnimation(quad, rotateClockwise);
            }
        }


        private void RotateAnimation(short quad, bool rotateClockwise)
        {
            switch (quad)
            {
                case 1:
                    if (rotateClockwise)
                    {
                        RotateQuad1Clockwise();
                    }
                    else
                    {
                        RotateQuad1CounterClockwise();
                    }
                    break;

                case 2:
                    if (rotateClockwise)
                    {
                        RotateQuad2Clockwise();
                    }
                    else
                    {
                        RotateQuad2CounterClockwise();
                    }
                    break;

                case 3:
                    if (rotateClockwise)
                    {
                        RotateQuad3Clockwise();
                    }
                    else
                    {
                        RotateQuad3CounterClockwise();
                    }
                    break;

                case 4:
                    if (rotateClockwise)
                    {
                        RotateQuad4Clockwise();
                    }
                    else
                    {
                        RotateQuad4CounterClockwise();
                    }
                    break;
            }

        }

        private void OnAnimationCompletition(object sender, EventArgs e)
        {
            RemoveGrid();
            RecreateGrid();
            MakeRotationsHidden();
        }

        private void RotateQuad1Clockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            Rectangle holder = rectangleChildren.ElementAt(0);
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;

            trans = new TranslateTransform();
            from = rectangleChildren[0];
            from.RenderTransform = trans;
            to = rectangleChildren[2];
            x = to.TranslatePoint(new Point(to.ActualWidth*2, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            trans = new TranslateTransform();
            from = rectangleChildren[1];
            from.RenderTransform = trans;
            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));

            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[8];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));

            trans.BeginAnimation(TranslateTransform.YProperty, animateY);


            trans = new TranslateTransform();
            from = rectangleChildren[2];
            from.RenderTransform = trans;
            to = rectangleChildren[14];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);


            trans = new TranslateTransform();
            from = rectangleChildren[8];
            from.RenderTransform = trans;
            to = rectangleChildren[14];
            y = from.TranslatePoint(new Point(from.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);


            trans = new TranslateTransform();
            from = rectangleChildren[14];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);


            trans = new TranslateTransform();
            from = rectangleChildren[13];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[6];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);


            trans = new TranslateTransform();
            from = rectangleChildren[12];
            from.RenderTransform = trans;
            to = holder;
            y = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateY = new DoubleAnimation(0, -y.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            trans = new TranslateTransform();
            from = rectangleChildren[6];
            from.RenderTransform = trans;
            to = holder;
            y = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            from = to;
            from.RenderTransform = trans;
            to = rectangleChildren[1];
            x = from.TranslatePoint(new Point(from.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            animateX.Completed += new EventHandler(OnAnimationCompletition);
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            

        }

        private void RotateQuad1CounterClockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;
        }

        private void RotateQuad2Clockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;
        }
        private void RotateQuad2CounterClockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;
        }

        private void RotateQuad3Clockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;
        }

        private void RotateQuad3CounterClockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;
        }

        private void RotateQuad4Clockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;
        }

        private void RotateQuad4CounterClockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;
        }



        /*
        Rectangle rect0 = Slot0;
        Rectangle rect2 = Slot2;
        Point x = rect2.TranslatePoint(new Point(rect2.ActualWidth, 0), Board);

        rect0.RenderTransform = trans;
        DoubleAnimation anim1 = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
        trans.BeginAnimation(TranslateTransform.XProperty, anim1);


        TranslateTransform trans1 = new TranslateTransform();
        Rectangle rect14 = Slot14;
        Rectangle rect12 = Slot12;

        x = rect12.TranslatePoint(new Point(rect12.ActualWidth*2, 0), Board);

        rect14.RenderTransform = trans1;
        DoubleAnimation anim2 = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
        trans1.BeginAnimation(TranslateTransform.XProperty, anim2);
         */

        //DoubleAnimation anim2 = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
        //trans.BeginAnimation(TranslateTransform.YProperty, anim1);
        /*
        Point relativePoint = rect0.TransformToVisual(Board).Transform(new Point(0, 0));
        Point x = rect2.TranslatePoint(new Point(rect2.ActualWidth, 0), Board);

        DoubleAnimation animateX = new DoubleAnimation(x.X, new Duration(TimeSpan.FromSeconds(10)));
        //DoubleAnimation animateY = new DoubleAnimation(Canvas.GetLeft(rect0), relativePoint.Y, new Duration(TimeSpan.FromSeconds(10)));

        rect0.BeginAnimation(Canvas.LeftProperty, animateX);
        //rect0.BeginAnimation(Canvas.TopProperty, animateY);


            
        //TranslateTransform translate = new TranslateTransform(x, y);
        */

        //private void InitiateRotation(bool rotateClockwise, short quad)
        //{

        //    SoundManager.playSFX(SoundManager.SoundType.Rotate);
        //    gameBrain.RotateBoard(rotateClockwise, quad);
        //    RePaintBoard();
        //    MakeRotationsHidden();
        //}

        private void sendNetworkMove(short pos, bool isClockwise, short quad)
        {
            networkUtil.SendMove(quad, pos, isClockwise);
            RePaintBoard();
        }

        private void btnCounterClockWise2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(false, 2);
            if (isNetwork)
            {
                sendNetworkMove(movePos, false, 2);
            }
        }

        private void btnClockWise1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(true, 1);
            if (isNetwork)
            {
                sendNetworkMove(movePos, true, 1);
            }
        }

        private void btnCounterClockWise1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(false, 1);
            if (isNetwork)
            {
                sendNetworkMove(movePos, false, 1);
            }
        }

        private void btnClockWise2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(true, 2);
            if (isNetwork)
            {
                sendNetworkMove(movePos, true, 2);
            }
        }

        private void btnClockWise3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(true, 3);
            if (isNetwork)
            {
                sendNetworkMove(movePos, true, 3);
            }
        }

        private void btnCounterClockWise3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(false, 3);
            if (isNetwork)
            {
                sendNetworkMove(movePos, false, 3);
            }
        }

        private void btnClockWise4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(true, 4);
            if (isNetwork)
            {
                sendNetworkMove(movePos, true, 4);
            }
        }

        private void btnCounterClockWise4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            InitiateRotation(false, 4);
            if (isNetwork)
            {
                sendNetworkMove(movePos, false, 4);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Window mainWindow = new MainMenu();
            App.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Hide();
            if (networkUtil != null)
            {
                networkUtil.stop();
            }
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

            const string message = "Are you sure you want to exit the game?";
            const string caption = "Dragon Horde";
            //MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            MessageWindow messageWindow = new MessageWindow(message, MessageBoxButton.YesNo);
            messageWindow.ShowDialog();

            if (messageWindow.DialogResult == true)//result == MessageBoxResult.Yes)
            {
                Window mainWindow = new MainMenu();
                App.Current.MainWindow = mainWindow;
                mainWindow.Show();
                this.Hide();
                if (networkUtil != null)
                {
                    networkUtil.stop();
                }
            }
        }

        private void ExitButton_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }
        private int SpeechCounter = 0;
        private void VikingButton_Click(object sender, RoutedEventArgs e)
        {
            SpeechBubble.Visibility = Visibility.Visible;
            BubbleText.Visibility = Visibility.Visible;
            if (SpeechCounter == 17)
            {
                SpeechCounter = 0;
            }
            BubbleText.Text = VikingSpeechChanger(SpeechCounter);
            SpeechCounter++;

            Storyboard storyboard = new Storyboard();
            TimeSpan duration = new TimeSpan(0, 0, 15);

            DoubleAnimation fade = new DoubleAnimation();

            fade.From = 1.0;
            fade.To = 0.0;
            fade.Duration = new Duration(duration);

            DoubleAnimation fade2 = new DoubleAnimation();

            fade2.From = 1.0;
            fade2.To = 0.0;
            fade2.Duration = new Duration(duration);

            Storyboard.SetTargetName(fade, "SpeechBubble");
            Storyboard.SetTargetProperty(fade, new PropertyPath(Control.OpacityProperty));
            storyboard.Children.Add(fade);
            Storyboard.SetTargetName(fade2, "BubbleText");
            Storyboard.SetTargetProperty(fade2, new PropertyPath(Control.OpacityProperty));
            storyboard.Children.Add(fade2);

            storyboard.Begin(this);
        }
        private string VikingSpeechChanger(int i)
        {
            string s = "horgis borgis njord";
            switch (i)
            {
                case 0: s = "Ice giants are nothing to dragon fire";
                    break;
                case 1: s = "What are your orders Warlord?";
                    break;
                case 2: s = "Your dragons are restless";
                    break;
                case 3: s = "CHARRRGGGEE!!!!";
                    break;
                case 4: s = "The ice giants will stop at nothing to freeze this world";
                    break;
                case 5: s = "I hope my wife isn't cooking freijord gopher again. YUCK!";
                    break;
                case 6: s = "That's an...interesting strategy my Lord";
                    break;
                case 7: s = "Stop poking me ya over sized chipmunk";
                    break;
                case 8: s = "The ice giants can't handle your attack keep it up!";
                    break;
                case 9: s = "Sometime's I wear my wife's clothing when she isn't home";
                    break;
                case 10: s = "It is a great honor to defend the world from a icy death";
                    break;
                case 11: s = "Ice giant heart's are as cold as they are dark";
                    break;
                case 12: s = "I once defeated 3 ice giants only using my dagger and an ill tempered bunny";
                    break;
                case 13: s = "You truly are our greatest strategist!";
                    break;
                case 14: s = "I'm going to poke you back with my sword if you don't stop that";
                    break;
                case 15: s = "YOLO. Yodeling on Large Oxen";
                    break;
                case 16: s = "SWAG...Sword, War Axe, Grunting";
                    break;

            }
            return s;
        }

        private void VikingButton_Mousedown(object sender, EventArgs e)
        {
            //Storyboard speechFadeOut = (Storyboard)FindResource("FadeOut");
            //speechFadeOut.Begin();



        }
    }
}
