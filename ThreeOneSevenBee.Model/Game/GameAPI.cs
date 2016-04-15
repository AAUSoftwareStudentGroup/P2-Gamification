﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Game
{
    public abstract class GameAPI
    {
        public abstract void GetPlayers(Action<List<Player>> callback);
        public abstract void SaveUserLevelProgress(int levelID, string currentExpression, Action<bool> callback);
        public abstract void GetCurrentPlayer(Action<CurrentPlayer> callback);
        public abstract bool Ready { get; }
    }
}
