using System;
using System.ComponentModel.DataAnnotations;

namespace CallMeAPI.Models
{
    public class Subscription
    {
        public static readonly string Plan_SoleTrader = "plan_DyKREpq8pGQqPo";
        public static readonly string Plan_SmallBusiness = "plan_DyKSvf9djCEUs6";
        public static  readonly string Plan_LargeBusiness = "plan_DyKTNXWj9EzUz6";
        public static readonly string Product_CallbackWidget = "prod_DyKPxXIDTQqPXW";


        public static string GetPlanName(string planID)
        {
            if (planID == Plan_SoleTrader)
                return "Sole Trader";
            else if (planID == Plan_SmallBusiness)
                return "Small Business";
            else if (planID == Plan_LargeBusiness)
                return "Large Business";
            else
                return "Unknown";
        }



        [Key]
        public string ID { get; set; }

        public DateTime Created { get; set; }

        public DateTime? CurrentPeriodStart { get; set; }
        public DateTime? CurrentPeriodEnd { get; set; }

        public string CustomerEmail { get; set; }

        public DateTime? TrialingUntil { get; set; }

        public DateTime? BillingCycleAnchor { get; set; }

        public DateTime? CanceledAtDate { get; set; }

        public string PlanID { get; set; }

        public string Status { get; set; }

        public bool CancelAtPeriodEnd { get; set; }

    }
}
