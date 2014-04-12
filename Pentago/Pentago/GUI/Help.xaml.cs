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
using System.Windows.Shapes;

namespace Pentago.GUI
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        public Help()
        {
            InitializeComponent();
        }
        private int helpImageChange = 1;

        private void HelpRight_Click(object sender, RoutedEventArgs e)
        {
            helpImageChange++;
            HelpImage.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap( new System.Drawing.Bitmap("GUI/Images/Help"+helpImageChange+".png").GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight((int)HelpImage.Width, (int)HelpImage.Height));
            if (helpImageChange > 8)
            {
                HelpRight.Visibility = Visibility.Hidden;
            }
            HelpLeft.Visibility = Visibility.Visible;
        }

        private void HelpLeft_Click(object sender, RoutedEventArgs e)
        {
            helpImageChange--;
            HelpImage.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(new System.Drawing.Bitmap("GUI/Images/Help" + helpImageChange + ".png").GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight((int)HelpImage.Width, (int)HelpImage.Height));
            if (helpImageChange < 2)
            {
                HelpLeft.Visibility = Visibility.Hidden;
            }
            HelpRight.Visibility = Visibility.Visible;
             //HelpImage.Source = "Help" + helpImageChange + ".png";
        }

        private void ExitHelp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExitHelp_MouseEnter(object sender, MouseEventArgs e)
        {

        }
    }
}
