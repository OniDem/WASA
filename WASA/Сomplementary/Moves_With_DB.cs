using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WASA.Сomplementary
{
    internal class Moves_With_DB
    {
        NpgsqlConnection con = new NpgsqlConnection(Connection.GetConnectionString());
        NpgsqlCommand? command;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="choice_article">
        /// If true article = internal; If false article = external</param>
        /// <returns></returns>
        public string Select(string? selected,  TextBox article, bool choice_article)
        {
            string? selected_data = "";
            try
            {
               
                con.Open();
                if (choice_article == true)
                {
                    switch (selected)
                    {
                        default:
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
            catch (Exception)
            {
            }
                return selected_data!;
        }

        public void Delete(TextBox delete_id)
        {
            try
            {
                con.Open();
                command = new NpgsqlCommand($"DELETE FROM sale WHERE id='{Convert.ToInt32(delete_id.Text)}'", con);
                command.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception)
            {
            }
            
        }
    }
}
