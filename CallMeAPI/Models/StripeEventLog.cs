using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallMeAPI.Models
{
    public class StripeEventLog
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string Event { get; set; }
        public DateTime EventDateTime { get; set; }
    }
}
