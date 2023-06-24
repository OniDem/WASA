using Npgsql;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WASA.Сomplementary;

namespace WASA
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string? user;
        public MainWindow()
        {
            InitializeComponent();
            ClockTimer clock = new(d => UserUI_Label_RealTime.Content = d.ToString("HH:mm:ss"));
            clock.Start();
            UserInfo userInfo = new();
            switch (userInfo.GetUserRole())
            {
                default:
                    Users.IsEnabled = false;
                    Users.Visibility = Visibility.Collapsed;
                    break;

                case "Администратор":
                    break;
            }
            NpgsqlConnection con = new NpgsqlConnection(Connection.GetConnectionString());
            con!.Open();
            NpgsqlCommand command = new NpgsqlCommand($"SELECT seller FROM settings WHERE settings_id = 1", con);
            user = Convert.ToString(command.ExecuteScalar());
            con.Close();
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
    }
}
