using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Framework.Composite
{
    class ViewContainer : View<List<IControllable>>
    {   
        public ViewContainer(IContext context) : base(context, new List<IControllable>()) { }

        public override void Draw()
        {
            foreach (IControllable child in _model)
                child.Draw();
        }

        public void Click(int x, int y)
        {
            foreach (IControllable child in _model)
                if (child.X <= x && x < child.X + child.Width && 
                    child.Y <= y && y < child.Y + child.Height)
                    child.Click(x, y);
        }
    }
}
