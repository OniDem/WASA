using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Controls;

namespace WASA.Сomplementary
{
    internal class Balance_Changes
    {
        NpgsqlCommand? command;
        NpgsqlConnection? con;
        UI_Updates? updates = new UI_Updates();
        Selected_Type? selected =new Selected_Type();
        string? internal_article;
        public void Balance_Change(TextBox change_external_article, TextBox change_internal_article, int _count, string current_user, Label UserUI_Label_RealTime, DataGrid dataGrid)
        {
            con = new NpgsqlConnection(Connection.GetConnectionString());
            con.Open();
            if (change_external_article.Text != "")
            {
                command = new NpgsqlCommand($"SELECT internal_article FROM products WHERE external_article = '{change_external_article.Text}';", con);
                internal_article = Convert.ToString(command!.ExecuteScalar());
            }
            else
            {
                internal_article = change_internal_article.Text;
            }
            command = new NpgsqlCommand($"UPDATE products SET product_count='{_count}' WHERE internal_article='{internal_article}';", con);
            command.ExecuteNonQuery();
            command = new NpgsqlCommand($"UPDATE products SET change='{current_user + " " + UserUI_Label_RealTime.Content}' WHERE internal_article='{internal_article}';", con);
            command.ExecuteNonQuery();
            con.Close();
                updates!.UI_Update(dataGrid, $"SELECT * FROM products;");
        }
    }
}
