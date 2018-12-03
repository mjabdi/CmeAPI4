using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CallMeAPI.DTO;
using CallMeAPI.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallMeAPI.Controllers
{
    [Route("api/subscription")]
    public class SubscriptionController : Controller
    {

        private readonly MyDBContext context;

        public SubscriptionController(MyDBContext _context)
        {
            this.context = _context;
        }

        [HttpGet]
        public async Task<IEnumerable<SubscriptionDTO>> GetMySubscriptions()
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);
            var email = this.HttpContext.Request.Headers["From"];

            List<CallMeAPI.Models.Subscription> subscriptions = await context.Subscriptions
                .Where(sub => sub.CustomerEmail == email)
                .OrderByDescending(sub => sub.Created)
                .ToListAsync();

            List<SubscriptionDTO> subscriptionsDTO = new List<SubscriptionDTO>();
            foreach (CallMeAPI.Models.Subscription sub in subscriptions)
            {
                subscriptionsDTO.Add(new SubscriptionDTO(sub));
            }

            return subscriptionsDTO;
        }

        [HttpGet("user/{email}")]
        public async Task<IEnumerable<SubscriptionDTO>> GetUserSubscriptions(string email)
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            List<CallMeAPI.Models.Subscription> subscriptions = await context.Subscriptions
                .Where(sub => sub.CustomerEmail == email)
                .OrderByDescending(sub => sub.Created)
                .ToListAsync();

            List<SubscriptionDTO> subscriptionsDTO = new List<SubscriptionDTO>();
            foreach (CallMeAPI.Models.Subscription sub in subscriptions)
            {
                subscriptionsDTO.Add(new SubscriptionDTO(sub));
            }

            return subscriptionsDTO;
        }




        [HttpGet("invoice/{subscriptionid}")]
        public async Task<IEnumerable<Invoice>> GetInvoices(string subscriptionid)
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            List<Invoice> invoices = await context.Invoices
                 .Where(inv => inv.SubscriptionID == subscriptionid)
                 .OrderByDescending(inv => inv.InvoiceDateTime)
                 .ToListAsync();

            return invoices;
        }

    }
}
