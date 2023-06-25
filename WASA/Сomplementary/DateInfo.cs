using System;
using System.Windows.Controls;

namespace WASA.Сomplementary
{
    internal class DateInfo
    {
        private readonly string _date = DateTime.Now.ToString("dd.MM.yyyy");
        public string Date
        {
            get
            {
                return _date;
            }
        }

        

        private readonly string _day_of_week = DateTime.Now.ToString("dddd");
        public string Day_Of_Week
        {
            get
            {
                return _day_of_week;
            }
        }

        private readonly int _day_of_year = DateTime.Now.DayOfYear;
        public int Day_Of_Year
        {
            get
            {
                return _day_of_year;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Title_name"> Available name is "Main", "Product", "Sell", "Settings", "Users"</param>
        /// <param name="UserUI_Label_Date"></param>
        /// <param name="UserUI_Label_Day_Of_Week"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string Set_DateInfo(string Title_name, DateTime d, string user, string user_role, string selected_type)
        {
            switch (Title_name)
            {
                case "Main":
                    return _date + " " + _day_of_week + d.ToString(" HH:mm:ss  ") + "Главное меню (Смена №" + _day_of_year + ")  " + user_role + ": " + user;
                case "Product":
                    return _date + " " + _day_of_week + d.ToString(" HH:mm:ss  ") + "Товар  " + user_role + ": " + user + "";
                case "Sell":
                    return _date + " " + _day_of_week + d.ToString(" HH:mm:ss  ") + "Смена №" + _day_of_year + "  " + user_role + ": " + user;
                case "Settings":
                    return _date + " " + _day_of_week + d.ToString(" HH:mm:ss  ") + "Товар   " + user_role + ": " + user + "Выбранный тип: " + selected_type;
                case "Users":
                    return _date + " " + _day_of_week + d.ToString(" HH:mm:ss  ") + "Пользователи  " + user_role + ": " + user ;
                default:
                    return null!;
            }
        }
    }
}
