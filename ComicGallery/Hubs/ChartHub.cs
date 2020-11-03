using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicGallery.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
        Task ReceiveMessage(string message);
    }
    public class ChartHub : Hub<IChatClient>
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }
    }
}
