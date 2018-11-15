using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CallMeAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using CallMeAPI.DTO;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CallMeAPI.Controllers
{


    [Route("api/auth")]
    public class AuthController : Controller
    {

        private readonly MyDBContext contextUsers;
        IConfiguration configuration;

        public AuthController(MyDBContext _contextUsers, IConfiguration config)
        {
            this.contextUsers = _contextUsers;
            this.configuration = config;
        }


        [HttpGet("myname")]
        public IEnumerable<string> GetMyName()
        {

            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            try
            {
                var email = this.HttpContext.Request.Headers["From"].ToString();
                return new string[]
                {contextUsers.Users.Find(email).Name
                };
            }
            catch(Exception)
            {
                return null;
            }
        }

        [HttpGet("myrole")]
        public IEnumerable<string> GetMyRole()
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            try
            {
                var email = this.HttpContext.Request.Headers["From"].ToString();
                return new string[]
                {contextUsers.Users.Find(email).Role
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet("customers")]
        public IEnumerable<CustomerDTO> GetCustomers()
        {
            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);

            try
            {
                List<User> users = contextUsers.Users.Where(u => u.IsActive == true).OrderByDescending(u => u.CreationDateTime).ToList();
                List<CustomerDTO> customers = new List<CustomerDTO>();
                foreach(User user in users)
                {
                    CustomerDTO customer = new CustomerDTO();
                    customer.name = user.Name;
                    customer.email = user.UserID;
                    customer.creationDateTime = user.CreationDateTime.ToShortDateString() + " [" +  user.CreationDateTime.ToShortTimeString() + "]";
                    if (user.LastLogon.HasValue)
                        customer.lastLogin = user.LastLogon?.ToShortDateString() + " [" + user.LastLogon?.ToShortTimeString() + "]";
                    customers.Add(customer);
                }

                return customers;
            }
            catch (Exception)
            {
                return null;
            }
        }




        // POST: /api/auth/login
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody]LoginDTO user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Invalid client request");
                }

                var _user = contextUsers.Users.Find(user.UserName);

                if (_user == null)
                {
                    return NotFound();
                }

                if (!_user.IsActive)
                {
                    return NotFound();
                }

                if (user.Password == _user.Password)
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));

                    var tokeOptions = new JwtSecurityToken(
                        issuer: configuration["Jwt:Issuer"],
                        audience: configuration["Jwt:Issuer"],
                        claims: claims,
                        expires: DateTime.Now.AddHours(10),
                        signingCredentials: signinCredentials
                    );

                    _user.LastLogon = DateTime.Now;
                    contextUsers.SaveChanges();
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    return Ok(new { Token = tokenString, Name = _user.Name });
                }
                else
                {
                    return Unauthorized();
                }
            }catch (Exception ex)
            {
                throw ex;
            }
        }




        // POST: /api/auth/register
        [HttpPost]
        [Route("register")]
        public IActionResult RegisterUser([FromBody]RegisterUserDTO registerInfo)
        {
            if (registerInfo == null)
            {
                return NotFound();
            }

            User user = contextUsers.Users.Find(registerInfo.Email);
            if (user != null && user.IsActive)
            {
                return BadRequest("AlreadyExists");
            }

            if (user == null)
            {
                user = new User();
                user.UserID = registerInfo.Email;
                user.Name = registerInfo.Name;
                user.Password = registerInfo.Password;
                user.IsActive = false;
                user.IsFirstLogon = true;
                user.Role = "user";
                user.CreationDateTime = DateTime.Now;
                contextUsers.Users.Add(user);
            }
            else
            {
                user.Name = registerInfo.Name;
                user.Password = registerInfo.Password;
                user.IsActive = false;
                user.IsFirstLogon = true;
                user.Role = "user";
                user.CreationDateTime = DateTime.Now;
            }

            Guid token = Guid.NewGuid();

            try
            {
                EmailManager.SendActivationlinkEmail(registerInfo.Email,user.Name, token.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            SecurityLinkToken securityLinkToken = new SecurityLinkToken();
            securityLinkToken.Email = registerInfo.Email;
            securityLinkToken.Token = token.ToString();
            securityLinkToken.Type = "activation";
            securityLinkToken.IsDone = false;
            securityLinkToken.CreationDateTime = DateTime.Now;

            contextUsers.SecurityLinkTokens.Add(securityLinkToken);

            contextUsers.SaveChanges();
            return Ok();
        }

        // POST: /api/auth/register
        [HttpPost]
        [Route("resetpassword")]
        public IActionResult ResetPassword([FromBody]EmailDTO email)
        {
            if (email == null)
            {
                return NotFound();
            }

            var user = contextUsers.Users.Find(email.Email);

            if (user == null || !user.IsActive)
            {
                 return NotFound();
            }

            Guid token = Guid.NewGuid();

            try
            {
                EmailManager.SendResetPasswordLinkEmail(email.Email,user.Name,token.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            SecurityLinkToken securityLinkToken = new SecurityLinkToken();
            securityLinkToken.Email = email.Email;
            securityLinkToken.Token = token.ToString();
            securityLinkToken.Type = "resetpassword";
            securityLinkToken.IsDone = false;

            contextUsers.SecurityLinkTokens.Add(securityLinkToken);
            contextUsers.SaveChanges();


            return Ok();
        }





        public static string ValidateAndGetCurrentUserName(HttpRequest request)
        {
            var Authtoken = request.Headers["Authorization"];
            var FromToken = request.Headers["From"];
            var accessToken = "";
            var username = "";

            if (Authtoken.ToString().StartsWith("Bearer"))
            {
                accessToken = Authtoken.ToString().Substring(7);
            }

            if (String.IsNullOrEmpty(accessToken))
            {
                throw new SecurityTokenInvalidLifetimeException();
            }

            JwtSecurityToken token = new JwtSecurityToken(accessToken);
            username = token.Subject;

            if (username != FromToken)
            {
                throw new SecurityTokenInvalidLifetimeException();
            }

            return username;
        }


        // POST: /api/auth/checkactivationtoken
        [HttpPost]
        [Route("checkactivationtoken")]
        public async Task<IActionResult> CheckActivationToken([FromBody]TokenDTO token)
        {
            if (token == null)
                return NotFound();

            SecurityLinkToken _token = contextUsers.SecurityLinkTokens.Find(token.token);

            if (_token == null)
                return NotFound();

            User user = contextUsers.Users.Find(_token.Email);
            user.IsActive = true;

            contextUsers.SecurityLinkTokens.Remove(_token);

            await contextUsers.SaveChangesAsync();

            return Ok();
        }


        // POST: /api/auth/checktoken
        [HttpPost]
        [Route("checktoken")]
        public IActionResult CheckToken([FromBody]TokenDTO token)
        {
            if (token == null)
                return NotFound();

            SecurityLinkToken _token = contextUsers.SecurityLinkTokens.Find(token.token);

            if (_token == null)
                return NotFound();

            return Ok();
        }


        // POST: /api/auth/changepasswordtoken
        [HttpPost]
        [Route("changepasswordtoken")]
        public async Task<IActionResult> ChangePasswordWithToken([FromBody]TokenPasswordDTO token)
        {
            if (token == null)
                return NotFound();

            SecurityLinkToken _token = contextUsers.SecurityLinkTokens.Find(token.Token);

            if (_token == null)
                return NotFound();

            User user = contextUsers.Users.Find(_token.Email);

            if (user == null || !user.IsActive)
                return NotFound();

            user.Password = token.Password;

            contextUsers.SecurityLinkTokens.Remove(_token);

            await contextUsers.SaveChangesAsync();

            return Ok();
        }

        // POST: /api/auth/changepasswordemail
        [HttpPost]
        [Route("changepasswordemail")]
        public async Task<IActionResult> ChangePasswordWithEmail([FromBody]EmailPasswordDTO info)
        {

            AuthController.ValidateAndGetCurrentUserName(this.HttpContext.Request);


            if (info == null)
                return NotFound();


            User user = contextUsers.Users.Find(info.Email);

            if (user == null || !user.IsActive)
                return NotFound();

            if (user.Password != info.OldPassword)
                return BadRequest("WrongPassword");

            user.Password = info.NewPassword;

            await contextUsers.SaveChangesAsync();

            return Ok();
        }
    }

    public class CustomerDTO
    {
        public string name { get; set; }
        public string email { get; set; }
        public string creationDateTime { get; set; }
        public string lastLogin { get; set; }
        public int widgetsCount { get; set; }
        public string subscription { get; set; }

    }

    public class EmailDTO
    {
        public string Email { get; set; }
    }

    public class TokenPasswordDTO
    {
        public string Token { get; set; }
        public string Password { get; set; }
    }

    public class EmailPasswordDTO
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
