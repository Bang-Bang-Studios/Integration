using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Pentago.GameCore;
using Pentago.AI;

namespace Pentago.GUI
{
    public class GameOptions
    {
        public enum TypeOfGame { QuickMatch, Campaign, Network, AI };
        public TypeOfGame _TypeOfGame;

        //Human vs Human
        public Player _Player1;
        public Player _Player2;
        public GameOptions(TypeOfGame typeOfGame, Player player1, Player player2)
        {
            this._TypeOfGame = typeOfGame;
            this._Player1 = player1;
            this._Player2 = player2;
        }

        public PentagoNetwork _NetworkUtil;
        public GameOptions(TypeOfGame typeOfGame, Player player1, Player player2, PentagoNetwork networkUtil)
        {
            this._TypeOfGame = typeOfGame;
            this._Player1 = player1;
            this._Player2 = player2;
            this._NetworkUtil = networkUtil;
        }

        //Human vs AI
        public computerAI.Difficulty _Difficulty;
        public computerAI _ComputerPlayer;
        public GameOptions(TypeOfGame typeOfGame, Player player1, computerAI computerPlayer)
        {
            this._TypeOfGame = typeOfGame;
            this._Player1 = player1;
            this._ComputerPlayer = computerPlayer;
        }
         
    }
}
