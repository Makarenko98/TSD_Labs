using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab4.BLL;
using Lab4.BLL.Models;
using Lab4.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Data.SqlClient;
using Lab4.BLL.Utils;
using Microsoft.AspNetCore.Authorization;

namespace Lab4.WebApp.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private ChatService chatService;
        private SocialNetDbContext dbContext;

        public ChatController(SocialNetDbContext dbContext, ChatService chatService)
        {
            this.chatService = chatService;
            this.dbContext = dbContext;
        }

        [HttpPost]
        public int NewChat(string chatName)
        {
            var chat = new Chat() {
                Name = chatName,
                ChatUsers = new List<ChatUser>() {
                    new ChatUser() {
                        UserId = HttpContext.GetUser(dbContext).Id
                    }
                }
            };
            dbContext.Chats.Add(chat);
            dbContext.SaveChanges();
            return chat.Id;
        }

        public IActionResult Index()
        {
            return View(dbContext.ChatUsers
                .Where(cu => cu.User.Login == HttpContext.User.Identity.Name)
                .Select(cu => cu.Chat));
        }

        public IActionResult Chat(int id)
        {
            return View(id);
        }

        public Chat GetChatInfo(int id)
        {
            var chat = dbContext.Chats
                .Include(c => c.ChatUsers)
                .ThenInclude((ChatUser c) => c.User)
                .Where(c => c.Id == id)
                .FirstOrDefault();
            chat.Messages = chatService.GetMessages(HttpContext.GetUser(dbContext).Id, id).ToArray();
            return chat;
        }

        public IEnumerable<Message> GetNextMessages(int lastMessageId)
        {
            return chatService.GetNextMessages(lastMessageId, HttpContext.GetUser(dbContext).Id);
        }

        public IEnumerable<User> GetGetPotentialChatParticipants(int chatId)
        {
            return chatService.GetGetPotentialChatParticipants(chatId, HttpContext.User.Identity.Name);
        }

        [HttpPost]
        public void AddParticipants(IEnumerable<int> users, int chatId)
        {
            foreach (var u in users) {
                dbContext.ChatUsers.Add(new ChatUser() {
                    UserId = u,
                    ChatId = chatId
                });
            }
            dbContext.SaveChanges();
        }
    }
}