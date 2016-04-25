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
        private void getCategories(Action<List<LevelCategory>> callback)
        {
            jQuery.Get(
                "/api/?action=get_levels&debug=1",
                new object(),
                (data, textStatus, request) =>
                {
                    var jdata = JSON.Parse((string)data);
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
            jQuery.Get(
                "/api/?action=get_current_user&debug=1",
                new object(),
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
                    callback(currentPlayer);
                }
            );
        }

        public void GetPlayers(Action<List<Player>> callback)
        {
            jQuery.Get(
                "/api/?action=get_users",
                new object(),
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
                    stars = stars
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
