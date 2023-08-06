using Npgsql;
using System;
using System.Threading.Tasks;
using System.Timers;
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

        DateInfo dateInfo = new();
        Checks check = new();
        UI_Updates updates = new();
        UserInfo userInfo = new ();
        Moves moves = new();
        NpgsqlCommand? command;
        NpgsqlConnection con = new(Connection.GetConnectionString());
        Timer? _timer = new();
        ClockTimer? clock;
        string? user, user_role;
        DateTime selectedDate;
        int selected_shift;
        bool focus, user_choice_time = false;

        public SellWindow()
        {            
            try
            {
                selected_shift = dateInfo.Day_Of_Year;
                Task.Run(async () => await Dispatcher.InvokeAsync(() =>
                {
                    updates.UI_UpdateAsync(all_cash, all_aq, all, dg_sell, $"SELECT * FROM sale WHERE shift = '{selected_shift}' ORDER BY id", selected_shift);
                    Title += "       Получение актуальных данных";
                    
                        con.Open();
                        command = new($"SELECT id FROM sale WHERE shift = '{selected_shift}' ORDER BY id DESC", con);
                        delete_id.Text = Convert.ToString(command.ExecuteScalar());
                        con.Close();
                        if (delete_id.Text == "")
                            delete.IsEnabled = false;
                        if (delete_id.Text != "")
                            delete.IsEnabled = true;
                    
                }));
                _timer.Interval = (4 * 500);//Шаг в 500мс(по умолчанию 2000мс(2с)
                _timer.Elapsed += Timer_UI_UpdateAsync!;
                _timer.AutoReset = true;
                _timer.Enabled = true;
                InitializeComponent();
                user = userInfo.GetCurrentUser();
                user_role = userInfo.GetUserRole(user);
                clock = new(d =>
                {
                    Title = dateInfo.Set_DateInfo("Sell", d, user!, user_role!, null!);

                    if (user_choice_time == false)
                        time.Text = d.ToString(" HH:mm:ss");
                });
                clock.Start();
                
                switch (userInfo.GetUserRole(user!))
                {
                    default:
                        break;

                    case "Администратор":
                        calendar1.Visibility = Visibility.Visible;
                        delete_text.Visibility = Visibility.Visible;
                        delete_id.Visibility = Visibility.Visible;
                        delete.Visibility = Visibility.Visible;
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        private async void Timer_UI_UpdateAsync(object sender, ElapsedEventArgs e)
        {
            if (focus == true)
            {
                await Task.Run(async () => await Dispatcher.InvokeAsync(() =>
                {
                    updates.UI_UpdateAsync(all_cash, all_aq, all, dg_sell, $"SELECT * FROM sale WHERE shift = '{selected_shift}' ORDER BY id", selected_shift);
                    Title += "       Получение актуальных данных";
                    con.Open();
                    command = new($"SELECT id FROM sale WHERE shift = '{selected_shift}' ORDER BY id DESC", con);
                    delete_id.Text = Convert.ToString(command.ExecuteScalar());
                    con.Close();
                    if (delete_id.Text == "")
                        delete.IsEnabled = false;
                    if (delete_id.Text != "")
                        delete.IsEnabled = true;
                    
                }));
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
                    moves.Adding(cash, aq, all_cash, all_aq, time, article, position, count, price, discount, user!, selected_shift);
                    moves.Change_Balance(article, count, time, user!);
                    balance_text.Text = "Остаток на складе: " + moves.Select("product_count", article);
                    if (position.Text == "" && article.Text != "")
                    {
                        balance_text.Text = "Остаток на складе: " + moves.Select("product_count", article);
                    }
                    updates.UI_Update(all_cash, all_aq, all, dg_sell, $"SELECT * FROM sale WHERE shift = '{selected_shift}' ORDER BY id", selected_shift);
                    user_choice_time = false;
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
            user_choice_time = false;
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
                updates.UI_Update(all_cash, all_aq, all, dg_sell, $"SELECT * FROM sale WHERE shift = '{selected_shift}' ORDER BY id", selected_shift);
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
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

        private async void article_TextChanged(object sender, TextChangedEventArgs e)
        {

            add.IsEnabled = check.InputMultyplyCheck(article, price, count, discount);
            try
            {
                if (article.Text.Length == 6 && check.InputCheck(article))
                {
                    await Task.Run(() => Dispatcher.Invoke(() => moves.SelectPositionAsync(article, position, price, count)));
                    balance_text.Visibility = Visibility.Visible;
                    balance_text.Text = "Остаток на складе: " + count.Text;
                    count.Text = "1";
                    discount.Text = "0";
                }
                if (article.Text.Length < 5 )
                {
                    position.Text = price.Text = count.Text = discount.Text = "";
                    balance_text.Visibility = Visibility.Hidden;
                    cash.IsChecked = aq.IsChecked = false;

                }
            }
            finally
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
            selectedDate = DateTime.Today;
            selectedDate = Convert.ToDateTime(calendar1.SelectedDate);
            selected_shift = selectedDate.DayOfYear;
            updates.UI_Update(all_cash, all_aq, all, dg_sell, $"SELECT * FROM sale WHERE shift = '{selected_shift}' ORDER BY id", selected_shift);
        }


        private void discount_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InputMultyplyCheck(article, price, count, discount);
        }

        private void delete_id_TextChanged(object sender, TextChangedEventArgs e)
        {
            delete.IsEnabled = check.InputCheck(delete_id);
        }

        private void aq_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            aq.IsChecked = true;
        }

        private void cash_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            cash.IsChecked = true;
        }

        private void delete_id_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0 && delete_id.Text != "")
                delete_id.Text = Convert.ToString(Convert.ToInt32(delete_id.Text) + 1);
            if (e.Delta < 0 && delete_id.Text != "")
                delete_id.Text = Convert.ToString(Convert.ToInt32(delete_id.Text) - 1);
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            focus = true;
            clock.Start();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            focus = false;
            clock.Stop();
        }

        private void time_GotFocus(object sender, RoutedEventArgs e)
        {
            user_choice_time = true;
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
