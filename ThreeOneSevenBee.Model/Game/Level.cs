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

        //Dictionary over lvl_id and level descriptions
        private Dictionary<int, string> descriptions = new Dictionary<int, string>()
        {
            //Tutorial
            { 109, "For at lægge to, eller flere, elementer sammen, skal der trykkes på de elementer, \n der ønskes sammenlagt. Det samme gælder for gange og minus." },
            { 128, "For at udregne potensudtryk, trykkes på grundtallet (nederste del) \n og eksponenten (øverste del)." },
            { 129, "For at fjerne et udtryks parentes (hvis det er muligt), \n trykkes på parentesen." },
            { 131, "Hvis tæller og nævner i en brøk, har samme værdi, \n kan det omskrives til 1, ved at trykke på brøkstregen." },
            { 88, "For at gange et udtryk ind i en brøk, \n trykkes på udtrykket og brøkstregen." },
            { 60, "For at lægge to, eller flere, brøker sammen, \n trykkes der på begge (alle) brøkstreger. \n For at splitte dem igen, trykkes der igen på brøkstregen." },
            { 147, "For at gange et udtryk med en parentes, \n trykkes på udtrykket og paretesen." },
            { 112, "Hvis to, eller flere, potensudtryk har samme eksponent, \n og er ganget sammen, kan der enten trykkes på eksponenterne eller på grundtallene." },
            { 56, "Hvis der er ens udtryk i flere led, \n kan man markere de ens variable/tal, for at trække dem uden for en parentes." },

            //Parentes
            { 95, "Parentesen udregnes først, derefter ganges resultatet." },
            { 113, "Parentesen udregnes først, derefter ganges resultatet." },
            { 136, "Når der kun er gangetegn i en parentes, kan den ophæves." },
            { 118, "Minusparentes skal ophæves, derefter ganges parenteserne. Til sidst udregnes udtrykket." },

            //Potenser
            { 89, "Et grundtal opløftet i 1., er altid grundtallet selv: n^1 = n." },
            { 125, "Et grundtal opløftet i 0., er altid 1: n^0 = 1." },
            { 90, "Hvis et grundtal er opløftet i to, eller flere, eksponenter, \n laves det om til én eksponent, som består af alle eksponenter ganget sammen: (n^2)^3 = n^2*3 = n^6" },
            { 9, "Hvis grundtallet for to potenser ganget sammen er ens, \n kan eksponenterne lægges sammen, ved at trykke på begge grundtal, eller begge eksponenter." },
            { 138, "For at omskrive en kvadratrod, trykkes på kvadratroden. " },
        };

        public Level(string startExpression, string currentExpression, int stars, string[] starExpressions) : this(-1, -1, -1, startExpression, stars, currentExpression, starExpressions)
        { }

        public Level(int levelID, string startExpression, int stars, string currentExpression, string[] starExpressions) : this(levelID, -1, -1, startExpression, stars, currentExpression, starExpressions)
        { }

        public Level(int levelID, int levelIndex, int categoryIndex, string startExpression, int stars, string currentExpression, string[] starExpressions)
        {
            LevelID = levelID;
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
