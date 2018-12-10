﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CallMeAPI.Models;
using System.Text.RegularExpressions;
using CallMeAPI.DTO;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallMeAPI.Controllers
{
    [Route("api/callme")]
    public class CallmeController : Controller
    {

        public const string DAYOPEN = "Open";
        public const string DAYCLOSED = "Close";


        private MyDBContext context;

        public CallmeController(MyDBContext _context)
        {
            context = _context;
        }



        [HttpGet("checkscheduledcallback")]
        public async Task<string> CheckScheduledCallback()
        {
            try
            {
                DateTime now = DateTime.Parse(GetUKDateTime());
                List<CallbackSchedule> email_schedules = await context.CallbackSchedules.Include(c => c.widget)
                                                                      .Include(c => c.widget.User)
                                             .Where(c => c.EmailNotificationDone == false).ToListAsync();

                foreach(CallbackSchedule cs in email_schedules)
                {
                    if (now.AddMinutes(15) >= cs.ScheduledDateTime)
                    {
                        
                        try
                        {
                            //send email notification

                            EmailManager.SendCallNotificationEmail(cs.widget.DomainURL , cs.widget.NotificationEmail, cs.widget.User.Name, cs.LeadName, cs.LeadEmail, cs.LeadPhoneNumber, cs.ScheduledDateTime.ToString(), cs.LeadMessage);

                            cs.EmailNotificationDone = true;
                            await context.SaveChangesAsync();
                        }catch(Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }


                List<CallbackSchedule> call_schedules = await context.CallbackSchedules.Include(c => c.widget)
                                                                     .Where(c => c.CallDone == false).ToListAsync();

                foreach (CallbackSchedule cs in call_schedules)
                {
                    if (now >= cs.ScheduledDateTime)
                    {
                        try
                        {
                            //do callback 

                            string response = await Call(cs.widget.ID, cs.LeadPhoneNumber, cs.widget.AuthKey, cs.widget.Extension, cs.LeadName , 1);

                            cs.CallDone = true;
                            await context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }

                return "OK";
            }catch (Exception ex)
            {
                return (DateTime.Now.ToString() + ": " + ex.Message);
            }
        }


        // POST: /api/callme/validate
        [HttpPost("validate")]
        public async Task<string> ValidateAsync([FromBody]TokenDTO token)
        {

            if (token == null)
                return  "NotFound";

            Widget wgt = context.Widgets.Find(Guid.Parse(token.token));

            if (wgt == null)
                return "NotFound";

            if (wgt.Status != "Active")
                return "NotActive";

            if (string.IsNullOrWhiteSpace(wgt.AuthKey) || string.IsNullOrWhiteSpace(wgt.Extension))
                return "NotActive";

            if (String.IsNullOrEmpty(wgt.subscriptionId))
                return "NoSubscription";
            
            if (await IsWidgetOutofCallAsync(wgt,context))
                return "OutofCall";

            if (IsWidgetOutofSubscription(wgt,context))
                return "SubscriptionExpired";
            
            return "OK";
        }


        public static async Task<bool> IsWidgetOutofCallAsync(Widget wgt,MyDBContext context)
        {
            int calls = 0;

            CallMeAPI.Models.Subscription subscription = context.Subscriptions.Find(wgt.subscriptionId);

            int maxCalls = Subscription.GetPlanMaxCalls(subscription.PlanID);

            List<Widget> widgets = await context.Widgets.Where(widget => widget.subscriptionId == wgt.subscriptionId).ToListAsync();

            foreach(Widget widget in widgets)
            {
                calls += widget.CallsCountMonth;
            }

            return calls >= maxCalls;
        }

        public static bool IsWidgetOutofSubscription(Widget wgt,MyDBContext context)
        {
            CallMeAPI.Models.Subscription subscription = context.Subscriptions.Find(wgt.subscriptionId);

            return (subscription.Status.ToLower() == "canceled");
        } 




        //private string tryGetUKDateTime()
        //{
        //    try
        //    {

        //        string url = "http://api.geonames.org/timezoneJSON?lat=51.5074&lng=0.1278&username=demo";
        //        var request = (HttpWebRequest)WebRequest.Create(url);

        //        request.Method = "GET";
        //        request.ContentType = "application/json";

        //        var response = (HttpWebResponse)request.GetResponse();
        //        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        //        var resource = JObject.Parse(responseString);

        //        return resource["time"].ToString();
        //    }
        //    catch(Exception)
        //    {
        //        return "Error";
        //    }
        //}


        //[HttpGet("getdatetime")]
        //public string GetUKDateTime()
        //{
        //    var maxTry = 3;

        //    var result = "Error";

        //    var i = 0;
        //    while (result == "Error" && i < maxTry)
        //    {
        //        result = tryGetUKDateTime();
        //        i++;
        //    }

        //    return result;
        //}



        [HttpGet("getdatetime")]
        public string GetUKDateTime()
        {
            try{

                DateTime now = DateTime.Now;

                if (!Program.onAzure)
                    return now.ToString();

                var britishZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                var newDate = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.Local, britishZone);

                return newDate.ToString();


            }catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet("getdayofweek")]
        public string GetDayofWeek()
        {
            try
            {

                DateTime now = DateTime.Now;

                if (!Program.onAzure)
                    return now.DayOfWeek.ToString();

                var britishZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                var newDate = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.Local, britishZone);

                return newDate.DayOfWeek.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }




        // POST: /api/callme/getschedule
        [HttpPost("getschedule")]
        public async Task<ValidateResponse> GetScheduleAsync([FromBody]TokenDTO token)
        {
            try
            {

                DateTime now = DateTime.Parse(GetUKDateTime());
                string dayOfWeek = GetDayofWeek();

                if (token == null)
                    return new ValidateResponse { result = "NotFound" };

                Widget wgt = context.Widgets.Include(widget => widget.User)
                                    .Where(widget => widget.ID == Guid.Parse(token.token))
                                    .FirstOrDefault();

                if (wgt == null)
                    return new ValidateResponse { result = "NotFound" };

                if (wgt.Status != "Active")
                    return new ValidateResponse { result = "NotActive" };

                WidgetDTO wgtDTO = new WidgetDTO(wgt);

                bool isOpen = false;
                string startTime = "";
                string endTime = "";
                foreach (WeekDay day in wgtDTO.WeekDays)
                {
                    if (day.name == dayOfWeek)
                    {
                        isOpen = day.isOpen;
                        startTime = day.startTime;
                        endTime = day.endTime;
                    }
                }

                ValidateResponse response = new ValidateResponse();

                if (!isOpen)
                {
                    response.result = DAYCLOSED;
                    response.datetimes = BuildSchedule(now, false ,wgtDTO.WeekDays);
                    return response;
                }

                string[] startTimeSplit = startTime.Split(":. ".ToCharArray());
                int startTimeHour = int.Parse(startTimeSplit[0],System.Globalization.NumberStyles.Any);
                int startTimeMin = int.Parse(startTimeSplit[1],System.Globalization.NumberStyles.Any);

                string[] endTimeSplit = endTime.Split(":.".ToCharArray());
                int endTimeHour = int.Parse(endTimeSplit[0],System.Globalization.NumberStyles.Any);
                int endTimeMin = int.Parse(endTimeSplit[1],System.Globalization.NumberStyles.Any);

                int nowHour = now.TimeOfDay.Hours;
                int nowMin = now.TimeOfDay.Minutes;

                if (nowHour < startTimeHour)
                {
                    response.result = DAYCLOSED;
                    response.datetimes = BuildSchedule(now, true, wgtDTO.WeekDays);
                    return response;
                }

                if (nowHour > endTimeHour)
                {
                    response.result = DAYCLOSED;
                    response.datetimes = BuildSchedule(now, false, wgtDTO.WeekDays);
                    return response;
                }


                if (nowHour == startTimeHour && nowMin < startTimeMin)
                {
                    response.result = DAYCLOSED;
                    response.datetimes = BuildSchedule(now, true, wgtDTO.WeekDays);
                    return response;
                }

                if (nowHour == endTimeHour && nowMin > endTimeMin)
                {
                    response.result = DAYCLOSED;
                    response.datetimes = BuildSchedule(now, false, wgtDTO.WeekDays);
                    return response;
                }

                response.result = DAYOPEN;

                return response;
            }
            catch(Exception ex)
            {
                AppException exception = new AppException();
                exception.ErrorDateTime = DateTime.Now;
                exception.Message = ex.Message;
                exception.FullString = ex.ToString();
                context.AppExceptions.Add(exception);
                await context.SaveChangesAsync();
               
                throw ex;
            }

            //List<CustomDateTime> datetimes = new List<CustomDateTime>();

            //datetimes.Add(new CustomDateTime
            //{
            //    date = "Oct 20",
            //    fulldate = "October 20, 2018",
            //    today = "y",
            //    day = "Thu",
            //    fullday = "Thursday",
            //    times = new string[] { "12:00", "12:30", "13:00", "13:30" }
            //});

            //datetimes.Add(new CustomDateTime
            //{
            //    date = "Sep 10",
            //    fulldate = "September 10, 2018",
            //    today = "n",
            //    day = "Fri",
            //    fullday = "Friday",
            //    times = new string[] { "09:00", "09:30", "10:00", "10:30" }
            //});

            //datetimes.Add(new CustomDateTime
            //{
            //    date = "Sep 14",
            //    fulldate = "September 14, 2018",
            //    today = "n",
            //    day = "Wed",
            //    fullday = "Wednesday",
            //    times = new string[] { "09:30", "10:30", "11:00", "11:30" }
            //});

            //response.datetimes = datetimes;

            //return response;
        }

        private IEnumerable<CustomDateTime> BuildSchedule(DateTime now, bool includeToday, WeekDay[] weekDays)
        {
            List<DateTime> selectedDays = new List<DateTime>();

            if (includeToday)
                selectedDays.Add(now);


            DateTime date = now.AddDays(1);

            while (selectedDays.Count < 3)
            {
                foreach (WeekDay day in weekDays)
                {
                    if (date.DayOfWeek.ToString() == day.name && day.isOpen)
                    {
                        selectedDays.Add(date);
                    }
                }

                date = date.AddDays(1);
            }

            List<CustomDateTime> customDates = new List<CustomDateTime>();

            foreach(DateTime day in selectedDays)
            {
                customDates.Add(buildCustomDateTime(now,day,weekDays));
            }

            return customDates;
        }

        private CustomDateTime buildCustomDateTime(DateTime now, DateTime date, WeekDay[] weekDays)
        {
            CustomDateTime customDate = new CustomDateTime();

            customDate.date = date.ToString("MMM") + " " + date.Day;
            customDate.fulldate = date.ToString("MMMM") + " " + date.Day + ", " + date.Year;
            customDate.today = (date.Date == now.Date) ? "<div class='today'>-today-</div>" : "";
            customDate.todaym = (date.Date == now.Date) ? "<strong style='color:gray' class='today-mobilewg'>&nbsp;&nbsp;-today-</strong>" : "";
            customDate.day = date.ToString("ddd");
            customDate.fullday = date.DayOfWeek.ToString();

            WeekDay day = null;
            foreach (WeekDay dd in weekDays)
            {
                if (dd.name == date.DayOfWeek.ToString())
                {
                    day = dd;
                }
            }

            var times = new List<string>();

            string[] startTimeSplit = day.startTime.Split(":.".ToCharArray());
            int startTimeHour = int.Parse(startTimeSplit[0],System.Globalization.NumberStyles.Any);
            int startTimeMin = int.Parse(startTimeSplit[1],System.Globalization.NumberStyles.Any);

            string[] endTimeSplit = day.endTime.Split(":.".ToCharArray());
            int endTimeHour = int.Parse(endTimeSplit[0],System.Globalization.NumberStyles.Any);
            int endTimeMin = int.Parse(endTimeSplit[1],System.Globalization.NumberStyles.Any);

            for (int hour = startTimeHour; hour <= endTimeHour; hour++)
            {
                for (int min = 0; min < 60; min += 15)
                {
                    if (hour == startTimeHour)
                    {
                        if (min >= startTimeMin)
                        {
                            times.Add(TwoDigits(hour) + ":" + TwoDigits(min));
                        }
                    }
                    else if (hour == endTimeHour)
                    {
                        if (min <= endTimeMin)
                        {
                            times.Add(TwoDigits(hour) + ":" + TwoDigits(min));
                        }
                    }
                    else
                    {
                        times.Add(TwoDigits(hour) + ":" + TwoDigits(min));
                    }
                }
            }


            //string timesHtml = "<label class='dropdown-times' for='styledSelect12567'> <select id='styledSelect12567' name='options' >";

            string timesHtml = "<select id='schedule_calltime'>";

            for (int i = 0; i < times.Count; i++)
            {
                timesHtml += "<option value='" + times[i] + "'>" + times[i] + "</option>";
            }

            timesHtml += "</select>";

            //StringBuilder htmlStr = new StringBuilder("");




            //htmlStr.Append("<div id='dd_891' class='wrapper-dropdown-5' tabindex='1'> <span id='time_dd_891'>--:--</span> <ul class='dropdown'>");

            //for (int i = 0; i < times.Count; i++)
            //{
            //    htmlStr.Append($"<li><a href='#' onclick=&quot; alert('hi'); document.getElementById('time_dd_891').innerHtml='{times[i]}'&quot;> {times[i]} </a></li>");
            //}

            //htmlStr.Append("</ul></div>");
            //htmlStr.Append("<script>$('#dd_891').on('click', function(event){$(this).toggleClass('active');return false;});$(function(){$(document).click(function() {$('.wrapper-dropdown-1').removeClass('active');});});</script>");

            //timesHtml = timesHtml.Replace("$$", "'");

            customDate.times = timesHtml;

            return customDate;
        }


        private string TwoDigits(int value)
        {
            var str = value.ToString();

            if (str.Length == 1)
                return "0" + str;
            else
                return str;
        }

        // POST: /api/callme
        [HttpPost]
        public async Task<string> CallmeAsync([FromBody]CallmeDTO callInfo)
        {
            try
            {
                if (callInfo == null)
                    return "NotFound";

                Widget wgt = await context.Widgets.Include(widget => widget.User)
                    .Where(widget => widget.ID == Guid.Parse(callInfo.token))
                    .FirstOrDefaultAsync();

                if (wgt == null)
                    return "NotFound";

                await Call(wgt.ID, callInfo.phone, wgt.AuthKey, wgt.Extension, callInfo.name,0);

                try
                {
                    EmailManager.SendOnlineCallNotificationEmail(wgt.DomainURL, wgt.NotificationEmail, wgt.User.Name, callInfo.name, callInfo.email, callInfo.phone, GetUKDateTime());
                }
                catch (Exception) { }

                return "OK: Call request sent";
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        // POST: /api/schedulecall
        [HttpPost("schedulecall")]
        public async Task<string> ScheduleCallAsync([FromBody]ScheduleCallDTO callInfo)
        {
            try
            {
                if (callInfo == null)
                    return "NotFound";

                //Widget wgt = context.Widgets.Find(Guid.Parse(callInfo.token));
                //wgt.User = context.Users.Find(wgt.UserID);

                Widget wgt = await context.Widgets.Include(widget => widget.User)
                    .Where(widget => widget.ID == Guid.Parse(callInfo.token))
                    .FirstOrDefaultAsync();

                if (wgt == null)
                    return "NotFound";

                CallbackSchedule schedule = new CallbackSchedule();
                schedule.widget = wgt;
                schedule.LeadName = callInfo.name;
                schedule.LeadEmail = callInfo.email;
                schedule.LeadMessage = callInfo.message ?? "";
                schedule.LeadPhoneNumber = callInfo.phone;

                schedule.CallDone = false;
                schedule.EmailNotificationDone = false;

                schedule.ScheduledDateTime = ParseDateTime(callInfo.date, callInfo.time);
                context.CallbackSchedules.Add(schedule);
                await context.SaveChangesAsync();

                try
                {
                    EmailManager.SendCallNotificationEmail(wgt.DomainURL, wgt.NotificationEmail, wgt.User.Name, schedule.LeadName, schedule.LeadEmail, schedule.LeadPhoneNumber, schedule.ScheduledDateTime.ToString(), schedule.LeadMessage);
                }catch(Exception) { }

                return "OK: Call request sent";
            }catch(Exception ex)
            {
                AppException exception = new AppException();
                exception.ErrorDateTime = DateTime.Now;
                exception.Message = ex.Message;
                exception.FullString = ex.ToString();
                context.AppExceptions.Add(exception);
                await context.SaveChangesAsync();
                return "Error";
            }
        }

        private DateTime ParseDateTime(string date, string time)
        {
            return DateTime.Parse(date + " " + time);
        }


        // POST: /api/callme
        [HttpGet("snippetcode/{token}")]
        public IEnumerable<string> GetSnippetCode(string token)
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            string host = Request.Host.Host;
            host = host + ":" + Request.Host.Port;

            if (Program.onAzure)
            {
                host = Program.Host;
            }

            string _rootPath = "js/";

            StreamReader reader = new StreamReader(_rootPath + "snippet-template.js");
            string content = reader.ReadToEnd();
            reader.Close();

            content = content.Replace("$server$", Program.HTTP_PREFIX + host);
            content = content.Replace("$token$", token);

            content = Regex.Replace(content, @"\n|\r", "");

            List<string> result = new List<string>();
            result.Add(content);

            return result;
        }


        public static string GetSnippetCodeStatic(string token)
        {
            string  host = Program.Host;

            string _rootPath = "js/";

            StreamReader reader = new StreamReader(_rootPath + "snippet-template.js");
            string content = reader.ReadToEnd();
            reader.Close();

            content = content.Replace("$server$", Program.HTTP_PREFIX + host);
            content = content.Replace("$token$", token);

            content = Regex.Replace(content, @"\n|\r", "");

            return content;
        }


        public async Task<string> Call(Guid widgetID, string phoneNumber, string apiKey, string extension, string customerName,int wait = 0)//[FromBody]TestCall callInfo)
        {
            string requestStr = "";
            string responseStr = "";
            DateTime now = DateTime.Parse(GetUKDateTime());

            try
            {
                var request = WebRequest.Create("https://call-api.gradwell.com/0.9.3/call");


                if (!phoneNumber.Trim().StartsWith("0"))
                {
                    phoneNumber = "0" + phoneNumber;
                }

                string sep = "&";

                var postData = "auth=" + apiKey + sep
                    + "extension=" + extension + sep
                    + "destination=" + phoneNumber + sep
                    + "cidname=" + customerName + sep
                    + "wait=" + wait.ToString();

                requestStr = postData;


                if (phoneNumber.Trim().Length < 5)
                {
                    throw new Exception("Error : Invalid Phone Number!");
                }

                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                request.ContentLength = data.Length;

                using (var stream = await request.GetRequestStreamAsync())
                {
                    stream.Write(data, 0, data.Length);

                    Widget widget = context.Widgets.Find(widgetID);

                    if (IsWidgetOutofSubscription(widget,context))
                        throw new Exception("Subscription Expired!");

                    if (await IsWidgetOutofCallAsync(widget,context))
                        throw new Exception("Widget Out of Call!");

                    var response = await request.GetResponseAsync();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                   
                    widget.CallsCount = widget.CallsCount + 1;
                    widget.CallsCountMonth = widget.CallsCountMonth + 1;

                    context.SaveChanges();

                    responseStr = responseString;

                    return responseString;
                }
            }
            catch (Exception ex)
            {
                responseStr = ex.Message;
                throw ex;
            }
            finally
            {
                CallLog log = new CallLog { Request = requestStr, Response = responseStr, ReqDateTime = now };
                context.CallLogs.Add(log);
                await context.SaveChangesAsync();
            }
        }
    





        // POST: /api/callme/test
        [HttpGet("test")]
        public async Task<string> CallmeTestAsync()//[FromBody]TestCall callInfo)
        {
            try
            {

                TestCall callInfo = new TestCall();
               // callInfo.srcPhone = "07825199046";
                //callInfo.destPhone = "07469721240";
                callInfo.destPhone = "07469721241";


                var request = WebRequest.Create("https://call-api.gradwell.com/0.9.3/call");

                // for example, assumes that postData value is "param1=value1;param2=value2"

                string sep = "&";

                //var postData = "auth=3I5J6PFFB7OZ00WGSNGYU5EUU6" + sep
                //  + "extension=2034389" + sep
                //  + "destination=" + callInfo.destPhone + sep
                //  + "cidname=alireza" + sep
                ////  + "source=" + callInfo.srcPhone + sep
                //+ "wait=0";


                var postData = "auth=3I5J6PFFB7OZ00WGSNGYU5EUU89" + sep
                + "extension=2034355" + sep
                    + "destination=" + callInfo.destPhone + sep
                 + "cidname=alireza" + sep
                //  + "source=" + callInfo.srcPhone + sep
                + "wait=0";

                UTF8Encoding enc = new UTF8Encoding();

                request.Method = "POST";
                //request.ContentType = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";

                using (var stream = await request.GetRequestStreamAsync())
                {
                    stream.Write(enc.GetBytes(postData), 0, postData.Length);


                    var response = await request.GetResponseAsync();

                    Stream receiveStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                    string responseString = reader.ReadToEnd();

                    return responseString;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }




    public class TokenDTO
    {
        public string token { get; set; }
    }

    public class CallmeDTO
    {
        public string site { get; set; }
        public string token { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
    }


    public class ScheduleCallDTO
    {
        public string site { get; set; }
        public string token { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string message { get; set; }
        public string phone { get; set; }
        public string date { get; set; }
        public string time { get; set; }
    }





    public class TestCall
    {
        public string srcPhone { get; set; }
        public string destPhone { get; set; }
    }

    public class ValidateResponse{
        public string result { get; set; }
        public  IEnumerable<CustomDateTime> datetimes { get; set; }
    }

    public class CustomDateTime
    {
        public string date { get; set; }
        public string fulldate { get; set; }
        public string day { get; set; }
        public string fullday { get; set; }
        public string today { get; set; }
        public string todaym { get; set; }
        public string times { get; set; }
    }
}
