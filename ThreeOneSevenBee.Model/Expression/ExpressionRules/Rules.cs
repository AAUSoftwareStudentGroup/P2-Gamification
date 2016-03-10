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

        public static bool MultiplyingWith1Rule(ExpressionBase expression, List<ExpressionBase> seclection, out ExpressionBase identity)
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
                        identity = serializer.Deserialize(serializer.Serialize(terminal.Expression) + "/" + terminal.Expression);
                        return true;
                    }
                }
            }
            identity = null;
            return false;
        }
    }
}