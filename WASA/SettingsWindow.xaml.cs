using Npgsql;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
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
        FileIO fileIO = new();
        string? user;
        bool saved = false;
        public SettingsWindow()
        {
            InitializeComponent();
            
            user = userInfo.GetCurrentUser();
            name.Text = fileIO.GetAddressData("name");
            ip.Text = fileIO.GetAddressData("ip");
            port.Text = fileIO.GetAddressData("port");
            username.Text = fileIO.GetAddressData("username");
            password.Password = fileIO.GetAddressData("password");

            bool theme = fileIO.GetThemeData();
            switch (theme)
            {
                case true:
                    light.IsChecked = false;
                    black.IsChecked = true;
                    break;
                case false:
                   light.IsChecked = true;
                    black.IsChecked = false;
                    break;
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (saved)
            {
                HelloWindow helloWindow = new();
                helloWindow.Show();
                Close();
            }
            else
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show("Несохранённые данные будут утеряны", "Предупреждение", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    HelloWindow helloWindow = new();
                    helloWindow.Show();
                    Close();
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            con!.Open();
            command = new($"UPDATE settings SET seller='{user}' WHERE settings_id='1';", con);
            command.ExecuteNonQuery();
            con!.Close();
        }

        //206477 896053
        private void save_Click(object sender, RoutedEventArgs e)
        {

            
                if (light.IsChecked == true)
                    fileIO.SetData(name.Text.Replace('"', ' ').Trim(), ip.Text.Replace('"', ' ').Trim(), port.Text.Replace('"', ' ').Trim(), "0", username.Text.Replace('"', ' ').Trim(), password.Password.Replace('"', ' ').Trim());
                if (black.IsChecked == true)
                    fileIO.SetData(name.Text.Replace('"', ' ').Trim(), ip.Text.Replace('"', ' ').Trim(), port.Text.Replace('"', ' ').Trim(), "1", username.Text.Replace('"', ' ').Trim(), password.Password.Replace('"', ' ').Trim());
                saved = true;
          
            

            System.Windows.MessageBox.Show("Изменения успешно сохранены!");
        }

    }
}
