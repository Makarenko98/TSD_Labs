using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab4.BLL;
using Lab4.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab4.WebApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private SocialNetDbContext dbContext;
        private const int pageSize = 20;
        public UserController(SocialNetDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private bool CanSendFriendRequest(User user)
        {
            return
                user.Login != HttpContext.User.Identity.Name
                && dbContext.FriendRequests
                .Where(fr => fr.FromUser.Login == HttpContext.User.Identity.Name
                    && fr.ToUserId == user.Id)
                    .Count() == 0
                && dbContext.UserFriends
                    .Where(uf => uf.User.Login == HttpContext.User.Identity.Name
                    && uf.Friend.Id == user.Id).Count() == 0;
        }

        [Route("User")]
        public IActionResult Index(int? page)
        {
            if (page == null)
                page = 0;
            ViewBag.pageNumber = dbContext.Users.Count() / pageSize + 1;
            ViewBag.page = page;
            return View(dbContext.Users
                .Where(u => u.Login != HttpContext.User.Identity.Name)
                .Skip(page.Value * pageSize)
                .Take(pageSize)
                .ToList());
        }

        [Route("User/{id}")]
        public IActionResult Record(int id)
        {
            if (HttpContext.GetUser(dbContext).Id == id)
                return RedirectToAction("Index", "Home");

            var user = dbContext.Users
                .Include(u => u.UserFriends)
                .ThenInclude((UserFriend uf) => uf.Friend)
                .Include(u => u.FriendRequests)
                .ThenInclude((FriendRequest fr) => fr.FromUser)
                .FirstOrDefault(u => u.Id == id);

            ViewBag.CanSendFriendRequest = CanSendFriendRequest(user);

            if (user != null)
                return View(user);
            else
                return new StatusCodeResult(404);
        }
    }
}