using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MiniCMS.Api.Hubs
{
    public class ContentHub : Hub
    {
        public async Task JoinApp(string appName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, appName);
        }

        public async Task LeaveApp(string appName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, appName);
        }
    }
}
