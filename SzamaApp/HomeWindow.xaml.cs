using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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
using System.Windows.Markup;
using System.Data;

namespace SzamaApp
{
    public partial class HomeWindow : Window
    {
        public User user;

        public HomeWindow(User currentUser)
        {
            this.user = currentUser;

            InitializeComponent();
            usr_text.Text = this.user.Username;

            string ConnectionString = "Server=MATEUSZ\\SQL2019X;Database=Test_Db;Integrated Security=True;";
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            string Query = "SELECT * FROM Post";
            SqlCommand cmd = new SqlCommand(Query, conn);

            SqlDataReader reader = cmd.ExecuteReader();

            Grid allGrid = FindName("allGrid") as Grid;

            int rowIndex = 0;
            int columnIndex = 0;

            while (reader.Read())
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Margin = new Thickness(10);
                stackPanel.Width = 350;

                Image obrazek = new Image();
                obrazek.Source = new BitmapImage(new Uri("C:\\Users\\meqiu\\source\\repos\\SzamaApp\\SzamaApp\\foto\\"+ reader.GetString(reader.GetOrdinal("image")) +""));

                TextBlock tytul = new TextBlock();
                tytul.FontSize = 20;
                tytul.FontWeight = FontWeights.Bold;
                tytul.TextWrapping = TextWrapping.Wrap;

                tytul.Text = reader.GetString(reader.GetOrdinal("title"));

                TextBlock data = new TextBlock();
                data.Foreground = Brushes.Gray;
                data.Margin = new Thickness(0, 5, 0, 0);
                DateTime dataDb = reader.GetDateTime(reader.GetOrdinal("date"));
                string dataFormat = "yyyy/MM/dd";
                data.Text = dataDb.ToString(dataFormat);

                TextBlock opis = new TextBlock();
                opis.TextWrapping = TextWrapping.Wrap;
                Italic kursywa = new Italic();

                string descriptionDb = reader.GetString(reader.GetOrdinal("description"));
                string descriptionDbShort = descriptionDb.Length <= 45 ? descriptionDb : descriptionDb.Substring(0, 245);

                kursywa.Inlines.Add(descriptionDbShort);
                opis.Inlines.Add(kursywa);

                Button przycisk = new Button();
                przycisk.Width = 150;
                przycisk.Margin = new Thickness(0, 20, 0, 0);
                przycisk.HorizontalAlignment = HorizontalAlignment.Right;
                przycisk.Padding = new Thickness(6);
                przycisk.Background = new SolidColorBrush(Color.FromRgb(255, 179, 198));
                przycisk.BorderThickness = new Thickness(0);

                Bold pogrubienie = new Bold();
                pogrubienie.Inlines.Add("Zobacz przepis ->");
                przycisk.Content = pogrubienie;

                int recieId = Convert.ToInt32(reader["id"]);
                przycisk.Click += (sender, e) => show_recipe(sender, e, recieId);

                stackPanel.Children.Add(obrazek);
                stackPanel.Children.Add(tytul);
                stackPanel.Children.Add(data);
                stackPanel.Children.Add(opis);
                stackPanel.Children.Add(przycisk);

                Grid.SetRow(stackPanel, rowIndex);
                Grid.SetColumn(stackPanel, columnIndex);

                allGrid.Children.Add(stackPanel);

                columnIndex++;
                if (columnIndex == 2)
                {
                    columnIndex = 0;
                    rowIndex++;
                }
            }
        }

        public void show_recipe(object sender, RoutedEventArgs e, int recipeId)
        {
            RecipeWindow recipeWindow = new RecipeWindow(this.user, recipeId);
            recipeWindow.Show();
            Close();
        }

        private void Button_Home(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            Search search = new Search(user);
            search.Show();
            Close();
        }
    }
}
