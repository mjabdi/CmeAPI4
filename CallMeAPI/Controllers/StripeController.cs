using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallMeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallMeAPI.Controllers
{
    [Route("api/stripepayment")]
    public class StripeController : Controller
    {

        private MyDBContext context;

        public StripeController(MyDBContext _context)
        {
            context = _context;
        }


        [HttpPost("createcustomer")]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerRequest req)
        {
            try
              {
                AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);
                var email = this.HttpContext.Request.Headers["From"];

                var user = context.Users.Find(email);

                if (user == null)
                    return NotFound();

                var customers = new CustomerService();
                var customerID = "";

                if (string.IsNullOrEmpty(user.CustomerID))
                {
                    var customer = await customers.CreateAsync(new CustomerCreateOptions
                    {
                        Email = email,
                        SourceToken = req.s_token
                    });

                    customerID = customer.Id;
                    user.CustomerID = customerID;
                }
                else
                {
                    var customer = await customers.UpdateAsync(user.CustomerID, new CustomerUpdateOptions
                    {
                        Email = email,
                        SourceToken = req.s_token
                    });
                }

                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 

        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeAsync([FromBody] SubscribeRequest req)
        {
            try
            {
                AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);
                var email = this.HttpContext.Request.Headers["From"];

                var user = await context.Users.FindAsync(email);

                if (user == null)
                    return NotFound();

                for (int i = 0; i < 3; i++)
                {
                    if (string.IsNullOrEmpty(user.CustomerID))
                    {
                        await Task.Delay(2000);
                        user = await context.Users.FindAsync(email);
                    }
                }

                if (string.IsNullOrEmpty(user.CustomerID))
                {
                    return NotFound();
                }

                var prevsub = context.Subscriptions.FirstOrDefault(sub => sub.CustomerEmail == email);
                bool newCustomer = (prevsub == null);

                var subscriptions = new SubscriptionService();

                var customerID = user.CustomerID;

                SubscriptionCreateOptions mysub = new SubscriptionCreateOptions();
                mysub.CustomerId = customerID;
                mysub.Items = new List<SubscriptionItemOption>();
                mysub.TrialFromPlan = newCustomer;
                mysub.TaxPercent = 20m;

                mysub.Items.Add(new SubscriptionItemOption { PlanId = req.planId, Quantity = 1 });
                var subscription = await subscriptions.CreateAsync(mysub);

                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost("unsubscribe")]
        public async Task<IActionResult> UnSubscribeAsync([FromBody] UnSubscribeRequest req)
        {
            try
            {
                AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);
                var email = this.HttpContext.Request.Headers["From"];

                var user = await context.Users.FindAsync(email);

                if (user == null)
                    return NotFound();

                for (int i = 0; i < 3; i++)
                {
                    if (string.IsNullOrEmpty(user.CustomerID))
                    {
                        await Task.Delay(2000);
                        user = await context.Users.FindAsync(email);
                    }
                }

                if (string.IsNullOrEmpty(user.CustomerID))
                {
                    return NotFound();
                }

                var subscriptions = new SubscriptionService();

                await subscriptions.CancelAsync(req.subscriptionId, new SubscriptionCancelOptions());

                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }





    public class CustomerRequest
    {
        public string s_token { get; set; }  
    }

    public class SubscribeRequest
    {
        public string planId { get; set; }
    }

    public class UnSubscribeRequest
    {
        public string subscriptionId { get; set; }
    }


}
