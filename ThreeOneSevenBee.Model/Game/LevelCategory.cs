using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if BRIDGE
using Bridge.Html5;
#endif

namespace ThreeOneSevenBee.Model.Game
{
    public class LevelCategory : IEnumerable<Level>
    {
        public string Name;
        public int categoryIndex;
        public int CategoryIndex {
            get
            { return categoryIndex; }
            set
            {
                categoryIndex = value;
                foreach (Level level in levels)
                {
                    level.CategoryIndex = categoryIndex;
                }
            }
        }
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
