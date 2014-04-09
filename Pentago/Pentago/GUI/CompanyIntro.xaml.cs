using Pentago.GUI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Pentago.GUI;

namespace Pentago
{
    /// <summary>
    /// Interaction logic for CompanyIntro.xaml
    /// </summary>
    public partial class CompanyIntro : Window
    {
        public CompanyIntro()
        {
            InitializeComponent();
        }

        /*

        private void Window_Activated(object sender, EventArgs e)
        {
            
        }

        private void IntroAnimation()
        {
            //SoundManager.backgroundMusicPlayer.Open(new Uri("pack://application:,,,/guns_short.wav", UriKind.Absolute));

            try
            {
                Thread.Sleep(500);
                //Dispatcher.BeginInvoke(new Action(delegate()
                //{
                //    var fadeInAnimation = new DoubleAnimation(1d, new Duration(new TimeSpan(0, 0, 0, 0, 100)));
                //    White.Opacity = 0;
                //    White.Visibility = Visibility.Visible;
                //    White.BeginAnimation(OpacityProperty, fadeInAnimation);
                //}), null);
                Dispatcher.BeginInvoke(new Action(playMusic), null);
                Thread.Sleep(100);
                Dispatcher.BeginInvoke(new Action(flashWhiteOn), null);
                Thread.Sleep(100);
                Dispatcher.BeginInvoke(new Action(flashWhiteOff), null);
                //Dispatcher.BeginInvoke(new Action(showOutline), null);
                Thread.Sleep(200);
                Dispatcher.BeginInvoke(new Action(flashWhiteOn), null);
                Thread.Sleep(100);
                Dispatcher.BeginInvoke(new Action(showLogo), null);
                Dispatcher.BeginInvoke(new Action(flashWhiteOff), null);
                Dispatcher.BeginInvoke(new Action(fadeLogoOut), null);
                Thread.Sleep(2000);
                Dispatcher.BeginInvoke(new Action(delegate() { try { Close(); } catch { } }));
            }
            catch
            {

            }
        }

        private void playMusic()
        {
            //SoundManager.backgroundMusicPlayer.Volume = 1;
            //SoundManager.backgroundMusicPlayer.Open(new Uri("pack://application:,,,/guns_short.wav", UriKind.Absolute));
            //SoundManager.backgroundMusicPlayer.Play();
            SoundManager.playSFX(SoundManager.SoundType.IntroMusic);
        }

        private void flashWhiteOn()
        {
            //DoubleAnimation fadeInAnimation = new DoubleAnimation(1d, new Duration(new TimeSpan(0, 0, 0, 0, 100)));
            //fadeInAnimation.Completed += (o, e3) => { White.Opacity = 0; White.Visibility = Visibility.Hidden; };
            //White.Opacity = 0;
            //White.Visibility = Visibility.Visible;
            //White.BeginAnimation(OpacityProperty, fadeInAnimation);

            //if (White.Visibility == Visibility.Visible)
            //{
            //    White.Visibility = System.Windows.Visibility.Hidden;
            //}
            //else
            //{
                White.Visibility = System.Windows.Visibility.Visible;
            //}
        }

        private void flashWhiteOff()
        {
            White.Visibility = System.Windows.Visibility.Hidden;
        }

        private void showOutline()
        {
            logo.Source = new BitmapImage(new Uri("pack://application:,,,/GUI/images/StudioOutline.png", UriKind.Absolute));
            logo.Visibility = Visibility.Visible;
        }

        private void fadeLogoOut()
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation(0d, new Duration(new TimeSpan(0, 0, 0, 0, 2000)));
            logo.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void showLogo()
        {
            logo.Source = new BitmapImage(new Uri("pack://application:,,,/GUI/images/StudioWhite.png", UriKind.Absolute));
            logo.Visibility = Visibility.Visible;
        }*/

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Space)
            //{
                Close();
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Thread t = new Thread(IntroAnimation);
            //t.Start();
            
            //video.Play();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //SoundManager.stopSFX();
            Window mainMenu = new MainMenu();
            mainMenu.Show();
            App.Current.MainWindow = mainMenu;
            //Hide();
        }

        private void video_MediaEnded(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
