using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab4.BLL.Services;
using Lab4.BLL;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.WebApp.Controllers
{
    public class FriendController : Controller
    {
        private FriendService friendService;
        private SocialNetDbContext dbContext;

        public FriendController(SocialNetDbContext dbContext, FriendService friendService)
        {
            this.friendService = friendService;
            this.dbContext = dbContext;
        }

        [HttpPost]
        public void SendFriendRequest(int userId)
        {
            friendService.SendFriendRequest(
                HttpContext.GetUser(dbContext).Id,
                userId);
        }

        [HttpPost]
        public bool AcceptFriendRequest(int friendRequestId)
        {
            friendService.AcceptFriendRequest(dbContext.FriendRequests.Find(friendRequestId));
            return true;
        }

        [HttpPost]
        public bool RejectFriendRequest(int friendRequestId)
        {
            friendService.RejectFriendRequest(dbContext.FriendRequests.Find(friendRequestId));
            return true;
        }
    }
}