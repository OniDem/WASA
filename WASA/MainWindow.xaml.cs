using Npgsql;
using System.Windows;
using WASA.Сomplementary;

namespace WASA
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NpgsqlCommand? command;
        NpgsqlConnection? con = new(Connection.GetConnectionString());
        readonly DateInfo dateInfo = new();
        UserInfo userInfo = new();
        string? user, user_role;
        public MainWindow()
        {
            InitializeComponent();
            user = userInfo.GetCurrentUser();
            user_role = userInfo.GetUserRole(user);
            ClockTimer clock = new(d => Title = dateInfo.Set_DateInfo("Main", d, user!, user_role, null!));
            clock.Start();           
            
            switch (userInfo.GetUserRole(user!))
            {
                default:
                    Users.IsEnabled = false;
                    Users.Visibility = Visibility.Collapsed;
                    break;

                case "Администратор":
                    break;
            }
          
        }

        private void Sell_Click(object sender, RoutedEventArgs e)
        {
            SellWindow sellWindow = new();
            sellWindow.Show();
            Close();
        }

        private void Product_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow productWindow = new();
            productWindow.Show();
            Close();
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            Users_Window users_Window = new();
            users_Window.Show();
            Close();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new();
            settingsWindow.Show();
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void User_Exit_Click(object sender, RoutedEventArgs e)
        {
            HelloWindow helloWindow = new();
            helloWindow.Show();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            con!.Open();
            command = new($"UPDATE settings SET seller='{user}' WHERE settings_id='1';", con);
            command.ExecuteNonQuery();
            con!.Close();
        }
    }
}
