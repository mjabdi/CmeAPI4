using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallMeAPI.Models
{
    public class EventLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string IP { get; set; }
        public string PhoneNumber { get; set; }
        public Guid WidgetID { get; set; }
        public virtual Widget Widget { get; set; }
        public string EventType { get; set; }
        public string EventDesc { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
