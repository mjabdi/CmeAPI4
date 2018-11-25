﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CallMeAPI.Models;
using Microsoft.AspNet.WebHooks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stripe;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallMeAPI.Controllers
{
    [Route("api/stripewebhook")]
    public class StripeEventsController : Controller
    {

        private MyDBContext context;

        public StripeEventsController(MyDBContext _context)
        {
            context = _context;
        }

        [HttpGet("test")]
        public void Test()
        {
            try
            {
                long testID = 88;
                string testEvent = context.StripeEventLogs.Find(testID).Event;

                var obj = JObject.Parse(testEvent);

                string type = (string)obj.SelectToken("type");
                bool livemode = (bool)obj.SelectToken("livemode");

                if (type == "customer.subscription.created")
                {
                    string subscriptionID = (string)obj.SelectToken("data.object.id");
                    DateTime billing_cycle_anchor = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.billing_cycle_anchor")).DateTime;
                    bool cancel_at_period_end = (bool)obj.SelectToken("data.object.cancel_at_period_end");
                    string canceled_at = (string)obj.SelectToken("data.object.canceled_at");
                    DateTime created = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.created")).DateTime;
                    DateTime current_period_end = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.current_period_end")).DateTime;
                    DateTime current_period_start = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.current_period_start")).DateTime;

                    string customer = (string)obj.SelectToken("data.object.customer");
                    string planID = (string)obj.SelectToken("data.object.items.data[0].plan.id");
                    string status = (string)obj.SelectToken("data.object.status");

                }
                else if (type == "customer.subscription.updated")
                {
                    string subscriptionID = (string)obj.SelectToken("data.object.id");
                    DateTime billing_cycle_anchor = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.billing_cycle_anchor")).DateTime;
                    bool cancel_at_period_end = (bool)obj.SelectToken("data.object.cancel_at_period_end");
                    string canceled_at = (string)obj.SelectToken("data.object.canceled_at");
                    DateTime? canceled_at_date = null;
                    if (!string.IsNullOrEmpty(canceled_at) && !canceled_at.Contains("null"))
                    {
                        canceled_at_date = DateTimeOffset.FromUnixTimeSeconds(long.Parse(canceled_at)).DateTime;
                    }
                    DateTime created = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.created")).DateTime;
                    DateTime current_period_end = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.current_period_end")).DateTime;
                    DateTime current_period_start = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.current_period_start")).DateTime;
                    string customer = (string)obj.SelectToken("data.object.customer");
                    string planID = (string)obj.SelectToken("data.object.items.data[0].plan.id");
                    string status = (string)obj.SelectToken("data.object.status");
                }
                else if (type == "customer.created")
                {
                    string customerID = (string)obj.SelectToken("data.object.id");
                    DateTime created = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.created")).DateTime;
                    string email = (string)obj.SelectToken("data.object.email");
                }

                Console.WriteLine(obj.ToString());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public void Index()
        {
            try
            {

                var json = new StreamReader(HttpContext.Request.Body).ReadToEnd();


                StripeEventLog log = new StripeEventLog();
                log.Event = json.ToString();
                log.EventDateTime = DateTime.Now;
                context.StripeEventLogs.Add(log);
                context.SaveChanges();



                var obj = JObject.Parse(json);

                string type = (string)obj.SelectToken("type");
                bool livemode = (bool)obj.SelectToken("livemode");

                if (type == "customer.subscription.created")
                {
                    string subscriptionID = (string)obj.SelectToken("data.object.id");
                    DateTime billing_cycle_anchor = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.billing_cycle_anchor")).DateTime;
                    bool cancel_at_period_end = (bool)obj.SelectToken("data.object.cancel_at_period_end");
                    string canceled_at = (string)obj.SelectToken("data.object.canceled_at");
                    DateTime created = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.created")).DateTime;
                    DateTime current_period_end = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.current_period_end")).DateTime;
                    DateTime current_period_start = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.current_period_start")).DateTime;
                    string customer = (string)obj.SelectToken("data.object.customer");
                    string planID = (string)obj.SelectToken("data.object.items.data[0].plan.id");
                    string status = (string)obj.SelectToken("data.object.status");
                    DateTime? trial_end = null;
                    try
                    {
                        trial_end = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.trial_end")).DateTime;
                    }
                    catch (Exception) { }

                    var newSubscription = new CallMeAPI.Models.Subscription();
                    newSubscription.ID = subscriptionID;
                    newSubscription.BillingCycleAnchor = billing_cycle_anchor;
                    newSubscription.Created = created;
                    newSubscription.CurrentPeriodStart = current_period_start;
                    newSubscription.CurrentPeriodEnd = current_period_end;
                    var user = context.Users.FirstOrDefault(u => u.CustomerID == customer);
                    newSubscription.CustomerEmail = user.UserID;
                    newSubscription.PlanID = planID;
                    newSubscription.Status = status;
                    newSubscription.TrialingUntil = trial_end;

                    context.Subscriptions.Add(newSubscription);
                    context.SaveChanges();
                }
                else if (type == "customer.subscription.updated")
                {
                    string subscriptionID = (string)obj.SelectToken("data.object.id");
                    DateTime billing_cycle_anchor = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.billing_cycle_anchor")).DateTime;
                    bool cancel_at_period_end = (bool)obj.SelectToken("data.object.cancel_at_period_end");
                    string canceled_at = (string)obj.SelectToken("data.object.canceled_at");
                    DateTime? canceled_at_date = null;
                    if (!string.IsNullOrEmpty(canceled_at) && !canceled_at.Contains("null"))
                    {
                        canceled_at_date = DateTimeOffset.FromUnixTimeSeconds(long.Parse(canceled_at)).DateTime;
                    }
                    DateTime created = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.created")).DateTime;
                    DateTime current_period_end = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.current_period_end")).DateTime;
                    DateTime current_period_start = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.current_period_start")).DateTime;
                    string customer = (string)obj.SelectToken("data.object.customer");
                    string planID = (string)obj.SelectToken("data.object.items.data[0].plan.id");
                    string status = (string)obj.SelectToken("data.object.status");
                    DateTime? trial_end = null;
                    try
                    {
                        trial_end = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.trial_end")).DateTime;
                    }catch(Exception){}

                    var subscription = context.Subscriptions.Find(subscriptionID);
                    subscription.CanceledAtDate = canceled_at_date;
                    subscription.Status = status;
                    subscription.PlanID = planID;
                    subscription.BillingCycleAnchor = billing_cycle_anchor;
                    subscription.CurrentPeriodEnd = current_period_end;
                    subscription.CurrentPeriodStart = current_period_start;
                    subscription.CancelAtPeriodEnd = cancel_at_period_end;
                    subscription.TrialingUntil = trial_end;

                    context.SaveChanges();
                }
                else if (type == "customer.subscription.deleted")
                {
                    string subscriptionID = (string)obj.SelectToken("data.object.id");
                    DateTime billing_cycle_anchor = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.billing_cycle_anchor")).DateTime;
                    string canceled_at = (string)obj.SelectToken("data.object.canceled_at");
                    bool cancel_at_period_end = (bool)obj.SelectToken("data.object.cancel_at_period_end");
                    DateTime? canceled_at_date = null;
                    if (!string.IsNullOrEmpty(canceled_at) && !canceled_at.Contains("null"))
                    {
                        canceled_at_date = DateTimeOffset.FromUnixTimeSeconds(long.Parse(canceled_at)).DateTime;
                    }
                    DateTime created = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.created")).DateTime;
                    DateTime current_period_end = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.current_period_end")).DateTime;
                    DateTime current_period_start = DateTimeOffset.FromUnixTimeSeconds((long)obj.SelectToken("data.object.current_period_start")).DateTime;
                    string customer = (string)obj.SelectToken("data.object.customer");
                    string planID = (string)obj.SelectToken("data.object.items.data[0].plan.id");
                    string status = (string)obj.SelectToken("data.object.status");

                    var subscription = context.Subscriptions.Find(subscriptionID);
                    subscription.CanceledAtDate = canceled_at_date;
                    subscription.Status = status;
                    subscription.PlanID = planID;
                    subscription.BillingCycleAnchor = billing_cycle_anchor;
                    subscription.CurrentPeriodEnd = current_period_end;
                    subscription.CurrentPeriodStart = current_period_start;
                    subscription.CancelAtPeriodEnd = cancel_at_period_end;
                    context.SaveChanges();
                }
            }
            catch(Exception)
            {
                
            }
        }
    }
}