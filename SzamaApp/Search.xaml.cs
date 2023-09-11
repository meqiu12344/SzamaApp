using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SzamaApp
{
    /// <summary>
    /// Logika interakcji dla klasy Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        User user;
        public Search( User currentUser )
        {
            InitializeComponent();
            this.user = currentUser;
        }

        private void Button_Home(object sender, RoutedEventArgs e)
        {
            HomeWindow homeWindow = new HomeWindow(user);
            homeWindow.Show();
            Close();
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            return;
        }
    }
}
