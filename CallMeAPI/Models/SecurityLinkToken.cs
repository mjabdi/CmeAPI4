using System;
using System.ComponentModel.DataAnnotations;

namespace CallMeAPI.Models
{
    public class SecurityLinkToken
    {
        [Key]
        public string Token { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
