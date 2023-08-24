using Npgsql;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WASA.Сomplementary;

namespace WASA
{
    /// <summary>
    /// Interaction logic for HelloWindow.xaml
    /// </summary>
    public partial class HelloWindow : Window
    {

        readonly NpgsqlConnection? con = new(Connection.GetConnectionString());
        private string? ver = "1.4.8";
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
                await Task.Run(() => Dispatcher.Invoke(() => Auth(login, password)));
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

        private async Task Auth(TextBox login, PasswordBox password)
        {

            NpgsqlConnection con = new(Connection.GetConnectionString());
            if (login.Text.Length > 1 && password.Password.Length > 1)
            {
                await con.OpenAsync();
                NpgsqlCommand command = new($"SELECT user_password FROM users WHERE user_name = '{login.Text}'", con);
                if (Convert.ToString(await command.ExecuteScalarAsync()) == password.Password)
                {
                    command = new($"SELECT verifided FROM users WHERE user_name = '{login.Text}'", con);
                    bool verifided = Convert.ToBoolean(await command.ExecuteScalarAsync()!);


                    if (verifided == true)
                    {
                        command = new($"UPDATE settings SET seller='{login.Text}' WHERE settings_id='1';", con);
                        await command.ExecuteNonQueryAsync();
                        MainWindow mainWindow = new();
                        mainWindow.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Ваша учётная запись не верифицирована, обратитесь к администратору!");
                        login.Clear();
                        password.Clear();

                    }
                }
                else
                    MessageBox.Show("Неккоректные данные!");

                await con.CloseAsync();
            }
            else
                MessageBox.Show("Одно или оба поля пустые!");
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
