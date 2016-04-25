using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Game
{
    public interface IGameAPI
    {
        void GetPlayers(Action<List<Player>> callback);
        void SaveUserLevelProgress(int levelID, string currentExpression, int stars, Action<bool> callback);
        void GetCurrentPlayer(Action<CurrentPlayer> callback);
    }
}
