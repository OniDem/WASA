using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WASA.Сomplementary;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public Users_Window()
        {
            InitializeComponent();
            ClockTimer clock = new ClockTimer(d => UserUI_Label_RealTime.Content = d.ToString("HH:mm:ss"));
            clock.Start();
            dateInfo.Set_DateInfo(UserUI_Label_Date, UserUI_Label_Day_Of_Week);
            updates.UI_Update(dg_users, $"SELECT * FROM users");
        }

        private void user_id_TextChanged(object sender, TextChangedEventArgs e)
        {
            updates.UI_Update(dg_users, $"SELECT * FROM users WHERE user_id = '{user_id.Text}';");
            
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

       
    }
}
