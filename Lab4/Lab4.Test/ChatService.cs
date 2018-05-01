using System;
using System.Collections.Generic;
using System.Text;
using Lab4.BLL.Services;
using Lab4.BLL.Models;
using NUnit.Framework;

namespace Lab4.Test
{
    [TestFixture]
    class ChatServiceTest
    {
        [Test]
        public void GetNextMessagesTest()
        {
            var service = new ChatService(Utils.ConnectionString);
            var messages = service.GetNextMessages(14, 15, 10);
        }
    }
}
