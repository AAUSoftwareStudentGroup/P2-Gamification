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

        // Commutative Rule: a + b = b + a
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

        // a^-n = 1/(a)^n
		public static bool InversePowerRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
		{
			OperatorExpression operatorExpression;
			ExpressionSerializer serializer = new ExpressionSerializer();
			identity = null;

			if ((operatorExpression = expression as OperatorExpression) != null)
			{
				if (operatorExpression.Type == OperatorType.Power)
				{
                    if (operatorExpression.Right is UnaryMinusExpression)
                    {
						UnaryMinusExpression power = operatorExpression.Right as UnaryMinusExpression;
                        identity = serializer.Deserialize("1/" + serializer.Serialize(operatorExpression.Left) + "^(" + power.Expression + ")");
						return true;
					}
				}
			}
            return false;
        }

        // a^0 = 1
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

        // a/c + b/c = (a+b)/c
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
                                identity = serializer.Deserialize("(" + serializer.Serialize(lefthand.Left) + "+" + 
                                serializer.Serialize(righthand.Left) + ")/" + serializer.Serialize(lefthand.Right));
                                return true;
                            }
                        }
                    }
                }
            }
            identity = null;
            return false;
        }

        // a/b * c/d = a*c/(b*d)
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
                            identity = serializer.Deserialize(serializer.Serialize(lefthand.Left) + "*" + serializer.Serialize(righthand.Left) + 
                            "/(" + serializer.Serialize(lefthand.Right) + "*" + serializer.Serialize(righthand.Right) + ")");
                            return true;
                        }
                    }
                }
            }
            identity = null;
            return false;
        }

        // (a)^n * (a)^p = a^(n+p)
        public static bool SameVariableDifferentExpMultiplyRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Multiply)
                {
                    OperatorExpression lefthand, righthand;
                    if((lefthand = operatorExpression.Left as OperatorExpression) != null && 
                        (righthand = operatorExpression.Right as OperatorExpression) != null)
                    {
                        if(lefthand.Type == OperatorType.Power && righthand.Type == OperatorType.Power)
                        {
                            if(lefthand.Left == righthand.Left)
                            {
                                // May be missing parenthesis 
                                identity = serializer.Deserialize(serializer.Serialize(lefthand.Left) + "^" + 
                                           serializer.Serialize(lefthand.Right) + "+" + 
                                           serializer.Serialize(righthand.Right));
                                return true;
                            }
                        }
                    }
                }
            }

            identity = null;
            return false;
        }

        // a * b/c = a*b/c
        public static bool FractionVariableMultiplyRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                if(operatorExpression.Type == OperatorType.Multiply)
                {
                    OperatorExpression righthand;
                    // Skal der ikke tjekkes for:  && operatorExpression.Left is VariableExpression i nedenstående?
                    if ((righthand = operatorExpression.Right as OperatorExpression) != null)
                    {
                        if(righthand.Type == OperatorType.Divide)
                        {
                            identity = serializer.Deserialize(serializer.Serialize(operatorExpression.Left) + "*" + serializer.Serialize(righthand.Left) + 
                            "/" + serializer.Serialize(righthand.Left));
                            return true;
                        }
                    }
                }
            }
            identity = null;
            return false;
        }

        // a * (b + c) = a*b + a*c
        public static bool MultiplyVariableIntoParentheses(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                if(operatorExpression.Type == OperatorType.Multiply)
                {
                    OperatorExpression righthand;
                    if ((righthand = operatorExpression.Right as OperatorExpression) != null)
                    {
                        if(righthand.Type == OperatorType.Multiply)
                        {
                            identity = serializer.Deserialize(serializer.Serialize(operatorExpression.Left) + "*" + serializer.Serialize(righthand.Left) + 
                            "+" + serializer.Serialize(operatorExpression.Left) + "*" + serializer.Serialize(righthand.Right));
                            return true;
                        }
                    }
                }
            }
            identity = null;
            return false;
        }

        // (a * b)^n = a^n + b^n
        public static bool PowerOfVariablesMultiplied(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                if(operatorExpression.Type == OperatorType.Power)
                {
                    OperatorExpression lefthand;
                    if ((lefthand = operatorExpression.Right as OperatorExpression) != null)
                    {
                        if(lefthand.Type == OperatorType.Multiply)
                        {
                            identity = serializer.Deserialize(serializer.Serialize(lefthand.Left) + "^" + serializer.Serialize(operatorExpression.Right) + 
                            "+" + serializer.Serialize(lefthand.Right) + "^" + serializer.Serialize(operatorExpression.Right));
                            return true;
                        }
                    }
                }
            }
            identity = null;
            return false;
        }

        // 1 * a = a V a * 1 = a
        public static bool MultiplyingWithOneRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Multiply && operatorExpression.Left.Value.Equals("1"))
                {
                    identity = operatorExpression.Right;
                    return true;
                }
                else if ((operatorExpression.Type == OperatorType.Multiply && operatorExpression.Right.Value.Equals("1")))
                {
                    identity = operatorExpression.Left;
                    return true;
                }
            }
            identity = null;
            return false;
        }

        // b/1 = b
        public static bool DenumeratorIsOneRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Divide)
                {
                    if (operatorExpression.Right.Value.Equals("1"))
                { 
                    identity = operatorExpression.Left;
                    return true;
                }
            }
            }
            identity = null;
            return false;
        }

        // 0/b = 0
        public static bool NumeratorIsZero(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Divide)
                {
                    if (operatorExpression.Left.Value.Equals("0"))
                    {
                        identity = serializer.Deserialize("0");
                        return true;
                    }
                }
            }
            identity = null;
            return false;
        }

        // -a/-b = a/b
        public static bool RemovingUnaryMinusInDivisionRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Divide)
                {
                    if ((operatorExpression.Left is UnaryMinusExpression) && operatorExpression.Right is UnaryMinusExpression)
                    {
                        UnaryMinusExpression terminal = operatorExpression.Left as UnaryMinusExpression;
                        UnaryMinusExpression terminal2 = operatorExpression.Right as UnaryMinusExpression;
                        identity = serializer.Deserialize(serializer.Serialize(terminal.Expression) + "/" + terminal2.Expression);
                        return true;
                    }
                }
            }
            identity = null;
            return false;
        }

        // --b = b
        public static bool DoubleMinusEqualsPlus(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            UnaryMinusExpression operatorExpression;
            UnaryMinusExpression unary1;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as UnaryMinusExpression) != null)
            {
                if ((unary1 = operatorExpression.Expression as UnaryMinusExpression) != null )
                {
                    identity = serializer.Deserialize(serializer.Serialize(unary1.Expression));
                    return true;
                }
            }
            identity = null;
            return false;
        }
    }
}
