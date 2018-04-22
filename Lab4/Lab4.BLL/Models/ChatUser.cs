using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lab4.BLL.Models
{
    public class ChatUser
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int ChatId { get; set; }
        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}
