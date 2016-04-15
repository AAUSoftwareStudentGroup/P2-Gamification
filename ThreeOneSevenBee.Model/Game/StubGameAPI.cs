using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if BRIDGE
using Bridge.Html5;
#endif

namespace ThreeOneSevenBee.Model.Game
{
    public class StubGameAPI : GameAPI
    {
        CurrentPlayer currentPlayer;

        public override bool Ready
        {
            get
            {
                return true;
            }
        }

        public StubGameAPI()
        {
            //currentPlayer = new CurrentPlayer("Morten");

            //LevelCategory numbers = new LevelCategory("Numbers");
            //currentPlayer.AddCategory(numbers);
            //numbers.Add(new Level("4+(4+5)", "a*a", "4+9", "13"));
            //numbers.Add(new Level("4*6+5", "4*6+5", "24+5", "29"));
            

            //LevelCategory variables = new LevelCategory("Variables");
            //currentPlayer.AddCategory(variables);
            //variables.Add(new Level("a*a*a", "a*a*a", "a^{2+1}", "a^3"));
            //variables.Add(new Level("a^2*a^3", "a^2*a^3", "a^{2+3}", "a^5"));
            

            //currentPlayer.AddCategory(new LevelCategory("Numbers") {
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //    });
            //currentPlayer.AddCategory(new LevelCategory("Variables") {
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //            });
            //currentPlayer.Badges = new List<string> { "FractionBadge", "ExponentBadge" };
            //currentPlayer.CurrentCategory = 0;
            //currentPlayer.CurrentLevel = 0;
        }

        public override void GetCurrentPlayer(Action<CurrentPlayer> callback)
        {
            callback(currentPlayer);
        }

        public override void GetPlayers(Action<List<Player>> callback)
        {
            callback(new List<Player>() {
                currentPlayer,
                new Player("Anton"),
                new Player("Christian"),
                new Player("Lasse"),
                new Player("Mathias P."),
                new Player("Mathias I."),
                new Player("Nikolaj")
            });
        }


        public override void SaveUserLevelProgress(int levelID, string currentExpression, Action<bool> callback)
        {
            throw new NotImplementedException();
        }
    }
}
