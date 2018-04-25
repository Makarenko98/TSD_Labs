using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4.BLL.Models
{
    public class UserFriend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public User User { get; set; }
        public User Frient { get; set; }
    }
}
