using System.Net.NetworkInformation;
using System.Windows;

namespace WASA
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            HelloWindow helloWindow = new HelloWindow();
            helloWindow.Show();
            Close();
        }
    }
}
