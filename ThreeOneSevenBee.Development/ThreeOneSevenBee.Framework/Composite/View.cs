using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeOneSevenBee.Framework.Composite
{
    abstract class View<T> : IDrawable
    {
        protected T _model;
        protected IContext _context;

        public View(IContext context, T model)
        {
            _model = model;
            _context = context;
        }

        public abstract void Draw();

        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
