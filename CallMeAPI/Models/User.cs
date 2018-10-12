using System;
using System.ComponentModel.DataAnnotations;

namespace CallMeAPI.Models
{
    public class User
    {

        [Key]
        public string UserID { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }

        public bool IsFirstLogon { get; set; }

        public DateTime? LastLogon { get; set; }

        public DateTime CreationDateTime { get; set; }

        public bool IsActive { get; set; }

        public string Role { get; set; }

    }
}
