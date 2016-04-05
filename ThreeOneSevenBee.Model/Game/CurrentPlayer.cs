using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if BRIDGE
using Bridge.Html5;
#endif

namespace ThreeOneSevenBee.Model.Game
{
    public class CurrentPlayer : Player
    {
        public CurrentPlayer(string player) : base(player)
        {
            Categories = new List<LevelCategory>();
        }

        public int CurrentCategory;
        public int CurrentLevel;

        public void AddCategory(LevelCategory category)
        {
            category.CategoryIndex = Categories.Count;

            Categories.Add(category);
        }



        public List<LevelCategory> Categories;
    }
}
