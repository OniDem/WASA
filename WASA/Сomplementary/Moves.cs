﻿using Npgsql;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WASA.Сomplementary
{
    internal class Moves
    {
        DateInfo dateInfo = new DateInfo();
        NpgsqlCommand command = new NpgsqlCommand();
        NpgsqlConnection con = new NpgsqlConnection();
        Current_User user = new Current_User();
        Moves_With_DB moves = new Moves_With_DB();
        UI_Updates updates = new UI_Updates();
        int _all_cash, _all_aq, _all, _count = 1;
        string? internal_article;

        public void Adding(CheckBox cash, CheckBox aq, TextBlock all_cash, TextBlock all_aq, TextBlock all, TextBox time, TextBox article, TextBox position, TextBox count, TextBox price, TextBox discount)
        {

            
            try
			{
                con = new NpgsqlConnection(Connection.GetConnectionString());
                con.Open();
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
                con.Close();
            }
            catch (Exception)
            {
                throw;
			}
        }

        public void Change_Balance(TextBox article, TextBox count, TextBox time)
        {
            try
            {
                con.Open();
                string balance;
                balance = moves.Select("product_count", article, true);
                if(balance != "")
                {
                    command = new NpgsqlCommand($"UPDATE products SET product_count='{Convert.ToInt32(balance) - Convert.ToInt32(count.Text)}' WHERE internal_article='{article.Text}';", con);
                    command.ExecuteNonQuery();
                    command = new NpgsqlCommand($"UPDATE products SET change='{user.GetCurrenUser() + " " + time.Text}' WHERE internal_article='{article.Text}';", con);
                    command.ExecuteNonQuery();
                }
                else
                {
                    balance = moves.Select("product_count", article, false);
                    command = new NpgsqlCommand($"UPDATE products SET product_count='{Convert.ToInt32(balance) - Convert.ToInt32(count.Text)}' WHERE external_article='{article.Text}';", con);
                    command.ExecuteNonQuery();
                    command = new NpgsqlCommand($"UPDATE products SET change='{user.GetCurrenUser() + " " + time.Text}' WHERE external_article='{article.Text}';", con);
                    command.ExecuteNonQuery();
                    
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        public void ChangeProduct(CheckBox plus, CheckBox minus, CheckBox set, TextBox change_count, TextBox change_position, TextBox change_price, TextBox change_external_article, TextBox change_internal_article, string current_user, Label UserUI_Label_RealTime,
            DataGrid dg_product)
        {
            con = new NpgsqlConnection(Connection.GetConnectionString());
            con.Open();
            
            if (plus.IsEnabled || minus.IsEnabled || set.IsEnabled == true)
            {
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

                if (change_external_article.Text != "")
                {
                    command = new NpgsqlCommand($"SELECT internal_article FROM products WHERE external_article = '{change_external_article.Text}';", con);
                    internal_article = Convert.ToString(command!.ExecuteScalar());
                }
                else
                {
                    internal_article = change_internal_article.Text;
                }

                if (plus.IsChecked == true)
                {
                    _count = _count + Convert.ToInt32(change_count.Text);
                    command = new NpgsqlCommand($"UPDATE products SET product_count='{_count}' WHERE internal_article='{internal_article}';", con);
                    UpdateChanges("Other", current_user, UserUI_Label_RealTime, change_internal_article, change_external_article, internal_article!);
                }
                else if (minus.IsChecked == true)
                {
                    if ((_count - Convert.ToInt32(change_count.Text)) >= 0)
                    {
                        _count = _count - Convert.ToInt32(change_count.Text);
                        command = new NpgsqlCommand($"UPDATE products SET product_count='{_count}' WHERE internal_article='{internal_article}';", con);
                        UpdateChanges("Other", current_user, UserUI_Label_RealTime, change_internal_article, change_external_article, internal_article!);
                    }
                    else
                    {
                        MessageBox.Show("Вы хотите изменить больше чем есть!");
                    }
                }
                else if (set.IsChecked == true)
                {
                    _count = Convert.ToInt32(change_count.Text);
                    command = new NpgsqlCommand($"UPDATE products SET product_count='{_count}' WHERE internal_article='{internal_article}';", con);
                    UpdateChanges("Other", current_user, UserUI_Label_RealTime, change_internal_article, change_external_article, internal_article!);
                }
            }
            else if(change_price.IsEnabled == true)
            {
                if (change_external_article.Text == "" && change_internal_article.Text != "")
                {
                    command = new NpgsqlCommand($"UPDATE products SET product_price='{change_price.Text}' WHERE internal_article='{change_internal_article.Text}';", con);
                    UpdateChanges("Internal", current_user, UserUI_Label_RealTime, change_internal_article, change_external_article, internal_article!);
                }
                else if (change_external_article.Text != "" && change_internal_article.Text == "")
                {
                    command = new NpgsqlCommand($"UPDATE products SET product_price='{change_price.Text}' WHERE external_article='{change_external_article.Text}';", con);
                    UpdateChanges("External", current_user, UserUI_Label_RealTime, change_internal_article, change_external_article, internal_article!);
                }
                else if (change_external_article.Text != "" && change_internal_article.Text != "")
                {
                    command = new NpgsqlCommand($"UPDATE products SET product_price='{change_price.Text}' WHERE internal_article='{change_internal_article.Text}';", con);
                    UpdateChanges("Internal", current_user, UserUI_Label_RealTime, change_internal_article, change_external_article, internal_article!);
                }
            }
            else if(change_position.IsEnabled == true)
            {
                if (change_external_article.Text == "" && change_internal_article.Text != "")
                {
                    command = new NpgsqlCommand($"UPDATE products SET product_name='{change_position.Text}' WHERE internal_article='{change_internal_article.Text}';", con);
                    UpdateChanges("Internal", current_user, UserUI_Label_RealTime, change_internal_article, change_external_article, internal_article!);
                }
                else if (change_external_article.Text != "" && change_internal_article.Text == "")
                {
                    command = new NpgsqlCommand($"UPDATE products SET product_name='{change_position.Text}' WHERE external_article='{change_external_article.Text}';", con);
                    UpdateChanges("External", current_user, UserUI_Label_RealTime, change_internal_article, change_external_article, internal_article!);
                }
                else if (change_external_article.Text != "" && change_internal_article.Text != "")
                {
                    command = new NpgsqlCommand($"UPDATE products SET product_name='{change_position.Text}' WHERE internal_article='{change_internal_article.Text}';", con);
                    UpdateChanges("Internal", current_user, UserUI_Label_RealTime, change_internal_article, change_external_article, internal_article!);
                }
            }
            else
            {
                MessageBox.Show("Выберите действие!");
            }
            updates!.UI_Update(dg_product, $"SELECT * FROM products");
            con.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="article">Internal, External, Other</param>
        private void UpdateChanges(string article, string current_user, Label UserUI_Label_RealTime, TextBox change_internal_article, TextBox change_external_article, string internal_article)
        {
            command.ExecuteNonQuery();
            if (article == "Internal")
            {
                command = new NpgsqlCommand($"UPDATE products SET change='{current_user + " " + UserUI_Label_RealTime.Content}' WHERE internal_article='{change_internal_article.Text}';", con);
            }
            else if (article == "External")
            {
                command = new NpgsqlCommand($"UPDATE products SET change='{current_user + " " + UserUI_Label_RealTime.Content}' WHERE external_article='{change_external_article.Text}';", con);
            }
            if (article == "Other")
            {
                command = new NpgsqlCommand($"UPDATE products SET product_count='{_count}' WHERE internal_article='{internal_article}';", con);
                command.ExecuteNonQuery();
                command = new NpgsqlCommand($"UPDATE products SET change='{current_user + " " + UserUI_Label_RealTime.Content}' WHERE internal_article='{internal_article}';", con);
                command.ExecuteNonQuery();
            }
            command.ExecuteNonQuery();
        }
    }
}
