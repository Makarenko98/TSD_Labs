using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4.BLL.Models
{
    public class UserPhoto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime LoadTime { get; set; }
    }
}
