using Jazyshaly.Models;
using Microsoft.AspNetCore.SignalR;

namespace Jazyshaly.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}