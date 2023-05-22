﻿using Npgsql;
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
            updates.UI_Update(dg_product, con, $"SELECT * FROM products ORDER BY article DESC");
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
                if (check.InCheck(add_article) && check.InCheck(add_price) && check.InCheck(add_count) == true)
                {

                    if (Select_All.Background != Brushes.Aqua)
                    {
                        if (add_article.Text.Length > 0 && add_name.Text.Length > 0 && add_count.Text.Length > 0 && add_price.Text.Length > 0)
                        {
                            con = new NpgsqlConnection(Connection.GetConnectionString());
                            con.Open();
                            string sql = $"INSERT INTO products (article, product_type, product_name, product_count, product_price, add_man) VALUES ('{add_article.Text}', '{selected_type}', '{add_name.Text}', '{add_count.Text}', '{add_price.Text}', '{current_user}')";
                            command = new NpgsqlCommand(sql, con);
                            command.ExecuteNonQuery();
                            con.Close();
                            add_article.Text = "";
                            updates.UI_Update(dg_product, con, $"SELECT * FROM products WHERE product_type = '{selected_type}' ORDER BY article;");
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
                if (check.InCheck(change_article) && check.InCheck(change_count) == true)
                {
                    change.IsEnabled = check.InCheck(change_article);
                    change.IsEnabled = check.InCheck(change_count);
                    con = new NpgsqlConnection(Connection.GetConnectionString());
                    con.Open();
                    int _count = 1;
                    command = new NpgsqlCommand($"SELECT product_count FROM products WHERE article = '{change_article.Text}';", con);
                    _count = Convert.ToInt32(command.ExecuteScalar());
                    if ((_count - Convert.ToInt32(change_count.Text)) < 0)
                    {
                        MessageBox.Show("Вы хотите изменить больше чем есть!");
                    }
                    else
                    {
                        _count = _count - Convert.ToInt32(change_count.Text);
                        command = new NpgsqlCommand($"UPDATE products SET product_count='{_count}' WHERE article='{change_article.Text}';", con);
                        command.ExecuteNonQuery();
                        command = new NpgsqlCommand($"UPDATE products SET change='{current_user + " " + UserUI_Label_RealTime.Content}' WHERE article='{change_article.Text}';", con);
                        command.ExecuteNonQuery();
                        con.Close();
                        string selected_type = "";
                        if (Select_All.Background != Brushes.Aqua)
                        {
                            updates.UI_Update(dg_product, con, $"SELECT * FROM products WHERE product_type = '{selected_type}' ORDER BY article;");
                        }
                        else
                        {
                            updates.UI_Update(dg_product, con, $"SELECT * FROM products ORDER BY article;");
                        }
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
            updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
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
            updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
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
            updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
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
            updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
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
            updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
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
            updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products ORDER BY "));
        }

        private void add_article_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InCheck(add_article);
        }

        private void add_price_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InCheck(add_price);
        }

        private void add_count_TextChanged(object sender, TextChangedEventArgs e)
        {
            add.IsEnabled = check.InCheck(add_count);
        }

        private void change_article_TextChanged(object sender, TextChangedEventArgs e)
        {
            change.IsEnabled = check.InCheck(change_article);
        }

        private void change_count_TextChanged(object sender, TextChangedEventArgs e)
        {
            change.IsEnabled = check.InCheck(change_count);
        }

        private void Sort_Article_Click(object sender, RoutedEventArgs e)
        {
            Sort_Article.Background = Brushes.Aqua;
            Sort_Name.Background = Brushes.LightGray;
            Sort_Price.Background = Brushes.LightGray;
            Sort_Balance.Background = Brushes.LightGray;
            Sort_Add_man.Background = Brushes.LightGray;
            Sort_Change_Text.Background = Brushes.LightGray;
            updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
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
            updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
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
            updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
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
            updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
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
            updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
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
            updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products ORDER BY "));
        }

        private void Sort_Type_Click(object sender, RoutedEventArgs e)
        {
            if (Sort_Type.Background == Brushes.Coral)
            {
                Sort_Type.Background = Brushes.Aqua;
                Sort_Type.Content = "Убывание";

                if (selected_type != "all")
                    updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products WHERE product_type = '" + selected_type + "' ORDER BY "));

                if (selected_type == "all")
                    updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                    $"SELECT * FROM products ORDER BY "));
            }
            else
            {
                Sort_Type.Background = Brushes.Coral;
                Sort_Type.Content = "Возрастание";

                if (selected_type != "all")
                    updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products WHERE product_type = '" + selected_type + "' ORDER BY "));

                if (selected_type == "all")
                    updates.UI_Update(dg_product, con, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                    $"SELECT * FROM products ORDER BY "));
            }
        }
    }
}
