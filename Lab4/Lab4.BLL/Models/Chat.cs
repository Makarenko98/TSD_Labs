using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4.BLL.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ChatUser> ChatUsers { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
