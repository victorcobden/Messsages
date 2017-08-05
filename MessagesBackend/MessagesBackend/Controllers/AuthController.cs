using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using MessagesBackend.Models;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MessagesBackend.Controllers
{
    public class JwtPacket
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
    }

    public class LoginData
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    [Produces("application/json")]
    [Route("auth")]
    public class AuthController : Controller
    {
        readonly ApiContext _db;

        public AuthController(ApiContext db)
        {
            _db = db;
        }

        [HttpPost("register")]
        public JwtPacket Register([FromBody]User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();

            return CreateJwt(user);
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginData user)
        {
            var userInDb = _db.Users.Where(n => n.Email == user.Email && n.Password == user.Password).FirstOrDefault();
            ActionResult result = null;
            if (userInDb == null)
                result = NotFound("Email or password incorrect");

            result = Ok(CreateJwt(userInDb));

            return result;
        }

        JwtPacket CreateJwt(User user)
        {
            var singingKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes("this is the secret phrase")
                );

            var signingCredentials = new SigningCredentials(singingKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };
            var jwt = new JwtSecurityToken(claims: claims,signingCredentials: signingCredentials);
            var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new JwtPacket { Token = encodeJwt, FirstName = user.FirstName };

        }
    }
}