using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Game
{
    public class LevelCategory
    {
        public string Name;
        public List<Level> Levels;

        public LevelCategory(string name)
        {
            Name = name;
        }
    }
}
