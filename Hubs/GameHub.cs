using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ServerTicTacToeHandin.Models;
using ServerTicTacToeHandin.Services;

namespace ServerTicTacToeHandin.Hubs
{
    public class GameHub : Hub
    {
        GameService _gameService;

        public GameHub(GameService gameService)
        {
            _gameService = gameService;
        }

        public async Task CreateGame(string ownerName, string groupName)
        {
            int id = 0;
            if (_gameService.SessionIds.Count != 0)
                id = _gameService.SessionIds.Max() + 1;

            Session s = new Session(id, ownerName);
            _gameService.SessionIds.Add(id);
            _gameService.GameSessions.Add(groupName, s);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("UpdateGame", JsonConvert.SerializeObject(_gameService.GameSessions[groupName]));
        }

        public async Task JoinGame(string playerName, string groupName)
        {
            _gameService.GameSessions[groupName].Players[1].Name = playerName;
            _gameService.GameSessions[groupName].CalcIsTurn();
            await Clients.Group(groupName).SendAsync("UpdateGame", JsonConvert.SerializeObject(_gameService.GameSessions[groupName]));
        }

        public async Task LeaveGame(int playerId, string groupName)
        {
            string playerName = _gameService.GameSessions[groupName].Players[playerId].Name;

            _gameService.GameSessions.Remove(groupName);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("PlayerLeft", playerName);
        }

        public async Task UpdateGame(string groupName, MoveRequest moveRequest)
        {
            _gameService.GameSessions[groupName].HandleMoveRequest(moveRequest);
            await Clients.Group(groupName).SendAsync("UpdateGame", JsonConvert.SerializeObject(_gameService.GameSessions[groupName]));
        }

        public async Task ResetGame(string groupName)
        {
            _gameService.GameSessions[groupName].Reset();
            _gameService.GameSessions[groupName].CalcIsTurn();
            await Clients.Group(groupName).SendAsync("UpdateGame", JsonConvert.SerializeObject(_gameService.GameSessions[groupName]));
        }

        #region ChatTest
        public async Task SendMessage(string message)
        {
            Console.WriteLine(message);
            await Clients.All.SendAsync("MessageReceived", message);
        }
        #endregion
    }
}
