using Npgsql;
using System;
using System.Windows;
using System.Windows.Controls;
using WASA.Сomplementary;

namespace WASA
{

    /// <summary>
    /// Логика взаимодействия для SellWindow.xaml
    /// </summary>
    public partial class SellWindow : Window
    {

        DateInfo dateInfo = new DateInfo();
        InputCheck check = new InputCheck();
        UI_Updates updates = new UI_Updates();
        Current_User user = new Current_User();
        Moves_With_DB moves = new Moves_With_DB();
        Sell_Moves s_moves = new Sell_Moves();
        int _all_cash, _all_aq, _all = 1;
        NpgsqlConnection? con;
        NpgsqlCommand? command;


        public SellWindow()
        {
            try
            {
                InitializeComponent();
                /*
                switch (current_user)
                {
                    case "test":
                        shift.Visibility = Visibility.Visible;
                        seller.Visibility = Visibility.Visible;
                        break;
                    case "OniDem1":
                        shift.Visibility = Visibility.Visible;
                        seller.Visibility = Visibility.Visible;
                        break;
                    case "admin":
                        shift.Visibility = Visibility.Hidden;
                        break;

                }
                */
                con = new NpgsqlConnection(Connection.GetConnectionString());
                ClockTimer clock = new ClockTimer(d => UserUI_Label_RealTime.Content = time.Text = d.ToString("HH:mm:ss"));
                clock.Start();
                Title = dateInfo.Set_DateInfo(UserUI_Label_Date, UserUI_Label_Day_Of_Week);
                updates.UI_Update(delete_id, delete, all_cash, all_aq, all, _all_cash, _all_aq, _all, dg_sell, $"SELECT * FROM sale WHERE shift = '{dateInfo.Day_Of_Year}' ORDER BY id");
                delete.IsEnabled = false;
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
                    s_moves.Adding(cash, aq, all_cash, all_aq, all, time, article, position, count, price, discount);
                    s_moves.Change_Balance(article, count, UserUI_Label_RealTime);
                    updates.UI_Update(delete_id, delete, all_cash, all_aq, all, _all_cash, _all_aq, _all, dg_sell, $"SELECT * FROM sale WHERE shift = '{dateInfo.Day_Of_Year}' ORDER BY id");
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
                updates.UI_Update(delete_id, delete, all_cash, all_aq, all, _all_cash, _all_aq, _all, dg_sell, $"SELECT * FROM sale WHERE shift = '{dateInfo.Day_Of_Year}' ORDER BY id");
        }



        private void back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();           
        }

        private void cash_Checked(object sender, RoutedEventArgs e)
        {
            aq.IsEnabled = false;
        }

        private void cash_Unchecked(object sender, RoutedEventArgs e)
        {
            aq.IsEnabled = true;
        }
        private void aq_Checked(object sender, RoutedEventArgs e)
        {
            cash.IsEnabled = false;

        }
        private void aq_Unchecked(object sender, RoutedEventArgs e)
        {
            cash.IsEnabled = true;

        }

        private void article_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InputMultyplyCheck(article, price, count, discount, delete_id);
            try
            {
                position.Text = moves.Select("position_name", article, true);
                price.Text = moves.Select("position_price", article, true);
                if (position.Text == "" && article.Text != "")
                {
                    position.Text = moves.Select("position_name", article, false);
                    price.Text = moves.Select("position_price", article, false);
                }
            }
            catch (Exception)
            {

            }

        }
        private void price_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InputMultyplyCheck(article, price, count, discount, delete_id);
        }

        private void count_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InputMultyplyCheck(article, price, count, discount, delete_id);
        }

        private void discount_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InputMultyplyCheck(article, price, count, discount, delete_id);
        }

        private void delete_id_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InputMultyplyCheck(article, price, count, discount, delete_id);
        }
    }
}
