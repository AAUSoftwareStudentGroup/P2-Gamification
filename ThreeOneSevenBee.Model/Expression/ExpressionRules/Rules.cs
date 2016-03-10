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
                else if (operatorExpression.Type == OperatorType.Multiply)
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
						OperatorExpression newDivision = serializer.Deserialize ("1/b") as OperatorExpression;
						OperatorExpression newPower = serializer.Deserialize ("a^b") as OperatorExpression;
						newPower.Left = operatorExpression.Left;
						newPower.Right = power.Expression;
						newDivision.Right = newPower;
						identity = newDivision;
						return true;
					}
				}
			}
            return false;
        }

        public static bool PowerZeroRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Power)
                {
                    if (operatorExpression.Right.Value.Equals("0"))
                    {
                        identity = serializer.Deserialize("1");
                        return true;
                }
            }
            }
            identity = null;
            return false;
        }

        public static bool FractionAddRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if((operatorExpression = expression as OperatorExpression) != null)
            {
                if(operatorExpression.Type == OperatorType.Add)
                {
                    OperatorExpression lefthand, righthand;
                    if ((lefthand = operatorExpression.Left as OperatorExpression) != null &&
                       (righthand = operatorExpression.Right as OperatorExpression) != null)
                    {
                        if(lefthand.Type == OperatorType.Divide && righthand.Type == OperatorType.Divide)
                        {
                            if(lefthand.Right == righthand.Right)
                            {
								OperatorExpression newDivision = serializer.Deserialize ("a/b") as OperatorExpression;
								OperatorExpression newAddition = serializer.Deserialize ("a+b") as OperatorExpression;
								newAddition.Left = lefthand.Left;
								newAddition.Right = righthand.Left;
								newDivision.Left = newAddition;
								newDivision.Right = lefthand.Right;
								identity = newDivision;
                                return true;
                            }
                        }
                    }
                }
            }
            identity = null;
            return false;
        }

        public static bool FractionMultiplyRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Multiply)
                {
                    OperatorExpression lefthand, righthand;
                    if ((lefthand = operatorExpression.Left as OperatorExpression) != null &&
                       (righthand = operatorExpression.Right as OperatorExpression) != null)
                    {
                        if (lefthand.Type == OperatorType.Divide && righthand.Type == OperatorType.Divide)
                        {
							OperatorExpression division = serializer.Deserialize ("a/b") as OperatorExpression;

							division.Left = (serializer.Deserialize(serializer.Serialize (lefthand.Left) + "*" + serializer.Serialize (righthand.Left)));
							division.Right = (serializer.Deserialize(serializer.Serialize (lefthand.Right) + "*" + serializer.Serialize (righthand.Right)));
							identity = division;
							return true;
                        }
                    }
                }
            }
            identity = null;
            return false;
        }

        public static bool FractionVariableMultiplyRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                if(operatorExpression.Type == OperatorType.Multiply)
                {
                    OperatorExpression righthand;
                    if ((righthand = operatorExpression.Right as OperatorExpression) != null && operatorExpression.Left is VariableExpression)
                    {
                        if(righthand.Type == OperatorType.Divide)
                        {
							identity = serializer.Deserialize("a/"+serializer.Serialize(righthand.Left));
							identity.Replace (serializer.Deserialize ("a"), serializer.Deserialize (serializer.Serialize (operatorExpression.Left) + "*" + serializer.Serialize (righthand.Left)));
                            return true;
                        }
                    }
                }
            }
            identity = null;
            return false;
        }

        public static bool MultiplyVariableIntoParentheses(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {

            }


                identity = null;
            return false;
        }

        public static bool MultiplyingWith1Rule(ExpressionBase expression, List<ExpressionBase> seclection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if((operatorExpression = expression as OperatorExpression) != null)
            {
                if(operatorExpression.Type == OperatorType.Multiply && operatorExpression.Left.Value.Equals("1"))
                {
                    identity = operatorExpression.Right;
                    return true;
                }
                else if((operatorExpression.Type == OperatorType.Multiply && operatorExpression.Right.Value.Equals("1")))
                { 
                    identity = operatorExpression.Left;
                    return true;
                }
            }
            identity = null;
            return false;
        }

    }
}
