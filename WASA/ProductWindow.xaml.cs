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
        NpgsqlConnection con;
        NpgsqlCommand? command;
        Checks check = new Checks();
        UI_Updates updates = new UI_Updates();
        Selected_Type selected = new Selected_Type();
        Balance_Changes changes = new Balance_Changes();
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

                    moves.ChangeProduct(plus, minus, set, change_count, change_position, change_price, change_external_article, change_internal_article, current_user!, UserUI_Label_RealTime, Select_All, dg_product,
                        Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type, selected_type!);
                }
                else
                {
                    MessageBox.Show("В данные при изменении количества това ра допущена ошибка!");
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
                Sort_UI_Update();

            }
            else
            {
                Sort_Type.Background = Brushes.Coral;
                Sort_Type.Content = "Возрастание";
                Sort_UI_Update();
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
            change_count_text.Foreground = Brushes.Black;
            change_count.IsEnabled = true;
            plus_text.Foreground = Brushes.Black;
            plus.IsEnabled = true;
            minus_text.Foreground = Brushes.Black;
            minus.IsEnabled = true;
            set_text.Foreground = Brushes.Black;
            set.IsEnabled = true;

            change_position_text.Foreground = Brushes.LightGray;
            change_position.IsEnabled = false;
            change_price_text.Foreground = Brushes.LightGray;
            change_price.IsEnabled = false;

        }

        private void choice_change_name_Click(object sender, RoutedEventArgs e)
        {
            change_position_text.Foreground = Brushes.Black;
            change_position.IsEnabled = true;

            change_price_text.Foreground = Brushes.LightGray;
            change_price.IsEnabled = false;
            change_count_text.Foreground = Brushes.LightGray;
            change_count.IsEnabled = false;
            plus_text.Foreground = Brushes.LightGray;
            plus.IsEnabled = false;
            minus_text.Foreground = Brushes.LightGray;
            minus.IsEnabled = false;
            set_text.Foreground = Brushes.LightGray;
            set.IsEnabled = false;
        }

        private void choice_change_price_Click(object sender, RoutedEventArgs e)
        {
            change_price_text.Foreground = Brushes.Black;
            change_price.IsEnabled = true;

            change_count_text.Foreground = Brushes.LightGray;
            change_count.IsEnabled = false;
            plus_text.Foreground = Brushes.LightGray;
            plus.IsEnabled = false;
            minus_text.Foreground = Brushes.LightGray;
            minus.IsEnabled = false;
            set_text.Foreground = Brushes.LightGray;
            set.IsEnabled = false;
            change_position_text.Foreground = Brushes.LightGray;
            change_position.IsEnabled = false;
        }


        void Sort_UI_Update()
        {
            if (selected_type != "all")
                updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
            $"SELECT * FROM products WHERE product_type = '" + selected_type + "' ORDER BY "));

            if (selected_type == "all")
                updates.UI_Update(dg_product, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                $"SELECT * FROM products ORDER BY "));
        }
    }
}
