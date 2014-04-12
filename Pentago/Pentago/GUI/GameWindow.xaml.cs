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
using System.IO;
using System.Windows.Interop;

namespace Pentago
{
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

        private Point vikingArmPivot;
        private Point zero;
        private Point topRight;
        private Point iceGiantArmPivot;

        private bool isAnimationExecuting = false;
        private bool isAnimationEnterExecuting = false;

        private Quotes quotes;
        bool lockSword = false;
        bool lockClub = true;

        Image currentDragon;
        bool fireDragon = false;

        Point fireDragon1Origin;
        Point fireDragon2Origin;
        Point fireDragon3Origin;
        Point fireDragon4Origin;
        Point fireDragon5Origin;
        Point fireDragon6Origin;

        Point iceDragon1Origin;
        Point iceDragon2Origin;
        Point iceDragon3Origin;
        Point iceDragon4Origin;
        Point iceDragon5Origin;
        Point iceDragon6Origin;

        public SolidColorBrush gridOutline = new SolidColorBrush(Color.FromArgb(255, 56, 56, 56));

        public GameWindow(GameOptions options)
        {
            InitializeComponent();
            CreateChildrenList();
            quotes = new Quotes();
            SoundManager.backgroundMusicPlayer.Open(new Uri("GUI/Sounds/Gameplay.mp3", UriKind.Relative));
            SoundManager.backgroundMusicPlayer.Play();
            gameOptions = options;
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
            if(File.Exists(@"GUI\Images\CustomVikings\" + player1.Name + ".png"))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(@"GUI\Images\CustomVikings\" + player1.Name + ".png");
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img);
                BitmapSource bmpSrc = Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), 
                    IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                bmp.Dispose();
                VikingButton.Background = new ImageBrush(bmpSrc);
            }
            vikingArmPivot = new Point(167 + 40, this.Height - 420 + 121);
            zero = new Point(0, 0);
            topRight = new Point(Width, 0);
            iceGiantArmPivot = new Point(Width - 261, Height - 600);

            unMuteMusicVol = SoundManager.musicVolume / 16;
            unMuteSoundVol = SoundManager.sfxVolume / 16;
            currentMusicVol = SoundManager.musicVolume / 16;
            currentSoundVol = SoundManager.sfxVolume / 16;
            restoreMusicVol(currentMusicVol);
            restoreSoundVol(currentSoundVol);

            Stream cur = File.OpenRead("GUI/images/MouseArrow.cur");
            this.Cursor = new Cursor(cur);

            InitializeDragonOrigins();
            MakeDragonsVisble();
        }

        private void InitializeDragonOrigins()
        {
            fireDragon1Origin = new Point(fireDragonEntryImages[0].Margin.Left, fireDragonEntryImages[0].Margin.Top);
            fireDragon2Origin = new Point(fireDragonEntryImages[1].Margin.Left, fireDragonEntryImages[1].Margin.Top);
            fireDragon3Origin = new Point(fireDragonEntryImages[2].Margin.Left, fireDragonEntryImages[2].Margin.Top);
            fireDragon4Origin = new Point(fireDragonEntryImages[3].Margin.Left, fireDragonEntryImages[3].Margin.Top);
            fireDragon5Origin = new Point(fireDragonEntryImages[4].Margin.Left, fireDragonEntryImages[4].Margin.Top);
            fireDragon6Origin = new Point(fireDragonEntryImages[5].Margin.Left, fireDragonEntryImages[5].Margin.Top);

            iceDragon1Origin = new Point(fireDragonEntryImages[0].Margin.Left, fireDragonEntryImages[0].Margin.Top);
            iceDragon2Origin = new Point(fireDragonEntryImages[1].Margin.Left, fireDragonEntryImages[1].Margin.Top);
            iceDragon3Origin = new Point(fireDragonEntryImages[2].Margin.Left, fireDragonEntryImages[2].Margin.Top);
            iceDragon4Origin = new Point(fireDragonEntryImages[3].Margin.Left, fireDragonEntryImages[3].Margin.Top);
            iceDragon5Origin = new Point(fireDragonEntryImages[4].Margin.Left, fireDragonEntryImages[4].Margin.Top);
            iceDragon6Origin = new Point(fireDragonEntryImages[5].Margin.Left, fireDragonEntryImages[5].Margin.Top);
        }

        private List<Rectangle> rectangleChildren = null;
        private List<Image> fireDragonEntryImages;
        private List<Image> iceDragonEntryImages;
        private void CreateChildrenList()
        {
            rectangleChildren = new List<Rectangle>();
            var rectangles = Board.Children;
            foreach (Rectangle element in rectangles)
            {
                rectangleChildren.Add(element);
            }

            fireDragonEntryImages = new List<Image>();
            var fireDragons = fireDragonsStackPanel.Children;
            foreach (Image dragon in fireDragons)
            {
                fireDragonEntryImages.Add(dragon);
            }

            iceDragonEntryImages = new List<Image>();
            var iceDragons = iceDragonsStackPanel.Children;
            foreach(Image dragon in iceDragons)
            {
                iceDragonEntryImages.Add(dragon);
            }
        }

        private void Board_MouseMove(object sender, MouseEventArgs e)
        {
            if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch || player1.ActivePlayer && !isAnimationExecuting)
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

        private short col;
        private short row;
        private void Board_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch || player1.ActivePlayer && !isAnimationExecuting)
            {
                lockSword = true;
                int rectSize = (int)Board.Width / MAXCOLUMNS;

                Point mousePosition = e.GetPosition(Board);
                row = (short)(mousePosition.Y / rectSize);
                if (row == 6)
                    row--;
                col = (short)(mousePosition.X / rectSize);
                if (col == 6)
                    col--;
                int winner = gameBrain.CheckForWin();
                movePos = 0;
                movePos += col;
                movePos += (short)(row * 6);
                RePaintBoard();
                if (userMadeRotation && winner == 0 && gameBrain.PlacePiece(row, col))
                {
                    
                    SoundManager.playSFX(SoundManager.SoundType.Click);
                    TranslateTransform translate = new TranslateTransform();
                    DoubleAnimation enter;
                    Point targetPoint;
                    userMadeRotation = false;
                    var element = MAXCOLUMNS * row + col;
                    Rectangle rec = rectangleChildren.ElementAt(MAXCOLUMNS * row + col);
                    targetPoint = rec.TranslatePoint(new Point(rec.ActualWidth, 0), Board);
                    if (gameBrain.isPlayer1Turn())
                    {
                        fireDragon = true;
                        currentDragon = fireDragonEntryImages[row];
                        currentDragon.RenderTransform = translate;
                        enter = new DoubleAnimation(0, GetFireAnimationDestination(element, targetPoint), TimeSpan.FromSeconds(1));
                        //rec.Fill = player1.Image;
                    }
                    else
                    {
                        fireDragon = false;
                        currentDragon = iceDragonEntryImages[row];
                        currentDragon.RenderTransform = translate;
                        enter = new DoubleAnimation(0, -GetIceAnimationDestination(element), TimeSpan.FromSeconds(1));
                        //rec.Fill = player2.Image;
                    }
                    enter.Completed += new EventHandler(OnAnimationEnterCompletition);
                    isAnimationEnterExecuting = true;
                    translate.BeginAnimation(TranslateTransform.XProperty, enter);
                    
                }
                else if (winner != 0)
                    ShowWinner(winner);
            }

        }

        private void MakeDragonsVisble()
        {
            foreach (Image dragon in fireDragonEntryImages)
            {
                dragon.Visibility = Visibility.Visible;
            }

            foreach (Image dragon in iceDragonEntryImages)
            {
                dragon.Visibility = Visibility.Visible;
            }
        }

        private void MakeDragonsVisble(object sender, EventArgs e)
        {
            foreach ( Image dragon in fireDragonEntryImages)
            {
                dragon.Visibility = Visibility.Visible;
            }

            foreach (Image dragon in iceDragonEntryImages)
            {
                dragon.Visibility = Visibility.Visible;
            }
        }

        private void ReturnDragon()
        {
            TranslateTransform translate = new TranslateTransform();
            DoubleAnimation exit;
            var element = MAXCOLUMNS * row + col;
            Rectangle rec = rectangleChildren.ElementAt(MAXCOLUMNS * row + col);
            currentDragon.RenderTransform = translate;

            if (fireDragon)
            {
                exit = new DoubleAnimation(0, -GetFireAnimationDestination(element, new Point(-1, 0)), TimeSpan.FromSeconds(1));               
            }
            else
            {
                exit = new DoubleAnimation(0, GetIceAnimationDestination(element), TimeSpan.FromSeconds(1));
            }
            exit.Completed += new EventHandler(MakeDragonsVisble);

            exit.BeginAnimation(TranslateTransform.XProperty, exit);
        }

        private void OnAnimationEnterCompletition(object sender, EventArgs e)
        {
            isAnimationEnterExecuting = false;
            currentDragon.Visibility = Visibility.Hidden;
            Rectangle rec = rectangleChildren.ElementAt(MAXCOLUMNS * row + col);
            if (gameBrain.isPlayer1Turn())
                rec.Fill = player1.Image;
            else
                rec.Fill = player2.Image;

            int winner = gameBrain.CheckForWin();
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

            ReturnDragon();
        }

        private double GetFireAnimationDestination(int element, Point point)
        {
            double destination = 0;
            while (element > 5)
            {
                element -= 6;
            }

            switch (element)
            {
                case 0:
                    destination = (point.X * 7.0) - 22;
                    break;
                case 1:
                    destination = (point.X * 4.0) - 22;
                    break;
                case 2:
                    destination = (point.X * 3.0) - 22;
                    break;
                case 3:
                    destination = (point.X * 2.5) - 22;
                    break;
                case 4:
                    destination = (point.X * 2.2) - 22;
                    break;
                case 5:
                    destination = (point.X * 2) - 22;
                    break;
            }

            return destination;
        }

        private double GetIceAnimationDestination(int element)
        {
            Point point;
            double destination = 0;
            while (element > 5)
            {
                element -= 6;
            }
            Rectangle rec = rectangleChildren.ElementAt(MirrorBoard(element));
            point = rec.TranslatePoint(new Point(rec.ActualWidth, 0), Board);
            
            switch (element)
            {
                case 5:
                    destination = (point.X * 7.0) - 22;
                    break;
                case 4:
                    destination = (point.X * 4.0) - 22;
                    break;
                case 3:
                    destination = (point.X * 3.0) - 22;
                    break;
                case 2:
                    destination = (point.X * 2.5) - 22;
                    break;
                case 1:
                    destination = (point.X * 2.2) - 22;
                    break;
                case 0:
                    destination = (point.X * 2.0) - 22;
                    break;
            }

            return destination;
        }

        private int MirrorBoard(int value)
        {
            int result = 0;

            switch (value)
            {
                case 0:
                    result = 5;
                    break;
                case 1:
                    result = 4;
                    break;
                case 2:
                    result = 3;
                    break;
                case 3:
                    result = 2;
                    break;
                case 4:
                    result = 1;
                    break;
                case 5:
                    result = 0;
                    break;
            }

            return result;
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
                        winnerText = "The " + computerPlayer.Name + " has won.";
                    else if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.Network)
                        winnerText = player2.Name + " has defeated you.";
                    break;
                case 3:
                    winnerText = "It is a tie.";
                    break;
                default:
                    break;
            }

            winnerAnnoucement.Text = winnerText;

            TranslateTransform translate;
            DoubleAnimation exitAttack;
            foreach (Point point in gameBrain.GetWinningPoints)
            {
                translate = new TranslateTransform();
                exitAttack = new DoubleAnimation();
                Rectangle rec = rectangleChildren.ElementAt(MAXCOLUMNS * (short)point.X + (short)point.Y);
                rec.RenderTransform = translate;
                exitAttack = new DoubleAnimation(0, fireDragon?3:-1, TimeSpan.FromSeconds(1));
                //translate.BeginAnimation(TranslateTransform.XProperty, exitAttack);
            }
            
        }

        private void ShowActivePlayer()
        {
            if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.QuickMatch || gameOptions._TypeOfGame == GameOptions.TypeOfGame.Network)
            {
                if (player1.ActivePlayer)
                {
                    ActivePlayer.Fill = player1.Image;
                    ActiveTurnText.Text = player1.Name;
                    lockSword = false;
                }
                else
                {
                    ActivePlayer.Fill = player2.Image;
                    ActiveTurnText.Text = player2.Name;
                }
                    
            }
            else if (gameOptions._TypeOfGame == GameOptions.TypeOfGame.AI)
            {
                if (player1.ActivePlayer)
                {
                    ActivePlayer.Fill = player1.Image;
                    ActiveTurnText.Text = player1.Name;
                    lockSword = false;
                }
                else
                {
                    ActivePlayer.Fill = computerPlayer.Image;
                    ActiveTurnText.Text = computerPlayer.Name;
                }
                    
            }
        }

        private void RePaintBoard()
        {
            if (!isAnimationExecuting && !isAnimationEnterExecuting)
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
                rec.Stroke = gridOutline;
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

        private void MakeRotationsHidden()
        {
            btnClockWise1.Visibility = Visibility.Hidden;
            btnCounterClockWise1.Visibility = Visibility.Hidden;
            btnClockWise2.Visibility = Visibility.Hidden;
            btnCounterClockWise2.Visibility = Visibility.Hidden;
            btnClockWise3.Visibility = Visibility.Hidden;
            btnCounterClockWise3.Visibility = Visibility.Hidden;
            btnClockWise4.Visibility = Visibility.Hidden;
            btnCounterClockWise4.Visibility = Visibility.Hidden;
        }

        private void ChangeTurnOnGUI()
        {
            userMadeRotation = true;

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
                    TranslateTransform translate = new TranslateTransform();
                    DoubleAnimation enter;
                    Point targetPoint;
                    //userMadeRotation = false;
                    var element = gameBrain.GetComputerMove();
                    Rectangle rec = rectangleChildren.ElementAt(gameBrain.GetComputerMove());
                    targetPoint = rec.TranslatePoint(new Point(rec.ActualWidth, 0), Board);
                    if (gameBrain.isPlayer1Turn())
                    {
                        fireDragon = true;
                        currentDragon = fireDragonEntryImages[row];
                        currentDragon.RenderTransform = translate;
                        enter = new DoubleAnimation(0, GetFireAnimationDestination(element, targetPoint), TimeSpan.FromSeconds(1));
                    }
                    else
                    {
                        fireDragon = false;
                        int computerRow = gameBrain.GetComputerMove() / 6;
                        currentDragon = iceDragonEntryImages[computerRow];
                        currentDragon.RenderTransform = translate;
                        enter = new DoubleAnimation(0, -GetIceAnimationDestination(element), TimeSpan.FromSeconds(1));
                    }
                    enter.Completed += new EventHandler(OnAnimationEnterComputerCompletition);
                    isAnimationEnterExecuting = true;
                    translate.BeginAnimation(TranslateTransform.XProperty, enter);
                }
                else if (winner != 0)
                {
                    RePaintBoard();
                    ShowWinner(winner);
                }

            };
            AIbackgroundWorker.RunWorkerAsync();
        }

        private void OnAnimationEnterComputerCompletition(object sender, EventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.Click);
            isAnimationEnterExecuting = false;
            currentDragon.Visibility = Visibility.Hidden;
            Rectangle rec = rectangleChildren.ElementAt(gameBrain.GetComputerMove());
            if (gameBrain.isPlayer1Turn())
                rec.Fill = player1.Image;
            else
                rec.Fill = computerPlayer.Image;
            int winner = gameBrain.CheckForWin();
            if (winner != 0)
                ShowWinner(winner);
            else
                GetComputerRotation();

            ReturnDragon();
        }

        private void GetComputerRotation()
        {
            for (int i = 1; i <= 2; i++)
                gameBrain.MakeComputerRotation(i);

            int[] computerRotation = gameBrain.GetComputerRotation();
            short quad = (short)computerRotation[0];
            bool isClockWise;
            if (computerRotation[1] == 1)
                isClockWise = true;
            else
                isClockWise = false;

            SoundManager.playSFX(SoundManager.SoundType.Rotate);
            RotateAnimation(quad, isClockWise);
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
            isAnimationExecuting = true;
            switch (quad)
            {
                case 1:
                    if (rotateClockwise)
                        RotateQuad1Clockwise();
                    else
                        RotateQuad1CounterClockwise();
                    break;

                case 2:
                    if (rotateClockwise)
                        RotateQuad2Clockwise();
                    else
                        RotateQuad2CounterClockwise();
                    break;

                case 3:
                    if (rotateClockwise)
                        RotateQuad3Clockwise();
                    else
                        RotateQuad3CounterClockwise();
                    break;

                case 4:
                    if (rotateClockwise)
                        RotateQuad4Clockwise();
                    else
                        RotateQuad4CounterClockwise();
                    break;
            }
            MakeRotationsHidden();
        }

        private void OnAnimationCompletition(object sender, EventArgs e)
        {
            RemoveGrid();
            RecreateGrid();
            ChangeTurnOnGUI();
            isAnimationExecuting = false;
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

            //0
            trans = new TranslateTransform();
            from = rectangleChildren[0];
            from.RenderTransform = trans;
            to = rectangleChildren[1];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //1
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

            //2
            trans = new TranslateTransform();
            from = rectangleChildren[2];
            from.RenderTransform = trans;
            to = rectangleChildren[14];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //8
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

            //14
            trans = new TranslateTransform();
            from = rectangleChildren[14];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //13
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

            //12
            trans = new TranslateTransform();
            from = rectangleChildren[12];
            from.RenderTransform = trans;
            to = holder;
            y = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateY = new DoubleAnimation(0, -y.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //6
            trans = new TranslateTransform();
            from = rectangleChildren[6];
            from.RenderTransform = trans;
            to = holder;
            y = from.TranslatePoint(new Point(from.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            from.RenderTransform = trans;
            to = rectangleChildren[1];
            x = holder.TranslatePoint(new Point(holder.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            animateX.Completed += new EventHandler(OnAnimationCompletition);
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);
        }

        private void RotateQuad1CounterClockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            Rectangle holder = rectangleChildren.ElementAt(0);
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;

            //0
            trans = new TranslateTransform();
            from = rectangleChildren[0];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //1
            trans = new TranslateTransform();
            from = rectangleChildren[1];
            from.RenderTransform = trans;
            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[6];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //2
            trans = new TranslateTransform();
            from = rectangleChildren[2];
            from.RenderTransform = trans;
            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth*2, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //8
            trans = new TranslateTransform();
            from = rectangleChildren[8];
            from.RenderTransform = trans;
            to = rectangleChildren[2];
            y = from.TranslatePoint(new Point(from.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //14
            trans = new TranslateTransform();
            from = rectangleChildren[14];
            from.RenderTransform = trans;
            to = holder;
            y = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateY = new DoubleAnimation(0, -y.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //13
            trans = new TranslateTransform();
            from = rectangleChildren[13];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[8];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //12
            trans = new TranslateTransform();
            from = rectangleChildren[12];
            from.RenderTransform = trans;
            to = rectangleChildren[13];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //6
            trans = new TranslateTransform();
            from = rectangleChildren[6];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            y = from.TranslatePoint(new Point(from.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            
            from.RenderTransform = trans;
            to = rectangleChildren[13];
            x = holder.TranslatePoint(new Point(holder.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            animateX.Completed += new EventHandler(OnAnimationCompletition);
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);
        }

        private void RotateQuad2Clockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            Rectangle holder = rectangleChildren.ElementAt(0);
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;

            //3
            trans = new TranslateTransform();
            from = rectangleChildren[3];
            from.RenderTransform = trans;
            to = rectangleChildren[1];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //4
            trans = new TranslateTransform();
            from = rectangleChildren[4];
            from.RenderTransform = trans;
            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[8];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //5
            trans = new TranslateTransform();
            from = rectangleChildren[5];
            from.RenderTransform = trans;
            to = rectangleChildren[14];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //11
            trans = new TranslateTransform();
            from = rectangleChildren[11];
            from.RenderTransform = trans;
            to = rectangleChildren[14];
            y = from.TranslatePoint(new Point(from.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //17
            trans = new TranslateTransform();
            from = rectangleChildren[17];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //16
            trans = new TranslateTransform();
            from = rectangleChildren[16];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[6];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //15
            trans = new TranslateTransform();
            from = rectangleChildren[15];
            from.RenderTransform = trans;
            to = holder;
            y = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateY = new DoubleAnimation(0, -y.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //9
            trans = new TranslateTransform();
            from = rectangleChildren[9];
            from.RenderTransform = trans;
            to = holder;
            y = from.TranslatePoint(new Point(from.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            from.RenderTransform = trans;
            to = rectangleChildren[1];
            x = holder.TranslatePoint(new Point(holder.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            animateX.Completed += new EventHandler(OnAnimationCompletition);
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);
        }
        private void RotateQuad2CounterClockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            Rectangle holder = rectangleChildren.ElementAt(0);
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;

            //3
            trans = new TranslateTransform();
            from = rectangleChildren[3];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //4
            trans = new TranslateTransform();
            from = rectangleChildren[4];
            from.RenderTransform = trans;
            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[6];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //5
            trans = new TranslateTransform();
            from = rectangleChildren[5];
            from.RenderTransform = trans;
            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //11
            trans = new TranslateTransform();
            from = rectangleChildren[11];
            from.RenderTransform = trans;
            to = rectangleChildren[2];
            y = from.TranslatePoint(new Point(from.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //17
            trans = new TranslateTransform();
            from = rectangleChildren[17];
            from.RenderTransform = trans;
            to = holder;
            y = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateY = new DoubleAnimation(0, -y.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //16
            trans = new TranslateTransform();
            from = rectangleChildren[16];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[8];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //15
            trans = new TranslateTransform();
            from = rectangleChildren[15];
            from.RenderTransform = trans;
            to = rectangleChildren[13];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //9
            trans = new TranslateTransform();
            from = rectangleChildren[9];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            y = from.TranslatePoint(new Point(from.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            from.RenderTransform = trans;
            to = rectangleChildren[13];
            x = holder.TranslatePoint(new Point(holder.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            animateX.Completed += new EventHandler(OnAnimationCompletition);
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);
        }

        private void RotateQuad3Clockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            Rectangle holder = rectangleChildren.ElementAt(0);
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;

            //18
            trans = new TranslateTransform();
            from = rectangleChildren[18];
            from.RenderTransform = trans;
            to = rectangleChildren[1];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //19
            trans = new TranslateTransform();
            from = rectangleChildren[19];
            from.RenderTransform = trans;
            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[8];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //20
            trans = new TranslateTransform();
            from = rectangleChildren[20];
            from.RenderTransform = trans;
            to = rectangleChildren[14];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //26
            trans = new TranslateTransform();
            from = rectangleChildren[26];
            from.RenderTransform = trans;
            to = rectangleChildren[8];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //32
            trans = new TranslateTransform();
            from = rectangleChildren[32];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //31
            trans = new TranslateTransform();
            from = rectangleChildren[31];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[6];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //30
            trans = new TranslateTransform();
            from = rectangleChildren[30];
            from.RenderTransform = trans;
            to = holder;
            y = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateY = new DoubleAnimation(0, -y.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //24
            trans = new TranslateTransform();
            from = rectangleChildren[24];
            from.RenderTransform = trans;
            to = rectangleChildren[6];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

       
            from.RenderTransform = trans;
            to = rectangleChildren[1];
            x = holder.TranslatePoint(new Point(holder.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            animateX.Completed += new EventHandler(OnAnimationCompletition);
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);
        }

        private void RotateQuad3CounterClockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            Rectangle holder = rectangleChildren.ElementAt(0);
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;

            //18
            trans = new TranslateTransform();
            from = rectangleChildren[18];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //19
            trans = new TranslateTransform();
            from = rectangleChildren[19];
            from.RenderTransform = trans;
            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[6];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //20
            trans = new TranslateTransform();
            from = rectangleChildren[20];
            from.RenderTransform = trans;
            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //26
            trans = new TranslateTransform();
            from = rectangleChildren[26];
            from.RenderTransform = trans;
            to = rectangleChildren[8];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //32
            trans = new TranslateTransform();
            from = rectangleChildren[32];
            from.RenderTransform = trans;
            to = holder;
            y = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateY = new DoubleAnimation(0, -y.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //31
            trans = new TranslateTransform();
            from = rectangleChildren[31];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[8];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //30
            trans = new TranslateTransform();
            from = rectangleChildren[30];
            from.RenderTransform = trans;
            to = rectangleChildren[13];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //24
            trans = new TranslateTransform();
            from = rectangleChildren[24];
            from.RenderTransform = trans;
            to = rectangleChildren[6];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            from.RenderTransform = trans;
            to = rectangleChildren[13];
            x = from.TranslatePoint(new Point(from.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            animateX.Completed += new EventHandler(OnAnimationCompletition);
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);
        }

        private void RotateQuad4Clockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            Rectangle holder = rectangleChildren.ElementAt(0);
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;

            //21
            trans = new TranslateTransform();
            from = rectangleChildren[21];
            from.RenderTransform = trans;
            to = rectangleChildren[1];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //22
            trans = new TranslateTransform();
            from = rectangleChildren[22];
            from.RenderTransform = trans;
            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[8];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //23
            trans = new TranslateTransform();
            from = rectangleChildren[23];
            from.RenderTransform = trans;
            to = rectangleChildren[14];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //29
            trans = new TranslateTransform();
            from = rectangleChildren[29];
            from.RenderTransform = trans;
            to = rectangleChildren[8];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //35
            trans = new TranslateTransform();
            from = rectangleChildren[35];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //34
            trans = new TranslateTransform();
            from = rectangleChildren[34];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[6];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //33
            trans = new TranslateTransform();
            from = rectangleChildren[33];
            from.RenderTransform = trans;
            to = holder;
            y = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateY = new DoubleAnimation(0, -y.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //27
            trans = new TranslateTransform();
            from = rectangleChildren[27];
            from.RenderTransform = trans;
            to = rectangleChildren[6];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);


            from.RenderTransform = trans;
            to = rectangleChildren[1];
            x = holder.TranslatePoint(new Point(holder.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            animateX.Completed += new EventHandler(OnAnimationCompletition);
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);
        }

        private void RotateQuad4CounterClockwise()
        {
            TranslateTransform trans;
            Rectangle from;
            Rectangle to;
            Rectangle holder = rectangleChildren.ElementAt(0);
            DoubleAnimation animateX;
            DoubleAnimation animateY;
            Point x;
            Point y;

            //21
            trans = new TranslateTransform();
            from = rectangleChildren[21];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //22
            trans = new TranslateTransform();
            from = rectangleChildren[22];
            from.RenderTransform = trans;
            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[6];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //23
            trans = new TranslateTransform();
            from = rectangleChildren[23];
            from.RenderTransform = trans;
            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //29
            trans = new TranslateTransform();
            from = rectangleChildren[29];
            from.RenderTransform = trans;
            to = rectangleChildren[8];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            to = rectangleChildren[0];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, -x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //35
            trans = new TranslateTransform();
            from = rectangleChildren[35];
            from.RenderTransform = trans;
            to = holder;
            y = to.TranslatePoint(new Point(to.ActualWidth * 2, 0), Board);
            animateY = new DoubleAnimation(0, -y.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //34
            trans = new TranslateTransform();
            from = rectangleChildren[34];
            from.RenderTransform = trans;
            to = rectangleChildren[12];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            to = rectangleChildren[8];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, -y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            //33
            trans = new TranslateTransform();
            from = rectangleChildren[33];
            from.RenderTransform = trans;
            to = rectangleChildren[13];
            x = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);

            //27
            trans = new TranslateTransform();
            from = rectangleChildren[27];
            from.RenderTransform = trans;
            to = rectangleChildren[6];
            y = to.TranslatePoint(new Point(to.ActualWidth, 0), Board);
            animateY = new DoubleAnimation(0, y.Y, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.YProperty, animateY);

            from.RenderTransform = trans;
            to = rectangleChildren[13];
            x = holder.TranslatePoint(new Point(holder.ActualWidth, 0), Board);
            animateX = new DoubleAnimation(0, x.X, TimeSpan.FromSeconds(1));
            animateX.Completed += new EventHandler(OnAnimationCompletition);
            trans.BeginAnimation(TranslateTransform.XProperty, animateX);
        }

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
            if (networkUtil != null)
            {
                networkUtil.stop();
            }
            //this.Hide();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            //SoundManager.playSFX(SoundManager.SoundType.Click);

            //const string message = "Are you sure you want to exit the game?";
            //MessageWindow messageWindow = new MessageWindow(message, MessageBoxButton.YesNo);
            //messageWindow.ShowDialog();

            //if (messageWindow.DialogResult == true)
            //{
            //    Window mainWindow = new MainMenu();
            //    App.Current.MainWindow = mainWindow;
            //    mainWindow.Show();
            //    if (networkUtil != null)
            //    {
            //        networkUtil.stop();
            //    }
            //    this.Close();
            //}
            Message.IsOpen = !Message.IsOpen;
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
            BubbleText.Text = quotes.Viking;
            //BubbleText.Text = VikingSpeechChanger(SpeechCounter);
            SpeechCounter++;

            Storyboard storyboard = new Storyboard();
            TimeSpan duration = new TimeSpan(0, 0, 8);

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

        }

        private void RotateSword(MouseEventArgs e)
        {
            Point mousePositionRelativeToWindow = e.GetPosition(this);
            //Console.WriteLine("Window: " + mousePositionRelativeToWindow.ToString());



            double a = Math.Sqrt((vikingArmPivot.X * vikingArmPivot.X) + (vikingArmPivot.Y * vikingArmPivot.Y));
            double b = Math.Sqrt(((vikingArmPivot.X - mousePositionRelativeToWindow.X) * (vikingArmPivot.X - mousePositionRelativeToWindow.X)) +
                ((vikingArmPivot.Y - mousePositionRelativeToWindow.Y) * (vikingArmPivot.Y - mousePositionRelativeToWindow.Y)));
            double c = Math.Sqrt((mousePositionRelativeToWindow.X * mousePositionRelativeToWindow.X) + (mousePositionRelativeToWindow.Y * mousePositionRelativeToWindow.Y));

            double angle = Math.Acos((a * a + b * b - c * c) / (2 * a * b)) * (180 / Math.PI);


            VikingButton_Sword.RenderTransform = new RotateTransform(angle - 66, 40, 121);
        }

        private void RotateClub(MouseEventArgs e)
        {
            Point mousePositionRelativeToWindow = e.GetPosition(this);
            //Console.WriteLine("Window: " + mousePositionRelativeToWindow.ToString());



            double a = Math.Sqrt(((topRight.X - iceGiantArmPivot.X) * (topRight.X - iceGiantArmPivot.X)) + ((topRight.Y + iceGiantArmPivot.Y) * (topRight.Y + iceGiantArmPivot.Y)));
            double b = Math.Sqrt(((iceGiantArmPivot.X - mousePositionRelativeToWindow.X) * (iceGiantArmPivot.X - mousePositionRelativeToWindow.X)) +
                ((iceGiantArmPivot.Y - mousePositionRelativeToWindow.Y) * (iceGiantArmPivot.Y - mousePositionRelativeToWindow.Y)));
            double c = Math.Sqrt(((topRight.X - mousePositionRelativeToWindow.X) * (topRight.X - mousePositionRelativeToWindow.X)) + ((topRight.Y + mousePositionRelativeToWindow.Y) * (topRight.Y + mousePositionRelativeToWindow.Y)));

            double angle = 0 - Math.Acos((a * a + b * b - c * c) / (2 * a * b)) * (180 / Math.PI);

            TransformGroup tran = new TransformGroup();
            tran.Children.Add(new RotateTransform (angle, 0,0));
            tran.Children.Add(new ScaleTransform(-1, 0));
            IceGiant_Arm.RenderTransform =  new RotateTransform(angle - 180, 100, 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window mainWindow = new MainMenu();
            App.Current.MainWindow = mainWindow;
            mainWindow.Show();
            if (networkUtil != null)
            {
                networkUtil.stop();
            }
            Message.IsOpen = false;
            this.Hide();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Message.IsOpen = false;
        }

        private void Game_MouseMove(object sender, MouseEventArgs e)
        {
            if (!lockSword)
            {
                RotateSword(e);
            }
            //RotateClub(e);

            //Point mousePositionRelativeToWindow = e.GetPosition(this);
            //TransformGroup t = new TransformGroup();
            //t.Children.Add(new TranslateTransform(mousePositionRelativeToWindow.X + 1, mousePositionRelativeToWindow.Y + 1));
            //Pointer.RenderTransform = t;
        }

        //private void Pointer_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    Pointer.Visibility = Visibility.Hidden;
        //}

        private void SoudMuteToggle_Click(object sender, MouseButtonEventArgs e)
        {
            SoundToggle_Click(sender, e);
        }

        private void MusicMuteToggle_Click(object sender, MouseButtonEventArgs e)
        {
            MusicToggle_Click(sender, e);
        }

        public void MusicToggle_Click(object sender, RoutedEventArgs e)
        {
            if (currentMusicVol == 0)
            {
                restoreMusicVol(unMuteMusicVol);
                currentMusicVol = unMuteMusicVol;
                MusicMuteToggle.Source = new BitmapImage(new Uri("pack://application:,,,/GUI/images/Unmute.png", UriKind.Absolute));
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
                MusicMuteToggle.Source = new BitmapImage(new Uri("pack://application:,,,/GUI/images/MuteLight.png", UriKind.Absolute));
            }
        }

        public void SoundToggle_Click(object sender, RoutedEventArgs e)
        {
            if (currentSoundVol == 0)
            {
                restoreSoundVol(unMuteSoundVol);
                currentSoundVol = unMuteSoundVol;
                SoundMuteToggle.Source = new BitmapImage(new Uri("pack://application:,,,/GUI/images/Unmute.png", UriKind.Absolute));
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
                SoundMuteToggle.Source = new BitmapImage(new Uri("pack://application:,,,/GUI/images/MuteLight.png", UriKind.Absolute));
            }
        }
        private void restoreMusicVol(int i)
        {
            switch (i)
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

        public int unMuteMusicVol = 6;
        public int currentMusicVol = 6;
        public int unMuteSoundVol = 6;
        public int currentSoundVol = 6;

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

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Yes_Button.Visibility = Visibility.Visible;
            No_Button.Visibility = Visibility.Visible;
            ConfirmLabel.Visibility = Visibility.Visible;
            Exit_Button.Visibility = Visibility.Hidden;
        }

        private void No_Button_Click(object sender, RoutedEventArgs e)
        {
            Yes_Button.Visibility = Visibility.Hidden;
            No_Button.Visibility = Visibility.Hidden;
            ConfirmLabel.Visibility = Visibility.Hidden;
            Exit_Button.Visibility = Visibility.Visible;
        }
    }
}
