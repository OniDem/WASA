using Npgsql;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WASA.Сomplementary
{
    internal class Moves
    {
        DateInfo dateInfo = new();
        NpgsqlCommand command = new();
        NpgsqlConnection con = new();
        UI_Updates updates = new();
        int _cash, _aq, _count, cash_box, week_sum, _shift_sum = 0;

        public void Adding(CheckBox cash, CheckBox aq, TextBlock all_cash, TextBlock all_aq, TextBox time, TextBox article, TextBox position, TextBox count, TextBox price, TextBox discount, string user, int selected_shift)
        {
            try
			{
                
                con = new(Connection.GetConnectionString());
                con.Open();
                command = new($"SELECT shift_sum FROM accounting WHERE shift = '{Convert.ToInt32(selected_shift)}'", con);
                _shift_sum = Convert.ToInt32(command.ExecuteScalar());
                if (_shift_sum == 0)
                {
                    cash_box = Convert.ToInt32(command.ExecuteScalar());
                    command = new($"SELECT week_sum FROM accounting WHERE shift = '{Convert.ToInt32(selected_shift) - 1}'", con);
                    week_sum = Convert.ToInt32(command.ExecuteScalar());
                    command = new($"SELECT cash_box FROM accounting WHERE shift = '{Convert.ToInt32(selected_shift) - 1}'", con);
                    cash_box = Convert.ToInt32(command.ExecuteScalar());
                    command = new($"INSERT INTO accounting (date, shift, cash, acquiring, shift_sum, cash_box, week_sum) VALUES ('{dateInfo.Date}', '{selected_shift}', '{_cash}', '{_aq}', '{_shift_sum}', '{cash_box}', '{week_sum}')", con);
                    command.ExecuteNonQuery();
                }
                else
                {
                    command = new($"SELECT week_sum FROM accounting WHERE shift = '{Convert.ToInt32(selected_shift)}'", con);
                    week_sum = Convert.ToInt32(command.ExecuteScalar());
                    command = new($"SELECT cash_box FROM accounting WHERE shift = '{Convert.ToInt32(selected_shift)}'", con);
                    cash_box = Convert.ToInt32(command.ExecuteScalar());
                }

                if (cash.IsChecked == true)
                {
                    command = new($"SELECT cash FROM accounting WHERE shift = '{selected_shift}'", con);
                    _cash = Convert.ToInt32(command.ExecuteScalar());
                    _cash += Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text);
                    all_cash.Text = Convert.ToString(_cash);
                    command = new($"SELECT shift_sum FROM accounting WHERE shift = '{selected_shift}'", con);
                    _shift_sum = Convert.ToInt32(command.ExecuteScalar());
                    _shift_sum += Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text);
                    cash_box = cash_box + (Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text));
                    week_sum = week_sum + (Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text));
                    command = new($"UPDATE accounting SET cash='{_cash}' WHERE shift = '{Convert.ToInt32(selected_shift)}';", con);
                    command.ExecuteNonQuery();
                    command = new($"UPDATE accounting SET cash_box='{cash_box}' WHERE shift = '{Convert.ToInt32(selected_shift)}';", con);
                    command.ExecuteNonQuery();
                    command = new($"UPDATE accounting SET week_sum='{week_sum}' WHERE shift = '{Convert.ToInt32(selected_shift)}';", con);
                    command.ExecuteNonQuery();
                    command = new($"UPDATE accounting SET shift_sum='{_shift_sum}' WHERE shift = '{Convert.ToInt32(selected_shift)}';", con);
                    command.ExecuteNonQuery();
                }
                if (aq.IsChecked == true)
                {
                    command = new($"SELECT acquiring FROM accounting WHERE shift = '{selected_shift}'", con);
                    _aq = Convert.ToInt32(command.ExecuteScalar());
                    _aq += Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text);
                    all_aq.Text = Convert.ToString(_aq);
                    command = new($"SELECT shift_sum FROM accounting WHERE shift = '{selected_shift}'", con);
                    _shift_sum = Convert.ToInt32(command.ExecuteScalar());
                    _shift_sum += Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text);
                    week_sum = week_sum + (Convert.ToInt32(price.Text) - Convert.ToInt32(discount.Text));
                    command = new($"UPDATE accounting SET acquiring='{_aq}' WHERE shift = '{Convert.ToInt32(selected_shift)}';", con);
                    command.ExecuteNonQuery();
                    command = new($"UPDATE accounting SET week_sum='{week_sum}' WHERE shift = '{Convert.ToInt32(selected_shift)}';", con);
                    command.ExecuteNonQuery();
                    command = new($"UPDATE accounting SET shift_sum='{_shift_sum}' WHERE shift = '{Convert.ToInt32(selected_shift)}'    ;", con);
                    command.ExecuteNonQuery();
                }
                command = new($"INSERT INTO sale (shift, time, article, position, count,  price, discount, seller) VALUES ('{selected_shift}', '{time.Text}', '{article.Text}', '{position.Text}', '{count.Text}', '{price.Text}', '{discount.Text}', '{user}')", con);
                command.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception)
            {
                throw;
			}
        }

        /// <summary>
        /// Функция для изменения баланса товарной единицы на складе при продаже
        /// </summary>
        /// <param name="article">TextBox с артикулом</param>
        /// <param name="count">TextBox с количеством</param>
        /// <param name="time">TextBox с текущем временем</param>
        /// <param name="user">Строка с текущим пользователем</param>
        public void Change_Balance(TextBox article, TextBox count, TextBox time, string user)
        {
            try
            {
                con = new(Connection.GetConnectionString());
                string balance;

                balance = Select("product_count", article);
                if (balance != "")
                {
                    con.Open();
                    command = new($"UPDATE products SET product_count='{Convert.ToInt32(balance) - Convert.ToInt32(count.Text)}' WHERE article='{article.Text}';", con);
                    command.ExecuteNonQuery();
                    command = new($"UPDATE products SET change='{user + " " + time.Text}' WHERE article='{article.Text}';", con);
                    command.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Функция для изменения баланса товарной единицы на складе при удалении продажи
        /// </summary>
        /// <param name="delete_id">TextBox с id</param>
        /// <param name="time">TextBox с текущем временем</param>
        /// <param name="user">Строка с текущим пользователем</param>
        public void Change_Balance(TextBox delete_id, TextBox time, string user)
        {
            try
            {
                con = new(Connection.GetConnectionString());
                string balance;
                con.Open();
                command = new($"SELECT count FROM sale WHERE id = '{delete_id.Text}'", con);
                int count = Convert.ToInt32(command.ExecuteScalar());
                command = new($"SELECT article FROM sale WHERE id = '{delete_id.Text}'", con);
                string? article = Convert.ToString(command.ExecuteScalar());

                balance = Select("product_count", article!);
                if (balance != "")
                {
                    command = new($"UPDATE products SET product_count='{Convert.ToInt32(balance) + (count)}' WHERE article='{article}';", con);
                    command.ExecuteNonQuery();
                    command = new($"UPDATE products SET change='{user + " " + time.Text}' WHERE internal_article='{article}';", con);
                    command.ExecuteNonQuery();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChangeProduct(CheckBox plus, CheckBox minus, CheckBox set, TextBox change_count, TextBox change_position, TextBox change_price, TextBox change_internal_article, string current_user, string time,
            DataGrid dg_product, string selected_type)
        {
            con = new(Connection.GetConnectionString());
            con.Open();

            if (plus.IsEnabled || minus.IsEnabled || set.IsEnabled == true)
            {

                command = new($"SELECT product_count FROM products WHERE article = '{change_internal_article.Text}';", con);
                _count = Convert.ToInt32(command!.ExecuteScalar());

                if (plus.IsChecked == true)
                {
                    _count = _count + Convert.ToInt32(change_count.Text);
                    command = new($"UPDATE products SET product_count='{_count}' WHERE article='{change_internal_article.Text}';", con);
                    UpdateChanges(current_user, time, change_internal_article);
                }
                else if (minus.IsChecked == true)
                {
                    if ((_count - Convert.ToInt32(change_count.Text)) >= 0)
                    {
                        _count = _count - Convert.ToInt32(change_count.Text);
                        command = new($"UPDATE products SET product_count='{_count}' WHERE article='{change_internal_article.Text}';", con);
                        UpdateChanges(current_user, time, change_internal_article);
                    }
                    else
                    {
                        MessageBox.Show("Вы хотите изменить больше чем есть!");
                    }
                }
                else if (set.IsChecked == true)
                {
                    _count = Convert.ToInt32(change_count.Text);
                    command = new($"UPDATE products SET product_count='{_count}' WHERE article='{change_internal_article.Text}';", con);
                    UpdateChanges(current_user, time, change_internal_article);
                }
            }
            else if (change_price.IsEnabled == true)
            {
                command = new($"UPDATE products SET product_price='{change_price.Text}' WHERE internal_article='{change_internal_article.Text}';", con);
                UpdateChanges(current_user, time, change_internal_article);
            }
            else if (change_position.IsEnabled == true)
            {
                command = new($"UPDATE products SET product_name='{change_position.Text}' WHERE article='{change_internal_article.Text}';", con);
                UpdateChanges(current_user, time, change_internal_article);
            }
            else
            {
                MessageBox.Show("Выберите действие!");
            }
            con.Close();
            if (selected_type == "Всё")
                updates!.UI_Update(dg_product, $"SELECT * FROM products ORDER BY article;", con);
            else
                updates!.UI_Update(dg_product, $"SELECT * FROM products WHERE product_type = '{selected_type}' ORDER BY article;", con);
            
        }


        /// <summary>
        /// Функция для получения наименования, цены, количества из бд по внешнему/внутреннему артикулу
        /// </summary>
        /// <param name="selected">Выбор наименования, цены, количества (product_name, product_price, product_count)</param>
        /// <param name="article">TextBox c артикулом</param>
        /// <returns></returns>
        public string Select(string? selected, TextBox article)
        {

            string? selected_data = "";
            try
            {
                con = new(Connection.GetConnectionString());
                con.Open();
                    switch (selected)
                    {
                        case "product_name":
                            command = new($"SELECT product_name FROM products WHERE article = '{article.Text}'", con);
                            selected_data = Convert.ToString(command.ExecuteScalar());
                            break;
                        case "product_price":
                            command = new($"SELECT product_price FROM products WHERE article = '{article.Text}'", con);
                            selected_data = Convert.ToString(command.ExecuteScalar());
                            break;
                        case "product_count":
                            command = new($"SELECT product_count FROM products WHERE article = '{article.Text}'", con);
                            selected_data = Convert.ToString(command.ExecuteScalar());
                            break;
                    }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return selected_data!;
        }

        /// <summary>
        /// Функция для получения наименования, цены, количества из бд по внешнему/внутреннему артикулу
        /// </summary>
        /// <param name="selected">Выбор наименования, цены, количества (product_name, product_price, product_count)</param>
        /// <param name="article">Строка c артикулом</param>
        /// <param name="choice_article">Если true - внутренний, если false внешний</param>
        /// <returns></returns>
        public string Select(string? selected, string article)
        {

            string? selected_data = "";
            try
            {
                con = new(Connection.GetConnectionString());
                con.Open();                
                    switch (selected)
                    {
                        case "product_name":
                            command = new($"SELECT product_name FROM products WHERE article = '{article}'", con);
                            selected_data = Convert.ToString(command.ExecuteScalar());
                            break;
                        case "product_price":
                            command = new($"SELECT product_price FROM products WHERE article = '{article}'", con);
                            selected_data = Convert.ToString(command.ExecuteScalar());
                            break;
                        case "product_count":
                            command = new($"SELECT product_count FROM products WHERE article = '{article}'", con);
                            selected_data = Convert.ToString(command.ExecuteScalar());
                            break;
                    }                
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return selected_data!;
        }

        public async Task SelectPositionAsync(TextBox article, TextBox position)
        {

            //string? selected_data = "";
            try
            {
                con = new(Connection.GetConnectionString());
                await con.OpenAsync();
                command = new($"SELECT product_name FROM products WHERE article = '{article.Text}'", con);
                position.Text = Convert.ToString(await command.ExecuteScalarAsync());
                await con.CloseAsync();
            }
            finally
            {

            }
        }

        public void Delete(TextBox delete_id)
        {
            try
            {
                con = new(Connection.GetConnectionString());
                con.Open();
                command = new($"DELETE FROM sale WHERE id='{Convert.ToInt32(delete_id.Text)}'", con);
                command.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception)
            {
            }

        }

        public async Task Auth(TextBox login, PasswordBox password)
        {
            con = new(Connection.GetConnectionString());
            if (login.Text.Length > 1 && password.Password.Length > 1)
            {
                await con.OpenAsync();
                command = new($"SELECT user_password FROM users WHERE user_name = '{login.Text}'", con);
                if (Convert.ToString(await command.ExecuteScalarAsync()) == password.Password)
                {
                    command = new($"SELECT verifided FROM users WHERE user_name = '{login.Text}'", con);
                    bool verifided = Convert.ToBoolean(await command.ExecuteScalarAsync()!);


                    if (verifided == true)
                    {
                        command = new($"UPDATE settings SET seller='{login.Text}' WHERE settings_id='1';", con);
                        await command.ExecuteNonQueryAsync();
                        MainWindow mainWindow = new();
                        mainWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Ваша учётная запись не верифицирована, обратитесь к администратору!");
                        login.Clear();
                        password.Clear();
                    }
                }
                else
                    MessageBox.Show("Неккоректные данные!");
                await con.CloseAsync();
            }
            else
                MessageBox.Show("Одно или оба поля пустые!");
        }

        private void UpdateChanges(string current_user, string time, TextBox change_internal_article)
        {
            command.ExecuteNonQuery();
            command = new($"UPDATE products SET change='{current_user + " " + time}' WHERE article='{change_internal_article.Text}';", con);
            command.ExecuteNonQuery();
        }
    }
}
