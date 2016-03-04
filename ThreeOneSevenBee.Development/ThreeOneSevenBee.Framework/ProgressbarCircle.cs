using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Framework
{
    class ProgressbarCircle : ProgressbarBase
    {
        public List<ProgressbarStar> StarLevels;

        public ProgressbarCircle(params ProgressbarStar[] levels)
        {
            StarLevels = new List<ProgressbarStar>(levels);
        }

        public void Add(ProgressbarStar level)
        {
            StarLevels.Add(level);
        }
        public void Remove(ProgressbarStar level)
        {
            StarLevels.Remove(level);
        }
    }
}
