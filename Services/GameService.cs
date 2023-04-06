using ServerTicTacToeHandin.Models;

namespace ServerTicTacToeHandin.Services
{
    public class GameService
    {
        public Dictionary<string, Session> GameSessions = new Dictionary<string, Session>();
        public List<int> SessionIds = new List<int>();
    }
}
