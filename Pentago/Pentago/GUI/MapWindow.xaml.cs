using Pentago.GameCore;
using Pentago.GUI.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pentago.GUI
{
    /// <summary>
    /// Interaction logic for MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        ProfileManager profileManager = null;

        public int unMuteMusicVol = 6;
        public int currentMusicVol = 6;
        public int unMuteSoundVol = 6;
        public int currentSoundVol = 6;

        public MapWindow(GameOptions options)
        {
            InitializeComponent();
            profileManager = ProfileManager.InstanceCreator();
            InitializeProfileOnGUI(options._Player1);
            SoundManager.sfxVolume = Properties.Settings.Default.SFXVolume;
            SoundManager.musicVolume = Properties.Settings.Default.MusicVolume;
            unMuteMusicVol = SoundManager.musicVolume / 16;
            unMuteSoundVol = SoundManager.sfxVolume / 16;
            currentMusicVol = SoundManager.musicVolume / 16;
            currentSoundVol = SoundManager.sfxVolume / 16;
            SoundManager.backgroundMusicPlayer.Open(new Uri("GUI/Sounds/24.mp3", UriKind.Relative));
            SoundManager.backgroundMusicPlayer.Play();
        }

        private void InitializeProfileOnGUI(Player profilePlayer)
        {
            if (File.Exists(@"GUI\Images\CustomVikings\" + profilePlayer.Name + ".png"))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(@"GUI\Images\CustomVikings\" + profilePlayer.Name + ".png");
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img);
                BitmapSource bmpSrc = Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(),
                    IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                bmp.Dispose();
                VikingImage.Source = bmpSrc;
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            Window mainMenu = new MainMenu();
            App.Current.MainWindow = mainMenu;
            mainMenu.Show();
            this.Hide();
        }

        private void GoBackButton_MouseEnter(object sender, MouseEventArgs e)
        {
            SoundManager.playSFX(SoundManager.SoundType.MouseOver);
        }
    }
}
