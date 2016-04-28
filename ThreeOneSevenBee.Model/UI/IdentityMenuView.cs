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
        public void Update(List<Identity> identities, ExpressionModel model)
        {
            List<View> views = new List<View>();
            double x = 0;
            for (int index = 0; index < identities.Count; index++)
            {
                int indexCopy = index;
                ExpressionView expressionView = new ExpressionView();
                View view = expressionView.BuildView(identities[index].Suggestion, model);
                view.BackgroundColor = new Color(230, 230, 230);
                FrameView frameView = new FrameView(Width / identities.Count, Height, view, 4)
                {
                    PropagateClick = false
                };
                frameView.X = x;
                x += frameView.Width;
                frameView.OnClick = () => model.ApplyIdentity(identities[indexCopy].Result);
                views.Add(frameView);
            }
            Children = views;
        }

        public IdentityMenuView(ExpressionModel model, double width, double height) : base(width, height)
        {
            Update(model.Identities, model);
        }

		public override void Click(double x, double y, IContext context)
        {
			base.Click(x, y, context);
        }
    }
}
