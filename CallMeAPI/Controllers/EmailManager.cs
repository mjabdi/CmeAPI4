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

        public static void SendResetPasswordLinkEmail(string emailTo, string username, string token)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(emailTo));
                message.From = new MailAddress(FromEmail, FromEmailTitle);
                message.Subject = "Reset Password";
                message.Body = "Hello " + username + "!";
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
                message.Body = "Hello " + username + "!";
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


        public static void SendCallNotificationEmail(string emailTo, string username, string leadname, string leademail, string leadphone, string datetime, string leadmessage = "")
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(emailTo));
                message.From = new MailAddress(FromEmail, FromEmailTitle);
                message.Subject = "Call Notification";
                message.Body = "Hello " + username + "!";
                message.Body += "<br/>";
                message.Body += "<p>You will have a call to a lead in 15 minutes from talktoleadsnow!</p>";
                message.Body += "<p>Name: " + leadname + "</p>";
                message.Body += "<p>Email: " + leademail + "</p>";
                message.Body += "<p>Phone Number: " + leadphone + "</p>";
                message.Body += "<p>Call Scheduled at: " + datetime + "</p>";

                if (!string.IsNullOrEmpty(leadmessage))
                    message.Body += "<p>Message: " + leadmessage + "</p>";

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
                message.Body = "Hello " + widget.User.Name + "!";
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
    }
}
