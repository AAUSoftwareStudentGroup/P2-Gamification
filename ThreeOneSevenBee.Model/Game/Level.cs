using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Expression;

namespace ThreeOneSevenBee.Model.Game
{
    public class Level
    {
        public string StartExpression;
        public List<string> StarExpressions;
        public string CurrentExpression;
        public string Description;

        public int LevelID;
        public int LevelIndex;
        public int CategoryIndex;
        public int Stars;

        public bool Unlocked;

        //Dictionary over lvl_id and level descriptions
        private Dictionary<int, string> descriptions = new Dictionary<int, string>()
        {
            //Tutorial
            { 109, "For at lægge to, eller flere, elementer sammen,\n skal der trykkes på de elementer,\n  der ønskes sammenlagt. Det samme gælder\n for gange og minus." },
            { 128, "For at udregne potensudtryk, trykkes på\n grundtallet (nederste del) og\n eksponenten (øverste del)." },
            { 129, "For at fjerne et udtryks parentes (hvis det  er muligt),\n  trykkes på parentesen." },
            { 131, "Hvis tæller og nævner i en brøk, har samme værdi,\n  kan det omskrives til 1,\n ved at trykke på brøkstregen." },
            { 88, "For at gange et udtryk ind i en brøk,  trykkes\n på udtrykket og brøkstregen." },
            { 60, "For at lægge to, eller flere, brøker sammen,\n  trykkes der på begge (alle) brøkstreger.\n  For at splitte dem igen, trykkes der igen på brøkstregen." },
            { 147, "For at gange et udtryk med en parentes,\n  trykkes på udtrykket og paretesen." },
            { 112, "Hvis to, eller flere, potensudtryk har samme eksponent,\n  og er ganget sammen, kan der enten trykkes\n på eksponenterne eller på grundtallene." },
            { 56, "Hvis der er ens udtryk i flere led,\n kan man markere de ens variable/tal,\n  for at trække dem uden for en parentes." },

            //Parentes
            { 95, "Parentesen udregnes først,\n derefter ganges resultatet." },
            { 113, "Parentesen udregnes først,\n derefter ganges resultatet." },
            { 136, "Hvis indholdet i en parentes er et produkt\n kan det ophæves hvis det indgår i et produkt." },
            { 118, "Minusparentes skal ophæves,\n derefter ganges (-1) ind i parentesen." },

            //Potenser
            { 89, "Et grundtal opløftet i 1.,\n er altid grundtallet selv: n^1 = n." },
            { 125, "Et grundtal opløftet i 0.,\n er altid 1: n^0 = 1." },
            { 90, "Hvis et grundtal er opløftet i to,\n eller flere, eksponenter,\n  laves det om til én eksponent,\n som består af alle eksponenter ganget\n sammen: (n^2)^3 = n^2*3 = n^6" },
            { 9, "Hvis grundtallet for to potenser ganget sammen er ens,\n kan eksponenterne lægges sammen,\n ved at trykke på begge grundtal,\n eller begge eksponenter." },
            { 138, "For at omskrive en kvadratrod,\n trykkes på kvadratroden. " },
        };

        public Level(string startExpression, string currentExpression, int stars, string[] starExpressions) : this(-1, -1, -1, startExpression, stars, currentExpression, starExpressions)
        { }

        public Level(int levelID, string startExpression, int stars, string currentExpression, string[] starExpressions) : this(levelID, -1, -1, startExpression, stars, currentExpression, starExpressions)
        { }

        public Level(int levelID, int levelIndex, int categoryIndex, string startExpression, int stars, string currentExpression, string[] starExpressions)
        {
            LevelID = levelID;
            Unlocked = false;
            LevelIndex = levelIndex;
            CategoryIndex = categoryIndex;
            StartExpression = startExpression;
            CurrentExpression = currentExpression;

            if (descriptions.ContainsKey(levelID))
            {
                Description = descriptions[levelID];
            }
            else
            {
                Description = "";
            }

            Stars = stars;
            StarExpressions = new List<string>();
            foreach (var star in starExpressions)
            {
                StarExpressions.Add(star);
            }
            for (int n = 0; n < 3 - starExpressions.Count(); n++)
            {
                StarExpressions.Add(starExpressions.Last());
            }
        }
    }
}
