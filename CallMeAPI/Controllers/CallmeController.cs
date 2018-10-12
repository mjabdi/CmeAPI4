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
        public bool Validate([FromBody]TokenDTO token)
        {

            if (token == null)
                throw new Exception("Not Found");

            Widget wgt = context.Widgets.Find(Guid.Parse(token.token));

            if (wgt == null)
                throw new Exception("Not Found");

            if (wgt.Status != "Active")
                throw new Exception("Not Active");

            return true;
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
        [HttpPost("test")]
        public  string CallmeTest([FromBody]TestCall callInfo)
        {

            var request = (HttpWebRequest)WebRequest.Create("http://call-api.gradwell.com/0.9.3/call");

            // for example, assumes that postData value is "param1=value1;param2=value2"

            string sep = "&";

            var postData = "auth=1I8P8WB2CZU40L8TVSVOEHVJNX" + sep
                            + "extension=2034380" + sep
                            + "destination=" + callInfo.destPhone + sep
                            + "cidname=ashkan" + sep
                            + "source=" + callInfo.srcPhone + sep
                            + "wait=1";
                                                                       
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
}
