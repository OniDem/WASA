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
        public void Balance_Change(TextBox change_external_article, TextBox change_internal_article, int _count, string current_user, Label UserUI_Label_RealTime , Button Select_All,
            DataGrid dataGrid, Button Sort_Article, Button Sort_Name, Button Sort_Price, Button Sort_Balance, Button Sort_Add_man, Button Sort_Change_Text, Button Sort_Type, string selected_type)
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
            if (Select_All.Background != Brushes.Aqua)
            {

                updates!.UI_Update(dataGrid, selected!.Selected(Sort_Article, Sort_Name, Sort_Price, Sort_Balance, Sort_Add_man, Sort_Change_Text, Sort_Type,
                   $"SELECT * FROM products WHERE product_type = '{selected_type}' ORDER BY "));
            }
            else
            {
                updates!.UI_Update(dataGrid, $"SELECT * FROM products  ORDER BY internal_article;");
            }
        }
    }
}
