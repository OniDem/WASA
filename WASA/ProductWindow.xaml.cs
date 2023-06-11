﻿using Npgsql;
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
        NpgsqlConnection con;
        NpgsqlCommand? command;
        Checks check = new Checks();
        UI_Updates updates = new UI_Updates();
        UserInfo userInfo = new UserInfo();
        Moves moves = new Moves();
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
            switch (userInfo.GetUserRole())
            {
                default:
                    SP_Add_Change.Visibility = Visibility.Collapsed;
                    SP_Choice.Visibility = Visibility.Collapsed;
                    break;

                case "Администратор":

                    break;
            }
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
                if (check.InputCheck(add_external_article) && check.InputCheck(add_internal_article) && check.InputCheck(add_price) && check.InputCheck(add_count) == true)
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
                if (check.InputCheck(change_external_article) && check.InputCheck(change_internal_article) && check.InputCheck(change_count) == true)
                {
                    change.IsEnabled = check.InputCheck(change_external_article);
                    change.IsEnabled = check.InputCheck(change_internal_article);
                    change.IsEnabled = check.InputCheck(change_count);
                    moves.ChangeProduct(plus, minus, set, change_count, change_position, change_price, change_external_article, change_internal_article, current_user!, UserUI_Label_RealTime, dg_product);
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

        private void Select_Cable_Click(object sender, RoutedEventArgs e)
        {
            Select_Cable.Background = Brushes.Aqua;
            Select_Glass.Background = Brushes.LightGray;
            Select_Headphones.Background = Brushes.LightGray;
            Select_TWS.Background = Brushes.LightGray;
            Select_MonoTWS.Background = Brushes.LightGray;
            Select_All.Background = Brushes.LightGray;
            selected_type = "cable";
            updates.UI_Update(dg_product,$"SELECT * FROM products WHERE product_type = 'cable'");
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
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = 'glass'");
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
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = 'headphones'");
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
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = 'tws'");

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
            updates.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = 'monotws'");
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
            updates.UI_Update(dg_product, $"SELECT * FROM products");
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
        }

        private void change_internal_article_TextChanged(object sender, TextChangedEventArgs e)
        {
            change.IsEnabled = check.InputCheck(change_internal_article);
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
    }
}
