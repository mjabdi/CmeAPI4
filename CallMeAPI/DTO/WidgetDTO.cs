using System;
using System.Collections.Generic;
using CallMeAPI.Models;

namespace CallMeAPI.DTO
{
    public class WidgetDTO
    {       
        public WidgetDTO()
        {
            
        }

        public WidgetDTO(Widget widget)
        {
            ID = widget.ID.ToString();
            Email = widget.User.UserID;
            WidgetName = widget.WidgetName;
            ConnectedTo = widget.ConnectedTo;
            Status = widget.Status;
            TalkToUsText = widget.TalkToUsText;
            ColorWidget = widget.ColorWidget;
            ColorText = widget.ColorText;
            IsAnimated = widget.IsAnimated;
            CallsCount = widget.CallsCount;
            DomainUrl = widget.DomainURL;

            List<WeekDay> dayList = WeekDay.GetFromString(widget.WeekDays);

            WeekDays = new WeekDay[dayList.Count];
            for (int i = 0; i < WeekDays.Length; i++)
            {
                WeekDays[i] = dayList[i];
            }
        }

        public string ID { get; set; }

        public string Email { get; set; }

        public string WidgetName { get; set; }

        public string ConnectedTo { get; set; }

        public string Status { get; set; }

        public string TalkToUsText { get; set; }

        public string ColorWidget { get; set; }

        public string ColorText { get; set; }

        public bool IsAnimated { get; set; }

        public int CallsCount { get; set; }

        public string DomainUrl { get; set; }

        public WeekDay[] WeekDays { get; set; }
    }
}
