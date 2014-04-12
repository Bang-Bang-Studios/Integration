using Pentago.GameCore;
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
    /// Interaction logic for MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        ProfileManager profileManager = null;

        public MapWindow(GameOptions options)
        {
            InitializeComponent();
            profileManager = ProfileManager.InstanceCreator();
            InitializeProfileOnGUI(options._Player1);
        }

        private void InitializeProfileOnGUI(Player profilePlayer)
        {
            ProfileName.Text = profilePlayer.Name;
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            Window mainMenu = new MainMenu();
            App.Current.MainWindow = mainMenu;
            mainMenu.Show();
            this.Hide();
        }
    }
}
