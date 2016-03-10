using System;
using System.Collections.Generic;
using ThreeOneSevenBee.Model.Expression.Expressions;
using ThreeOneSevenBee.Model.Expression;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Expression.ExpressionRules
{
    public static class Rules
    {
        public static bool ItselfRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            identity = expression;
            return true;
        }

        public static bool CommutativeRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Add)
                {
                    identity = serializer.Deserialize(serializer.Serialize(operatorExpression.Right) + "+" + serializer.Serialize(operatorExpression.Left));
                    return true;
                }
                else if(operatorExpression.Type == OperatorType.Multiply)
                {
                    identity = serializer.Deserialize(serializer.Serialize(operatorExpression.Right) + "*" + serializer.Serialize(operatorExpression.Left));
                    return true;
                }
            }
            identity = null;
            return false;
        }

		public static bool InversePowerRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
		{
			OperatorExpression operatorExpression;
			ExpressionSerializer serializer = new ExpressionSerializer();
			identity = null;

			if ((operatorExpression = expression as OperatorExpression) != null)
			{
				if (operatorExpression.Type == OperatorType.Power)
				{
					if (operatorExpression.Right is UnaryMinusExpression) {
						UnaryMinusExpression power = operatorExpression.Right as UnaryMinusExpression;
						identity = serializer.Deserialize ("1/" + serializer.Serialize (operatorExpression.Left) + "^(" + power.Expression + ")");
						return true;
					}
				}
			}
			return false;
		}
    }
}
