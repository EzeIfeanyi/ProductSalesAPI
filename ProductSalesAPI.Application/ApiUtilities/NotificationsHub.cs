using Microsoft.AspNetCore.SignalR;

namespace ProductSalesAPI.Application.ApiUtilities;

public class NotificationsHub : Hub
{
    public async override Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("ReceiveMessage", Context.ConnectionId);

        await base.OnConnectedAsync();
    }
}
