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

        public string Set_DateInfo(Label UserUI_Label_Date, Label UserUI_Label_Day_Of_Week)
        {
            UserUI_Label_Date.Content = _date;
            UserUI_Label_Day_Of_Week.Content = _day_of_week;
            return "Смена №" + _day_of_year;
        }
    }
}
