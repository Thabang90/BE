using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace UberTrucking.Hubs
{
    public class ChatHub : Hub
    {

        public async Task JoinChat(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessage(string groupName, string user, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnDisconnectedAsync(System.Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "groupName");
            await base.OnDisconnectedAsync(exception);
        }

        //// Dictionary to keep track of connected users (userId -> connectionId)
        //private static Dictionary<string, string> connectedUsers = new Dictionary<string, string>();

        //public override Task OnConnectedAsync()
        //{
        //    var userId = "1";//Context.UserIdentifier; // This assumes you have user authentication in place
        //    if (!connectedUsers.ContainsKey(userId))
        //    {
        //        connectedUsers.Add(userId, Context.ConnectionId);
        //    }
        //    return base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    var userId = "1";//Context.UserIdentifier;
        //    if (connectedUsers.ContainsKey(userId))
        //    {
        //        connectedUsers.Remove(userId);
        //    }
        //    return base.OnDisconnectedAsync(exception);
        //}

        //// This method allows sending messages directly to another user
        //public async Task SendMessageToUser(string receiverUserId, string message)
        //{
        //    string senderUserId = "2";//Context.UserIdentifier;

        //    if (connectedUsers.TryGetValue(receiverUserId, out string receiverConnectionId) &&
        //        connectedUsers.TryGetValue(senderUserId, out string senderConnectionId))
        //    {
        //        // Send the message to both the sender and receiver
        //        await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", receiverUserId, message);
        //        await Clients.Client(senderConnectionId).SendAsync("ReceiveMessage", senderUserId, message);
        //    }
        //}
    }
}
