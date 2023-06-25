using Npgsql;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace WASA.Сomplementary
{
    internal class UI_Updates
    {
        private NpgsqlCommand? command;


        public void UI_Update(DataGrid dataGrid, string sql, NpgsqlConnection con)
        {
            try
            {
               // NpgsqlConnection con = new NpgsqlConnection(Connection.GetConnectionString());
                con.Open();
                DataTable dt = new DataTable();
                command = new NpgsqlCommand(sql, con);
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

        public async void UI_UpdateAsync(TextBox delete_id, Button delete, TextBlock all_cash, TextBlock all_aq, TextBlock all, DataGrid dataGrid, string sql, int dayofyear, NpgsqlConnection con)
        {
            try
            {
                await con.OpenAsync();
                DataTable dt = new DataTable();
                command = new NpgsqlCommand(sql, con);
                await command.ExecuteNonQueryAsync();
                dt.Load(await command.ExecuteReaderAsync(CommandBehavior.CloseConnection));
                dataGrid.DataContext = dt;
                dataGrid.DataContext = dt.DefaultView;
                command = new NpgsqlCommand($"SELECT id FROM sale WHERE shift = '{dayofyear}' ORDER BY id DESC", con);
                delete_id.Text = Convert.ToString(await command.ExecuteScalarAsync());
                command = new NpgsqlCommand($"SELECT cash FROM accounting WHERE shift = '{dayofyear}'", con);
                all_cash.Text = Convert.ToString(await command.ExecuteScalarAsync());
                command = new NpgsqlCommand($"SELECT acquiring FROM accounting WHERE shift = '{dayofyear}'", con);
                all_aq.Text = Convert.ToString(await command.ExecuteScalarAsync());
                command = new NpgsqlCommand($"SELECT shift_sum FROM accounting WHERE shift = '{dayofyear}'", con);
                all.Text = Convert.ToString(await command.ExecuteScalarAsync());
                await con.CloseAsync();

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

        public void UI_Update(TextBox delete_id, Button delete, TextBlock all_cash, TextBlock all_aq, TextBlock all, DataGrid dataGrid, string sql, int dayofyear)
        {
            try
            {
                NpgsqlConnection con = new NpgsqlConnection(Connection.GetConnectionString());

                UI_Update(dataGrid, sql, con);

                con.Open();
                command = new NpgsqlCommand($"SELECT id FROM sale WHERE shift = '{dayofyear}' ORDER BY id DESC", con);
                delete_id.Text = Convert.ToString(command.ExecuteScalar());
                command = new NpgsqlCommand($"SELECT cash FROM accounting WHERE shift = '{dayofyear}'", con);
                all_cash.Text = Convert.ToString( command.ExecuteScalar());
                command = new NpgsqlCommand($"SELECT acquiring FROM accounting WHERE shift = '{dayofyear}'", con);
                all_aq.Text = Convert.ToString(command.ExecuteScalar());
                command = new NpgsqlCommand($"SELECT shift_sum FROM accounting WHERE shift = '{dayofyear}'", con);
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

