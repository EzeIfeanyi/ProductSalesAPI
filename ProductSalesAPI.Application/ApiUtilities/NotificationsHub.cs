using Microsoft.AspNetCore.SignalR;

namespace ProductSalesAPI.Application.ApiUtilities;

public class NotificationsHub : Hub
{
    public override Task OnConnectedAsync()
    {
        Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId);

        return base.OnConnectedAsync();
    }
}
