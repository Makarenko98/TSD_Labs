using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4.BLL.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public User FromUser { get; set; }
        public User ToUser { get; set; }
        public int StateId { get; set; }
        public FriendRequestState State { get; set; }
    }
}
