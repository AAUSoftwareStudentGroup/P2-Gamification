#if BRIDGE
using Bridge.Html5;
#else
using System;
#endif
using ThreeOneSevenBee.Model.Expression;
using System.Collections.Generic;
using System.Linq;

namespace ThreeOneSevenBee.Model.UI
{
    public class IdentityMenuView : CompositeView
    {
        public List<View> Build(List<Identity> identities, ExpressionModel model)
        {
            List<View> views = new List<View>();
            double x = 0;
            for (int index = 0; index < identities.Count; index++)
            {
                int indexCopy = index;
                View view = ExpressionView.Build(identities[index].Suggestion, model);
                FrameView frameView = new FrameView(Width / identities.Count, Height, view, 1)
                {
                    PropagateClick = false
                };
                frameView.X = x;
                x += frameView.Width;
                frameView.OnClick = () => model.ApplyIdentity(identities[indexCopy].Result);
                views.Add(frameView);
            }
            return views;
        }

        public IdentityMenuView(ExpressionModel model, double width, double height) : base(width, height)
        {
            Children = Build(model.Identities, model);
            model.OnChanged += (m) => Children = Build(m.Identities, m);
        }

        public override void Click(double x, double y)
        {
            base.Click(x, y);
        }
    }
}
