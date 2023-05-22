using System;

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

        private readonly string _day_of_year = DateTime.Now.DayOfYear.ToString();
        public string Day_Of_Year
        {
            get
            {
                return _day_of_year;
            }
        }
    }
}
