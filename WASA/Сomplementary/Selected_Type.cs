using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace WASA.Сomplementary
{
    internal class Selected_Type
    {
        public string Selected(Button Article, Button Name, Button Price, Button Balance, Button Add_Man, Button Change_Text, Button Sort_Type, string sql)
        {
            if (Article.Background == Brushes.Aqua && (string)Sort_Type.Content == "Убывание")
                return sql + "internal_article DESC";
            if (Article.Background == Brushes.Aqua && (string)Sort_Type.Content == "Возрастание")
                return sql + "internal_article ASC";

            if (Name.Background == Brushes.Aqua && (string)Sort_Type.Content == "Убывание")
                return sql + "product_name DESC";
            if (Name.Background == Brushes.Aqua && (string)Sort_Type.Content == "Возрастание")
                return sql + "product_name ASC";

            if (Price.Background == Brushes.Aqua && (string)Sort_Type.Content == "Убывание")
                return sql + "product_price DESC";
            if (Price.Background == Brushes.Aqua && (string)Sort_Type.Content == "Возрастание")
                return sql + "product_price ASC";

            if (Balance.Background == Brushes.Aqua && (string)Sort_Type.Content == "Убывание")
                return sql + "product_count DESC";
            if (Balance.Background == Brushes.Aqua && (string)Sort_Type.Content == "Возрастание")
                return sql + "product_count ASC";

            if (Add_Man.Background == Brushes.Aqua && (string)Sort_Type.Content == "Убывание")
                return sql + "add_man DESC";
            if (Add_Man.Background == Brushes.Aqua && (string)Sort_Type.Content == "Возрастание")
                return sql + "add_man ASC";

            if (Change_Text.Background == Brushes.Aqua && (string)Sort_Type.Content == "Убывание")
                return sql + "change DESC";
            if (Change_Text.Background == Brushes.Aqua && (string)Sort_Type.Content == "Возрастание")
                return sql + "change ASC";

            return sql + "article DESC";
        }
    }
}
