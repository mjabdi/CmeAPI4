using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CallMeAPI.Models
{
    public class AppException
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public DateTime ErrorDateTime { get; set; }

        public string Message { get; set; }

        public string FullString { get; set; }

    }
}
