using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Pentago.GameCore
{
    public class Board
    {
        private const int BOARDSIZE = 36;
        private const int BOARWIDTH = 6;
        private int[] _board = new int[BOARDSIZE];
        
        public Board()
        {
            for (int i = 0; i < BOARDSIZE; i++) 
            {
                this._board[i] = 0;
            }
        }

        public int GetPlayer(short row, short col)
        {
            return this._board[BOARWIDTH * row + col]; 
        }

        public int GetPlayer(int i)
        {
            return this._board[i];
        }

        public int[] GetBoard
        {
            get { return this._board; }
        }

        public short PiecesOnBoard()
        {
            short count = 0;
            for (int i = 0; i < BOARDSIZE; i++)
            {
                if (this._board[i] != 0)
                    count++;
            }
            return count;
        }

        public void UpdateBoard(short row, short col, int player)
        {
            this._board[BOARWIDTH * row + col] = player;
        }

        public void UpdateBoard(int slot, int player)
        {
            this._board[slot] = player;
        }

        public int GetQuadStart(short quad)
        {
            int quadStart = 0;

            switch (quad)
            {
                case 1:
                    quadStart = 0;
                    break;
                case 2:
                    quadStart = 3;
                    break;
                case 3:
                    quadStart = 18;
                    break;
                case 4:
                    quadStart = 21;
                    break;
            };

            return quadStart;
        }

        public int[] CreatePath(int QuadStart)
        {
            int[] path = new int[9];

            // Path follows clockwise starting from top left square
            path[0] = QuadStart;
            path[1] = path[0] + 1;
            path[2] = path[1] + 1;
            path[3] = path[2] + 6;
            path[4] = path[3] + 6;
            path[5] = path[4] - 1;
            path[6] = path[5] - 1;
            path[7] = path[6] - 6;
            path[8] = path[0];

            return path;
        }

        // rotates quad one space
        public void RotateQuad(bool rotateClockwise, short quad)
        {
            int[] path;
            int placeHolder;
            int pos;

            path = CreatePath(GetQuadStart(quad));

            if (rotateClockwise)
            {
                placeHolder = this._board[path[7]];
                for (pos = 7; pos > 0; pos--)
                {
                    this._board[path[pos]] = this._board[path[pos - 1]]; 
                }
            }
            else
            {
                placeHolder = this._board[path[0]];
                for (pos = 0; pos < 7; pos++)
                {
                    this._board[path[pos]] = this._board[path[pos + 1]];
                }
            }
            this._board[path[pos]] = placeHolder;
        }

        public void RotateQuad1ClockWise() 
        {
            int placeHolder = _board[0];

            this._board[0] = this._board[12];
            this._board[12] = this._board[14];
            this._board[14] = this._board[2];
            this._board[2] = placeHolder;

            placeHolder = this._board[6];
            this._board[6] = this._board[13];
            this._board[13] = this._board[8];
            this._board[8] = this._board[1];
            this._board[1] = placeHolder;
        }

        public void RotateQuad1CounterClockWise()
        {
            int placeHolder = this._board[0];

            this._board[0] = this._board[2];
            this._board[2] = this._board[14];
            this._board[14] = this._board[12];
            this._board[12] = placeHolder;

            placeHolder = this._board[6];
            this._board[6] = this._board[1];
            this._board[1] = this._board[8];
            this._board[8] = this._board[13];
            this._board[13] = placeHolder;
        }

        public void RotateQuad2ClockWise()
        {

            int placeHolder = this._board[3];

            this._board[3] = this._board[15];
            this._board[15] = this._board[17];
            this._board[17] = this._board[5];
            this._board[5] = placeHolder;

            placeHolder = this._board[4];
            this._board[4] = this._board[9];
            this._board[9] = this._board[16];
            this._board[16] = this._board[11];
            this._board[11] = placeHolder;
        }

        public void RotateQuad2CounterClockWise()
        {
            int placeholder = this._board[3];

            this._board[3] = this._board[5];
            this._board[5] = this._board[17];
            this._board[17] = this._board[15];
            this._board[15] = placeholder;

            placeholder = this._board[4];
            this._board[4] = this._board[11];
            this._board[11] = this._board[16];
            this._board[16] = this._board[9];
            this._board[9] = placeholder;
        }

        public void RotateQuad3ClockWise()
        {
            int placeHolder = _board[18];

            this._board[18] = this._board[30];
            this._board[30] = this._board[32];
            this._board[32] = this._board[20];
            this._board[20] = placeHolder;

            placeHolder = this._board[24];
            this._board[24] = this._board[31];
            this._board[31] = this._board[26];
            this._board[26] = this._board[19];
            this._board[19] = placeHolder;
        }

        public void RotateQuad3CounterClockWise()
        {
            int placeHolder = this._board[18];

            this._board[18] = this._board[20];
            this._board[20] = this._board[32];
            this._board[32] = this._board[30];
            this._board[30] = placeHolder;

            placeHolder = this._board[24];
            this._board[24] = this._board[19];
            this._board[19] = this._board[26];
            this._board[26] = this._board[31];
            this._board[31] = placeHolder;
        }

        public void RotateQuad4ClockWise()
        {
            int placeholder = this._board[21];

            this._board[21] = this._board[33];
            this._board[33] = this._board[35];
            this._board[35] = this._board[23];
            this._board[23] = placeholder;

            placeholder = this._board[22];
            this._board[22] = this._board[27];
            this._board[27] = this._board[34];
            this._board[34] = this._board[29];
            this._board[29] = placeholder;
        }

        public void RotateQuad4CounterClockWise()
        {
            int placeholder = this._board[21];

            this._board[21] = this._board[23];
            this._board[23] = this._board[35];
            this._board[35] = this._board[33];
            this._board[33] = placeholder;

            placeholder = this._board[22];
            this._board[22] = this._board[29];
            this._board[29] = this._board[34];
            this._board[34] = this._board[27];
            this._board[27] = placeholder;
        }
    }
}
