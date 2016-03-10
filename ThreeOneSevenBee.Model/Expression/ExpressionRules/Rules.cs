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

        public static bool CommunicativeRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            OperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as OperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Add)
                {
                    identity = serializer.Deserialize(serializer.Serialize(operatorExpression.Right) + "+" + serializer.Serialize(operatorExpression.Left));
                    return true;
                }else if(operatorExpression.Type == OperatorType.Multiply)
                {
                    identity = serializer.Deserialize(serializer.Serialize(operatorExpression.Right) + "*" + serializer.Serialize(operatorExpression.Left));
                    return true;
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
                                identity = serializer.Deserialize("(" + serializer.Serialize(lefthand.Left) + "+" + serializer.Serialize(righthand.Left) + ")/" + serializer.Serialize(lefthand.Right));
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
                            identity = serializer.Deserialize(serializer.Serialize(lefthand.Left) + "*" + serializer.Serialize(righthand.Left) + "/(" + serializer.Serialize(lefthand.Right) + "*" + serializer.Serialize(righthand.Right) + ")");
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
                            identity = serializer.Deserialize(serializer.Serialize(operatorExpression.Left) + "*" + serializer.Serialize(righthand.Left) + "/" + serializer.Serialize(righthand.Left));
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





    }
}
