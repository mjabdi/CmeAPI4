using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallMeAPI.Controllers
{
    [Route("api/js")]
    public class JSController : Controller
    {

        // GET api/js/filename
        [HttpGet("{filename}")]
        public ContentResult Get(string filename)
        {
            string host = Request.Host.Host;
            host = host + ":" + Request.Host.Port;

            if (Program.onAzure)
            {
                host = Program.Host;
            }

            string _rootPath = "js/";

            StreamReader reader = new StreamReader(_rootPath + filename);
            string content = reader.ReadToEnd();
            reader.Close();

            content = content.Replace("$server$", "http://" + host);

            //content = Regex.Replace(content, @"\t|\n|\r| ", "");

            return Content(content, "text/javascript",System.Text.Encoding.UTF8);
        }
    }
}
