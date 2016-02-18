using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace www
{
    /// <summary>
    /// Summary description for GameWebService
    /// </summary>
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    
    public class GameWebService : System.Web.Services.WebService
    {
        private static string connectionString = "server=antonchristensen.net; database=gamedb; uid=gamedb; pwd=1374;";

        private static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public int GetCount()
        {
            var connection = GetConnection();
			int count = -1;

            connection.Open();

            using (var command = new MySqlCommand("SELECT * FROM test WHERE id = 1;", connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    count = (int)reader["counter"];
                }
            }

            connection.Close();

            return count;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public int AddCount()
        {
            var connection = GetConnection();

            connection.Open();

            var count = -1;

            using (var command = new MySqlCommand("UPDATE test SET counter = counter + 1 WHERE id = 1;", connection))
            {
                command.ExecuteNonQuery();
            }

			using (var command = new MySqlCommand("SELECT * FROM test WHERE id = 1;", connection))
			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					count = (int)reader["counter"];
				}
			}

            connection.Close();

            return count;
        }
    }
}
