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
                ClockTimer clock = new ClockTimer(d => UserUI_Label_RealTime.Content = time.Text = d.ToString("HH:mm:ss"));
                clock.Start();
                UserUI_Label_Date.Content = dateInfo.Date;
                UserUI_Label_Day_Of_Week.Content = dateInfo.Day_Of_Week;
                Title = "Смена №" + dateInfo.Day_Of_Year;
                delete.IsEnabled = false;
                con = new NpgsqlConnection(Connection.GetConnectionString());
                updates.UI_Update(delete_id, delete, all_cash, all_aq, all, _all_cash, _all_aq, _all, dg_sell, $"SELECT * FROM sale WHERE shift = '{dateInfo.Day_Of_Year}' ORDER BY id");
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
                    discount.Text = "0";
                if (position.Text.Length > 0 && price.Text.Length > 0 && discount.Text.Length > 0 && (cash.IsChecked == true || aq.IsChecked == true))
                {
                    con!.Open();


                    if (cash.IsChecked == true)
                    {
                        if (_all_cash == 0)
                        {
                            command = new NpgsqlCommand($"SELECT cash FROM sale WHERE shift = '{dateInfo.Day_Of_Year}' ORDER BY id DESC", con);
                            _all_cash = Convert.ToInt32(command.ExecuteScalar());
                            command = new NpgsqlCommand($"SELECT acquiring FROM sale WHERE shift = '{dateInfo.Day_Of_Year}' ORDER BY id DESC", con);
                            _all_aq = Convert.ToInt32(command.ExecuteScalar());
                        }

                        _all_cash += Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text);
                        all_cash.Text = Convert.ToString(_all_cash);
                    }


                    if (aq.IsChecked == true)
                    {

                        if (_all_aq == 0)
                        {
                            command = new NpgsqlCommand($"SELECT acquiring FROM sale WHERE shift = '{dateInfo.Day_Of_Year}' ORDER BY id DESC", con);
                            _all_aq = Convert.ToInt32(command.ExecuteScalar());
                            command = new NpgsqlCommand($"SELECT cash FROM sale WHERE shift = '{dateInfo.Day_Of_Year}' ORDER BY id DESC", con);
                            _all_cash = Convert.ToInt32(command.ExecuteScalar());
                        }
                        _all_aq += Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text);
                        all_aq.Text = Convert.ToString(_all_aq);
                    }


                    _all = _all_cash + _all_aq;
                    all.Text = Convert.ToString(_all);


                    command = new NpgsqlCommand($"INSERT INTO sale (shift, time, article, position, count,  price, discount, cash, acquiring, total, seller) VALUES ('{dateInfo.Day_Of_Year}', '{time.Text}', '{article.Text}', '{position.Text}', '{count.Text}', '{price.Text}', '{discount.Text}', '{_all_cash}', '{_all_aq}', '{_all}', '{user.GetCurrenUser()}')", con);
                    command.ExecuteNonQuery();
                    try
                    {
                        int balance;
                        balance = Convert.ToInt32(moves.Select("product_count", "products", article, true));
                        command = new NpgsqlCommand($"UPDATE products SET product_count='{balance - Convert.ToInt32(count.Text)}' WHERE internal_article='{article.Text}';", con);
                        command.ExecuteNonQuery();
                        command = new NpgsqlCommand($"UPDATE products SET change='{user.GetCurrenUser() + " " + UserUI_Label_RealTime.Content}' WHERE internal_article='{article.Text}';", con);
                        command.ExecuteNonQuery();

                    }
                    finally
                    {
                        int balance;
                        balance = Convert.ToInt32(moves.Select("product_count", "products", article, false));
                        command = new NpgsqlCommand($"UPDATE products SET product_count='{balance - Convert.ToInt32(count.Text)}' WHERE external_article='{article.Text}';", con);
                        command.ExecuteNonQuery();
                        command = new NpgsqlCommand($"UPDATE products SET change='{user.GetCurrenUser() + " " + UserUI_Label_RealTime.Content}' WHERE external_article='{article.Text}';", con);
                        command.ExecuteNonQuery();
                    }
                    con.Close();
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
            try
            {
                con!.Open();
                command = new NpgsqlCommand($"DELETE FROM sale WHERE id='{Convert.ToInt32(delete_id.Text)}'", con);
                command.ExecuteNonQuery();
                con!.Close();
                updates.UI_Update(delete_id, delete, all_cash, all_aq, all, _all_cash, _all_aq, _all, dg_sell, $"SELECT * FROM sale WHERE shift = '{dateInfo.Day_Of_Year}' ORDER BY id");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        private void back_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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
                position.Text = moves.Select("position_name", "products", article, true);
                price.Text = moves.Select("position_price", "products", article, true);
                if (position.Text == "" && article.Text != "")
                {
                    position.Text = moves.Select("position_name", "products", article, false);
                    price.Text = moves.Select("position_price", "products", article, false);
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
