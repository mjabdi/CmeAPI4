using System;

namespace CallMeAPI.DTO
{
    public class SubscriptionDTO
    {
        public SubscriptionDTO()
        {
        }

        public SubscriptionDTO(CallMeAPI.Models.Subscription subscription)
        {
            subscriptionID = subscription.ID;
            createdDateTime = subscription.Created;
            currentPeriodStartDate = subscription.CurrentPeriodStart;
            currentPeriodEndDate = subscription.CurrentPeriodEnd;
            trialingUntilDate = subscription.TrialingUntil;
            customerEmail = subscription.CustomerEmail;
            plan = CallMeAPI.Models.Subscription.GetPlanName(subscription.PlanID);
            status = subscription.Status;
        }

        public string subscriptionID { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime? currentPeriodStartDate { get; set; }
        public DateTime? currentPeriodEndDate { get; set; }
        public DateTime? trialingUntilDate { get; set; }
        public string customerEmail { get; set; }
        public string plan { get; set; } // Sole Trader, Small Business , Large Business
        public string status { get; set; } // Active, Out of Call , Canceled, Expired , ...

    }
}
