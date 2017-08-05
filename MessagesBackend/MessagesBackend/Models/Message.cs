using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesBackend.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Text { get; set; }
    }
}
