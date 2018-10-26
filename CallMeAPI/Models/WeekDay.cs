using System;
using System.Collections.Generic;

namespace CallMeAPI.Models
{
    public class WeekDay
    {

        public WeekDay(){}

        public string name { get; set; }
        public bool isOpen { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }

        public override string ToString()
        {
            return name + "|" + isOpen + "|" + startTime + "|" + endTime;
        }

        public WeekDay(string str)
        {
            string[] split_str = str.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (split_str.Length != 4)
                throw new InvalidCastException();

            name = split_str[0];
            isOpen = bool.Parse(split_str[1]);
            startTime = split_str[2];
            endTime = split_str[3];

        }

        public static string ConvertToString(IEnumerable<WeekDay> weekdays)
        {
            string result = "";
            foreach(WeekDay day in weekdays)
            {
                result += (day + "$");
            }
            return result;
        }

        public static List<WeekDay> GetFromString(string str)
        {
            List<WeekDay> weekDays = new List<WeekDay>();
            if (string.IsNullOrEmpty(str))
            {
                return weekDays;
            }

            string[] split_str = str.Split("$".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string split in split_str)
            {
                try
                {
                    weekDays.Add(new WeekDay(split));
                }
                catch (Exception) { }
            }
            return weekDays;
        }
    }
}
