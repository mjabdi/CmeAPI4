using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallMeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallMeAPI.Controllers
{
    [Route("api/callreports")]
    public class CallReportsController : Controller
    {

        private MyDBContext context;

        public CallReportsController(MyDBContext _context)
        {
            context = _context;
        }


        // GET: api/callreports/
        [HttpGet]
        public async Task<IEnumerable<CallReport>> GetAllCallReports()
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);


            var callreports = await context.CallReports.ToListAsync();

            return callreports;
        }


        // GET: api/callreports/me
        [HttpGet("me")]
        public async Task<IEnumerable<CallReport>> GetMyCallReports()
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            var email = this.HttpContext.Request.Headers["From"];

            var widgets = await context.Widgets.Where(wgt => wgt.UserID == email).ToListAsync();

            List<CallReport> reportList = new List<CallReport>();

            foreach(var widget in  widgets)
            {
                var callreports = await context.CallReports.Where(cr => cr.Extension == widget.Extension).ToListAsync();
                reportList.AddRange(callreports);
            }

            return reportList;
        }



    }
}
