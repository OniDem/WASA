using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WASA.Сomplementary
{
    internal class UI_Updates
    {
        private NpgsqlCommand? command;
        NpgsqlConnection con = new(Connection.GetConnectionString());

        /// <summary>
        /// Метод для обновление DataGrid
        /// </summary>
        /// <param name="dataGrid">DataGrid для обновения</param>
        /// <param name="sql">Запрос для заполнения DataGrid</param>
        /// <param name="con">Строка подключения к БД</param>
        public void UI_Update(DataGrid dataGrid, string sql, NpgsqlConnection con)
        {
            try
            {
                con.Open();
                DataTable dt = new();
                command = new(sql, con);
                command.ExecuteNonQuery();
                dt.Load(command.ExecuteReader(CommandBehavior.CloseConnection));
                dataGrid.DataContext = dt;
                dataGrid.DataContext = dt.DefaultView;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Асинхронный метод для обновления DataGrid и итогов
        /// </summary>
        /// <param name="delete_id">TextBox с id для удаления продажи</param>
        /// <param name="delete">Button для удаления продажи</param>
        /// <param name="all_cash">TextBlock с итогом по наличке для обновления</param>
        /// <param name="all_aq">TextBlock с итогом по эквайрингу для обновления</param>
        /// <param name="all">TextBlock с общим итогом для обновления</param>
        /// <param name="dataGrid">DataGrid для обновения</param>
        /// <param name="sql">Запрос для заполнения DataGrid</param>
        /// <param name="dayofyear">Текущая смена(int)</param>
        /// <returns></returns>
        public async void UI_UpdateAsync(TextBox delete_id, Button delete, TextBlock all_cash, TextBlock all_aq, TextBlock all, DataGrid dataGrid, string sql, int dayofyear)
        {
            try
            {
                await con.OpenAsync();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            DataTable dt = new();
            command = new(sql, con);
            await command.ExecuteNonQueryAsync();
            dt.Load(await command.ExecuteReaderAsync(CommandBehavior.CloseConnection));
            dataGrid.DataContext = dt;
            dataGrid.ItemsSource = dt.DefaultView;
            await con.OpenAsync();
            command = new($"SELECT id FROM sale WHERE shift = '{dayofyear}' ORDER BY id DESC", con);
            delete_id.Text = Convert.ToString(await command.ExecuteScalarAsync());
            command = new($"SELECT cash FROM accounting WHERE shift = '{dayofyear}'", con);
            all_cash.Text = Convert.ToString(await command.ExecuteScalarAsync());
            command = new($"SELECT acquiring FROM accounting WHERE shift = '{dayofyear}'", con);
            all_aq.Text = Convert.ToString(await command.ExecuteScalarAsync());
            command = new($"SELECT shift_sum FROM accounting WHERE shift = '{dayofyear}'", con);
            all.Text = Convert.ToString(await command.ExecuteScalarAsync());
            await con.CloseAsync();

            delete.IsEnabled = false;

            if (delete_id.Text != "")
            {
                delete.IsEnabled = true;
            }
        }

        /// <summary>
        /// Метод для обновления DataGrid и итогов
        /// </summary>
        /// <param name="delete_id">TextBox с id для удаления продажи</param>
        /// <param name="delete">Button для удаления продажи</param>
        /// <param name="all_cash">TextBlock с итогом по наличке для обновления</param>
        /// <param name="all_aq">TextBlock с итогом по эквайрингу для обновления</param>
        /// <param name="all">TextBlock с общим итогом для обновления</param>
        /// <param name="dataGrid">DataGrid для обновения</param>
        /// <param name="sql">Запрос для заполнения DataGrid</param>
        /// <param name="dayofyear">Текущая смена(int)</param>
        /// <returns></returns>
        public void UI_Update(TextBox delete_id, Button delete, TextBlock all_cash, TextBlock all_aq, TextBlock all, DataGrid dataGrid, string sql, int dayofyear)
        {
            try
            {
                UI_Update(dataGrid, sql, con);

                con.Open();
                command = new($"SELECT id FROM sale WHERE shift = '{dayofyear}' ORDER BY id DESC", con);
                delete_id.Text = Convert.ToString(command.ExecuteScalar());
                command = new($"SELECT cash FROM accounting WHERE shift = '{dayofyear}'", con);
                all_cash.Text = Convert.ToString( command.ExecuteScalar());
                command = new($"SELECT acquiring FROM accounting WHERE shift = '{dayofyear}'", con);
                all_aq.Text = Convert.ToString(command.ExecuteScalar());
                command = new($"SELECT shift_sum FROM accounting WHERE shift = '{dayofyear}'", con);
                all.Text = Convert.ToString(command.ExecuteScalar());
                con.Close();

                delete.IsEnabled = false;

                if (delete_id.Text != "")
                {
                    delete.IsEnabled = true;

                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

