using NUnit.Framework;
using Lab4.BLL.Services;
using Lab4.BLL.Models;
using Lab4.BLL;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Test
{
    [TestFixture]
    class UserServiceTest
    {
        [SetUp]
        public void SetUp()
        {
            using (var db = Utils.BuildDbContext()) {
                db.Database.ExecuteSqlCommand("delete from Users");
            }
        }

        [Test]
        public void SignUpTest()
        {
            using (var db = Utils.BuildDbContext()) {
                var service = new UserService(db);
                var user = service.SignUp(new User() {
                    Login = "user",
                    Password = "pass",
                    FirstName = "James",
                    Email = "james@gmail.com"
                });
                Assert.AreEqual(user.Login, db.Users.Find(user.Id).Login);
            }
        }

        [Test]
        public void SighInTest()
        {
            using (var db = Utils.BuildDbContext()) {
                var service = new UserService(db);

                var createdUser = service.SignUp(new User() {
                    Login = "user",
                    Password = "pass",
                    FirstName = "James",
                    Email = "james@gmail.com"
                });

                var user = service.SignIn("user", "pass");

                Assert.AreEqual(user.Login, createdUser.Login);

                Assert.IsNull(service.SignIn("user", "123"));
                Assert.IsNull(service.SignIn("usss", "pass"));
            }
        }
    }
}