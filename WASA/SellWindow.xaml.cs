using Npgsql;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using WASA.Сomplementary;
using System.Timers;
using System.Globalization;

namespace WASA
{

    /// <summary>
    /// Логика взаимодействия для SellWindow.xaml
    /// </summary>
    public partial class SellWindow : Window
    {

        DateInfo dateInfo = new DateInfo();
        Checks check = new Checks();
        UI_Updates updates = new UI_Updates();
        UserInfo userInfo = new UserInfo();
        Moves moves = new Moves();


        public SellWindow()
        {
            try
            {
                InitializeComponent();
                ClockTimer clock = new ClockTimer(d => UserUI_Label_RealTime.Content = time.Text = d.ToString("HH:mm:ss"));
                clock.Start();
                Title = dateInfo.Set_DateInfo(UserUI_Label_Date, UserUI_Label_Day_Of_Week);
                updates.UI_Update(delete_id, delete, all_cash, all_aq, all, dg_sell, $"SELECT * FROM sale WHERE shift = '{dateInfo.Day_Of_Year}' ORDER BY id", dateInfo.Day_Of_Year);
                delete.IsEnabled = false;

                switch (userInfo.GetUserRole())
                {
                    default:
                        calendar1.Visibility = Visibility.Collapsed;
                        break;

                    case "Администратор":

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (discount.Text == "")
                {
                    discount.Text = "0";
                }
                if (position.Text.Length > 0 && price.Text.Length > 0 && discount.Text.Length > 0 && (cash.IsChecked == true || aq.IsChecked == true))
                {
                    moves.Adding(cash, aq, all_cash, all_aq, all, time, article, position, count, price, discount);
                    moves.Change_Balance(article, count, time);
                    balance_text.Text = "Остаток на складе: " + moves.Select("product_count", article, true);
                    if (position.Text == "" && article.Text != "")
                    {
                        balance_text.Text = "Остаток на складе: " + moves.Select("product_count", article, false);
                    }
                    updates.UI_Update(delete_id, delete, all_cash, all_aq, all, dg_sell, $"SELECT * FROM sale WHERE shift = '{dateInfo.Day_Of_Year}' ORDER BY id", dateInfo.Day_Of_Year);
                }
                else
                {
                    MessageBox.Show("Ячейки пусты или не выполняют условия");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void clean_Click(object sender, RoutedEventArgs e)
        {
            time.Text = null;
            article.Text = null;
            position.Text = null;
            count.Text = null;
            price.Text = null;
            discount.Text = null;
            cash.IsChecked = false;
            aq.IsChecked = false;
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
                moves.Delete(delete_id);
                updates.UI_Update(delete_id, delete, all_cash, all_aq, all, dg_sell, $"SELECT * FROM sale WHERE shift = '{dateInfo.Day_Of_Year}' ORDER BY id", dateInfo.Day_Of_Year);
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();           
        }

        private void cash_Checked(object sender, RoutedEventArgs e)
        {
            aq.IsChecked = false;
        }

        private void aq_Checked(object sender, RoutedEventArgs e)
        {
            cash.IsChecked = false;

        }

        private void article_TextChanged(object sender, TextChangedEventArgs e)
        {

            add.IsEnabled = check.InputMultyplyCheck(article, price, count, discount);
            try
            {
                if (article.Text.Length >= 5)
                {
                    position.Text = moves.Select("product_name", article, true);
                    price.Text = moves.Select("product_price", article, true);
                    balance_text.Visibility = Visibility.Visible;
                    balance_text.Text = "Остаток на складе: " + moves.Select("product_count", article, true);
                    count.Text = "1";
                    discount.Text = "0";
                    if (position.Text == "" && article.Text != "")
                    {
                        position.Text = moves.Select("product_name", article, false);
                        price.Text = moves.Select("product_price", article, false);
                        balance_text.Visibility = Visibility.Visible;
                        balance_text.Text = "Остаток на складе: " + moves.Select("product_count", article, false);
                        count.Text = "1";
                        discount.Text = "0";
                    }
                }
                if (article.Text.Length < 5 )
                {
                    position.Text = price.Text = count.Text = discount.Text = "";
                    balance_text.Visibility = Visibility.Hidden;
                    cash.IsChecked = aq.IsChecked = false;

                }
            }
            catch (Exception)
            {

            }

        }
        private void price_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InputMultyplyCheck(article, price, count, discount);
        }

        private void count_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InputMultyplyCheck(article, price, count, discount);
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = DateTime.Today;
            selectedDate = Convert.ToDateTime(calendar1.SelectedDate);
            int selected_shift = selectedDate.DayOfYear;
            updates.UI_Update(delete_id, delete, all_cash, all_aq, all, dg_sell, $"SELECT * FROM sale WHERE shift = '{selected_shift}' ORDER BY id", selected_shift);
        }


        private void discount_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InputMultyplyCheck(article, price, count, discount);
        }

        private void delete_id_TextChanged(object sender, TextChangedEventArgs e)
        {
            delete.IsEnabled = check.InputCheck(delete_id);
        }
    }
}
