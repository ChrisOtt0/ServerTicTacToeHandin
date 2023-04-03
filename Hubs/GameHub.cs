using Microsoft.AspNetCore.SignalR;

namespace ServerTicTacToeHandin.Hubs
{
    public class GameHub : Hub
    {
        public async Task SendMessage(string message)
        {
            Console.WriteLine(message);
            await Clients.All.SendAsync("MessageReceived", message);
        }
    }
}
