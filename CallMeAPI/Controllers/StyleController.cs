using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using System.Text.RegularExpressions;
using CallMeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace CallMeAPI.Controllers
{
    [Route("api/style")]
    [ApiController]
    public class StyleController : ControllerBase
    {

        private MyDBContext context;

        public StyleController(MyDBContext _context)
        {
            context = _context;
        }

        // GET api/style/token
        [HttpGet("css/{id}")]
        public ContentResult Get(string id)
        {
            string host = Request.Host.Host;
            host = host + ":" + Request.Host.Port;

            if (Program.onAzure)
            {
                host = Program.Host;
            }

            string _rootPath = "css/";

            Widget widget = context.Widgets.Find(Guid.Parse(id));

            bool isAnimate = widget.IsAnimated;
            string cssFilename = isAnimate ?  "widget-animate.css" : "widget.css";


            StreamReader reader = new StreamReader(_rootPath + cssFilename);
            string content = reader.ReadToEnd();
            reader.Close();

            reader = new StreamReader(_rootPath + "widget-text.css");
            string content2 = reader.ReadToEnd();
            reader.Close();

            reader = new StreamReader(_rootPath + "schedulewidget.css");
            string content3 = reader.ReadToEnd();
            reader.Close();

            content = content + "\r\n" + content2;

            content = content.Replace("$server$", Program.HTTP_PREFIX + host);



            content = content.Replace("$colortext$", widget.ColorText);
            content = content.Replace("$colorwidget$", widget.ColorWidget);
            content = content.Replace("$text-talktous$", widget.TalkToUsText);


            content = content + "\r\n" + content3;



            //content = Regex.Replace(content, @"\t|\n|\r| ", "");

            return Content(content, "text/css",System.Text.Encoding.UTF8);
        }


        [HttpGet("images/{imagename}")]
        public IActionResult GetImage(string imagename)
        {
            string _rootPath = "images/";
            string imagePath = imagename;

            var serverPath = Path.Combine(_rootPath, imagePath);
            var imageFileStream = System.IO.File.OpenRead(serverPath);
            return File(imageFileStream, "image/jpeg");
        }

        //[HttpGet("images/{imagename}/{color}")]
        //public IActionResult GetColoredImage(string imagename,string color)
        //{
        //    try
        //    {
        //        string _rootPath = "images/";
        //        string imagePath = imagename;

        //        var serverPath = Path.Combine(_rootPath, imagePath);

        //        var bmp = (Bitmap)Image.FromFile(serverPath);
        //        bmp = ChangeColor(bmp, Color.Red);
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            bmp.Save(_rootPath + "red.png");
        //            memoryStream.Seek(0, SeekOrigin.Begin);
        //            return this.File(memoryStream, "image/jpeg");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return  BadRequest(ex.ToString());
        //    }
        //}


        // GET api/style/filename
        [HttpGet("{filename}")]
        public ContentResult GetCss(string filename)
        {
            string host = Request.Host.Host;
            host = host + ":" + Request.Host.Port;

            if (Program.onAzure)
            {
                host = Program.Host;
            }

            string _rootPath = "css/";

            StreamReader reader = new StreamReader(_rootPath + filename);
            string content = reader.ReadToEnd();
            reader.Close();

            content = content.Replace("$server$", Program.HTTP_PREFIX + host);

            //content = Regex.Replace(content, @"\t|\n|\r| ", "");

            return Content(content, "text/css");
        }

        public static Bitmap ChangeColor(Bitmap scrBitmap,Color newColor)
        {
            //You can change your new color here. Red,Green,LawnGreen any..
            Color actualColor;
            //make an empty bitmap the same size as scrBitmap
            Bitmap newBitmap = new Bitmap(scrBitmap.Width, scrBitmap.Height);
            for (int i = 0; i < scrBitmap.Width; i++)
            {
                for (int j = 0; j < scrBitmap.Height; j++)
                {
                    //get the pixel from the scrBitmap image
                    actualColor = scrBitmap.GetPixel(i, j);
                    // > 150 because.. Images edges can be of low pixel colr. if we set all pixel color to new then there will be no smoothness left.
                    if (actualColor.A > 150)
                        newBitmap.SetPixel(i, j, newColor);
                    else
                        newBitmap.SetPixel(i, j, actualColor);
                }
            }
            return newBitmap;
        }
    }
}
