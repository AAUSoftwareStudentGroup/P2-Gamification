using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;
using ThreeOneSevenBee.Framework.Html;

namespace ThreeOneSevenBee.Framework
{
    public abstract class Game
    {
        private double lastTotalTime;

        protected Game(CanvasElement canvas)
        {
            Canvas = canvas;
            Context2D = canvas.GetContext(CanvasTypes.CanvasContext2DType.CanvasRenderingContext2D);
            Input = new ElementInput(Canvas);
        }

        public bool IsRunning { get; set; }

        public CanvasElement Canvas { get; private set; }

        public CanvasRenderingContext2D Context2D { get; private set; }

        public ElementInput Input { get; private set; }

        public void Run()
        {
            if (IsRunning == true) // == true is needed because of issue #933 in Bridge.Net.
                throw new InvalidOperationException("Game is already running.");
            IsRunning = true;

            Initialize();

            Global.RequestAnimationFrame(OnAnimationFrame);
        }

        private void OnAnimationFrame(double totalTime)
        {
            var deltaTime = totalTime - lastTotalTime;

            Update(deltaTime, totalTime);
            Draw(deltaTime, totalTime);

            lastTotalTime = totalTime;
            if (IsRunning)
                Global.RequestAnimationFrame(OnAnimationFrame);
        }

        public virtual void Initialize() { }

        public virtual void Update(double deltaTime, double totalTime) { }

        public virtual void Draw(double deltaTime, double totalTime) { }
    }
}
