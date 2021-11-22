using BookStoreManager.Interface;
using BookStoreModel;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager manager;

        // Constructor Dependency Injection
        public UserController(IUserManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("api/register")]
        public IActionResult Register([FromBody] UserModel user)
        {
            try
            {
                UserModel userData = this.manager.Register(user);

                if (userData != null)
                {
                    return this.Ok(new { success = true, message = "Registration Successful", result = userData });
                }
                return this.Ok(new { success = false, message = "Registration Failed, User Already Exists" });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            try
            {
                string resultMessage = this.manager.Login(user);

                if (resultMessage.Equals("Login Successful"))
                {
                    // Connection to Redis Server
                    ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                    IDatabase database = connectionMultiplexer.GetDatabase();

                    // Get values from Redis Cache and store to dictionary
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    data.Add("_id", database.StringGet("UserId"));
                    data.Add("fullName", database.StringGet("FullName"));
                    data.Add("phone", database.StringGet("Phone"));
                    data.Add("email", user.email);
                    data.Add("accessToken", this.manager.GenerateToken(user.email));

                    return this.Ok(new { success = true, message = resultMessage, result = data });
                }
                else if (resultMessage.Equals("Incorrect Password"))
                {
                    return this.Ok(new { success = false, message = resultMessage });
                }
                return this.Ok(new { success = false, message = resultMessage });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/forgot-password")]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                string resultMessage = this.manager.ForgotPassword(email);

                if (resultMessage.Equals("Email has been sent"))
                {
                    return this.Ok(new { success = true, message = resultMessage });
                }
                return this.Ok(new { success = false, message = resultMessage });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/reset-password")]
        public IActionResult ResetPassword(ResetPasswordModel user)
        {
            try
            {
                string resultMessage = this.manager.ResetPassword(user);

                if (resultMessage.Equals("Reset Password Successful"))
                {
                    return this.Ok(new { success = true, message = resultMessage });
                }

                return this.Ok(new { success = false, message = resultMessage });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
