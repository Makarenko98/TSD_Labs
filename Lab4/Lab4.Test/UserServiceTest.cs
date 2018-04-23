using NUnit.Framework;
using Lab4.BLL.Services;
using Lab4.BLL.Models;
using Lab4.BLL;
using Microsoft.EntityFrameworkCore;
using System;

namespace Lab4.Test
{
    [TestFixture]
    class UserServiceTest
    {
        static Random random = new Random();

        [Test]
        public void SignUpTest()
        {
            var service = new UserService(Utils.ConnectionString);
            var uid = random.Next();
            var login = "user" + uid;
            var pass = "pass" + uid;
            var user = service.SignUp(new User() {
                Login = login,
                Password = pass,
                FirstName = "James" + uid,
                Email = $"james@gmail{uid}.com"
            });
            using (var dbConext = new SocialNetDbContext(Utils.ConnectionString)) {
                Assert.AreEqual(user.Login, dbConext.Users.Find(user.Id).Login);
            }
        }

        [Test]
        public void SighInTest()
        {
            var service = new UserService(Utils.ConnectionString);
            var uid = random.Next();
            var login = "user" + uid;
            var pass = "pass" + uid;
            var createdUser = service.SignUp(new User() {
                Login = login,
                Password = pass,
                FirstName = "James" + uid,
                Email = $"james@gmail{uid}.com"
            });

            var user = service.SignIn(login, pass);

            Assert.AreEqual(user.Login, createdUser.Login);

            Assert.IsNull(service.SignIn(login, random.Next().ToString()));
            Assert.IsNull(service.SignIn("user" + random.Next(), pass));
        }
    }
}