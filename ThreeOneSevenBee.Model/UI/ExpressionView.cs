#if BRIDGE
using Bridge.Html5;
#else
using System;
#endif
using ThreeOneSevenBee.Model.Expression;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.UI
{
    public class ExpressionView : CompositeView
    {

        public View Build(ExpressionBase expression, ExpressionModel model)
        {
            Children = new List<View>();
            Children.Add(new ButtonView(expression.ToString(), () => model.Select(expression))
            {
                X = 20,
                Y = 20,
                Width = 100,
                Height = 100
            });
            return this;
        }

        public ExpressionView(ExpressionModel model, double width, double height) : base(width, height)
        {
            Build(model.Expression, model);
            model.OnChanged += (m) => Console.WriteLine(m.Expression.ToString());
        }
    }
}
