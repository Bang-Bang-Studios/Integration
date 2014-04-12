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
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow()
        {
            InitializeComponent();
        }

        public MessageWindow(string text, MessageBoxButton buttons)
        {
            InitializeComponent();
            Message.Text = text;
            if (buttons == MessageBoxButton.OK)
            {
                AcceptButton.Content = "OK";
                AcceptButton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Hidden;
                NoButton.Visibility = Visibility.Hidden;
            }
            else if (buttons == MessageBoxButton.OKCancel)
            {
                AcceptButton.Content = "OK";
                AcceptButton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Visible;
                NoButton.Visibility = Visibility.Hidden;
            }
            else if (buttons == MessageBoxButton.YesNo)
            {
                AcceptButton.Content = "Yes";
                AcceptButton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Hidden;
                NoButton.Visibility = Visibility.Visible;
            }
            else if (buttons == MessageBoxButton.YesNoCancel)
            {
                AcceptButton.Content = "Yes";
                AcceptButton.Visibility = Visibility.Visible;
                CancelButton.Visibility = Visibility.Visible;
                NoButton.Visibility = Visibility.Visible;
            }
        }

        public MessageWindow(string text)
        {
            InitializeComponent();
            Message.Text = text;

            AcceptButton.Content = "OK";
            AcceptButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Hidden;
            NoButton.Visibility = Visibility.Hidden;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePositionRelativeToWindow = e.GetPosition(this);
            TransformGroup t = new TransformGroup();
            t.Children.Add(new TranslateTransform(mousePositionRelativeToWindow.X + 1, mousePositionRelativeToWindow.Y + 1));
            Pointer.RenderTransform = t;
        }

    }
}
