using Npgsql;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using WASA.Сomplementary;

namespace WASA
{
    /// <summary>
    /// Interaction logic for HelloWindow.xaml
    /// </summary>
    public partial class HelloWindow : Window
    {

        readonly NpgsqlConnection? con = new(Connection.GetConnectionString());
        Moves moves = new();
        string ver = "1.4.7";
        public HelloWindow()
        {
            InitializeComponent();
            version.Content = ver;
            try
            {
                Task.Run(() => CheckVersion());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() => Dispatcher.Invoke(() => moves.Auth(login, password)));
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void CheckVersion()
        {
            con!.Open();
            NpgsqlCommand command = new($"SELECT version FROM settings WHERE settings_id=1;", con);
            string? cur_ver = Convert.ToString(await command.ExecuteScalarAsync());
            if (cur_ver != ver)
            {
                MessageBox.Show("У вас не актуальная версия");
            }
            con!.Close();
        }

        private void Create_User_Click(object sender, RoutedEventArgs e)
        {
            Reg_Window reg_Window = new();
            reg_Window.Show();
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
    }
}
