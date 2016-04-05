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
    class JQueryGameAPI : StubGameAPI
    {
        public override void GetCurrentPlayer(Action<CurrentPlayer> callback)
        {
            base.GetCurrentPlayer(callback);
        }
        public override void GetPlayers(Action<List<Player>> callback)
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
    }
}
