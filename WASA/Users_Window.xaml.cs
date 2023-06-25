using Npgsql;
using System;
using System.Windows;
using System.Windows.Controls;
using WASA.Сomplementary;

namespace WASA
{
    /// <summary>
    /// Логика взаимодействия для Users_Window.xaml
    /// </summary>
    public partial class Users_Window : Window
    {
        UI_Updates updates = new UI_Updates();
        Checks Check = new Checks();
        DateInfo dateInfo = new DateInfo();
        NpgsqlCommand? command;
        NpgsqlConnection con = new(Connection.GetConnectionString());
        UserInfo userInfo = new();
        string? user, user_role;

        public Users_Window()
        {
            InitializeComponent();
            user = userInfo.GetCurrentUser();
            user_role = userInfo.GetUserRole(user);
            ClockTimer clock = new ClockTimer(d => {
                Title = dateInfo.Set_DateInfo("Users", UserUI_Label_Date, UserUI_Label_Day_Of_Week, d, user!, user_role, null!);
            });
            clock.Start();
            
            updates.UI_Update(dg_users, $"SELECT * FROM users", con);
        }

        private void user_id_TextChanged(object sender, TextChangedEventArgs e)
        {
            updates.UI_Update(dg_users, $"SELECT * FROM users WHERE user_id = '{user_id.Text}';", con);
            
        }

        private void user_Checked(object sender, RoutedEventArgs e)
        {
            admin.IsEnabled = false;
        }

        private void user_Unchecked(object sender, RoutedEventArgs e)
        {
            admin.IsEnabled = true;
        }

        private void admin_Checked(object sender, RoutedEventArgs e)
        {
            admin.IsEnabled = false;
        }

        private void admin_Unchecked(object sender, RoutedEventArgs e)
        {
            admin.IsEnabled = true;
        }

        private void change_Click(object sender, RoutedEventArgs e)
        {

        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void delete_id_TextChanged(object sender, TextChangedEventArgs e)
        {
            delete.IsEnabled = Check.InputCheck(delete_id);
        }

        private void clean_Click(object sender, RoutedEventArgs e)
        {

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            con!.Open();
            command = new NpgsqlCommand($"UPDATE settings SET seller='{user}' WHERE settings_id='1';", con);
            command.ExecuteNonQuery();
            con!.Close();
        }
    }
}
