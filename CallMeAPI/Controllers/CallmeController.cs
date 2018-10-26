using System;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallMeAPI.Controllers
{
    [Route("api/callme")]
    public class CallmeController : Controller
    {

        private MyDBContext context;

        public CallmeController(MyDBContext _context)
        {
            context = _context;
        }

        // POST: /api/callme/validate
        [HttpPost("validate")]
        public string Validate([FromBody]TokenDTO token)
        {

            if (token == null)
                return  "NotFound";

            Widget wgt = context.Widgets.Find(Guid.Parse(token.token));

            if (wgt == null)
                return "NotFound";

            if (wgt.Status != "Active")
                return "NotActive";

            return "OK";
        }


        // POST: /api/callme/getschedule
        [HttpPost("getschedule")]
        public ValidateResponse GetSchedule([FromBody]TokenDTO token)
        {

            if (token == null)
                return new ValidateResponse { result = "NotFound" };

            Widget wgt = context.Widgets.Find(Guid.Parse(token.token));

            if (wgt == null)
                return new ValidateResponse { result = "NotFound" };

            if (wgt.Status != "Active")
                return new ValidateResponse { result = "NotActive" };

            ValidateResponse response = new ValidateResponse();
            response.result = "OK";

            List<CustomDateTime> datetimes = new List<CustomDateTime>();

            datetimes.Add(new CustomDateTime
            {
                date = "Oct 20",
                fulldate = "October 20, 2018",
                today = "y",
                day = "Thu",
                fullday = "Thursday",
                times = new string[] { "12:00", "12:30", "13:00", "13:30" }
            });

            datetimes.Add(new CustomDateTime
            {
                date = "Sep 10",
                fulldate = "September 10, 2018",
                today = "n",
                day = "Fri",
                fullday = "Friday",
                times = new string[] { "09:00", "09:30", "10:00", "10:30" }
            });

            datetimes.Add(new CustomDateTime
            {
                date = "Sep 14",
                fulldate = "September 14, 2018",
                today = "n",
                day = "Wed",
                fullday = "Wednesday",
                times = new string[] { "09:30", "10:30", "11:00", "11:30" }
            });

            response.datetimes = datetimes;

            return response;
        }



        // POST: /api/callme
        [HttpPost]
        public string Callme([FromBody]CallmeDTO callInfo)
        {

            return "OK: Call request sent";
        }


        // POST: /api/callme
        [HttpGet("snippetcode/{token}")]
        public IEnumerable<string> GetSnippetCode(string token)
        {
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

            content = content.Replace("$server$", "http://" + host);
            content = content.Replace("$token$", token);

            content = Regex.Replace(content, @"\n|\r", "");

            List<string> result = new List<string>();
            result.Add(content);

            return result;
        }
        


        // POST: /api/callme/test
        [HttpGet("test")]
        public  string CallmeTest()//[FromBody]TestCall callInfo)
        {
            try
            {
                TestCall callInfo = new TestCall();
               // callInfo.srcPhone = "07825199046";
                callInfo.destPhone = "07469721240";


                var request = (HttpWebRequest)WebRequest.Create("https://call-api.gradwell.com/0.9.3/call");

                // for example, assumes that postData value is "param1=value1;param2=value2"

                string sep = "&";

                var postData = "auth=3I5J6PFFB7OZ00WGSNGYU5EUU6" + sep
                                + "extension=2034389" + sep
                                + "destination=" + callInfo.destPhone + sep
                                + "cidname=alireza" + sep
                              //  + "source=" + callInfo.srcPhone + sep
                                + "wait=0";

                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                //request.ContentType = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";

                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return responseString;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            //string baseUrl = "https://call-api.gradwell.com/0.9.3/call";
            ////The 'using' will help to prevent memory leaks.
            ////Create a new instance of HttpClient
            //using (HttpClient client = new HttpClient())
            //{
            //    //Setting up the response...         

            //    client.

            //    using (HttpResponseMessage res = await client.GetAsync(baseUrl))
            //    {

            //        using (HttpContent content = res.Content)
            //        {
            //            string data = await content.ReadAsStringAsync();
            //            if (data != null)
            //            {
            //                return data;
            //            }
            //        }
            //    }
            //}
            

            //return "OK: Call request sent";
            //if (token.token == "86d698c3adce4ad3df8ce8ed304b9986")
            //{
            //    return true;
            //}
            //else
            //{
            //    throw new Exception();
            //}
        }

    }




    public class TokenDTO
    {
        public string token { get; set; }
    }

    public class CallmeDTO
    {
        public string reqUrl { get; set; }
        public string token { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
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
        public string[] times { get; set; }
    }
}
