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
            MAC_Address.Text = GetMacAddress();
        }


        private void Check_Click(object sender, RoutedEventArgs e)
        {
            MAC_Address.Text = GetMacAddress();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            HelloWindow helloWindow = new HelloWindow();
            helloWindow.Show();
            Close();
        }

        static string GetMacAddress()
        {
            string macAddresses = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces, thereby ignoring any
                // loopback devices etc.
                if (nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet) continue;
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }
    }
}
