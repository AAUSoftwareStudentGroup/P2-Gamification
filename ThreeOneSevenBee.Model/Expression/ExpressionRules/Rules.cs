using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections.Generic;
using ThreeOneSevenBee.Model.Expression.Expressions;
using ThreeOneSevenBee.Model.Expression;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Expression.ExpressionRules
{
    public static class Rules
    {
        public static Identity ProductToExponentRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            VariadicExpression product = expression as VariadicOperatorExpression;

            if(product != null && product.Type == OperatorType.Multiply)
            {
                
                if(selection.TakeWhile( (e) => { return selection[0] == e && e.Parent == expression; }).Count() == selection.Count)
                {

                    BinaryExpression suggestion = new BinaryOperatorExpression(selection[0].Clone(), new NumericExpression(selection.Count), OperatorType.Power);

                    if (product.Count == selection.Count)
                    {
                        return new Identity(suggestion, suggestion);
                    }else
                    {
                        var indexes = selection.Select((s) => product.IndexOfReference(s)).Where((i) => i != -1).ToList();
                        indexes.Sort();
                        VariadicOperatorExpression result = product.Clone() as VariadicOperatorExpression;
                        for (int i = 0; i < indexes.Count; i++)
                        {
                            result.RemoveAt(indexes[i] - i);
                        }
                        result.Insert(indexes[0], suggestion);
                        return new Identity(suggestion, result);
                    }
                }
            }
            return null;
        }
    }
}
