using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lab4.BLL.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}
