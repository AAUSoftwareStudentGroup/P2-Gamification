using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Framework
{
    public abstract class ProgressbarBase
    {
        public int MaxProgress { get; protected set; }
        public int CurrentProgress { get; protected set; }
    }
}
