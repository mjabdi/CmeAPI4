using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallMeAPI.Models
{
    public class CallbackSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public Guid widgetID { get; set; }

        public virtual Widget widget { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public string LeadName { get; set; }
        public string LeadEmail { get; set; }
        public string LeadPhoneNumber { get; set; }
        public string LeadMessage { get; set; }
        public bool CallDone { get; set; }
        public bool EmailNotificationDone { get; set; }
        public string Comment { get; set; }
    }
}
