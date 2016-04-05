using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Game
{
    public class LevelCategory : IEnumerable<Level>
    {
        public string Name;
        public int CategoryIndex;
        private List<Level> levels;

        public LevelCategory(string name)
        {
            Name = name;
            levels = new List<Level>();
        }

        public void Add(Level level)
        {
            level.CategoryIndex = CategoryIndex;
            level.LevelIndex = levels.Count;
            levels.Add(level);
        }

        public IEnumerator<Level> GetEnumerator()
        {
            return levels.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public Level this[int index]
        {
            get
            {
                return levels[index];
            }

            set
            {
                levels[index] = value;
            }
        }

        public int Count
        {
            get { return levels.Count; }
        }

    }
}
