using System;
using System.ComponentModel.DataAnnotations;

namespace CallMeAPI.Models
{
    public class Subscription
    {
        public readonly static string Plan_SoleTrader = "plan_DyKREpq8pGQqPo";
        public readonly static string Plan_SmallBusiness = "plan_DyKSvf9djCEUs6";
        public readonly static string Plan_LargeBusiness = "plan_DyKTNXWj9EzUz6";
        public readonly static string Product_CallbackWidget = "prod_DyKPxXIDTQqPXW";

        [Key]
        public string ID { get; set; }

        public DateTime Created { get; set; }

        public DateTime? CurrentPeriodStart { get; set; }
        public DateTime? CurrentPeriodEnd { get; set; }

        public string CustomerEmail { get; set; }

        public DateTime? TrialingUntil { get; set; }

        public string ProductID { get; set; }
        public string PlanID { get; set; }

    }
}
