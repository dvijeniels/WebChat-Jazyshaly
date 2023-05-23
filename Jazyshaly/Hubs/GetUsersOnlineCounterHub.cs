using Jazyshaly.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Jazyshaly.Hubs
{
    public class GetUsersOnlineCounterHub : Hub
    {
        private readonly SignInManager<User> _signInManager;
        private static readonly ConcurrentDictionary<string, string> OnlineUsers = new ConcurrentDictionary<string, string>();

        public GetUsersOnlineCounterHub(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async void SendUsersCounter()
        {
            var onlineUserNames = OnlineUsers.Values.ToList();
            await Clients.All.SendAsync("GetUsersCounter", onlineUserNames.Count, onlineUserNames);
        }

        public override Task OnConnectedAsync()
        {
            if (_signInManager.IsSignedIn(Context.User))
            {
                var userName = Context.User.Identity.Name;
                OnlineUsers.TryAdd(Context.ConnectionId, userName);
            }

            SendUsersCounter();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (_signInManager.IsSignedIn(Context.User))
            {
                OnlineUsers.TryRemove(Context.ConnectionId, out _);
            }

            SendUsersCounter();
            return base.OnDisconnectedAsync(exception);
        }
    }
}
