using System;
using System.Collections.Generic;
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

namespace SzamaApp
{
    /// <summary>
    /// Logika interakcji dla klasy RecipeWindow.xaml
    /// </summary>
    public partial class RecipeWindow : Window
    {
        User user;
        static string ConnectionString = "Server=MATEUSZ\\SQL2019X;Database=Test_Db;Integrated Security=True;";
        SqlConnection conn = new SqlConnection(ConnectionString);
        

        public RecipeWindow(User currentUser, int reciepId)
        {
            InitializeComponent();
            this.user = currentUser;

            conn.Open();

            string Query = "SELECT * FROM Post INNER JOIN Users ON Users.id = Post.user_id WHERE Post.id = " + reciepId +"";
            SqlCommand cmd = new SqlCommand(Query, conn);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // Width = 800;
                // Margin = new Thickness(20);

                var mainStackPanel = new StackPanel();
                var header = new StackPanel();

                var title = new TextBlock
                {
                    FontSize = 30,
                    Inlines = { new Bold(new Run(reader.GetString(reader.GetOrdinal("title")))) },
                    Margin = new Thickness(0, 10, 0, 10),
                };

                var author = new TextBlock
                {
                    Inlines = { new Italic(new Run(reader.GetString(reader.GetOrdinal("username")))) },
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 179, 198)),
                    FontWeight = FontWeights.Bold,
                };

                var przepisTextBlock = new TextBlock
                {
                    FontSize = 20,
                    Inlines = { new Bold(new Run("Przepis")) }
                };

                var separator = new Separator
                {
                    Width = 760,
                    Margin = new Thickness(0, 25, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                var image = new Image
                {
                    Source = new BitmapImage(new Uri("C:\\Users\\meqiu\\source\\repos\\SzamaApp\\SzamaApp\\foto\\" + reader.GetString(reader.GetOrdinal("image")) + "")),
                    Width = 500,
                    Margin = new Thickness(0, 20, 0, 0),
                    RenderTransformOrigin = new Point(-0.181, 0.48)
                };

                var skladnikiTextBlock = new TextBlock
                {
                    FontWeight = FontWeights.Black,
                    Margin = new Thickness(20, 20, 20, 10),
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 179, 198)),
                    FontSize = 20,
                    Text = "Składniki:"
                };

                var skladnikiStackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(20, 0, 0, 0)
                };

                string ingredients = reader.GetString(reader.GetOrdinal("ingredients"));
                string[] ingredients_list = ingredients.Split('|');

                foreach (string ingredient in ingredients_list)
                {
                    TextBlock textBlock = new TextBlock
                    {
                        Width = 250,
                        Margin = new Thickness(5),
                        Text = ingredient,
                        TextWrapping = TextWrapping.WrapWithOverflow,
                    };
                    skladnikiStackPanel.Children.Add(textBlock);
                }

                var sposobPrzygotowaniaTextBlock = new TextBlock
                {
                    FontSize = 20,
                    Margin = new Thickness(0, 20, 0, 0),
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 179, 198)),
                    FontWeight = FontWeights.Black,
                    Text = "Sposób przygotowania:"
                };

                var sposobPrzygotowaniaStackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(10, 20, 0, 0)
                };


                string preparation = reader.GetString(reader.GetOrdinal("Preparation"));
                string[] preparation_list = preparation.Split('|');

                foreach (string pre in preparation_list)
                {
                    TextBlock textBlock = new TextBlock
                    {
                        Width = 250,
                        Margin = new Thickness(5),
                        Text = pre,
                        TextWrapping = TextWrapping.WrapWithOverflow,
                    };

                    sposobPrzygotowaniaStackPanel.Children.Add(textBlock);
                }

                var opisTextBlock = new TextBlock
                {
                    FontSize = 20,
                    Margin = new Thickness(0, 20, 0, 0),
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 179, 198)),
                    FontWeight = FontWeights.Black,
                    Text = "Opis:"
                };

                var opisTextBlockWrapping = new TextBlock
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(20, 20, 0, 20),
                    Text = reader.GetString(reader.GetOrdinal("description"))
                };

                var commentTextBlock = new TextBlock
                {
                    Text = "Komentarze",
                    Margin = new Thickness(0, 10, 0, 10),
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 179, 198)),
                    FontWeight = FontWeights.Black,
                    FontSize = 20,
                };

                var commentBox = new TextBox
                {
                    Width = 740,
                    Height = 100,
                    Padding = new Thickness(5),
                    Margin = new Thickness(0, 0, 0, 20),
                };

                var commentAddButton = new Button
                {
                    Width = 380,
                    Height = 30,
                    Content = "Dodaj komentarz",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = new SolidColorBrush(Color.FromRgb(255, 179, 198)),
                    
                };

                commentAddButton.Click += (sender, e) => AddComment(sender, e, reciepId, Convert.ToInt32(reader["user_id"]), commentBox.Text);

                mainStackPanel.Children.Add(przepisTextBlock);
                mainStackPanel.Children.Add(separator);

                var verticalIngStackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };

                var verticalPreStackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(0, 20, 0, 0)
                };

                var horizontalStackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

                var verticalAddCommentStackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                };

                header.Children.Add(title);
                header.Children.Add(author);

                verticalIngStackPanel.Children.Add(skladnikiTextBlock);
                verticalIngStackPanel.Children.Add(skladnikiStackPanel);

                horizontalStackPanel.Children.Add(image);
                horizontalStackPanel.Children.Add(verticalIngStackPanel);

                verticalPreStackPanel.Children.Add(sposobPrzygotowaniaTextBlock);
                verticalPreStackPanel.Children.Add(sposobPrzygotowaniaStackPanel);

                verticalAddCommentStackPanel.Children.Add(commentTextBlock);
                verticalAddCommentStackPanel.Children.Add(commentBox);
                verticalAddCommentStackPanel.Children.Add(commentAddButton);

                mainStackPanel.Children.Add(header);
                mainStackPanel.Children.Add(horizontalStackPanel);
                mainStackPanel.Children.Add(verticalPreStackPanel);
                mainStackPanel.Children.Add(opisTextBlock);
                mainStackPanel.Children.Add(opisTextBlockWrapping);
                mainStackPanel.Children.Add(verticalAddCommentStackPanel);


                recipeMainStackPanel.Children.Add(mainStackPanel);
            }
        }

        private void AddComment(object sender, EventArgs e, int post_id, int user_id, string message)
        {
            string Query = "INSERT INTO Comment (user_id, post_id, date, message) VALUES (" + user_id + ", " + post_id + ", '" + currentDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + message + "')";
            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.ExecuteNonQuery();
        }

        private void Button_Home(object sender, RoutedEventArgs e)
        {
            HomeWindow mainWindow = new HomeWindow(this.user);
            mainWindow.Show();
            Close();
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            Search search = new Search(this.user);
            search.Show();
            Close();
        }
    }
}
