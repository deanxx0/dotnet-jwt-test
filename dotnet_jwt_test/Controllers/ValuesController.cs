using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_jwt_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("getFruits")]
        public ActionResult GetFruits()
        {
            List<string> myList = new List<string>() { "apples", "bananas" };
            return Ok(myList);
        }

        [HttpGet("getFruitsAuth")]
        public ActionResult GetFruitsAuth()
        {
            List<string> myList = new List<string>() { "apples", "bananas" };
            return Ok(myList);
        }

        [AllowAnonymous]
        [HttpPost("getToken")]
        public async Task<ActionResult> GetToken([FromBody] MyLoginModelType myLoginModel)
        {
            if (myLoginModel.Email == "st@msn.com" && myLoginModel.Password == "stst")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaa");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, myLoginModel.Email)
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized("try again");
            }
        }
    }
}
