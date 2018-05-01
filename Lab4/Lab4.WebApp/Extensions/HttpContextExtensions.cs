using Lab4.BLL;
using Lab4.BLL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.WebApp
{
    public static class HttpContextExtensions
    {
        public static User GetUser(this HttpContext httpContext, SocialNetDbContext dbContext)
        {
            return dbContext.Users.Where(u => u.Login == httpContext.User.Identity.Name).FirstOrDefault();
        }
    }
}
