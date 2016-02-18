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

        protected override void OnLoad(EventArgs e)
        {
            Type type = this.GetType();
			MethodInfo method = type.GetMethod( Request.QueryString["func"] );
            
			Response.Write( method.Invoke(this, new object[0]) );

            base.OnLoad(e);
        }
    }
}

