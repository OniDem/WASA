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
        public MainWindow()
        {
            InitializeComponent();
            ClockTimer clock = new ClockTimer(d => UserUI_Label_RealTime.Content = d.ToString("HH:mm:ss"));
            clock.Start();
            UserInfo userInfo = new UserInfo();
            switch (userInfo.GetUserRole())
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
            SellWindow sellWindow = new SellWindow();
            sellWindow.Show();
            Close();
        }

        private void Product_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow productWindow = new ProductWindow();
            productWindow.Show();
            Close();
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            Users_Window users_Window = new Users_Window();
            users_Window.Show();
            Close();
        }
    }
}
