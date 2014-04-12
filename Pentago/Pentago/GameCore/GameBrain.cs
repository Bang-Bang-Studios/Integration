using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using Pentago.AI;
using System.ComponentModel;

namespace Pentago.GameCore
{
    public class GameBrain
    {
        private Board board = null;

        //These are the player for the GameBrain
        private Player player1 = null;
        private computerAI player2;
        private const int MAXMOVES = 36;

        //Initializes a human vs human game
        public GameBrain(Player player1)
        {
            this.player1 = player1;
            InitializeBoard();
        }

        //Initializes a human vs computer game
        public GameBrain(Player player1, computerAI computerPlayer)
        {
            this.player1 = player1;
            this.player2 = computerPlayer;
            InitializeBoard();
        }

        public bool MakeComputerMove()
        {
            player2.MakeAIMove(board);
            if (PlacePiece(player2.GetMoveChoice()))
                return true;

            Console.WriteLine("Something went WRONG! :(");
            return false;
        }

        public int GetComputerMove()
        {
            return player2.GetMoveChoice();
        }

        private bool PlacePiece(int slot)
        {
            int player;
            if (player1.ActivePlayer)
                player = 1;
            else
                player = 2;

            if (ValidateMove(slot))
            {
                board.UpdateBoard(slot, player);
                return true;
            }
            else
                return false;
        }

        private bool ValidateMove(int slot)
        {
            if (board.GetPlayer(slot) == 0)
                return true;
            return false;
        }

        public void MakeComputerRotation(int rotation)
        {
            bool rotateClockWise = player2.GetRotationDirection();
            short quad = player2.GetCuadrant();
            RotateBoard(rotateClockWise, quad, rotation);
        }

        public int[] GetComputerRotation()
        {
            int[] rotation = new int[2];
            bool rotateClockWise = player2.GetRotationDirection();
            int rotationDirection;
            if (rotateClockWise)
                rotationDirection = 1;
            else
                rotationDirection = 0;

            short quad = player2.GetCuadrant();
            rotation[0] = (int)quad;
            rotation[1] = rotationDirection;
            return rotation;
        }

        private void InitializeBoard()
        {
            board = new Board();
            winningPoints = new List<Point>();
        }

        public bool PlacePiece(short row, short col)
        {
            int player;
            if (player1.ActivePlayer)
                player = 1;
            else
                player = 2;

            if (ValidateMove(row, col))
            {
                board.UpdateBoard(row, col, player);
                return true;
            }
            else
            {
                //Show the user thats a bad move
                return false;
            }
        }

        public bool PlacePieceByPos(short pos)
        {
            short row, col;
            row = (short)(pos / 6);
            col = (short)(pos % 6);
            return PlacePiece(row, col);
        }

        public void RotateBoard(bool rotateClockWise, short quad, int rotationNumber)
        {
            board.RotateQuad(rotateClockWise, quad);
            if (rotationNumber > 1)
            {
                ChangeTurn();
            }
        }

        //public void RotateBoard(bool rotateClockWise, short quad)
        //{
        //    switch (quad)
        //    {
        //        case 1:
        //            if (rotateClockWise)
        //                board.RotateQuad1ClockWise();
        //            else
        //                board.RotateQuad1CounterClockWise();
        //            break;
        //        case 2:
        //            if (rotateClockWise)
        //                board.RotateQuad2ClockWise();
        //            else
        //                board.RotateQuad2CounterClockWise();
        //            break;
        //        case 3:
        //            if (rotateClockWise)
        //                board.RotateQuad3ClockWise();
        //            else
        //                board.RotateQuad3CounterClockWise();
        //            break;
        //        case 4:
        //            if (rotateClockWise)
        //                board.RotateQuad4ClockWise();
        //            else
        //                board.RotateQuad4CounterClockWise();
        //            break;
        //        default:
        //            break;
        //    }
        //    ChangeTurn();
        //}

        private bool ValidateMove(short row, short col)
        {
            if (board.GetPlayer(row, col) == 0)
                return true;
            return false;
        }

        public bool isPlayer1Turn()
        {
            return player1.ActivePlayer;
        }

        private void ChangeTurn()
        {
            if (player1.ActivePlayer)
                player1.ActivePlayer = false;
            else
                player1.ActivePlayer = true;
        }

        public int[] GetBoard
        {
            get { return board.GetBoard; }
        }

        public int CheckForWin()
        {
            bool res = true;
            bool p1w = false;
            bool p2w = false;
            bool tie = false;
            int numMoves = board.PiecesOnBoard();

            if (numMoves >= 9) // First check to see if it's even possible to win (Fifth move for player 1)
            {
                // Check for horizontal win. If no win, continue to checking vert and diag.
                int horiz = CheckHorizontals();
                if (horiz == 0) // No one won on a horizontal. Check for verticals.
                {

                }
                else if (horiz == 1) // Player 1 won on a horizontal
                {
                    p1w = true;
                    res = false;
                }
                else if (horiz == 2) // Player 2 wins on a horizontal
                {
                    p2w = true;
                    res = false;
                }
                else
                {
                    tie = true;
                    res = false;
                }

                int vert = CheckVerticals();

                if (vert == 0) // No one won on a vertical. Check for diagonals.
                {

                }
                else if (vert == 1) // Player 1 won on a vertical
                {
                    p1w = true;
                    res = false;
                }
                else if (vert == 2) // Player 2 won on a vertical
                {
                    p2w = true;
                    res = false;
                }
                else // vert is 3 (A tie was caused by the move)
                {
                    tie = true;
                    res = false;
                }

                int diag = CheckDiags();
                if (diag == 0) // No one won on a diagonal. Check to see if it's possible to make more moves.
                {
                }
                else if (diag == 1) // Player 1 won on a diagonal
                {
                    p1w = true;
                    res = false;
                }
                else if (diag == 2) // Player 2 won on a diagonal
                {
                    p2w = true;
                    res = false;
                }
                else // diag is 3 (A tie was caused by the move)
                {
                    tie = true;
                    res = false;
                }


                if (res && numMoves < MAXMOVES)
                    return 0; // The game continues
                if (tie || (p1w && p2w))
                    return 3;
                if (p1w)
                    return 1;
                if (p2w)
                    return 2;
                if (numMoves == MAXMOVES)
                    return 3;
            }
            return 0;
        }

        private int CheckHorizontals()
        {
            bool res = true;
            bool p1w = false;
            bool p2w = false;

            int returnValue = 0;
            short[] possibilities = new short[12];
            possibilities[0] = (short)CheckPiecesOnBoard(new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(0, 3), new Point(0, 4));
            possibilities[1] = (short)CheckPiecesOnBoard(new Point(0, 1), new Point(0, 2), new Point(0, 3), new Point(0, 4), new Point(0, 5));
            possibilities[2] = (short)CheckPiecesOnBoard(new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(1, 3), new Point(1, 4));
            possibilities[3] = (short)CheckPiecesOnBoard(new Point(1, 1), new Point(1, 2), new Point(1, 3), new Point(1, 4), new Point(1, 5));
            possibilities[4] = (short)CheckPiecesOnBoard(new Point(2, 0), new Point(2, 1), new Point(2, 2), new Point(2, 3), new Point(2, 4));
            possibilities[5] = (short)CheckPiecesOnBoard(new Point(2, 1), new Point(2, 2), new Point(2, 3), new Point(2, 4), new Point(2, 5));
            possibilities[6] = (short)CheckPiecesOnBoard(new Point(3, 0), new Point(3, 1), new Point(3, 2), new Point(3, 3), new Point(3, 4));
            possibilities[7] = (short)CheckPiecesOnBoard(new Point(3, 1), new Point(3, 2), new Point(3, 3), new Point(3, 4), new Point(3, 5));
            possibilities[8] = (short)CheckPiecesOnBoard(new Point(4, 0), new Point(4, 1), new Point(4, 2), new Point(4, 3), new Point(4, 4));
            possibilities[9] = (short)CheckPiecesOnBoard(new Point(4, 1), new Point(4, 2), new Point(4, 3), new Point(4, 4), new Point(4, 5));
            possibilities[10] = (short)CheckPiecesOnBoard(new Point(5, 0), new Point(5, 1), new Point(5, 2), new Point(5, 3), new Point(5, 4));
            possibilities[11] = (short)CheckPiecesOnBoard(new Point(5, 1), new Point(5, 2), new Point(5, 3), new Point(5, 4), new Point(5, 5));

            foreach (short s in possibilities)
            {
                if (s == 1)
                {
                    p1w = true;
                    res = false;
                }
                if (s == 2)
                {
                    p2w = true;
                    res = false;
                }
            }

            if (res)
                return 0;
            if (p1w && p2w)
                return 3;
            if (p1w)
                return 1;
            if (p2w)
                return 2;
            return returnValue;
        }

        private int CheckVerticals()
        {
            bool res = true;
            bool p1w = false;
            bool p2w = false;

            int returnValue = 0;
            short[] possibilities = new short[12];

            possibilities[0] = (short)CheckPiecesOnBoard(new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0));
            possibilities[1] = (short)CheckPiecesOnBoard(new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0), new Point(5, 0));
            possibilities[2] = (short)CheckPiecesOnBoard(new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1), new Point(4, 1));
            possibilities[3] = (short)CheckPiecesOnBoard(new Point(1, 1), new Point(2, 1), new Point(3, 1), new Point(4, 1), new Point(5, 1));
            possibilities[4] = (short)CheckPiecesOnBoard(new Point(0, 2), new Point(1, 2), new Point(2, 2), new Point(3, 2), new Point(4, 2));
            possibilities[5] = (short)CheckPiecesOnBoard(new Point(1, 2), new Point(2, 2), new Point(3, 2), new Point(4, 2), new Point(5, 2));
            possibilities[6] = (short)CheckPiecesOnBoard(new Point(0, 3), new Point(1, 3), new Point(2, 3), new Point(3, 3), new Point(4, 3));
            possibilities[7] = (short)CheckPiecesOnBoard(new Point(1, 3), new Point(2, 3), new Point(3, 3), new Point(4, 3), new Point(5, 3));
            possibilities[8] = (short)CheckPiecesOnBoard(new Point(0, 4), new Point(1, 4), new Point(2, 4), new Point(3, 4), new Point(4, 4));
            possibilities[9] = (short)CheckPiecesOnBoard(new Point(1, 4), new Point(2, 4), new Point(3, 4), new Point(4, 4), new Point(5, 4));
            possibilities[10] = (short)CheckPiecesOnBoard(new Point(0, 5), new Point(1, 5), new Point(2, 5), new Point(3, 5), new Point(4, 5));
            possibilities[11] = (short)CheckPiecesOnBoard(new Point(1, 5), new Point(2, 5), new Point(3, 5), new Point(4, 5), new Point(5, 5));

            foreach (short s in possibilities)
            {
                if (s == 1)
                {
                    p1w = true;
                    res = false;
                }
                if (s == 2)
                {
                    p2w = true;
                    res = false;
                }
            }

            if (res)
                return 0;
            if (p1w && p2w)
                return 3;
            if (p1w)
                return 1;
            if (p2w)
                return 2;
            return returnValue;
        }

        private int CheckDiags()
        {
            bool res = true;
            bool p1w = false;
            bool p2w = false;

            int returnValue = 0;
            short[] possibilities = new short[8];

            // Top Left to Bottom Rights
            possibilities[0] = (short)CheckPiecesOnBoard(new Point(0, 1), new Point(1, 2), new Point(2, 3), new Point(3, 4), new Point(4, 5));
            possibilities[1] = (short)CheckPiecesOnBoard(new Point(0, 0), new Point(1, 1), new Point(2, 2), new Point(3, 3), new Point(4, 4));
            possibilities[2] = (short)CheckPiecesOnBoard(new Point(1, 1), new Point(2, 2), new Point(3, 3), new Point(4, 4), new Point(5, 5));
            possibilities[3] = (short)CheckPiecesOnBoard(new Point(1, 0), new Point(2, 1), new Point(3, 2), new Point(4, 3), new Point(5, 4));

            // Bottom Left to Top Rights
            possibilities[4] = (short)CheckPiecesOnBoard(new Point(0, 4), new Point(1, 3), new Point(2, 2), new Point(3, 1), new Point(4, 0));
            possibilities[5] = (short)CheckPiecesOnBoard(new Point(0, 5), new Point(1, 4), new Point(2, 3), new Point(3, 2), new Point(4, 1));
            possibilities[6] = (short)CheckPiecesOnBoard(new Point(1, 4), new Point(2, 3), new Point(3, 2), new Point(4, 1), new Point(5, 0));
            possibilities[7] = (short)CheckPiecesOnBoard(new Point(1, 5), new Point(2, 4), new Point(3, 3), new Point(4, 2), new Point(5, 1));

            foreach (short s in possibilities)
            {
                if (s == 1)
                {
                    p1w = true;
                    res = false;
                }
                if (s == 2)
                {
                    p2w = true;
                    res = false;
                }
            }

            if (res)
                return 0;
            if (p1w && p2w)
                return 3;
            if (p1w)
                return 1;
            if (p2w)
                return 2;
            return returnValue;

        }

        private int CheckPiecesOnBoard(Point piece1, Point piece2, Point piece3, Point piece4, Point piece5)
        {
            int playerAtPiece1 = board.GetPlayer((short)piece1.X, (short)piece1.Y);
            int playerAtPiece2 = board.GetPlayer((short)piece2.X, (short)piece2.Y);
            int playerAtPiece3 = board.GetPlayer((short)piece3.X, (short)piece3.Y);
            int playerAtPiece4 = board.GetPlayer((short)piece4.X, (short)piece4.Y);
            int playerAtPiece5 = board.GetPlayer((short)piece5.X, (short)piece5.Y);


            if (playerAtPiece1 == playerAtPiece2 && playerAtPiece2 == playerAtPiece3 &&
                playerAtPiece3 == playerAtPiece4 && playerAtPiece4 == playerAtPiece5)
            {
                if (winningPoints.Count() > 0)
                {
                    winningPoints.Clear();
                }
                winningPoints.Add(piece1);
                winningPoints.Add(piece2);
                winningPoints.Add(piece3);
                winningPoints.Add(piece4);
                winningPoints.Add(piece5);
                return playerAtPiece1;
            }
            return 0;
        }

        public List<Point> GetWinningPoints { get { return winningPoints; } }

        private List<Point> winningPoints;
    }
}
