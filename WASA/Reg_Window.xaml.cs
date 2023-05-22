using Npgsql;
using System.Windows;

namespace WASA
{
    /// <summary>
    /// Логика взаимодействия для Reg_Window.xaml
    /// </summary>
    public partial class Reg_Window : Window
    {
        private NpgsqlConnection con = new NpgsqlConnection("Host=5.137.198.135;Port=5432;DataBase=wasa;Username=postgres;Password=1234");
        public Reg_Window()
        {
            InitializeComponent();
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text.Length > 1 && email.Text.Length > 1 && email.Text.Contains("@") && phone_number.Text.Length > 1 && phone_model.Text.Length > 1 && password.Text.Length > 1)
            {
                con.Open();
                string sql = $"INSERT INTO users (user_name, user_email, user_phone_number, phone_model, user_password) VALUES ('{login.Text}', '{email.Text}', '{phone_number.Text}', '{phone_model.Text}', '{password.Text}');";
                NpgsqlCommand command = new NpgsqlCommand(sql, con);
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