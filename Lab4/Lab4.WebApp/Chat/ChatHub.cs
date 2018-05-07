using Lab4.BLL.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Lab4.BLL;
using System.Collections.Generic;
using System.Linq;
using Lab4.BLL.Services;

namespace Lab4.WebApp
{
    public class ChatHub : Hub
    {
        private SocialNetDbContext dbContext;
        private ChatService chatService;

        public ChatHub(SocialNetDbContext dbContext, ChatService chatService)
        {
            this.dbContext = dbContext;
            this.chatService = chatService;
        }

        public async Task SendMessage(int chatId, string message)
        {
            var insertedMessage = chatService.SendMessage(Context.User.Identity.Name, chatId, message);
            insertedMessage.User = dbContext.Users
                    .Where(u => u.Login == Context.User.Identity.Name)
                    .FirstOrDefault();
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", insertedMessage);
        }

        public async Task JoinChat(int chatId)
        {
            if (dbContext.ChatUsers
                .Where(uc => uc.User.Login == Context.User.Identity.Name
                && uc.ChatId == chatId).Count() > 0)
                await Groups.AddAsync(Context.ConnectionId, chatId.ToString());
        }
    }
}
