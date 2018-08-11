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
        protected readonly string ConnectionString;

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
                const string sql = @"insert into Messages (ChatId, UserId, Text)
                                output inserted.*
                            values(@chatId, (select top 1 id from Users u where u.Login = @login), @text)";
                return db.QueryFirst<Message>(sql, new {login, chatId, text});
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
                throw new ArgumentOutOfRangeException(nameof(count) + "Count must be more than 1");

            using (var db = new SqlConnection(ConnectionString)) {
                return db.Query<Message, User, Message>(
                    $@"select top {count} *
                    from Messages m
                        join ChatUsers cu on cu.ChatId = @chatId
                            and @userId = cu.UserId
                        join Users u on u.Id = m.UserId
                    order by m.time desc",
                    (message, user) => {
                        message.User = user;
                        return message;
                    },
                    new {chatId, userId});
            }
        }

        public IEnumerable<Message> GetNextMessages(int lastMessageId, int userId, int count = 20)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count) + " must be more than 1");

            using (var db = new SqlConnection(ConnectionString)) {
                return db.Query<Message, User, Message>(
                    $@"select top {count} m.*, u.*
                    from Messages m
                        join Users u on u.Id = m.UserId
                        join Messages m1 on m1.Id = @lastMessageId
                            and m.ChatId = m1.ChatId
                            and m.Time < m1.Time
                        join ChatUsers cu on m1.ChatId = cu.ChatId
                            and @userId = cu.UserId
                    order by m.time desc",
                    (message, user) => {
                        message.User = user;
                        return message;
                    },
                    new {lastMessageId, userId});
            }
        }

        public IEnumerable<User> GetGetPotentialChatParticipants(int chatId, string userLogin)
        {
            using (var db = new SqlConnection(ConnectionString)) {
                const string sql = @"select u1.*
                            from UserFriends uf
                                join Users u on u.login = @login
                                    and u.Id = uf.UserId
                                join Users u1 on u1.id = uf.FriendId
                            where not exists (
                                    select null
                                    from ChatUsers cu
                                    where cu.ChatId = @chatId
                                        and cu.UserId = uf.FriendId)";
                return db.Query<User>(sql, new {login = userLogin, chatId = chatId});
            }
        }
    }
}