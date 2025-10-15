using Applicazione_1.Models;
using Applicazione_1.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Applicazione_1.Services;
using System.Dynamic;
using Newtonsoft.Json.Linq;


namespace Applicazione_1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly HttpConnectionSettings _connectionSettings;
        private readonly DocumentStorageSettings _documentStorageSettings;
        private readonly JWT _jwt;
        private readonly EmailValidation _emailvalidation;

        public TestController(IOptions<HttpConnectionSettings> connectionSettingsOption, IOptions<DocumentStorageSettings> documentStorageSettingsOption, JWT jwt, EmailValidation emailvalidation)
        {
            _connectionSettings = connectionSettingsOption.Value;
            _documentStorageSettings = documentStorageSettingsOption.Value;
            _jwt = jwt;
            _emailvalidation = emailvalidation;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            User user = new User();
            user.UserName = "UsernameAhmed";
            user.Password = "passwordAhmed";
            user.Role = "adminAhmed";
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Login(dynamic data)
        {
            User user = new User();
            user.UserName = data.UserName;
            user.Password = data.Password;
            var email= _emailvalidation.IsEmailValid(user.UserName);
            if (email)
            {
                var domain = _emailvalidation.GetEmailDomain(user.UserName);
                if (domain != null && domain == "Admin.com")
                {
                    user.Role = "admin";
                }
                else if (domain != null && domain == "Employee.com")
                {
                    user.Role = "employee";
                }
                else if(domain != null && domain == "Client.com")
                {
                    user.Role="client";
                }
            }
            else { return BadRequest(); }

            if (user.Password == "password"&& (user.Role == "admin" || user.Role == "employee" || user.Role == "client"))
            {
                var token = _jwt.GenerateJwtToken(user.UserName,user.Role);
                int size = 26;

                for (int i = 0; i < size; i++) // Rows
                {
                    for (int j = 0; j < size; j++) // Columns

                            Console.Write("* ");

                    
                    Console.WriteLine(); // New line after each row
                }
                return Ok(token);
            }
            return Unauthorized();
        }
    }
}
