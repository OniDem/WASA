﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="choice_article">
        /// If true article = internal; If false article = external</param>
        /// <returns></returns>
        public string Select(string? selected, string? table_name,  TextBox article, bool choice_article)
        {
            string? selected_data = "";
            try
            {
                NpgsqlConnection con = new NpgsqlConnection(Connection.GetConnectionString());
                con.Open();
                if (choice_article == true)
                {
                    switch (selected)
                    {
                        default:

                        case "product_name":
                            if (table_name == "products")
                            {
                                NpgsqlCommand command = new NpgsqlCommand($"SELECT product_name FROM products WHERE internal_article = '{article.Text}'", con);
                                selected_data = Convert.ToString(command.ExecuteScalar());
                            }
                            break;
                        case "product_price":
                            if (table_name == "products")
                            {
                                NpgsqlCommand command = new NpgsqlCommand($"SELECT product_price FROM products WHERE internal_article = '{article.Text}'", con);
                                selected_data = Convert.ToString(command.ExecuteScalar());
                            }
                            
                            break;
                        case "product_count":
                            if (table_name == "products")
                            {
                                NpgsqlCommand command = new NpgsqlCommand($"SELECT product_count FROM products WHERE internal_article = '{article.Text}'", con);
                                selected_data = Convert.ToString(command.ExecuteScalar());
                            }
                            break;

                    }
                }
                else if (choice_article == false)
                {
                    switch (selected)
                    {
                        default:

                        case "product_name":
                            if (table_name == "products")
                            {
                                NpgsqlCommand command = new NpgsqlCommand($"SELECT product_name FROM products WHERE external_article = '{article.Text}'", con);
                                selected_data = Convert.ToString(command.ExecuteScalar());
                            }
                            break;
                        case "product_price":
                            if (table_name == "products")
                            {
                                NpgsqlCommand command = new NpgsqlCommand($"SELECT product_price FROM products WHERE external_article = '{article.Text}'", con);
                                selected_data = Convert.ToString(command.ExecuteScalar());
                            }

                            break;
                        case "product_count":
                            if (table_name == "products")
                            {
                                NpgsqlCommand command = new NpgsqlCommand($"SELECT product_count FROM products WHERE external_article = '{article.Text}'", con);
                                selected_data = Convert.ToString(command.ExecuteScalar());
                            }
                            break;
                    }
                }
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
                return selected_data!;
        }
    }
}
