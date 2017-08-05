using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MessagesBackend.Models;
using Microsoft.AspNetCore.Authorization;

namespace MessagesBackend.Controllers
{
    public class EditProfileData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        readonly ApiContext _db;
        public UsersController(ApiContext db)
        {
            _db = db;
        }
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var user = _db.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound("User not found");
            return Ok(user);
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult Get()
        {
            return Ok(GetSecureUser());
        }


        [Authorize]
        [HttpPost("Edit")]
        public ActionResult Post([FromBody]EditProfileData profileData)
        {
            var user = GetSecureUser();
            user.FirstName = profileData.FirstName ?? user.FirstName;
            user.LastName = profileData.LastName ?? user.LastName;

            _db.SaveChanges();

            return Ok(user);
        }

        User GetSecureUser()
        {
            var id = HttpContext.User.Claims.First().Value;
            return _db.Users.SingleOrDefault(u => u.Id == id);

        }
        //protected override void Dispose(bool disposing)
        //{
        //    if (_db!=null)
        //    {
        //        _db.Dispose();
        //    }
        //}
    }
}