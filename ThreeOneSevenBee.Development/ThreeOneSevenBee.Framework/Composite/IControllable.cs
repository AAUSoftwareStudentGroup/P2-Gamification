using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Framework.Composite
{
    interface IControllable : IDrawable
    {
        void Click(int x, int y);
    }
}
