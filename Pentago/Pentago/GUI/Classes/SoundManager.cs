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


        private static int playerNumber = 0;

        private static MediaPlayer[] sfxPlayers = new MediaPlayer[5];

        public static MediaPlayer backgroundMusicPlayer = new MediaPlayer();

        private static int SfxVolume;
        public static int sfxVolume { get { return SfxVolume; } set { Properties.Settings.Default.SFXVolume = value; 
            Properties.Settings.Default.Save();
            SfxVolume = value;
        }
        }

        private static int MusicVolume;
        public static int musicVolume { 
            get { 
                return MusicVolume; 
            } 
            set { backgroundMusicPlayer.Volume = (float)value / 100f; 
            Properties.Settings.Default.MusicVolume = value; Properties.Settings.Default.Save();
            MusicVolume = value;
            }
        }


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

        //private static void playSound(string sound)
        //{
        //    SoundPlayer sp = new SoundPlayer(sound);
        //    sp.Play();
        //}

        private static void playSound(string sound)
        {
            //SoundPlayer sp = new SoundPlayer(sound);
            //sp.Play();
            //MediaPlayer sp = new MediaPlayer();
            //sp.Open(new Uri(sound, UriKind.Relative));
            //sp.Volume = 100f/sfxVolume;
            MediaPlayer sp = sfxPlayers[playerNumber % 5];
            if (sp == null)
            {
                sp = new MediaPlayer();
                sfxPlayers[playerNumber % 5] = sp;
            }
            playerNumber++;
            sp.Open(new Uri(sound, UriKind.Relative));
            sp.Volume = (float)sfxVolume / 100f;
            Console.WriteLine(sp.Volume);
            sp.Play();

            //Thread t = new Thread(play);
            //t.Start(sound);
        }
    }
}
