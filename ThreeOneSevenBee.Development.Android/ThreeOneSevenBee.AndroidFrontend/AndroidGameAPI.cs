using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ThreeOneSevenBee.Model.Game;
using System.Net;
using System.IO;
using Org.Json;

namespace ThreeOneSevenBee.AndroidFrontend
{
    class AndroidGameAPI : IGameAPI
    {

        private void getCategories(Action<List<LevelCategory>> callback)
        {
            var request = HttpWebRequest.Create("http://webmat.cs.aau.dk/api/?action=get_levels&debug=1");
            request.ContentType = "application/json";
            request.Method = "GET";

            List<LevelCategory> categories = new List<LevelCategory>();

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    JSONObject json = new JSONObject(content);
                    JSONArray jsonArray = json.GetJSONArray("data");
                    for (int index = 0; index < jsonArray.Length(); index++)
                    {
                        LevelCategory levelCategory = new LevelCategory(jsonArray.GetJSONObject(index).GetString("name"));
                        var levelsData = jsonArray.GetJSONObject(index).GetJSONArray("levels");
                        for (int i = 0; i < levelsData.Length(); i++)
                        {
                            List<string> starExpressions = new List<string>();
                            var starArray = levelsData.GetJSONObject(i).GetJSONArray("star_expressions");
                            for (int y = 0; y < starArray.Length(); y++)
                            {
                                starExpressions.Add(starArray.GetString(y));
                            }
                            Level level = new Level(
                                levelsData.GetJSONObject(i).GetInt("id"),
                                levelsData.GetJSONObject(i).GetString("initial_expression"),
                                levelsData.GetJSONObject(i).GetString("initial_expression"),
                                starExpressions.ToArray());
                            levelCategory.Add(level);
                        }
                        categories.Add(levelCategory);
                    }
                    callback(categories);
                }
            }
        }

        public void GetCurrentPlayer(Action<CurrentPlayer> callback)
        {
            var request = HttpWebRequest.Create("http://webmat.cs.aau.dk/api/?action=get_current_user&debug=1");
            request.ContentType = "application/json";
            request.Method = "GET";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    JSONObject json = new JSONObject(content);
                    JSONObject jsonData = json.GetJSONObject("data");
                    CurrentPlayer currentPlayer = new CurrentPlayer(jsonData.GetString("name"));
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
            }
        }

        public void GetPlayers(Action<List<Player>> callback)
        {
            var request = HttpWebRequest.Create("http://webmat.cs.aau.dk/api/?action=get_users&debug=1");
            request.ContentType = "application/json";
            request.Method = "GET";

            List<Player> players = new List<Player>();

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    JSONObject json = new JSONObject(content);
                    JSONArray jsonArray = json.GetJSONArray("data");
                    for (int index = 0; index < jsonArray.Length(); index++)
                    {
                        players.Add(new Player(jsonArray.GetJSONObject(index).GetString("name")));
                    }
                    callback(players);
                }
            }
        }

        public void SaveUserLevelProgress(int levelID, string currentExpression, Action<bool> callback)
        {
            callback(true);
        }
    }
}