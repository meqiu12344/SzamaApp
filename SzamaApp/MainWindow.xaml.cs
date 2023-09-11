using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Data.SqlClient;

namespace SzamaApp
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int NumberOfFollowers {  get; set; }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string loginText = login.Text;
            string passwordText = password.Password;

            string ConnectionString = "Server=MATEUSZ\\SQL2019X;Database=Test_Db;Integrated Security=True;";
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            string Query = "SELECT * FROM Users";
            SqlCommand cmd = new SqlCommand(Query, conn);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["id"]);
                string login = reader.GetString(reader.GetOrdinal("login"));
                string username = reader.GetString(reader.GetOrdinal("username"));
                string password = reader.GetString(reader.GetOrdinal("password"));
                int numberOfFollowers = Convert.ToInt32(reader["numberOfFollowers"]);

                if (loginText == login && passwordText == password)
                {
                    User currentUser = new User { Id = id, Username = username, Login = loginText, Password = passwordText, NumberOfFollowers = numberOfFollowers };
                    HomeWindow home = new HomeWindow(currentUser);
                    home.Show();
                    Close();

                }
            }

            loginInformation.Text = "Błędne dane";

        }
    }
}
