using Npgsql;
using System;
using System.Windows;
using WASA.Сomplementary;

namespace WASA
{
    /// <summary>
    /// Interaction logic for HelloWindow.xaml
    /// </summary>
    public partial class HelloWindow : Window
    {

        private NpgsqlConnection? con;
        DateInfo DateInfo = new DateInfo();
        string ver = "0.2";
        public HelloWindow()
        {
            InitializeComponent();
            version.Content = ver;
            try
            {
                con = new NpgsqlConnection(Connection.GetConnectionString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //con.Open();
            //NpgsqlCommand command = new NpgsqlCommand($"SELECT version FROM settings WHERE settings_id=1;", con);
            //if (command.ExecuteScalar()!.ToString() != ver)
            //{
            //    MessageBox.Show("У вас не актуальная версия");
            //}
            //con.Close();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con!.Open();
                if (login.Text.Length > 1 && password.Text.Length > 1)
                {
                    NpgsqlCommand command = new NpgsqlCommand($"SELECT user_password FROM users WHERE user_name = '{login.Text}'", con);
                    if (command.ExecuteScalar()!.ToString() == password.Text)
                    {
                        command = new NpgsqlCommand($"UPDATE settings SET seller='{login.Text}' WHERE settings_id=1;", con);
                        command.ExecuteNonQuery();
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Close();
                    }
                    else
                        MessageBox.Show("Неккоректные данные!");
                }
                else
                    MessageBox.Show("Одно или оба поля пустые!");
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Create_User_Click(object sender, RoutedEventArgs e)
        {
            Reg_Window reg_Window = new Reg_Window();
            reg_Window.Show();
            Close();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
