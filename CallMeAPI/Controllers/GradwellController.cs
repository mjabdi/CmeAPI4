using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CallMeAPI.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallMeAPI.Controllers
{
    [Route("api/gradwell")]
    public class GradwellController : Controller
    {

        private MyDBContext context;

        public GradwellController(MyDBContext _context)
        {
            context = _context;
        }

        [HttpPost("uploadreport")]
        public async Task<int> UploadReport([FromBody]object[][] data)
        {

            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            int newRecords = 0;

            for (int i = 1; i < data.Length; i++)
            {
                CallReport callReport = new CallReport();

                callReport.CallType = data[i][0].ToString();
                callReport.Time = DateTime.Parse(data[i][1].ToString());
                callReport.Extension = data[i][2].ToString();
                callReport.Source = data[i][3].ToString();
                callReport.Destination = data[i][4].ToString();
                callReport.Duration = data[i][5].ToString();
                callReport.Seconds = int.Parse(data[i][6].ToString());
                callReport.Cost = decimal.Parse(data[i][7].ToString());

                var tmpCall = await context.CallReports.FirstOrDefaultAsync(call => call.Time == callReport.Time && call.Extension == callReport.Extension);

                if (tmpCall == null)
                {
                    context.CallReports.Add(callReport);
                    await context.SaveChangesAsync();
                    newRecords++;
                }
            }

            return newRecords;
        }







        //[HttpGet("getreport/{vcp}/{date}")]
        //public string getReport(string vcp,string date)
        //{
        //    try
        //    {
        //        CookieContainer cookiesContainer = new CookieContainer();

        //        var request = (HttpWebRequest)WebRequest.Create("https://voip.gradwell.com/login/calls_out/unbilled." + date + ".csv?version=2");
        //        cookiesContainer = new CookieContainer();
        //        var mycookie = new Cookie("vcp", vcp, "/", ".gradwell.com");
        //        cookiesContainer.Add(mycookie);
        //        request.CookieContainer = cookiesContainer;

        //        request.Method = "GET";

        //        var response = (HttpWebResponse)request.GetResponse();
        //        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //        return responseString;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Not Found";
        //    }
        //}


           
      



        //[HttpGet("getreportauto")]
        //public string getAutoReport()
        //{
        //    try
        //    {
        //        CookieContainer cookiesContainer = new CookieContainer();

        //        var request = (HttpWebRequest)WebRequest.Create("https://login.gradwell.com");
        //        request.CookieContainer = cookiesContainer;

        //        // for example, assumes that postData value is "param1=value1;param2=value2"

        //        string sep = "&";

        //        var postData = "u=matt@dubseo.co.uk" + sep
        //            + "p=Sade1212" + sep
        //            + "s=Login";

        //        var data = Encoding.ASCII.GetBytes(postData);

        //        request.Method = "POST";
        //        request.ContentType = "application/x-www-form-urlencoded";

        //        request.ContentLength = data.Length;

        //        using (var stream = request.GetRequestStream())
        //        {
        //            stream.Write(data, 0, data.Length);
        //        }




        //        var response = (HttpWebResponse)request.GetResponse();
        //        cookiesContainer.Add(response.Cookies);
        //        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        //        if (responseString.Contains("pinsubmit"))
        //        {
        //            request = (HttpWebRequest)WebRequest.Create("https://login.gradwell.com/");
        //            Cookie mycookie = new Cookie("vcp", "qno10elr76vjco0c2rduklmik7", "/", ".gradwell.com");
        //            Cookie mycookie2 = new Cookie("GSSO", "rvl1dm8ard00nr560a2zzkfxrh1kgnzqzsycakny", "/", ".gradwell.com");
        //            cookiesContainer.Add(mycookie);
        //            cookiesContainer.Add(mycookie2);

        //            request.CookieContainer = cookiesContainer;

        //            // for example, assumes that postData value is "param1=value1;param2=value2"


        //            postData = "validatepin=2536" + sep
        //                + "rememberpin=on" + sep
        //                + "pinsubmit=Continue";

        //            data = Encoding.ASCII.GetBytes(postData);

        //            request.Method = "POST";
        //            request.ContentType = "application/x-www-form-urlencoded";

        //            request.ContentLength = data.Length;

        //            using (var stream = request.GetRequestStream())
        //            {
        //                stream.Write(data, 0, data.Length);
        //            }

        //            response = (HttpWebResponse)request.GetResponse();
        //            cookiesContainer.Add(response.Cookies);
        //            responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        //            //var request2 = (HttpWebRequest)WebRequest.Create("https://voip.gradwell.com/admin/validate_pin.php");
        //            //request2.Method = "POST";
        //            //request2.ContentType = "application/json";
        //            //request2.CookieContainer = cookiesContainer;
        //            //using (var streamWriter = new StreamWriter(request2.GetRequestStream()))
        //            //{
        //            //    string json = "{pin:2536,id:31277272}";

        //            //    streamWriter.Write(json);
        //            //    streamWriter.Flush();
        //            //    streamWriter.Close();
        //            //}

        //            //var httpResponse = (HttpWebResponse)request2.GetResponse();
        //            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //            //{
        //            //    var result = streamReader.ReadToEnd();
        //            //}



        //            if (responseString.Contains("Logged In"))
        //            {
        //                request = (HttpWebRequest)WebRequest.Create("https://voip.gradwell.com/login/calls_out/unbilled.2018-10-03.html?GSSO=rvl1dm8ard00nr560a2zzkfxrh1kgnzqzsycaknp");
        //                              //Cookie cookie = new Cookie("vcp", "l8nlfqt76u3he7v6qtue8qi333", "", "voip.gradwell.com");
        //                              //cookiesContainer.Add(cookie);
        //                              request.CookieContainer = cookiesContainer;

        //                              request.Method = "GET";
        //                              request.ContentType = "text/html";

        //                              response = (HttpWebResponse)request.GetResponse();
        //                              responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //                              //cookiesContainer.Add(response.Cookies); 


        //                //var cookieVCP = response.Cookies[0];

        //                request = (HttpWebRequest)WebRequest.Create("https://voip.gradwell.com/login/calls_out/unbilled.2018-10-03.csv?version=2");
        //                //cookiesContainer = new CookieContainer();
        //                //mycookie = new Cookie("vcp", "qno10elr76vjco0c2rduklmik7", "/", ".gradwell.com");
        //                //cookiesContainer.Add(mycookie);
        //                request.CookieContainer = cookiesContainer;

        //                request.Method = "GET";

        //                response = (HttpWebResponse)request.GetResponse();
        //                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //                return responseString;
        //            }

        //        }

        //        return "";
        //    }


           
        //    catch (Exception ex)
        //    {
        //        return "Not Found";
        //    }
        //}
    }
}
