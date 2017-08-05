using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MessagesBackend.Models;

namespace MessagesBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessagesController : Controller
    {
        private readonly ApiContext _db;

        public MessagesController(ApiContext db)
        {
            _db = db;
        }

        public IEnumerable<Message> Get()
        {
            return _db.Messages;
        }
        
        [HttpGet("{name}")]
        public IEnumerable<Message> Get(string name)
        {
            return _db.Messages.Where(m => m.Owner == name);
        }

        [HttpPost]
        public Message Post([FromBody]Message message)
        {
            var msge = _db.Messages.Add(message).Entity;
            _db.SaveChanges();
            return msge;
        }
    }
}