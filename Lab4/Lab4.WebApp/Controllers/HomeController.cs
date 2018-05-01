using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lab4.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Lab4.BLL;
using Lab4.BLL.Models;
using Dapper;
using Lab4.BLL.Constants;

namespace Lab4.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private SocialNetDbContext dbContext;
        public HomeController(SocialNetDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var user = dbContext.Users
                .Include(u => u.UserFriends)
                .ThenInclude((UserFriend uf) => uf.Friend)
                .Include(u => u.FriendRequests)
                .ThenInclude((FriendRequest fr) => fr.FromUser)
                .Where(u => u.Login == HttpContext.User.Identity.Name)
                .FirstOrDefault();

            user.FriendRequests = user.FriendRequests.Where(fr => fr.StateId == FriendRequestStateConstants.New).ToArray();

            return View(user);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
