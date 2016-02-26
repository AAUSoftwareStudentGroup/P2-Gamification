namespace api
{
    using System;
    using System.Reflection;
    using System.Web;
    using System.Web.UI;
    using MySql.Data.MySqlClient;

    public partial class Default : System.Web.UI.Page
    {
        private static string connectionString = "server=antonchristensen.net; database=gamedb; uid=gamedb; pwd=1374;";

        private static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

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

        public int AddCount()
        {
            var connection = GetConnection();

            connection.Open();

            using (var command = new MySqlCommand("UPDATE test SET counter = counter + 1 WHERE id = 1;", connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();

            return GetCount();
        }

        public string GetUser() {
            var connection = GetConnection();

            connection.Open();

            var userid = Request.QueryString["id"];

            string result = "<html><head><meta charset=\"UTF-8\"></head><table><tr><th>Brugernavn</th><th>Kode Hash</th><th>Lære Email</th><th>Klassenavn</th><th>Skolenavn</th></tr>";
            using (var command = new MySqlCommand(
                "SELECT user.name, user.password_hash, supervisor.email, class.name AS class_name, school.name AS school_name "+
                "FROM gamedb.user "+
                "LEFT JOIN gamedb.supervisor ON user.id = supervisor.user_id "+
                "LEFT JOIN gamedb.class ON user.class_id = class.id "+
                "LEFT JOIN gamedb.school ON user.school_id = school.id;", 
                connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result += "<tr>";
                    result += "<td>"+reader["name"]+"</td>";
                    result += "<td>"+reader["password_hash"]+"</td>";
                    result += "<td>"+reader["email"]+"</td>";
                    result += "<td>"+reader["class_name"]+"</td>";
                    result += "<td>"+reader["school_name"]+"</td>";
                    result += "</tr>";
                }
            }
            result += "</table>";

            connection.Close();

            return result;
        }

        protected override void OnLoad(EventArgs e)
        {
            Type type = this.GetType();
			MethodInfo method = type.GetMethod( Request.QueryString["func"] );
            
			Response.Write( method.Invoke(this, new object[0]) );

            base.OnLoad(e);
        }
    }
}

