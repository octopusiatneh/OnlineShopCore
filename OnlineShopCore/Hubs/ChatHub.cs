using Microsoft.AspNetCore.SignalR;
using OnlineShopCore.Application.ViewModels.System;
using System.Threading.Tasks;

namespace OnlineShopCore.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendNotification(AnnouncementViewModel message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
