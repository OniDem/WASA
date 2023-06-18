using Npgsql;
using System.Windows;
using WASA.Сomplementary;

namespace WASA
{
    /// <summary>
    /// Логика взаимодействия для Reg_Window.xaml
    /// </summary>
    public partial class Reg_Window : Window
    {
        NpgsqlConnection con;
        public Reg_Window()
        {
            InitializeComponent();
            con = new NpgsqlConnection(Connection.GetConnectionString());
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            
                if (login.Text.Length > 1 && password.Password.Length > 1)
                {
                    con.Open();
                    NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO users (user_name, user_email, user_phone_number, phone_model, user_password, user_role) VALUES ('{login.Text}', '{password.Password}', 'Кассир');", con);
                    command.ExecuteNonQuery();
                    con.Close();

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
                else
                    MessageBox.Show("Одно и более окон пустые!");
            
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            HelloWindow helloWindow = new HelloWindow();
            helloWindow.Show();
            Close();
        }
    }
}