using System;
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




    }
}
