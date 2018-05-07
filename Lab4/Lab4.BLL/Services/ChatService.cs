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

        public Message SendMessage(string login, int chatId, string text)
        {
            using (var db = new SqlConnection(ConnectionString)) {
                var sql = "insert into Messages (ChatId, UserId, Text)".NewLine() +
                        "output inserted.*".NewLine() +
                        "values(@chatId, (select top 1 id from Users u where u.Login = @login), @text)";
                return db.QueryFirst<Message>(sql, new { login, chatId, text });
            }
        }

        public Message SendMessage(Message message)
        {
            using (var dbConect = new SocialNetDbContext(ConnectionString)) {
                return dbConect.Messages.Add(message).Entity;
            }
        }

        public IEnumerable<Message> GetMessages(int userId, int chatId, int count = 20)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException("Count must be more than 1");

            using (var db = new SqlConnection(ConnectionString)) {
                return db.Query<Message, User, Message>(
                    $"select top {count} *".NewLine() +//m.Id as Message, m.Text, u.Id as [User], u.FirstName, u.LastName
                    "from Messages m".NewLineWithTab() +
                        "join ChatUsers cu on cu.ChatId = @chatId".NewLineWithTab(2) +
                            "and @userId = cu.UserId".NewLineWithTab() +
                        "join Users u on u.Id = m.UserId".NewLine() +
                    "order by m.time desc",
                    (message, user) => {
                        message.User = user;
                        return message;
                    },
                    new { chatId, userId });
            }
        }

        public IEnumerable<Message> GetNextMessages(int lastMessageId, int userId, int count = 20)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException("Count must be more than 1");

            using (var db = new SqlConnection(ConnectionString)) {
                return db.Query<Message, User, Message>(
                    $"select top {count} m.*, u.*" +
                    "from Messages m".NewLineWithTab() +
                        "join Users u on u.Id = m.UserId".NewLineWithTab() +
                        "join Messages m1 on m1.Id = @lastMessageId".NewLineWithTab(2) +
                            "and m.ChatId = m1.ChatId".NewLineWithTab(2) +
                            "and m.Time < m1.Time".NewLineWithTab() +
                        "join ChatUsers cu on m1.ChatId = cu.ChatId".NewLineWithTab(2) +
                            "and @userId = cu.UserId".NewLineWithTab() +
                    "order by m.time desc",
                    (message, user) => {
                        message.User = user;
                        return message;
                    },
                    new { lastMessageId, userId });
            }
        }

        public IEnumerable<User> GetGetPotentialChatParticipants(int chatId, string userLogin)
        {
            using (var db = new SqlConnection(ConnectionString)) {
                var sql = "select u1.*".NewLineWithTab() +
                            "from UserFriends uf".NewLineWithTab(2) +
                                "join Users u on u.login = @login".NewLineWithTab(3) +
                                    "and u.Id = uf.UserId".NewLineWithTab(4) +
                                "join Users u1 on u1.id = uf.FriendId".NewLineWithTab(3) +
                            "where not exists (".NewLineWithTab() +
                                    "select null".NewLineWithTab(3) +
                                    "from ChatUsers cu".NewLineWithTab(3) +
                                    "where cu.ChatId = @chatId".NewLineWithTab(3) +
                                        "and cu.UserId = uf.FriendId)".NewLineWithTab(4);
                return db.Query<User>(sql, new { login = userLogin, chatId = chatId });
            }
        }
    }
}
