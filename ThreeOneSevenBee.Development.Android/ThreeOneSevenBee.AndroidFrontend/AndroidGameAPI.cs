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
using RestSharp;

namespace ThreeOneSevenBee.AndroidFrontend
{
    class AndroidGameAPI : IGameAPI
    {
        string token;

        public AndroidGameAPI()
        {
            token = null;
        }

        private void getCategories(Action<List<LevelCategory>> callback)
        {
            List<LevelCategory> categories = new List<LevelCategory>();

            var client = new RestClient("http://webmat.cs.aau.dk/api/");

            var request = new RestRequest(Method.POST);

            request.AddParameter("action", "get_levels");
            request.AddParameter("token", token);

            string data = client.Execute(request).Content;
            Console.WriteLine(data);
            JSONObject json = new JSONObject(data);
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
                        levelsData.GetJSONObject(i).GetInt("stars"),
                        levelsData.GetJSONObject(i).GetString("current_expression"),
                        starExpressions.ToArray());
                    levelCategory.Add(level);
                }
                categories.Add(levelCategory);
            }
            callback(categories);

        }

        public void GetCurrentPlayer(Action<CurrentPlayer> callback)
        {
            var client = new RestClient("http://webmat.cs.aau.dk/api/");

            var request = new RestRequest(Method.POST);

            request.AddParameter("action", "get_current_user");
            request.AddParameter("token", token);

            string data = client.Execute(request).Content;
            Console.WriteLine(data);
            JSONObject json = new JSONObject(data);
            JSONObject jsonData = json.GetJSONObject("data");
            CurrentPlayer currentPlayer = new CurrentPlayer(jsonData.GetString("name"));
            var badges = jsonData.GetJSONArray("badges");
            for (int index = 0; index < badges.Length(); index++)
            {
                string badgeString = badges.GetString(index);
                if (badgeString != "")
                {
                    currentPlayer.Badges.Add((BadgeName)int.Parse(badgeString));
                }
            }
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

        public void GetPlayers(Action<List<Player>> callback)
        {
            List<Player> players = new List<Player>();

            var client = new RestClient("http://webmat.cs.aau.dk/api/");

            var request = new RestRequest(Method.POST);

            request.AddParameter("action", "get_users");
            request.AddParameter("token", token);

            string data = client.Execute(request).Content;
            JSONObject json = new JSONObject(data);
            JSONArray jsonArray = json.GetJSONArray("data");
            for (int index = 0; index < jsonArray.Length(); index++)
            {
                var jsonPlayer = jsonArray.GetJSONObject(index);
                Player player = new Player(jsonPlayer.GetString("name"));
                var badges = jsonPlayer.GetJSONArray("badges");
                for (int badgeIndex = 0; badgeIndex < badges.Length(); badgeIndex++)
                {
                    string badgeString = badges.GetString(badgeIndex);
                    if (badgeString != "")
                    {
                        player.Badges.Add((BadgeName)int.Parse(badgeString));
                    }
                }
                players.Add(player);
            }
            callback(players);

        }

        public void IsAuthenticated(Action<bool> callback)
        {
            var client = new RestClient("http://webmat.cs.aau.dk/api/");

            var request = new RestRequest(Method.POST);

            request.AddParameter("action", "is_authenticated");

            string data = client.Execute(request).Content;
            Console.WriteLine(data);
            JSONObject json = new JSONObject(data);
            bool success = json.GetString("success") == "true";
            if(success)
            {
                token = json.GetJSONObject("data").GetString("token");
            }
            callback(success);
        }

        public void logout(Action<bool> callback)
        {
            var client = new RestClient("http://webmat.cs.aau.dk/api/");

            var request = new RestRequest(Method.POST);

            request.AddParameter("action", "user_logout");
            request.AddParameter("token", token);

            string data = client.Execute(request).Content;
            Console.WriteLine(data);
            JSONObject json = new JSONObject(data);
            bool success = json.GetString("success") == "true";
            callback(success);
        }

        public void Authenticate(string username, string password, Action<bool> callback)
        {
            var client = new RestClient("http://webmat.cs.aau.dk/api/");

            var request = new RestRequest(Method.POST);

            request.AddParameter("action", "user_login");
            request.AddParameter("username", username);
            request.AddParameter("password", password);

            string data = client.Execute(request).Content;
            Console.WriteLine(data);
            JSONObject json = new JSONObject(data);
            token = json.GetJSONObject("data").GetString("token");
            bool success = json.GetString("success") == "true";
            callback(success);
        }

        public void SaveUserLevelProgress(int levelID, string currentExpression, int stars, Action<bool> callback)
        {
            var client = new RestClient("http://webmat.cs.aau.dk/api/");

            var request = new RestRequest(Method.POST);

            request.AddParameter("action", "save_user_level_progress");
            request.AddParameter("token", token);
            request.AddParameter("level_id", levelID.ToString());
            request.AddParameter("current_expression", currentExpression);
            request.AddParameter("stars", stars.ToString());

            string data = client.Execute(request).Content;
            Console.WriteLine(data);
            JSONObject json = new JSONObject(data);
            bool success = json.GetString("success") == "true";
            callback(success);
        }

        public void UserAddBadge(BadgeName badge, Action<bool> callback)
        {
            var client = new RestClient("http://webmat.cs.aau.dk/api/");

            var request = new RestRequest(Method.POST);

            request.AddParameter("action", "user_add_badge");
            request.AddParameter("token", token);
            request.AddParameter("badge_id", (int)badge);

            string data = client.Execute(request).Content;
            Console.WriteLine(data);
            JSONObject json = new JSONObject(data);
            bool success = json.GetString("success") == "true";
            callback(success);
        }
    }
}