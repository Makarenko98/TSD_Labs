using Lab4.BLL;
using NUnit.Framework;
using System;

namespace Lab4.Test
{
    [TestFixture]
    public class SocialNetDbContextTest
    {
        [Test]
        public void CreateDBTest()
        {
            using(var dbContext = new SocialNetDbContext(Utils.ConnectionString)) {
                dbContext.Database.EnsureCreated();
            }
        }
    }
}
