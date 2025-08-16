using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
namespace RakbnyMa_aak.Hubs
{
    public class TripTrackingHub : Hub
    {
        public async Task SendLocation(string tripId, double latitude, double longitude)
        {
            await Clients.Group(tripId).SendAsync("ReceiveLocation", latitude, longitude);
        }

        public override async Task OnConnectedAsync()
        {
            var tripId = Context.GetHttpContext()?.Request.Query["tripId"];

            if (!string.IsNullOrEmpty(tripId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, tripId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var tripId = Context.GetHttpContext()?.Request.Query["tripId"];
            if (!string.IsNullOrEmpty(tripId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, tripId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }

}
