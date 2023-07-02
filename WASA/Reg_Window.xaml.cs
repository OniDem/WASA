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
        readonly NpgsqlConnection? con;
        public Reg_Window()
        {
            InitializeComponent();
            con = new(Connection.GetConnectionString());
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {

            if (login.Text.Length > 1 && password.Password.Length > 1)
            {
                con!.Open();
                NpgsqlCommand command = new($"INSERT INTO users (user_name, user_password, user_role, verifided) VALUES ('{login.Text}', '{password.Password}', 'Кассир', 'false');", con);
                command.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Верифицируйте учётную запись у администратора");
                Close();
            }
            else
                MessageBox.Show("Одно и более окон пустые!");
            
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            HelloWindow helloWindow = new();
            helloWindow.Show();
            Close();
        }
    }
}