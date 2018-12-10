using System;
using System.Net;
using System.Net.Mail;
using CallMeAPI.Models;

namespace CallMeAPI.Controllers
{


    public class EmailManager
    {

        public static string FromEmail = "admin@talktoleadsnow.com";
        public static string FromEmailTitle = "TalkToLeadsNow";
        public static string EmailPassword = "PetPet2019";

        public static string AdminEmail = "matt@dubseo.co.uk";

        public static void SendResetPasswordLinkEmail(string emailTo, string username, string token)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(emailTo));
                message.From = new MailAddress(FromEmail, FromEmailTitle);
                message.Subject = "Reset Password";
                message.Body = "Dear " + username + "!";
                message.Body += "<br/>";
                message.Body += "<p>Please click the link below to reset your password, and set a new password for your TalkToLeadsNow account!</p>";
                message.Body += "<a target='_balnk' href='" + "http://portal.talktoleadsnow.com/#/resetpassword/" + token + "'>" + "http://portal.talktoleadsnow.com/#/resetpassword/" + token + "</a>";
                message.Body += "<p>Regards,</p>";
                message.Body += "<p>TalkToLeadsNow team.</p>";
                message.Body += "<a target='_balnk' href='http://www.talktoleadsnow.com'> <img width='200px' src='http://api.talktoleadsnow.com/api/style/images/talktoleadsnow-small.png'></a>";
                message.Body += "<hr height='1' style='height: 1px; border: 0 none; color: #D7DFE3; background-color: #D7DFE3; margin-top:1.5em; margin-bottom:1.5em;'>";
                message.Body += "<p style='color:gray'>" +
                    "This email was automatically generated on your request for an account at TalkToLeadsNow. In case you didn't request an account, please ignore this e-mail. Your information will be removed automatically from our database."
                    + "</p>";

                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(FromEmail, EmailPassword);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }




        public static void SendActivationlinkEmail(string emailTo, string username, string token)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(emailTo));
                message.From = new MailAddress(FromEmail, FromEmailTitle);
                message.Subject = "Account Activation";
                message.Body = "Dear " + username + "!";
                message.Body += "<br/>";
                message.Body += "<p>Please click the link below to activate your account, and start using TalkToLeadsNow!</p>";
                message.Body += "<a target='_balnk' href='" + "http://portal.talktoleadsnow.com/#/activationlink/" + token + "'>" + "http://portal.talktoleadsnow.com/#/activationlink/" + token + "</a>";
                message.Body += "<p>Regards,</p>";
                message.Body += "<p>TalkToLeadsNow team.</p>";
                message.Body += "<a target='_balnk' href='http://talktoleadsnow.com'> <img width='200px' src='http://api.talktoleadsnow.com/api/style/images/talktoleadsnow-small.png'></a>";
                message.Body += "<hr height='1' style='height: 1px; border: 0 none; color: #D7DFE3; background-color: #D7DFE3; margin-top:1.5em; margin-bottom:1.5em;'>";
                message.Body += "<p style='color:gray'>" +
                    "This email was automatically generated on your request for an account at TalkToLeadsNow. In case you didn't request an account, please ignore this e-mail. Your information will be removed automatically from our database."
                    + "</p>";

                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(FromEmail, EmailPassword);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }


        public static void SendCallNotificationEmail(string fromWebsite, string emailTo, string username, string leadname, string leademail, string leadphone, string datetime, string leadmessage = "")
        {
            using (var message = new MailMessage())
            {

                string[] split_email = emailTo.Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in split_email)
                {
                    message.To.Add(new MailAddress(str));
                }
                message.From = new MailAddress(FromEmail, FromEmailTitle);
                message.Subject = "Call Notification";
                message.Body = "Dear " + username + "!";
                message.Body += "<br/>";
                message.Body += "<p>A lead has scheduled a call via your website <a target='_balnk' href='http://www.talktoleadsnow.com'> talktoleadsnow.com </a> widget!</p>";
                message.Body += "<p>Website: <a href='"+  fromWebsite + "' target='_blank'> " + fromWebsite + " </a> </p>";
                message.Body += "<p>Name: " + leadname + "</p>";
                message.Body += "<p>Email: " + leademail + "</p>";
                message.Body += "<p>Phone Number: " + leadphone + "</p>";
                message.Body += "<p>Call Scheduled at: " + datetime + "</p>";

                if (!string.IsNullOrEmpty(leadmessage))
                    message.Body += "<p>Message: " + leadmessage + "</p>";

               

                message.Body += "<p>Our clever widget will do all the work, it will call and connect you to your lead at the scheduled time so you don't have to lift a finger, all arranged for your convenience. </p>";

                //message.Body += "<a target='_balnk' href='" + "http://www.talktoleadsnow.com/#/resetpassword/" + token + "'>" + "http://talktoleadsnow.com/#/resetpassword/" + token + "</a>";
                message.Body += "<p>Regards,</p>";
                message.Body += "<p>TalkToLeadsNow team.</p>";
                message.Body += "<a target='_balnk' href='http://www.talktoleadsnow.com'> <img width='200px' src='http://api.talktoleadsnow.com/api/style/images/talktoleadsnow-small.png'></a>";
                message.Body += "<hr height='1' style='height: 1px; border: 0 none; color: #D7DFE3; background-color: #D7DFE3; margin-top:1.5em; margin-bottom:1.5em;'>";

                //message.Body += "<p style='color:gray'>" +
                //"This email was automatically generated on your request for an account at TalkToLeadsNow. In case you didn't request an account, please ignore this e-mail. Your information will be removed automatically from our database."
                //+ "</p>";

                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(FromEmail, EmailPassword);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }




        public static void SendOnlineCallNotificationEmail(string fromWebsite, string emailTo, string username, string leadname, string leademail, string leadphone, string datetime, string leadmessage = "")
        {
            using (var message = new MailMessage())
            {

                string[] split_email = emailTo.Split(",;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in split_email)
                {
                    message.To.Add(new MailAddress(str));
                }
                message.From = new MailAddress(FromEmail, FromEmailTitle);
                message.Subject = "Call Report";
                message.Body = "Dear " + username + "!";
                message.Body += "<br/>";
                message.Body += "<p>A lead had a call via your website <a target='_balnk' href='http://www.talktoleadsnow.com'> talktoleadsnow.com </a> widget!</p>";
                message.Body += "<p>Website: <a href='" + fromWebsite + "' target='_blank'> " + fromWebsite + " </a> </p>";
                message.Body += "<p>Name: " + leadname + "</p>";
                message.Body += "<p>Email: " + leademail + "</p>";
                message.Body += "<p>Phone Number: " + leadphone + "</p>";
                message.Body += "<p>Call time: " + datetime + "</p>";

                if (!string.IsNullOrEmpty(leadmessage))
                    message.Body += "<p>Message: " + leadmessage + "</p>";


                message.Body += "<p>If you didn't get this call or your line was busy at that time, you can call him directly. </p>";

                //message.Body += "<p><a href='' target='_blank'>  </a></p>";

                //message.Body += "<a target='_balnk' href='" + "http://www.talktoleadsnow.com/#/resetpassword/" + token + "'>" + "http://talktoleadsnow.com/#/resetpassword/" + token + "</a>";
                message.Body += "<p>Regards,</p>";
                message.Body += "<p>TalkToLeadsNow team.</p>";
                message.Body += "<a target='_balnk' href='http://www.talktoleadsnow.com'> <img width='200px' src='http://api.talktoleadsnow.com/api/style/images/talktoleadsnow-small.png'></a>";
                message.Body += "<hr height='1' style='height: 1px; border: 0 none; color: #D7DFE3; background-color: #D7DFE3; margin-top:1.5em; margin-bottom:1.5em;'>";

                //message.Body += "<p style='color:gray'>" +
                //"This email was automatically generated on your request for an account at TalkToLeadsNow. In case you didn't request an account, please ignore this e-mail. Your information will be removed automatically from our database."
                //+ "</p>";

                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(FromEmail, EmailPassword);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }


        public static void SendActivationWidgetNotification(Widget widget)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(widget.User.UserID));
                message.From = new MailAddress(FromEmail, FromEmailTitle);
                message.Subject = "Your Widget Activated";
                message.Body = "Dear " + widget.User.Name + "!";
                message.Body += "<br/>";
                message.Body += "<p>Your callback widget is active now! You can go and check it out at <a href='https://portal.talktoleadsnow.com' target='_blank'> https://portal.talktoleadsnow.com </a> </p>";
                message.Body += "<p>Widget Name: " + widget.WidgetName + "</p>";
                message.Body += "<p>Phone Number: " + widget.ConnectedTo + "</p>";
                message.Body += "<p>Domain Url: " + widget.DomainURL + "</p> <br>";

                //message.Body += "<a target='_balnk' href='" + "http://www.talktoleadsnow.com/#/resetpassword/" + token + "'>" + "http://talktoleadsnow.com/#/resetpassword/" + token + "</a>";
                message.Body += "<p>Regards,</p>";
                message.Body += "<p>TalkToLeadsNow team.</p>";
                message.Body += "<a target='_balnk' href='http://www.talktoleadsnow.com'> <img width='200px' src='http://api.talktoleadsnow.com/api/style/images/talktoleadsnow-small.png'></a>";
                message.Body += "<hr height='1' style='height: 1px; border: 0 none; color: #D7DFE3; background-color: #D7DFE3; margin-top:1.5em; margin-bottom:1.5em;'>";

                message.Body += "<p style='color:gray'>" +
                "This email was automatically generated on your request for your callback widget activation at TalkToLeadsNow. In case you didn't request that, please ignore this e-mail. Your information will be removed automatically from our database."
                + "</p>";

                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(FromEmail, EmailPassword);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }

        public static void SendCreationWidgetNotification(Widget widget)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(widget.User.UserID));
                message.From = new MailAddress(FromEmail, FromEmailTitle);
                message.Subject = "New Widget Created";
                message.Body = "Dear " + widget.User.Name + "!";
                message.Body += "<br/>";
                message.Body += "<p>Thank you for joining Talk to Leads Now and taking the first step in converting more leads from your website. We will notify you when your widget is activated. Please go ahead and install it on your website, your widget will not be visible while we are enabling it, when it's activated it will appear on your website ( if you have installed the code already), please note in some cases it may take up to 24 hours for a call back widget to be activated.</p>";
                message.Body += "<p>Please do not hesitate to contact us if you need help in installing or using our widget. </p>";

                message.Body += "<p>Widget Name: " + widget.WidgetName + "</p>";
                message.Body += "<p>Phone Number: " + widget.ConnectedTo + "</p>";
                message.Body += "<p>Domain Url: " + widget.DomainURL + "</p> <br>";

                //message.Body += "<a target='_balnk' href='" + "http://www.talktoleadsnow.com/#/resetpassword/" + token + "'>" + "http://talktoleadsnow.com/#/resetpassword/" + token + "</a>";
                message.Body += "<p>Regards,</p>";
                message.Body += "<p>TalkToLeadsNow team.</p>";
                message.Body += "<a target='_balnk' href='http://www.talktoleadsnow.com'> <img width='200px' src='http://api.talktoleadsnow.com/api/style/images/talktoleadsnow-small.png'></a>";
                message.Body += "<hr height='1' style='height: 1px; border: 0 none; color: #D7DFE3; background-color: #D7DFE3; margin-top:1.5em; margin-bottom:1.5em;'>";

                message.Body += "<p style='color:gray'>" +
                "This email was automatically generated on your request for your callback widget creation at TalkToLeadsNow. In case you didn't request that, please ignore this e-mail. Your information will be removed automatically from our database."
                + "</p>";

                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(FromEmail, EmailPassword);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }



        public static void SendSignUpNotificationEmail(string emailTo, string user_email, string user_name)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(emailTo));
                message.From = new MailAddress(FromEmail, FromEmailTitle);
                message.Subject = "New Customer";
                message.Body = "Hello matt!";
                message.Body += "<br/>";
                message.Body += "<p>A new customer just signed up in your site (talktoleadsnow.com)!</p>";
                message.Body += "<p>Name: " + user_name + "</p>";
                message.Body += "<p>Email: " + user_email + "</p>";

                //message.Body += "<a target='_balnk' href='" + "http://www.talktoleadsnow.com/#/resetpassword/" + token + "'>" + "http://talktoleadsnow.com/#/resetpassword/" + token + "</a>";
                message.Body += "<p>Regards,</p>";
                message.Body += "<p>TalkToLeadsNow team.</p>";
                message.Body += "<a target='_balnk' href='http://www.talktoleadsnow.com'> <img width='200px' src='http://api.talktoleadsnow.com/api/style/images/talktoleadsnow-small.png'></a>";
                message.Body += "<hr height='1' style='height: 1px; border: 0 none; color: #D7DFE3; background-color: #D7DFE3; margin-top:1.5em; margin-bottom:1.5em;'>";

                //message.Body += "<p style='color:gray'>" +
                //"This email was automatically generated on your request for an account at TalkToLeadsNow. In case you didn't request an account, please ignore this e-mail. Your information will be removed automatically from our database."
                //+ "</p>";

                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(FromEmail, EmailPassword);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }



        public static void SendWidgetSnippetCode(Widget widget)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(widget.User.UserID));
                message.From = new MailAddress(FromEmail, FromEmailTitle);
                message.Subject = "Widget Script";
                message.Body = "Dear " + widget.User.Name + "!";
                message.Body += "<br/>";
                message.Body += "<p>Thank you for joining Talk to Leads Now and taking the first step in converting more leads from your website.</p>";
                message.Body += "<p>Please go ahead and install it on your website.You just need to copy the follwing code (Javascript) and paste it at the end of your html page just before the '&lt;/Body&gt;' tag. </p>";
                message.Body += "<p>Please do not hesitate to contact us if you need help in installing or using our widget. </p>";
                message.Body += "<p>Widget Code: </p>";

                string snippetCode = CallmeController.GetSnippetCodeStatic(widget.ID.ToString());
                //string snippetCode = "script hello script";
                message.Body += "<textarea style='width:70%;height:100px;text-align:left;font-size:14px;background:#2f353a;color: white;border: #ff6600 2px solid;padding: 5px' readonly='true'>" + snippetCode + "</textarea> <br>";

                //string snippetCode = CallmeController.GetSnippetCodeStatic(widget.ID.ToString());
                //message.Body += "<p style='text-decoration:italic;font-size:14px;padding:20px;border: 1px solid orange'>" + snippetCode + "</p> <br>";

                message.Body += "<p>Widget Name: " + widget.WidgetName + "</p>";
                message.Body += "<p>Phone Number: " + widget.ConnectedTo + "</p>";
                message.Body += "<p>Domain Url: " + widget.DomainURL + "</p>";


                //message.Body += "<a target='_balnk' href='" + "http://www.talktoleadsnow.com/#/resetpassword/" + token + "'>" + "http://talktoleadsnow.com/#/resetpassword/" + token + "</a>";
                message.Body += "<p>Regards,</p>";
                message.Body += "<p>TalkToLeadsNow team.</p>";
                message.Body += "<a target='_balnk' href='http://www.talktoleadsnow.com'> <img width='200px' src='http://api.talktoleadsnow.com/api/style/images/talktoleadsnow-small.png'></a>";
                message.Body += "<hr height='1' style='height: 1px; border: 0 none; color: #D7DFE3; background-color: #D7DFE3; margin-top:1.5em; margin-bottom:1.5em;'>";

                message.Body += "<p style='color:gray'>" +
                "This email was automatically generated on your request for your callback widget creation at TalkToLeadsNow. In case you didn't request that, please ignore this e-mail. Your information will be removed automatically from our database."
                + "</p>";

                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential(FromEmail, EmailPassword);
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
        }




    }
}
