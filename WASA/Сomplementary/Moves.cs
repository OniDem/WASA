using Npgsql;
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
        UI_Updates updates = new UI_Updates();
        int _cash, _aq, _count, cash_box, week_sum, _shift_sum = 0;
        string? internal_article;

        public void Adding(CheckBox cash, CheckBox aq, TextBlock all_cash, TextBlock all_aq, TextBlock all, TextBox time, TextBox article, TextBox position, TextBox count, TextBox price, TextBox discount, string user)
        {
            try
			{
                
                con = new NpgsqlConnection(Connection.GetConnectionString());
                con.Open();
                command = new NpgsqlCommand($"SELECT shift_sum FROM accounting WHERE shift = '{Convert.ToInt32(dateInfo.Day_Of_Year)}'", con);
                _shift_sum = Convert.ToInt32(command.ExecuteScalar());
                if (_shift_sum == 0)
                {
                    cash_box = Convert.ToInt32(command.ExecuteScalar());
                    command = new NpgsqlCommand($"SELECT week_sum FROM accounting WHERE shift = '{Convert.ToInt32(dateInfo.Day_Of_Year) - 1}'", con);
                    week_sum = Convert.ToInt32(command.ExecuteScalar());
                    command = new NpgsqlCommand($"SELECT cash_box FROM accounting WHERE shift = '{Convert.ToInt32(dateInfo.Day_Of_Year) - 1}'", con);
                    cash_box = Convert.ToInt32(command.ExecuteScalar());
                    command = new NpgsqlCommand($"INSERT INTO accounting (date, shift, cash, acquiring, shift_sum, cash_box, week_sum) VALUES ('{dateInfo.Date}', '{dateInfo.Day_Of_Year}', '{_cash}', '{_aq}', '{_shift_sum}', '{cash_box}', '{week_sum}')", con);
                    command.ExecuteNonQuery();
                }
                else
                {
                    command = new NpgsqlCommand($"SELECT week_sum FROM accounting WHERE shift = '{Convert.ToInt32(dateInfo.Day_Of_Year)}'", con);
                    week_sum = Convert.ToInt32(command.ExecuteScalar());
                    command = new NpgsqlCommand($"SELECT cash_box FROM accounting WHERE shift = '{Convert.ToInt32(dateInfo.Day_Of_Year)}'", con);
                    cash_box = Convert.ToInt32(command.ExecuteScalar());
                }
                //475310

                if (cash.IsChecked == true)
                {
                    command = new NpgsqlCommand($"SELECT cash FROM accounting WHERE shift = '{dateInfo.Day_Of_Year}'", con);
                    _cash = Convert.ToInt32(command.ExecuteScalar());
                    _cash += Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text);
                    all_cash.Text = Convert.ToString(_cash);
                    command = new NpgsqlCommand($"SELECT shift_sum FROM accounting WHERE shift = '{dateInfo.Day_Of_Year}'", con);
                    _shift_sum = Convert.ToInt32(command.ExecuteScalar());
                    _shift_sum += Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text);
                    cash_box = cash_box + (Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text));
                    week_sum = week_sum + (Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text));
                    command = new NpgsqlCommand($"UPDATE accounting SET cash='{_cash}' WHERE shift = '{Convert.ToInt32(dateInfo.Day_Of_Year)}';", con);
                    command.ExecuteNonQuery();
                    command = new NpgsqlCommand($"UPDATE accounting SET cash_box='{cash_box}' WHERE shift = '{Convert.ToInt32(dateInfo.Day_Of_Year)}';", con);
                    command.ExecuteNonQuery();
                    command = new NpgsqlCommand($"UPDATE accounting SET week_sum='{week_sum}' WHERE shift = '{Convert.ToInt32(dateInfo.Day_Of_Year)}';", con);
                    command.ExecuteNonQuery();
                    command = new NpgsqlCommand($"UPDATE accounting SET shift_sum='{_shift_sum}' WHERE shift = '{Convert.ToInt32(dateInfo.Day_Of_Year)}';", con);
                    command.ExecuteNonQuery();
                }
                if (aq.IsChecked == true)
                {
                    command = new NpgsqlCommand($"SELECT acquiring FROM accounting WHERE shift = '{dateInfo.Day_Of_Year}'", con);
                    _aq = Convert.ToInt32(command.ExecuteScalar());
                    _aq += Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text);
                    all_aq.Text = Convert.ToString(_aq);
                    command = new NpgsqlCommand($"SELECT shift_sum FROM accounting WHERE shift = '{dateInfo.Day_Of_Year}'", con);
                    _shift_sum = Convert.ToInt32(command.ExecuteScalar());
                    _shift_sum += Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text);
                    week_sum = week_sum + (Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text));
                    command = new NpgsqlCommand($"UPDATE accounting SET acquiring='{_aq}' WHERE shift = '{Convert.ToInt32(dateInfo.Day_Of_Year)}';", con);
                    command.ExecuteNonQuery();
                    command = new NpgsqlCommand($"UPDATE accounting SET week_sum='{week_sum}' WHERE shift = '{Convert.ToInt32(dateInfo.Day_Of_Year)}';", con);
                    command.ExecuteNonQuery();
                    command = new NpgsqlCommand($"UPDATE accounting SET shift_sum='{_shift_sum}' WHERE shift = '{Convert.ToInt32(dateInfo.Day_Of_Year)}'    ;", con);
                    command.ExecuteNonQuery();
                }
                command = new NpgsqlCommand($"INSERT INTO sale (shift, time, article, position, count,  price, discount, seller) VALUES ('{dateInfo.Day_Of_Year}', '{time.Text}', '{article.Text}', '{position.Text}', '{count.Text}', '{price.Text}', '{discount.Text}', '{user}')", con);
                command.ExecuteNonQuery();
                //106890
                con.Close();
            }
            catch (Exception)
            {
                throw;
			}
        }

        public void Change_Balance(TextBox article, TextBox count, TextBox time, string user)
        {
            try
            {
                con = new NpgsqlConnection(Connection.GetConnectionString());
                string balance;
                balance = Select("product_count", article, true);
                
                if (balance != "")
                {
                    con.Open();
                    command = new NpgsqlCommand($"UPDATE products SET product_count='{Convert.ToInt32(balance) - Convert.ToInt32(count.Text)}' WHERE internal_article='{article.Text}';", con);
                    command.ExecuteNonQuery();
                    command = new NpgsqlCommand($"UPDATE products SET change='{user + " " + time.Text}' WHERE internal_article='{article.Text}';", con);
                    command.ExecuteNonQuery();
                }
                else
                {

                    balance = Select("product_count", article, false);
                    con.Open();
                    command = new NpgsqlCommand($"UPDATE products SET product_count='{Convert.ToInt32(balance) - Convert.ToInt32(count.Text)}' WHERE external_article='{article.Text}';", con);
                    command.ExecuteNonQuery();
                    command = new NpgsqlCommand($"UPDATE products SET change='{user + " " + time.Text}' WHERE external_article='{article.Text}';", con);
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
            DataGrid dg_product, string selected_type)
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
            if (selected_type == "Всё")
                updates!.UI_Update(dg_product, $"SELECT * FROM products ORDER BY internal_article;", con);
            else
                        updates!.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{selected_type}' ORDER BY internal_article;", con);
            con.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="choice_article">
        /// If true article = internal; If false article = external</param>
        /// <returns></returns>
        public string Select(string? selected, TextBox article, bool choice_article)
        {

            string? selected_data = "";
            try
            {
                con = new NpgsqlConnection(Connection.GetConnectionString());
                con.Open();
                if (choice_article == true)
                {
                    switch (selected)
                    {
                        case "product_name":
                            command = new NpgsqlCommand($"SELECT product_name FROM products WHERE internal_article = '{article.Text}'", con);
                            selected_data = Convert.ToString(command.ExecuteScalar());
                            break;
                        case "product_price":
                            command = new NpgsqlCommand($"SELECT product_price FROM products WHERE internal_article = '{article.Text}'", con);
                            selected_data = Convert.ToString(command.ExecuteScalar());
                            break;
                        case "product_count":
                            command = new NpgsqlCommand($"SELECT product_count FROM products WHERE internal_article = '{article.Text}'", con);
                            selected_data = Convert.ToString(command.ExecuteScalar());
                            break;
                    }
                }
                else if (choice_article == false)
                {
                    switch (selected)
                    {
                        case "product_name":
                            command = new NpgsqlCommand($"SELECT product_name FROM products WHERE external_article = '{article.Text}'", con);
                            selected_data = Convert.ToString(command.ExecuteScalar());
                            break;
                        case "product_price":
                            command = new NpgsqlCommand($"SELECT product_price FROM products WHERE external_article = '{article.Text}'", con);
                            selected_data = Convert.ToString(command.ExecuteScalar());
                            break;
                        case "product_count":
                            command = new NpgsqlCommand($"SELECT product_count FROM products WHERE external_article = '{article.Text}'", con);
                            selected_data = Convert.ToString(command.ExecuteScalar());
                            break;
                    };
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return selected_data!;
        }

        public void Delete(TextBox delete_id)
        {
            try
            {
                con = new NpgsqlConnection(Connection.GetConnectionString());
                con.Open();
                command = new NpgsqlCommand($"DELETE FROM sale WHERE id='{Convert.ToInt32(delete_id.Text)}'", con);
                command.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception)
            {
            }

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
