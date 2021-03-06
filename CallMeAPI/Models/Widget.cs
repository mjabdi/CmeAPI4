﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CallMeAPI.DTO;

namespace CallMeAPI.Models
{
    public class Widget
    {
        [Key]
        public Guid ID { get; set; }

        public string UserID { get; set; }

        public virtual User User { get; set; }

        public string DomainURL { get; set; }

        public string WidgetName { get; set; }

        public string ConnectedTo { get; set; }

        public string Status { get; set; }

        public string TalkToUsText { get; set; }

        public string ColorWidget { get; set; }

        public string ColorText { get; set; }

        public bool IsAnimated { get; set; }

        public int CallsCount { get; set; }

        public int CallsCountMonth { get; set; }

        public DateTime CreationDateTime { get; set; }

        public string AuthKey { get; set; }
        public string Extension { get; set; }

        public string NotificationEmail { get; set; }

        public string WeekDays { get; set; }

        public string subscriptionId { get; set; }

        public Widget()
        {
            
        }

        public Widget(WidgetDTO widgetDTO, MyDBContext context)
        {
            ID = Guid.NewGuid();

            UserID = widgetDTO.Email;
            this.User = context.Users.Find(UserID);
            DomainURL = widgetDTO.DomainUrl;
            WidgetName = widgetDTO.WidgetName;
            ConnectedTo = widgetDTO.ConnectedTo;
            Status = widgetDTO.Status;
            TalkToUsText = widgetDTO.TalkToUsText;
            ColorWidget = widgetDTO.ColorWidget;
            ColorText = widgetDTO.ColorText;
            IsAnimated = widgetDTO.IsAnimated;
            CallsCount = 0;
            CallsCountMonth = 0;
            CreationDateTime = DateTime.Now;
            NotificationEmail = widgetDTO.NotificationEmail;
            WeekDays = WeekDay.ConvertToString(widgetDTO.WeekDays);
            subscriptionId = widgetDTO.subscriptionId;
        }

        public void updateFromWidgetDTO(WidgetDTO widget)
        {
            WidgetName = widget.WidgetName;
            Status = widget.Status;
            TalkToUsText = widget.TalkToUsText;
            ColorWidget = widget.ColorWidget;
            ColorText = widget.ColorText;
            IsAnimated = widget.IsAnimated;
            DomainURL = widget.DomainUrl;

            AuthKey = widget.AuthKey;
            Extension = widget.Extension;

            NotificationEmail = widget.NotificationEmail;

            WeekDays = WeekDay.ConvertToString(widget.WeekDays);

            subscriptionId = widget.subscriptionId;
        }
    }
}
