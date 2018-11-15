
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CallMeAPI.Models;
using Microsoft.AspNet.WebHooks;
using Microsoft.AspNetCore.Mvc;
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
            }
            catch(Exception)
            {
            }
        }
    }
}