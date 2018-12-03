using System;
using System.ComponentModel.DataAnnotations;

namespace CallMeAPI.Models
{
    public class Subscription
    {
        public static readonly string Plan_SoleTrader = "plan_E4lrKKOvSs1y6E";
        public static readonly string Plan_SmallBusiness = "plan_E4lsp3bZc4K4w5";
        public static  readonly string Plan_LargeBusiness = "plan_E4lsEL6EYsjTl4";


        public static readonly string Plan_VIP = "VIP_plan_RT4Y5JKR45";



        public static string GetPlanName(string planID)
        {
            if (planID == Plan_SoleTrader)
                return "Sole Trader";
            else if (planID == Plan_SmallBusiness)
                return "Small Business";
            else if (planID == Plan_LargeBusiness)
                return "Large Business";
            else if (planID == Plan_VIP)
                return "VIP Unlimited";
            else
                return "Unknown";
        }


        public static int GetPlanMaxCalls(string planID)
        {
            if (planID == Plan_SoleTrader)
                return 40;
            else if (planID == Plan_SmallBusiness)
                return 90;
            else if (planID == Plan_LargeBusiness)
                return 200;
            else if (planID == Plan_VIP)
                return int.MaxValue;
            else
                return 0;
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
