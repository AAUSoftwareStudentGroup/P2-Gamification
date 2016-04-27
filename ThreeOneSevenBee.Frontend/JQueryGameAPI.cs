using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Game;
using Bridge.jQuery2;
using Bridge;
using Bridge.Html5;

namespace ThreeOneSevenBee.Frontend
{
    class JQueryGameAPI : IGameAPI
    {
        private string token = null;

        private void getCategories(Action<List<LevelCategory>> callback)
        {
            Console.WriteLine(token);
            jQuery.Post(
                "/api/",
                new
                {
                    action = "get_levels",
                    token = token
                },
                (data, textStatus, request) =>
                {
                    var jdata = JSON.Parse((string)data);
                    Console.WriteLine(jdata["data"]);
                    List<LevelCategory> categories = new List<LevelCategory>();
                    var categoriesData = jdata["data"] as object[];
                    foreach (var categoryData in categoriesData)
                    {
                        
                        LevelCategory levelCategory = new LevelCategory((string)categoryData["name"]);
                        var levelsData = categoryData["levels"] as object[];


                        foreach (var levelData in levelsData)
                        {
                            Level level = new Level(
                                int.Parse((string)levelData["id"]),
                                (string)levelData["initial_expression"],
                                int.Parse((string)levelData["stars"] ?? "0"),
                                (string)levelData["current_expression"],
                                (levelData["star_expressions"] as object[]).Select((o) => (string)o).ToArray());
                            levelCategory.Add(level);
                        }
                        categories.Add(levelCategory);
                    }
                    
                    callback(categories);
                }
            );
        }

        public void GetCurrentPlayer(Action<CurrentPlayer> callback)
        {
            jQuery.Post(
                "/api/",
                new
                {
                    action = "get_current_user",
                    token = token
                },
                (data, textStatus, request) =>
                {
                    var jdata = JSON.Parse((string)data);
                    CurrentPlayer currentPlayer = new CurrentPlayer((string)jdata["data"]["name"]);
                    getCategories((categories) =>
                    {
                        foreach (LevelCategory category in categories)
                        {
                            currentPlayer.AddCategory(category);
                        }
                        callback(currentPlayer);
                    });
                }
            );
        }

        public void GetPlayers(Action<List<Player>> callback)
        {
            jQuery.Post(
                "/api/",
                new
                {
                    action = "get_users",
                    token = token
                },
                (data, textStatus, request) =>
                {
                    var jdata = JSON.Parse((string)data);
                    List<Player> result = (jdata["data"] as object[]).Select((s) => new Player((string)s["name"])).ToList();
                    callback(result);
                }
            );
        }

        public void SaveUserLevelProgress(int levelID, string currentExpression, int stars, Action<bool> callback)
        {
            jQuery.Post(
                "/api/",
                new {
                    action = "save_user_level_progress",
                    debug = 1,
                    level_id = levelID,
                    current_expression = currentExpression,
                    stars = stars,
                    token = token
                },
                (data, textStatus, request) =>
                {
                    var jdata = JSON.Parse((string)data);
                    callback((string)jdata["success"] == "true");
                }
            );
        }

        public void IsAuthenticated(Action<bool> callback)
        {
            jQuery.Get(
                "/api/?action=is_authenticated",
                new object(),
                (data, textStatus, request) =>
                {
                    var jdata = JSON.Parse((string)data);
                    bool success = (string)jdata["success"] == "true";
                    token = (string)jdata["success"] == "true" ? (string)jdata["data"]["token"] : null;
                    callback(success);
                }
            );
        }

        public void Authenticate(string username, string password, Action<bool> callback)
        {
            jQuery.Post(
                "/api/",
                new
                {
                    action = "user_login",
                    username = username,
                    password = password
                },
                (data, textStatus, request) =>
                {
                    var jdata = JSON.Parse((string)data);
                    bool success = (string)jdata["success"] == "true";
                    token = (string)jdata["success"] == "true" ? (string)jdata["data"]["token"] : null;
                    callback(success);
                }
            );
        }

        public void UserAddBadge(BadgeName badge, Action<bool> callback)
        {
            jQuery.Post(
                "/api/",
                new
                {
                    action = "user_add_badge",
                    token = token,
                    badge_id = (int)badge
                },
                (data, textStatus, request) =>
                {
                    var jdata = JSON.Parse((string)data);
                    callback((string)jdata["success"] == "true");
                }
            );
        }
    }
}
