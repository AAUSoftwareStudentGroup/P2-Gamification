using System;
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

		private void getCategories(Action<List<LevelCategory>> callback)
		{
			WebRequest request = HttpWebRequest.Create("http://webmat.cs.aau.dk/api/?action=get_levels&debug=1");
			request.ContentType = "application/json";
			request.Method = "GET";

			List<LevelCategory> categories = new List<LevelCategory>();

			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
			{
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					string content = reader.ReadToEnd();

					JArray data = JObject.Parse (content).SelectToken ("data") as JArray;
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
								levelsData[i].SelectToken("id").Value<int>(),
								levelsData[i].SelectToken("initial_expression").Value<string>(),
								levelsData[i].SelectToken("initial_expression").Value<string>(),
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
					JToken data = JObject.Parse (content).SelectToken ("data");

					CurrentPlayer currentPlayer = new CurrentPlayer(data.Value<string>("name"));
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
			WebRequest request = HttpWebRequest.Create("http://webmat.cs.aau.dk/api/?action=get_users&debug=1");
			request.ContentType = "application/json";
			request.Method = "GET";

			List<Player> players = new List<Player>();

			using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
			{
				using (StreamReader reader = new StreamReader(response.GetResponseStream()))
				{
					string content = reader.ReadToEnd();
					JArray data = JObject.Parse (content).SelectToken ("data") as JArray;

					for (int i = 0; i < data.Count; i++) {
						players.Add (new Player (data[i].Value<string>("name")));
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