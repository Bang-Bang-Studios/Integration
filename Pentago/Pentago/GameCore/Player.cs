using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Pentago.GameCore
{
    public class Player
    {
        private string _Name;
        private bool _ActivePlayer;
        private ImageBrush _Image;

        public Player(string name, bool activeTurn, ImageBrush playerImage)
        {
            this._Name = name;
            this._ActivePlayer = activeTurn;
            this._Image = playerImage;
        }

        public string Name
        {
            get { return this._Name; } 
        }

        public bool ActivePlayer
        {
            set { this._ActivePlayer = value; }
            get { return this._ActivePlayer; }
        }

        public Brush Image
        {
            get { return this._Image; }
        }

    }
}