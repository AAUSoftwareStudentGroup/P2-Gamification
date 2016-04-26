using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Game
{
    public interface IGameAPI
    {
        void IsAuthenticated(Action<bool> callback);
        void Authenticate(string username, string password, Action<bool> callback);
        void GetPlayers(Action<List<Player>> callback);
        void SaveUserLevelProgress(int levelID, string currentExpression, int stars, Action<bool> callback);
        void GetCurrentPlayer(Action<CurrentPlayer> callback);
        void UserAddBadge(BadgeName badge, Action<bool> callback);
    }
}
