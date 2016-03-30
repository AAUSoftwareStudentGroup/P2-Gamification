using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeOneSevenBee.Model.Game;

namespace ThreeOneSevenBee.Model.UI
{
    class ProgressbarCircle
    {
        public List<ProgressbarStar> StarLevels;

        public ProgressbarCircle(params ProgressbarStar[] levels)
        {
            StarLevels = new List<ProgressbarStar>(levels);
        }

        public void Add(ProgressbarStar level)
        {
            if (!StarLevels.Contains(level))
            {
                StarLevels.Add(level);
            }
        }

        public void Remove(ProgressbarStar level)
        {
            if (StarLevels.Contains(level))
            {
                StarLevels.Remove(level);
            }
        }
    }
}
