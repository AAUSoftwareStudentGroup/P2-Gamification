namespace P2Backend
{
    using System;
    using System.Reflection;
    using System.Collections.Generic; 
    using MySql.Data.MySqlClient;
    using System.Web.Script.Serialization;

    public class Api
    {
        private MySqlConnection db;
        private Dictionary<string, string> requestParams;
        private JavaScriptSerializer json;

        public Api(Dictionary<string, string> request) {
            db = new MySqlConnection("server=antonchristensen.net; database=gamedb; uid=gamedb; pwd=1374;");
            db.Open();
            
            requestParams = request;
            json = new JavaScriptSerializer();
        }

        ~Api() {
            db.Close();
        }

        public string Execute() {
            Type type = this.GetType();
            MethodInfo method = type.GetMethod( GetParam("func"), BindingFlags.NonPublic | BindingFlags.Instance );

            if(method == null)
                return "Error[API]: method not found";
            else {
                try {
                    return method.Invoke(this, new object[0]).ToString();
                }
                catch(TargetInvocationException e) {
                    return e.InnerException.Message;
                }
            }
        }

        private string GetParam(string key) {
            string val = "";
            if(!requestParams.TryGetValue(key, out val)) {
                throw new System.ArgumentException("Error[API]: Missing argument");
            }
            return val;
        }

        private string QueryGetJson(String sql) {

            Dictionary<string, string> values = new Dictionary<string, string>();

            using (var command = new MySqlCommand(sql, db))
            using (var reader = command.ExecuteReader())
            {
                while(reader.Read()) {
                    for(int i = 0; i < reader.FieldCount; i++) {
                       values.Add(reader.GetName(i), reader[i].ToString());
                    }
                }
            }

            return json.Serialize(values);
        }

        private string GetCount() {
            return QueryGetJson("SELECT counter FROM test WHERE id = 1;");
        }   

        private string AddCount() {
            QueryGetJson("UPDATE test SET counter = counter + 1 WHERE id = 1;");
            return GetCount();
        }

        private string GetUser() {
            return QueryGetJson("SELECT user.name, user.password_hash, supervisor.email, class.name AS class_name, school.name AS school_name "+
                                "FROM gamedb.user "+
                                "LEFT JOIN gamedb.supervisor ON user.id = supervisor.user_id "+
                                "LEFT JOIN gamedb.class ON user.class_id = class.id "+
                                "LEFT JOIN gamedb.school ON user.school_id = school.id "+
                                "AND user.id = "+GetParam("id")+";");
        }

        private string GetUsers() {
            return QueryGetJson("SELECT user.name, user.password_hash, supervisor.email, class.name AS class_name, school.name AS school_name "+
                                "FROM gamedb.user "+
                                "LEFT JOIN gamedb.supervisor ON user.id = supervisor.user_id "+
                                "LEFT JOIN gamedb.class ON user.class_id = class.id "+
                                "LEFT JOIN gamedb.school ON user.school_id = school.id;");
        }
    }
}