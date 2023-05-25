using Npgsql;
using System;
using System.Data;
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
        NpgsqlConnection con;
        NpgsqlCommand? command;
        InputCheck check = new InputCheck();
        UI_Updates updates = new UI_Updates();
        Selected_Type selected = new Selected_Type();
        Balance_Changes changes = new Balance_Changes();
        string? current_user;
        string? selected_type = "all";

        public ProductWindow()
        {
            InitializeComponent();
            ClockTimer clock = new ClockTimer(d => UserUI_Label_RealTime.Content = d.ToString("HH:mm:ss"));
            clock.Start();
            Select_Cable.Background = Brushes.LightGray;
            Select_Glass.Background = Brushes.LightGray;
            Select_Headphones.Background = Brushes.LightGray;
            Select_TWS.Background = Brushes.LightGray;
            Select_MonoTWS.Background = Brushes.LightGray;
            Select_All.Background = Brushes.Aqua;

            Sort_Article.Background = Brushes.Aqua;
            Sort_Type.Background = Brushes.Aqua;
            Sort_Type.Content = "Убывание"; // DESC по убыванию // ASC по возрастанию

            con = new NpgsqlConnection(Connection.GetConnectionString());
            con.Open();
            command = new NpgsqlCommand($"SELECT seller FROM settings WHERE settings_id = 1", con);
            current_user = Convert.ToString(command.ExecuteScalar());
            con.Close();
            updates.UI_Update(dg_product, $"SELECT * FROM products ORDER BY internal_article DESC");
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
                if (check.InCheck(add_external_article) && check.InCheck(add_internal_article) && check.InCheck(add_price) && check.InCheck(add_count) == true)
                {

                    if (Select_All.Background != Brushes.Aqua)
                    {
                        if (add_internal_article.Text.Length > 0 && add_name.Text.Length > 0 && add_count.Text.Length > 0 && add_price.Text.Length > 0)
                        {
                            con = new NpgsqlConnection(Connection.GetConnectionString());
                            con.Open();
                            string sql = $"INSERT INTO products (external_article, internal_article, product_type, product_name, product_count, product_price, add_man) VALUES ('{add_external_article.Text}', '{add_internal_article.Text}', '{selected_type}', '{add_name.Text}', '{add_count.Text}', '{add_price.Text}', '{current_user}')";
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
                if (check.InCheck(change_external_article) && check.InCheck(change_internal_article) && check.InCheck(change_count) == true)
                {
                    change.IsEnabled = check.InCheck(change_external_article);
                    change.IsEnabled = check.InCheck(change_internal_article);
                    change.IsEnabled = check.InCheck(change_count);
                    
                    int _count = 1;
                    con = new NpgsqlConnection(Connection.GetConnectionString());
                    con.Open();
                    if (change_external_article.Text == "" && change_internal_article.Text != "")
                    {
                        command = new NpgsqlCommand($"SELECT product_count FROM products WHERE internal_article = '{change_internal_article.Text}';", con);
                        _count = Convert.ToInt32(command!.ExecuteScalar());
                    }
                    else if (change_external_article.Text != "" && change_internal_article.Text == "")
                    {
                        command = new NpgsqlCommand($"SELECT product_count FROM products WHERE external_article = '{change_external_article.Text}';", con);
                        _count = Convert.ToInt32(command!.ExecuteScalar());
                    }
                    else if (change_external_article.Text != "" && change_internal_article.Text != "")
                    {
                        command = new NpgsqlCommand($"SELECT product_count FROM products WHERE internal_article = '{change_internal_article.Text}';", con);
                        _count = Convert.ToInt32(command!.ExecuteScalar());
                    }
                    con.Close();


                    if (plus.IsChecked == true)
                    {
                        _count = _count + Convert.ToInt32(change_count.Text);
                        changes.Balance_Change(change_external_article, change_internal_article, _count, current_user!, UserUI_Label_RealTime, Select_All, dg_product,
                                Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type, selected_type!);
                    }
                    else if (minus.IsChecked == true)
                    {
                        if ((_count - Convert.ToInt32(change_count.Text)) >= 0)
                        {
                            _count = _count - Convert.ToInt32(change_count.Text);
                            changes.Balance_Change(change_external_article, change_internal_article, _count, current_user!, UserUI_Label_RealTime, Select_All, dg_product,
                                Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type, selected_type!);
                        }
                        else
                        {
                            MessageBox.Show("Вы хотите изменить больше чем есть!");
                        }
                    }
                    else if (set.IsChecked == true)
                    {
                        changes.Balance_Change(change_external_article, change_internal_article, Convert.ToInt32(change_count.Text), current_user!, UserUI_Label_RealTime, Select_All, dg_product,
                            Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type, selected_type!);
                    }
                    else
                    {
                        MessageBox.Show("Выберите действие!");
                    }
                }
                else
                {
                    MessageBox.Show("В данные при изменении количества това ра допущена ошибка!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Select_Cable_Click(object sender, RoutedEventArgs e)
        {
            Select_Cable.Background = Brushes.Aqua;
            Select_Glass.Background = Brushes.LightGray;
            Select_Headphones.Background = Brushes.LightGray;
            Select_TWS.Background = Brushes.LightGray;
            Select_MonoTWS.Background = Brushes.LightGray;
            Select_All.Background = Brushes.LightGray;
            selected_type = "cable";
            updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products WHERE product_type = 'cable' ORDER BY "));
        }

        private void Select_Glass_Click(object sender, RoutedEventArgs e)
        {
            Select_Cable.Background = Brushes.LightGray;
            Select_Glass.Background = Brushes.Aqua;
            Select_Headphones.Background = Brushes.LightGray;
            Select_TWS.Background = Brushes.LightGray;
            Select_MonoTWS.Background = Brushes.LightGray;
            Select_All.Background = Brushes.LightGray;
            selected_type = "glass";
            updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products WHERE product_type = 'glass' ORDER BY "));
        }

        private void Select_Headphones_Click(object sender, RoutedEventArgs e)
        {
            Select_Cable.Background = Brushes.LightGray;
            Select_Glass.Background = Brushes.LightGray;
            Select_Headphones.Background = Brushes.Aqua;
            Select_TWS.Background = Brushes.LightGray;
            Select_MonoTWS.Background = Brushes.LightGray;
            Select_All.Background = Brushes.LightGray;
            selected_type = "headphones";
            updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products WHERE product_type = 'headphones' ORDER BY "));
        }

        private void Select_TWS_Click(object sender, RoutedEventArgs e)
        {
            Select_Cable.Background = Brushes.LightGray;
            Select_Glass.Background = Brushes.LightGray;
            Select_Headphones.Background = Brushes.LightGray;
            Select_TWS.Background = Brushes.Aqua;
            Select_MonoTWS.Background = Brushes.LightGray;
            Select_All.Background = Brushes.LightGray;
            selected_type = "tws";
            updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products WHERE product_type = 'tws' ORDER BY "));

        }

        private void Select_MonoTWS_Click(object sender, RoutedEventArgs e)
        {
            Select_Cable.Background = Brushes.LightGray;
            Select_Glass.Background = Brushes.LightGray;
            Select_Headphones.Background = Brushes.LightGray;
            Select_TWS.Background = Brushes.LightGray;
            Select_MonoTWS.Background = Brushes.Aqua;
            Select_All.Background = Brushes.LightGray;
            selected_type = "monotws";
            updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products WHERE product_type = 'monotws' ORDER BY "));
        }

        private void Select_All_Click(object sender, RoutedEventArgs e)
        {
            Select_Cable.Background = Brushes.LightGray;
            Select_Glass.Background = Brushes.LightGray;
            Select_Headphones.Background = Brushes.LightGray;
            Select_TWS.Background = Brushes.LightGray;
            Select_MonoTWS.Background = Brushes.LightGray;
            Select_All.Background = Brushes.Aqua;
            selected_type = "all";
            updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products ORDER BY "));
        }

        private void add_external_article_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InCheck(add_external_article);
        }


        private void add_internal_article_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InCheck(add_internal_article);
        }

        private void add_price_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InCheck(add_price);
        }

        private void add_count_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InCheck(add_count);
        }

        private void change_external_article_TextChanged(object sender, TextChangedEventArgs e)
        {
            change.IsEnabled = check.InCheck(change_external_article);
        }

        private void change_internal_article_TextChanged(object sender, TextChangedEventArgs e)
        {
            change.IsEnabled = check.InCheck(change_internal_article);
        }

        private void change_count_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            change.IsEnabled = check.InCheck(change_count);
        }
        private void change_position_TextChanged(object sender, TextChangedEventArgs e)
        {
            change.IsEnabled = check.InCheck(change_position);
        }

        private void change_price_TextChanged(object sender, TextChangedEventArgs e)
        {
            change.IsEnabled = check.InCheck(change_price);
        }

        private void Sort_Article_Click(object sender, RoutedEventArgs e)
        {
            Sort_Article.Background = Brushes.Aqua;
            Sort_Name.Background = Brushes.LightGray;
            Sort_Price.Background = Brushes.LightGray;
            Sort_Balance.Background = Brushes.LightGray;
            Sort_Add_man.Background = Brushes.LightGray;
            Sort_Change_Text.Background = Brushes.LightGray;
            updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products ORDER BY "));
        }

        private void Sort_Name_Click(object sender, RoutedEventArgs e)
        {
            Sort_Article.Background = Brushes.LightGray;
            Sort_Name.Background = Brushes.Aqua;
            Sort_Price.Background = Brushes.LightGray;
            Sort_Balance.Background = Brushes.LightGray;
            Sort_Add_man.Background = Brushes.LightGray;
            Sort_Change_Text.Background = Brushes.LightGray;
            updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products ORDER BY "));
        }

        private void Sort_Price_Click(object sender, RoutedEventArgs e)
        {
            Sort_Article.Background = Brushes.LightGray;
            Sort_Name.Background = Brushes.LightGray;
            Sort_Price.Background = Brushes.Aqua;
            Sort_Balance.Background = Brushes.LightGray;
            Sort_Add_man.Background = Brushes.LightGray;
            Sort_Change_Text.Background = Brushes.LightGray;
            updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products ORDER BY "));
        }

        private void Sort_Balance_Click(object sender, RoutedEventArgs e)
        {
            Sort_Article.Background = Brushes.LightGray;
            Sort_Name.Background = Brushes.LightGray;
            Sort_Price.Background = Brushes.LightGray;
            Sort_Balance.Background = Brushes.Aqua;
            Sort_Add_man.Background = Brushes.LightGray;
            Sort_Change_Text.Background = Brushes.LightGray;
            updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products ORDER BY "));
        }

        private void Sort_Add_Man_Click(object sender, RoutedEventArgs e)
        {
            Sort_Article.Background = Brushes.LightGray;
            Sort_Name.Background = Brushes.LightGray;
            Sort_Price.Background = Brushes.LightGray;
            Sort_Balance.Background = Brushes.LightGray;
            Sort_Add_man.Background = Brushes.Aqua;
            Sort_Change_Text.Background = Brushes.LightGray;
            updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products ORDER BY "));
        }

        private void Sort_Change_Text_Click(object sender, RoutedEventArgs e)
        {
            Sort_Article.Background = Brushes.LightGray;
            Sort_Name.Background = Brushes.LightGray;
            Sort_Price.Background = Brushes.LightGray;
            Sort_Balance.Background = Brushes.LightGray;
            Sort_Add_man.Background = Brushes.LightGray;
            Sort_Change_Text.Background = Brushes.Aqua;
            updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products ORDER BY "));
        }

        private void Sort_Type_Click(object sender, RoutedEventArgs e)
        {
            if (Sort_Type.Background == Brushes.Coral)
            {
                Sort_Type.Background = Brushes.Aqua;
                Sort_Type.Content = "Убывание";

                if (selected_type != "all")
                    updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products WHERE product_type = '" + selected_type + "' ORDER BY "));

                if (selected_type == "all")
                    updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                    $"SELECT * FROM products ORDER BY "));
            }
            else
            {
                Sort_Type.Background = Brushes.Coral;
                Sort_Type.Content = "Возрастание";

                if (selected_type != "all")
                    updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products WHERE product_type = '" + selected_type + "' ORDER BY "));

                if (selected_type == "all")
                    updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                    $"SELECT * FROM products ORDER BY "));
            }
        }

        private void plus_Checked(object sender, RoutedEventArgs e)
        {
            minus.IsEnabled = false;
            set.IsEnabled = false;
        }

        private void plus_Unchecked(object sender, RoutedEventArgs e)
        {
            minus.IsEnabled = true;
            set.IsEnabled = true;
        }

        private void minus_Checked(object sender, RoutedEventArgs e)
        {
            plus.IsEnabled = false;
            set.IsEnabled = false;
        }

        private void minus_Unchecked(object sender, RoutedEventArgs e)
        {
            plus.IsEnabled = true;
            set.IsEnabled = true;
        }

        private void set_Checked(object sender, RoutedEventArgs e)
        {
            plus.IsEnabled = false;
            minus.IsEnabled = false;
        }

        private void set_Unchecked(object sender, RoutedEventArgs e)
        {
            plus.IsEnabled = true;
            minus.IsEnabled = false;
        }

        private void choice_change_count_Click(object sender, RoutedEventArgs e)
        {
            change_count.IsEnabled = true;
            change_count.Visibility = Visibility.Visible;
            change_count_text.Visibility = Visibility.Visible;
            plus.IsEnabled = true;
            plus.Visibility = Visibility.Visible;
            plus_text.Visibility = Visibility.Visible;
            minus.IsEnabled = true;
            minus.Visibility = Visibility.Visible;
            minus_text.Visibility = Visibility.Visible;
            set.IsEnabled = true;
            set.Visibility = Visibility.Visible;
            set_text.Visibility = Visibility.Visible;

            change_position.IsEnabled = false;
            change_position.Visibility = Visibility.Hidden;
            change_position_text.Visibility = Visibility.Hidden;
            change_price.IsEnabled = false;
            change_price.Visibility = Visibility.Hidden;
            change_price_text.Visibility = Visibility.Hidden;
        }

        private void choice_change_name_Click(object sender, RoutedEventArgs e)
        {
            change_position.IsEnabled = true;
            change_position.Visibility = Visibility.Visible;
            change_position_text.Visibility = Visibility.Visible;

            change_count.IsEnabled = false;
            change_count.Visibility = Visibility.Hidden;
            change_count_text.Visibility = Visibility.Hidden;
            plus.IsEnabled = false;
            plus.Visibility = Visibility.Hidden;
            plus_text.Visibility = Visibility.Hidden;
            minus.IsEnabled = false;
            minus.Visibility = Visibility.Hidden;
            minus_text.Visibility = Visibility.Hidden;
            set.IsEnabled = false;
            set.Visibility = Visibility.Hidden;
            set_text.Visibility = Visibility.Hidden;
            change_price.IsEnabled = false;
            change_price.Visibility = Visibility.Hidden;
            change_price_text.Visibility = Visibility.Hidden;
        }

        private void choice_change_price_Click(object sender, RoutedEventArgs e)
        {
            change_price.IsEnabled = true;
            change_price.Visibility = Visibility.Visible;
            change_price_text.Visibility = Visibility.Visible;

            change_count.IsEnabled = false;
            change_count.Visibility = Visibility.Hidden;
            change_count_text.Visibility = Visibility.Hidden;
            plus.IsEnabled = false;
            plus.Visibility = Visibility.Hidden;
            plus_text.Visibility = Visibility.Hidden;
            minus.IsEnabled = false;
            minus.Visibility = Visibility.Hidden;
            minus_text.Visibility = Visibility.Hidden;
            set.IsEnabled = false;
            set.Visibility = Visibility.Hidden;
            set_text.Visibility = Visibility.Hidden;
            change_position.IsEnabled = false;
            change_position.Visibility = Visibility.Hidden;
            change_position_text.Visibility = Visibility.Hidden;
        }
    }
}
