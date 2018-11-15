using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallMeAPI.Models
{
    public class CallLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime ReqDateTime { get; set; }

    }
}
