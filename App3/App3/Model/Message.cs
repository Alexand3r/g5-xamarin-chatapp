using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.ViewModel
{
    public class Message
    {
        public int id { get; set; }
        public string username { get; set; }
        public string message { get; set; }
        [JsonConstructor]
        public Message(int id,string username,string msg)
        {
            this.id = id;
            this.username = username;
            this.message = msg;
        }
        public Message()
        {
        }

    }
}
