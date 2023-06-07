using Npgsql;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WASA.Сomplementary
{
    internal class UI_Updates
    {
        private NpgsqlCommand? command;


        public void UI_Update(DataGrid dataGrid, string sql)
        {
            try
            {
                NpgsqlConnection con = new NpgsqlConnection(Connection.GetConnectionString());
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

        public void UI_Update(TextBox delete_id, Button delete, TextBlock all_cash, TextBlock all_aq, TextBlock all, DataGrid dataGrid, string sql, int dayofyear)
        {
            try
            {
                NpgsqlConnection con = new NpgsqlConnection(Connection.GetConnectionString());
                
                UI_Update(dataGrid, sql);

                con.Open();
                command = new NpgsqlCommand($"SELECT id FROM sale WHERE shift = '{dayofyear}' ORDER BY id DESC", con);
                delete_id.Text = Convert.ToString(command.ExecuteScalar());
                command = new NpgsqlCommand($"SELECT cash FROM sale WHERE shift = '{dayofyear}' ORDER BY id DESC", con);
                all_cash.Text = Convert.ToString(command.ExecuteScalar());
                command = new NpgsqlCommand($"SELECT acquiring FROM sale WHERE shift = '{dayofyear}' ORDER BY id DESC", con);
                all_aq.Text = Convert.ToString(command.ExecuteScalar());
                command = new NpgsqlCommand($"SELECT total FROM sale WHERE shift = '{dayofyear}' ORDER BY id DESC", con);
                all.Text = Convert.ToString(command.ExecuteScalar());
                con.Close();

                delete.IsEnabled = false;

                if (delete_id.Text != "")
                {
                    delete.IsEnabled = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

