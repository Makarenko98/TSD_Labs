using Lab4.BLL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Lab4.BLL.Utils;
using Dapper;

namespace Lab4.BLL.Services
{
    public class ChatService
    {
        protected string ConnectionString;

        public ChatService(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public Message SendMessage(int userId, int chatId, string text)
        {
            return SendMessage(new Message() {
                UserId = userId,
                ChatId = chatId,
                Text = text
            });
        }

        public Message SendMessage(Message message)
        {
            using (var dbConect = new SocialNetDbContext(ConnectionString)) {
                return dbConect.Messages.Add(message).Entity;
            }
        }

        public IEnumerable<Message> GetNextMessages(int lastMessageId, int userId, int count = 20)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException("Count must be more than 1");

            using (var db = new SqlConnection(ConnectionString)) {
                return db.Query<Message>(
                    $"select top {count} m.*" +
                    "from Messages m".NewLineWithTab() +
                        "join Messages m1 on m1.Id = @lastMessageId".NewLineWithTab(2) +
                            "and m.ChatId = m1.ChatId".NewLineWithTab(2) +
                            "and m.Time < m1.Time".NewLineWithTab() +
                        "join ChatUsers cu on m1.ChatId = cu.ChatId".NewLineWithTab(2) +
                            "and @userId = cu.UserId".NewLineWithTab() +
                    "order by m.time desc", new { lastMessageId, userId });
            }
        }
    }
}
