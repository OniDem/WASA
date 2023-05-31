using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WASA.Сomplementary
{
    internal class Sell_Moves
    {
        DateInfo dateInfo = new DateInfo();
        NpgsqlCommand command = new NpgsqlCommand();
        NpgsqlConnection con = new NpgsqlConnection();
        Current_User user = new Current_User();
        Moves_With_DB moves = new Moves_With_DB();
        int _all_cash, _all_aq, _all = 1;

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
    }
}
