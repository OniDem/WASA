using Npgsql;
using System.Windows;
using WASA.Сomplementary;

namespace WASA
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        NpgsqlCommand command = new();
        NpgsqlConnection con = new(Connection.GetConnectionString());
        UserInfo userInfo = new();
        string? user;
        public SettingsWindow()
        {
            InitializeComponent();
            user = userInfo.GetCurrentUser();
        }

        private void back_Click(object sender, RoutedEventArgs e)
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
