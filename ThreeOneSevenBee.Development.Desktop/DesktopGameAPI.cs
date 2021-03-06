﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ThreeOneSevenBee.Model.Game;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ThreeOneSevenBee.Development.Desktop
{
	class DesktopGameAPI : IGameAPI
	{
		private string token = null;

		public void logout(Action<bool> callback) {
			JObject response = MakeRequest ("http://webmat.cs.aau.dk/api/?action=user_logout", true);
			if (string.Compare (response.Value<string> ("success"), "true") == 0) {
				callback (true);
			}
			else
				callback (false);
		}

		public void IsAuthenticated(Action<bool> callback)
		{
			JObject response = MakeRequest ("http://webmat.cs.aau.dk/api/?action=is_authenticated", true);
			if (string.Compare (response.Value<string> ("success"), "true") == 0) {
				token = response.SelectToken ("data").Value<string>("token");
				callback (true);
			}
			else
				callback (false);
		}

		public void Authenticate(string username, string password, Action<bool> callback)
		{
			JObject response = MakeRequest (
				string.Format (
					"http://webmat.cs.aau.dk/api/?action={0}&username={1}&password={2}",
					"user_login",
					username,
					password
				),
				false
			);
			if (string.Compare (response.Value<string> ("success"), "true") == 0) {
				token = response.SelectToken ("data").Value<string>("token");
				callback (true);
			}
			else
				callback (false);
		}

		public void GetCurrentPlayer(Action<CurrentPlayer> callback)
		{
			JObject response = MakeRequest ("http://webmat.cs.aau.dk/api/?action=get_current_user", true);
			JToken data = response.SelectToken ("data");

			CurrentPlayer currentPlayer = new CurrentPlayer(data.Value<string>("name"));

			JArray badges = data.SelectToken ("badges") as JArray;
			for (int j = 0; j < badges.Count; j++) {
				if (badges[j].Value<string>() != "")
					currentPlayer.Badges.Add ((BadgeName)int.Parse (badges[j].Value<string>()));
			}
			GetCategories((categories) =>
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
			JObject response = MakeRequest ("http://webmat.cs.aau.dk/api/?action=get_users", true);

			List<Player> players = new List<Player>();

			JArray data = response.SelectToken ("data") as JArray;

			for (int i = 0; i < data.Count; i++) {
				Player player = new Player (data[i].Value<string>("name"));
				JArray badges = data [i].SelectToken ("badges") as JArray;
				for (int j = 0; j < badges.Count; j++) {
					if (badges[j].Value<string>() != "")
						player.Badges.Add ((BadgeName)int.Parse (badges[j].Value<string>()));
				}
				players.Add (player);
			}
			callback(players);
		}

		public void SaveUserLevelProgress(int levelID, string currentExpression, int stars, Action<bool> callback)
		{
			JObject response = MakeRequest (
				string.Format(
					"http://webmat.cs.aau.dk/api/?action={0}&level_id={1}&current_expression={2}&stars={3}",
					"save_user_level_progress",
					levelID,
					currentExpression,
					stars
				),
				true
			);

			callback(string.Compare ("true", response.Value<string> ("success")) == 0);
		}

		private void GetCategories(Action<List<LevelCategory>> callback)
		{
			JObject response = MakeRequest ("http://webmat.cs.aau.dk/api/?action=get_levels", true);

			List<LevelCategory> categories = new List<LevelCategory>();

			JArray data = response.SelectToken ("data") as JArray;
			for (int index = 0; index < data.Count; index++)
			{
				LevelCategory levelCategory = new LevelCategory(data[index].Value<string>("name"));
				JArray levelsData = data [index].SelectToken ("levels") as JArray;
				for (int i = 0; i < levelsData.Count; i++)
				{
					List<string> starExpressions = new List<string>();
					JArray starArray = levelsData[i].SelectToken ("star_expressions") as JArray;
					for (int j = 0; j < starArray.Count; j++)
					{
						starExpressions.Add(starArray[j].ToString());
					}
					Level level = new Level(
						int.Parse(levelsData[i].SelectToken("id").Value<string>()),
						levelsData[i].SelectToken("initial_expression").Value<string>(),
						int.Parse(levelsData[i].SelectToken("stars").Value<string>()),
						levelsData[i].SelectToken("initial_expression").Value<string>(),
						starExpressions.ToArray());
					levelCategory.Add(level);
				}
				categories.Add(levelCategory);
			}
			callback(categories);
		}

		public void UserAddBadge(BadgeName badge, Action<bool> callback) {
			JObject response = MakeRequest (
				string.Format (
					"http://webmat.cs.aau.dk/api/?action={0}&badge_id={1}",
					"user_add_badge",
					badge
				),
				true
			);
			callback (string.Compare (response.Value<string> ("success"), "true") == 0);
		}

		private JObject MakeRequest(string URL, bool authed)
		{
			Console.WriteLine ("Request: "+URL);
			WebRequest request = HttpWebRequest.Create(URL+(authed == true ? "&token="+token : ""));
			request.ContentType = "application/json";
			request.Method = "GET";

			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
			{
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					string content = reader.ReadToEnd();
					return JObject.Parse (content);
				}
			}
		}
	}
}