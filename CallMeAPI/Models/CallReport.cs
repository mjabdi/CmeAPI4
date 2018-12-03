using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CallMeAPI.Models
{
    public class CallReport
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public string CallType { get; set; }
        public DateTime Time { get; set; }
        public string Extension { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Duration { get; set; }
        public int Seconds { get; set; }
        public decimal Cost { get; set; }

    }
}
