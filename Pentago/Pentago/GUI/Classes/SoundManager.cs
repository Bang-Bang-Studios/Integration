using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Media;

namespace Pentago.GUI.Classes
{
    class SoundManager
    {
        public enum SoundType
        {
            Click,
            MouseOver,
            Move,
            KeyDown,
            Rotate
        }

        public static MediaPlayer backgroundMusicPlayer = new MediaPlayer();

        public static void playSFX(SoundType type)
        {
            //Thread soundThread = new Thread(playSound);
            switch (type)
            {
                case SoundType.Click:
                    playSound("GUI/Sounds/click.wav");
                    //soundThread.Start(@"C:\Users\Team7\Documents\crash.wav");
                    break;
                case SoundType.MouseOver:
                    playSound("GUI/Sounds/swish.wav");
                    //soundThread.Start(@"C:\Users\Team7\Documents\swish.wav");
                    break;
                case SoundType.Move:
                    // Play move sound
                    break;
                case SoundType.KeyDown:
                    playSound("GUI/Sounds/typewriter.wav");
                    break;
                case SoundType.Rotate:
                    playSound("GUI/Sounds/swish.wav");
                    break;
                default:
                    //playSound(@"C:\Users\Team7\Documents\Windows Notify.wav");
                    break;
            }
        }

        private static void playSound(string sound)
        {
            SoundPlayer sp = new SoundPlayer(sound);
            sp.Play();
        }
    }
}
