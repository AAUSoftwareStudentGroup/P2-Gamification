using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Game
{
    public interface IGameAPI
    {
        /// <summary>
        /// Checks if the state is currently authenticated.
        /// </summary>
        /// <param name="callback">A callback to get the result.</param>
        void IsAuthenticated(Action<bool> callback);

        /// <summary>
        /// Logs out to allow another player to connect.
        /// </summary>
        /// <param name="callback">A callback to get the result.</param>
        void logout(Action<bool> callback);

        /// <summary>
        /// Attempts to authenticate.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="callback">A callback to get the result.</param>
        void Authenticate(string username, string password, Action<bool> callback);

        // TODO: Is this correct?
        /// <summary>
        /// Gets a list of players for the leaderboard.
        /// </summary>
        /// <param name="callback">A callback to get the result.</param>
        void GetPlayers(Action<List<Player>> callback);

        /// <summary>
        /// Saves the current levels progress.
        /// </summary>
        /// <param name="levelID"></param>
        /// <param name="currentExpression"></param>
        /// <param name="stars"></param>
        /// <param name="callback">A callback to get the result.</param>
        void SaveUserLevelProgress(int levelID, string currentExpression, int stars, Action<bool> callback);

        /// <summary>
        /// Gets the connected player.
        /// </summary>
        /// <param name="callback">A callback to get the result.</param>
        void GetCurrentPlayer(Action<CurrentPlayer> callback);

        //TODO: Is this correct?
        /// <summary>
        /// Adds a badge to the currently connected player.
        /// </summary>
        /// <param name="badge"></param>
        /// <param name="callback">A callback to get the result.</param>
        void UserAddBadge(BadgeName badge, Action<bool> callback);
    }
}
