using Npgsql;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WASA.Сomplementary;

namespace WASA
{
    /// <summary>
    /// Логика взаимодействия для Product.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        NpgsqlConnection? con;
        NpgsqlCommand? command;
        Checks check = new Checks();
        UI_Updates updates = new UI_Updates();
        UserInfo userInfo = new UserInfo();
        Moves moves = new Moves();
        string? selected_type = "Всё";

        public ProductWindow()
        {
            InitializeComponent();
            ClockTimer clock = new ClockTimer(d => UserUI_Label_RealTime.Content = d.ToString("HH:mm:ss"));
            clock.Start();
            
            switch (userInfo.GetUserRole())
            {
                default:
                    SP_Add_Change.Visibility = Visibility.Collapsed;
                    SP_Choice.Visibility = Visibility.Collapsed;
                    break;

                case "Администратор":

                    break;
            }
            Select_All.IsChecked = true;
            updates.UI_Update(dg_product, $"SELECT * FROM products");
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (check.InputCheck(add_external_article) && check.InputCheck(add_internal_article) && check.InputCheck(add_price) && check.InputCheck(add_count) == true)
                {

                    if (Select_All.Background != Brushes.Aqua)
                    {
                        if (add_internal_article.Text.Length > 0 && add_name.Text.Length > 0 && add_count.Text.Length > 0 && add_price.Text.Length > 0)
                        {
                            con = new NpgsqlConnection(Connection.GetConnectionString());
                            con.Open();
                            string sql = $"INSERT INTO products (external_article, internal_article, product_type, product_name, product_count, product_price, add_man) VALUES ('{add_external_article.Text}', '{add_internal_article.Text}', '{selected_type}', '{add_name.Text}', '{add_count.Text}', '{add_price.Text}', '{userInfo.GetCurrentUser()}')";
                            command = new NpgsqlCommand(sql, con);
                            command.ExecuteNonQuery();
                            con.Close();
                            add_external_article.Text = "";
                            add_internal_article.Text = "";
                            add_name.Text = "";
                            add_price.Text = "";
                            add_count.Text = "";
                            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{selected_type}' ORDER BY internal_article;");
                        }
                        else
                        {
                            MessageBox.Show("Одно или несколько полей пустые");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите тип товара для добавления!");
                    }
                }
                else
                {
                    MessageBox.Show("В данны при добавлении товара допущена ошибка!");
                }
            }
            catch
            (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void change_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClockTimer clock = new ClockTimer(d => UserUI_Label_RealTime.Content = d.ToString("HH:mm:ss"));
                clock.Start();
                if (check.InputCheck(change_external_article) && check.InputCheck(change_internal_article) && check.InputCheck(change_count) == true)
                {
                    change.IsEnabled = check.InputCheck(change_external_article);
                    change.IsEnabled = check.InputCheck(change_internal_article);
                    change.IsEnabled = check.InputCheck(change_count);
                    moves.ChangeProduct(plus, minus, set, change_count, change_position, change_price, change_external_article, change_internal_article, userInfo.GetCurrentUser(), UserUI_Label_RealTime, dg_product, selected_type!);
                }
                else
                {
                    MessageBox.Show("В данных при изменении количества товара допущена ошибка!");
                }
                change_external_article.Clear();
                change_internal_article.Clear();
                change_count.Clear();
                change_price.Clear();
                plus.IsChecked = false;
                minus.IsChecked = false;
                set.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void add_external_article_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InputCheck(add_external_article);
        }


        private void add_internal_article_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InputCheck(add_internal_article);
        }

        private void add_price_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InputCheck(add_price);
        }

        private void add_count_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InputCheck(add_count);
        }

        private void change_external_article_TextChanged(object sender, TextChangedEventArgs e)
        {
            change.IsEnabled = check.InputCheck(change_external_article);
            if (change_external_article.Text.Length >= 5)
            {
                balance_text.Text = "Остаток на складе: " + moves.Select("product_count", change_external_article, false);
                
            }

        }

        private void change_internal_article_TextChanged(object sender, TextChangedEventArgs e)
        {
            change.IsEnabled = check.InputCheck(change_internal_article);
            if (change_internal_article.Text.Length >= 5)
                balance_text.Text = "Остаток на складе: " + moves.Select("product_count", change_internal_article, true);
        }

        private void change_count_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            change.IsEnabled = check.InputCheck(change_count);
        }

        private void change_price_TextChanged(object sender, TextChangedEventArgs e)
        {
            change.IsEnabled = check.InputCheck(change_price);
        }

        private void plus_Checked(object sender, RoutedEventArgs e)
        {
            minus.IsChecked = false;
            set.IsChecked = false;
        }

        private void minus_Checked(object sender, RoutedEventArgs e)
        {
            plus.IsChecked = false;
            set.IsChecked = false;
        }

        private void set_Checked(object sender, RoutedEventArgs e)
        {
            plus.IsChecked = false;
            minus.IsChecked = false;
        }

        private void choice_change_count_Click(object sender, RoutedEventArgs e)
        {
            change_count_text.Visibility = Visibility.Visible;
            change_count.Visibility = Visibility.Visible;
            plus_text.Visibility = Visibility.Visible;
            plus.Visibility = Visibility.Visible;
            minus_text.Visibility = Visibility.Visible;
            minus.Visibility = Visibility.Visible;
            set_text.Visibility = Visibility.Visible;
            set.Visibility = Visibility.Visible;
            change_count.Clear();
            change_price.Clear();
            plus.IsChecked = false;
            minus.IsChecked = false;
            set.IsChecked = false;

            change_position_text.Visibility = Visibility.Collapsed;
            change_position.Visibility = Visibility.Collapsed;
            change_price_text.Visibility = Visibility.Collapsed;
            change_price.Visibility = Visibility.Collapsed;

        }

        private void choice_change_name_Click(object sender, RoutedEventArgs e)
        {
            change_position_text.Visibility = Visibility.Visible;
            change_position.Visibility = Visibility.Visible;
            change_count.Clear();
            change_price.Clear();
            plus.IsChecked = false;
            minus.IsChecked = false;
            set.IsChecked = false;

            change_price_text.Visibility = Visibility.Collapsed;
            change_price.Visibility = Visibility.Collapsed;
            change_count_text.Visibility = Visibility.Collapsed;
            change_count.Visibility = Visibility.Collapsed;
            plus_text.Visibility = Visibility.Collapsed;
            plus.Visibility = Visibility.Collapsed;
            minus_text.Visibility = Visibility.Collapsed;
            minus.Visibility = Visibility.Collapsed;
            set_text.Visibility = Visibility.Collapsed;
            set.Visibility = Visibility.Collapsed;
        }

        private void choice_change_price_Click(object sender, RoutedEventArgs e)
        {
            change_price_text.Visibility = Visibility.Visible;
            change_price.Visibility = Visibility.Visible;
            change_count.Clear();
            change_price.Clear();
            plus.IsChecked = false;
            minus.IsChecked = false;
            set.IsChecked = false;

            change_count_text.Visibility = Visibility.Collapsed;
            change_count.Visibility = Visibility.Collapsed;
            plus_text.Visibility = Visibility.Collapsed;
            plus.Visibility = Visibility.Collapsed;
            minus_text.Visibility = Visibility.Collapsed;
            minus.Visibility = Visibility.Collapsed;
            set_text.Visibility = Visibility.Collapsed;
            set.Visibility = Visibility.Collapsed;
            change_position_text.Visibility = Visibility.Collapsed;
            change_position.Visibility = Visibility.Collapsed;
        }

        private void Wall_Charge_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Wall_Charge.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Wall_Charge.Content}'");
        }

        private void Auto_Charge_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Auto_Charge.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Auto_Charge.Content}'");
        }

        private void Wireless_Charge_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Wireless_Charge.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Wireless_Charge.Content}'");
        }

        private void Portateble_Charge_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Portateble_Charge.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Portateble_Charge.Content}'");
        }

        private void Glass_Samsung_Click(object sender, RoutedEventArgs e)
        {
            Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;

            selected_type = Convert.ToString(Glass_Samsung.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Glass_Samsung.Content}'");
        }

        private void Glass_Huawei_Click(object sender, RoutedEventArgs e)
        {
            Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Glass_Huawei.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Glass_Huawei.Content}'");
        }

        private void Glass_Xiaomi_Click(object sender, RoutedEventArgs e)
        {
            Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Glass_Xiaomi.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Glass_Xiaomi.Content}'");
        }

        private void Glass_IPhone_Click(object sender, RoutedEventArgs e)
        {
            Film.IsExpanded = Case.IsExpanded  = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Glass_IPhone.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Glass_IPhone.Content}'");
        }

        private void Glass_Oppo_Click(object sender, RoutedEventArgs e)
        {
            Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Glass_Oppo.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Glass_Oppo.Content}'");
        }

        private void Glass_Tecno_Click(object sender, RoutedEventArgs e)
        {
            Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Glass_Tecno.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Glass_Tecno.Content}'");
        }

        private void Glass_Universal_Click(object sender, RoutedEventArgs e)
        {
            Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Glass_Universal.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Glass_Universal.Content}'");
        }

        private void Film_Samsung_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Film_Samsung.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Film_Samsung.Content}'");
        }

        private void Film_Huawei_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Film_Huawei.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Film_Huawei.Content}'");
        }

        private void Film_Xiaomi_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Film_Xiaomi.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Film_Xiaomi.Content}'");
        }

        private void Film_IPhone_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Film_IPhone.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Film_IPhone.Content}'");
        }

        private void Film_Oppo_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Film_Oppo.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Film_Oppo.Content}'");

        }

        private void Film_Tecno_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Film_Tecno.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Film_Tecno.Content}'");
        }

        private void Film_Universal_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Film_Universal.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Film_Universal.Content}'");
        }

        private void Case_Samsung_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Case_Samsung.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Case_Samsung.Content}'");
        }

        private void Case_Huawei_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Case_Huawei.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Case_Huawei.Content}'");
        }

        private void Case_Xiaomi_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Case_Xiaomi.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Case_Xiaomi.Content}'");
        }

        private void Case_IPhone_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Case_IPhone.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Case_IPhone.Content}'");
        }

        private void Case_Oppo_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Case_Oppo.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Case_Oppo.Content}'");
        }

        private void Case_Universal_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Case_Universal.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Case_Universal.Content}'");
        }

        private void Case_Tecno_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Case_Tecno.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Case_Tecno.Content}'");
        }




        private void Select_All_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Charge.IsExpanded = Case.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = "Всё";
            updates.UI_Update(dg_product, "SELECT * FROM products");
        }

        private void Cable_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Cable.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Cable.Content}'");
        }

        private void Headphones_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Headphones.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Headphones.Content}'");
        }

        private void TWS_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(TWS.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{TWS.Content}'");
        }

        private void MonoTWS_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(MonoTWS.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{MonoTWS.Content}'");
        }

        private void Holder_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Holder.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Holder.Content}'");
        }

        private void Acessories_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Storage.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Acessories.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Acessories.Content}'");
        }

        private void Storage_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = PC.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(Storage.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Storage.Content}'");
        }

        private void PC_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = Audio.IsChecked = false;
            selected_type = Convert.ToString(PC.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{PC.Content}'");
        }

        private void Audio_Click(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = Charge.IsExpanded = false;
            Glass_Samsung.IsChecked = Glass_Huawei.IsChecked = Glass_Xiaomi.IsChecked = Glass_IPhone.IsChecked = Glass_Oppo.IsChecked = Glass_Tecno.IsChecked = Glass_Universal.IsChecked = false;
            Film_Samsung.IsChecked = Film_Huawei.IsChecked = Film_Xiaomi.IsChecked = Film_IPhone.IsChecked = Film_Oppo.IsChecked = Film_Tecno.IsChecked = Film_Universal.IsChecked = false;
            Case_Samsung.IsChecked = Case_Huawei.IsChecked = Case_Xiaomi.IsChecked = Case_IPhone.IsChecked = Case_Oppo.IsChecked = Case_Tecno.IsChecked = Case_Universal.IsChecked = false;
            Wall_Charge.IsChecked = Auto_Charge.IsChecked = Wireless_Charge.IsChecked = Portateble_Charge.IsChecked = false;
            Select_All.IsChecked = Cable.IsChecked = Headphones.IsChecked = TWS.IsChecked = MonoTWS.IsChecked = Holder.IsChecked = Acessories.IsChecked = Storage.IsChecked = PC.IsChecked = false;
            selected_type = Convert.ToString(Audio.Content);
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{Audio.Content}'");
        }

        private void Glass_Expanded(object sender, RoutedEventArgs e)
        {
            Film.IsExpanded = Case.IsExpanded = false;
        }

        private void Film_Expanded(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Case.IsExpanded = false;
        }

        private void Case_Expanded(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = false;
        }

        private void Charge_Expanded(object sender, RoutedEventArgs e)
        {
            Glass.IsExpanded = Film.IsExpanded = Case.IsExpanded = false;
        }
    }
}
