using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallMeAPI.Controllers
{
    [Route("api/stripepayment")]
    public class StripeController : Controller
    {
        [HttpPost("charge")]
        public IActionResult Charge([FromBody] ChargeRequest req)
        {
            try
            {
                var customers = new CustomerService();
                var charges = new ChargeService();
                var subscriptions = new SubscriptionService();

                var customer = customers.Create(new CustomerCreateOptions
                {
                    Email = "m.jafarabdi@gmail.com",
                    SourceToken = req.s_token
                });

                SubscriptionCreateOptions mysub = new SubscriptionCreateOptions();
                mysub.CustomerId = customer.Id;
                mysub.Items = new List<SubscriptionItemOption>();
                mysub.TrialFromPlan = true;
       
                mysub.Items.Add(new SubscriptionItemOption { PlanId = Models.Subscription.Plan_LargeBusiness,Quantity = 1 }); 
                var subscription = subscriptions.Create(mysub);
                                                     


                //var charge = charges.Create(new ChargeCreateOptions
                //{
                //    Amount = 500,
                //    Description = "Sample Charge",
                //    Currency = "gbp",
                //    CustomerId = customer.Id
                //});



                return Ok();
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }

    public class ChargeRequest
    {
        public string s_token { get; set; }        
    }
}
