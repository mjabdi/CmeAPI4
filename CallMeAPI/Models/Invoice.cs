using System;
using System.ComponentModel.DataAnnotations;

namespace CallMeAPI.Models
{
    public class Invoice
    {
        [Key]
        public string InvoiceID { get; set; }
        public decimal AmountPaid { get; set; }
        public string CustomerID { get; set; }
        public DateTime InvoiceDateTime { get; set; }
        public string InvoicePdf { get; set; }
        public string Description { get; set; }
        public string PlanName { get; set; }
        public string SubscriptionID { get; set; }

    }
}
