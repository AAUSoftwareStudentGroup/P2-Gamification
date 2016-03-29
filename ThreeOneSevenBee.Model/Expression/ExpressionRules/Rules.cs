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
        public static bool CheckParentPaths(string checkString, params ExpressionBase[] expressions)
        {
            foreach (var expression in expressions)
            {
                var parentPath = expression.GetParentPath().ToList();
                if (parentPath.Count != checkString.Length) return false;
                for (int index = 0; index < parentPath.Count; index++)
                {
                    var expr = parentPath[index];
                    var c = checkString[index];

                    var numExpr = expr as NumericExpression;
                    if (numExpr != null && c != 'n' && c != 'l') return false;

                    var varExpr = expr as VariableExpression;
                    if (varExpr != null && c != 'v' && c != 'l') return false;

                    var minusExpr = expr as UnaryMinusExpression;
                    if (minusExpr != null && c != '_') return false;

                    var variadicExpr = expr as VariadicOperatorExpression;
                    if (variadicExpr != null)
                    {
                        if (variadicExpr.Type == OperatorType.Add && c != '+') return false;
                        if (variadicExpr.Type == OperatorType.Multiply && c != '*') return false;
                    }

                    var binaryExpr = expr as BinaryOperatorExpression;
                    if (binaryExpr != null)
                    {
                        if (binaryExpr.Type == OperatorType.Divide && c != '/') return false;
                        if (binaryExpr.Type == OperatorType.Power && c != '^') return false;
                        if (binaryExpr.Type == OperatorType.Subtract && c != '-') return false;
                    }

                    var functionExpr = expr as FunctionExpression;
                    if (functionExpr != null && c != 'f') return false;

                    var delimiterExpr = expr as DelimiterExpression;
                    if (delimiterExpr != null && c != 'd') return false;
                }
            }
            return true;
        }

        public static bool AllEqual(List<ExpressionBase> expressions)
        {
            if (expressions.Count < 1)
                return false;
            for (int i = 1; i < expressions.Count; i++)
            {
                if (expressions[i - 1] != expressions[i])
                    return false;
            }
            return true;
        }

        public static bool DivideRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            if (selection.Count > 0 && selection.Count % 2 == 0 && AllEqual(selection) && CheckParentPaths("l*/", selection.ToArray()))
            {
                BinaryExpression fraction = selection[0].Parent.Parent as BinaryExpression;
                VariadicOperatorExpression left = fraction.Left as VariadicOperatorExpression;
                VariadicOperatorExpression right = fraction.Right as VariadicOperatorExpression;
                List<int> leftIndexes = selection.Select(e => left.IndexOfReference(e)).Where(e => e != -1).ToList();
                List<int> rightIndexes = selection.Select(e => right.IndexOfReference(e)).Where(e => e != -1).ToList();
                if (leftIndexes.Count == rightIndexes.Count)
                {
                    leftIndexes.Sort();
                    rightIndexes.Sort();
                    BinaryExpression temp = expression.Clone() as BinaryExpression;
                    VariadicOperatorExpression tempLeft = temp.Left as VariadicOperatorExpression;
                    VariadicOperatorExpression tempRight = temp.Right as VariadicOperatorExpression;
                    for (int index = 0; index < leftIndexes.Count; index++)
                    {
                        tempLeft.RemoveAt(leftIndexes[index] - index);
                    }
                    for (int index = 0; index < rightIndexes.Count; index++)
                    {
                        tempRight.RemoveAt(rightIndexes[index] - index);
                    }
                    identity = temp;
                    return true;
                }
            }
            identity = null;
            return false;

        }

        public static bool ItselfRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            identity = expression.Clone();
            return true;
        }

        public static bool ProductToExponentRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            VariadicOperatorExpression operatorExpression = expression as VariadicOperatorExpression;
            if (operatorExpression != null && selection.Count > 0 && operatorExpression.Type == OperatorType.Multiply)
            {
                for (int index = 0; index < selection.Count; index++)
                {
                    if (ReferenceEquals(selection[index].Parent, operatorExpression) == false)
                    {
                        identity = null;
                        return false;
                    }
                    if (index > 0 && selection[index - 1] != selection[index])
                    {
                        identity = null;
                        return false;
                    }
                }
                List<int> indexes = new List<int>();
                for (int index = 0; index < operatorExpression.Count; index++)
                {
                    for (int selctionIndex = 0; selctionIndex < selection.Count; selctionIndex++)
                    {
                        if (ReferenceEquals(operatorExpression[index], selection[selctionIndex]))
                        {
                            indexes.Add(index);
                        }
                    }
                }
                VariadicOperatorExpression temp = operatorExpression.Clone() as VariadicOperatorExpression;
                for (int index = 1; index < indexes.Count; index++)
                {
                    temp.RemoveAt(indexes[index] - index + 1);
                }

                temp[indexes[0]] = new BinaryOperatorExpression(selection[0].Clone(), new NumericExpression(selection.Count), OperatorType.Power);
                temp[indexes[0]].Parent = temp;
                identity = temp;
                return true;
            }
            identity = null;
            return false;
        }

        public static bool ExponentToProductRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            BinaryExpression binaryExpression = expression as BinaryExpression;
            if (binaryExpression != null && binaryExpression.Type == OperatorType.Power)
            {
                if (selection.Count == 2 &&
                    (selection[0] == binaryExpression.Left && selection[1] == binaryExpression.Right) ||
                    (selection[1] == binaryExpression.Left && selection[0] == binaryExpression.Right))
                {
                    NumericExpression numericExpression = binaryExpression.Right as NumericExpression;
                    if (numericExpression != null)
                    {
                        if (numericExpression.Value == "0")
                        {
                            identity = new NumericExpression(1);
                        }
                        else if (numericExpression.Value == "1")
                        {
                            identity = binaryExpression.Left.Clone();
                        }
                        else
                        {
                            var temp = new VariadicOperatorExpression(OperatorType.Multiply,
                                binaryExpression.Left.Clone(), binaryExpression.Left.Clone());
                            for (int n = 2; n < numericExpression.Number; n++)
                            {
                                temp.Add(binaryExpression.Left.Clone());
                            }
                            identity = temp;
                        }
                        return true;
                    }
                }
            }
            identity = null;
            return false;
        }

        // Commutative Rule: a + b = b + a
        public static bool CommutativeRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            VariadicOperatorExpression operatorExpression = expression as VariadicOperatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if (operatorExpression != null && selection.Count == 2)
            {
                if (operatorExpression.Type == OperatorType.Add || operatorExpression.Type == OperatorType.Multiply)
                {
                    if (selection[0].Parent == expression && selection[1].Parent == expression)
                    {
                        VariadicOperatorExpression temp = operatorExpression.Clone() as VariadicOperatorExpression;
                        for (int index = 0; index < operatorExpression.Count; index++)
                        {
                            if (ReferenceEquals(operatorExpression[index], selection[0]))
                            {
                                temp[index] = selection[1].Clone();
                            }
                            if (ReferenceEquals(operatorExpression[index], selection[1]))
                            {
                                temp[index] = selection[0].Clone();
                            }
                            temp[index].Parent = temp;
                        }
                        identity = temp;
                        return true;
                    }
                }
            }
            identity = null;
            return false;
        }

        // a^-n = 1/(a)^n
        public static bool InversePowerRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            identity = null;

            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Power)
                {
                    if (operatorExpression.Right is UnaryMinusExpression)
                    {
                        UnaryMinusExpression power = operatorExpression.Right as UnaryMinusExpression;
                        BinaryOperatorExpression newDivision = serializer.Deserialize("1/b") as BinaryOperatorExpression;
                        BinaryOperatorExpression newPower = serializer.Deserialize("a^b") as BinaryOperatorExpression;
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

        // a^0 = 1
        public static bool PowerZeroRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
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
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Add)
                {
                    BinaryOperatorExpression lefthand, righthand;
                    if ((lefthand = operatorExpression.Left as BinaryOperatorExpression) != null &&
                       (righthand = operatorExpression.Right as BinaryOperatorExpression) != null)
                    {
                        if (lefthand.Type == OperatorType.Divide && righthand.Type == OperatorType.Divide)
                        {
                            if (lefthand.Right == righthand.Right)
                            {
                                BinaryOperatorExpression newDivision = serializer.Deserialize("a/b") as BinaryOperatorExpression;
                                BinaryOperatorExpression newAddition = serializer.Deserialize("a+b") as BinaryOperatorExpression;
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

        // a/b * c/d = a*c/(b*d)
        public static bool FractionMultiplyRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Multiply)
                {
                    BinaryOperatorExpression lefthand, righthand;
                    if ((lefthand = operatorExpression.Left as BinaryOperatorExpression) != null &&
                       (righthand = operatorExpression.Right as BinaryOperatorExpression) != null)
                    {
                        if (lefthand.Type == OperatorType.Divide && righthand.Type == OperatorType.Divide)
                        {
                            BinaryOperatorExpression division = serializer.Deserialize("a/b") as BinaryOperatorExpression;

                            division.Left = (serializer.Deserialize(serializer.Serialize(lefthand.Left) + "*" + serializer.Serialize(righthand.Left)));
                            division.Right = (serializer.Deserialize(serializer.Serialize(lefthand.Right) + "*" + serializer.Serialize(righthand.Right)));
                            identity = division;
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
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Multiply)
                {
                    BinaryOperatorExpression lefthand, righthand;
                    if ((lefthand = operatorExpression.Left as BinaryOperatorExpression) != null &&
                        (righthand = operatorExpression.Right as BinaryOperatorExpression) != null)
                    {
                        if (lefthand.Type == OperatorType.Power && righthand.Type == OperatorType.Power)
                        {
                            if (lefthand.Left == righthand.Left)
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
        // a^n^n
        public static bool VariableWithTwoExponent(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Power)
                {
                    BinaryOperatorExpression lefthand;
                    if ((lefthand = operatorExpression.Left as BinaryOperatorExpression) != null)
                    {
                        if (lefthand.Type == OperatorType.Power)
                        {
                            identity = serializer.Deserialize(serializer.Serialize(lefthand.Left) + "^" +
                                       serializer.Serialize(lefthand.Right) + "*" +
                                       serializer.Serialize(operatorExpression.Right));
                            return true;
                        }
                    }
                }
            }
            identity = null;
            return false;
        }
        // (a+b)^2 = a^2+b^2+2ab
        public static bool SquareSentenceRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Power && operatorExpression.Right.Value.Equals("2"))
                {
                    BinaryOperatorExpression lefthand;
                    if ((lefthand = operatorExpression.Left as BinaryOperatorExpression) != null)
                    {
                        if (lefthand.Type == OperatorType.Add)
                        {
                            identity = serializer.Deserialize(serializer.Serialize(lefthand.Left) + "^" +
                                       serializer.Serialize(operatorExpression.Right) + "+" +
                                       serializer.Serialize(lefthand.Right) + "^" +
                                       serializer.Serialize(operatorExpression.Right) + "+" +
                                       serializer.Serialize(operatorExpression.Right) + "*" +
                                       serializer.Serialize(lefthand.Left) + "*" +
                                       serializer.Serialize(lefthand.Right));
                            return true;
                        }
                    }
                }
            }
            identity = null;
            return false;
        }
        // sqrt(a^2) = a
        public static bool SquareRootAndPowerRule(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            FunctionExpression functionExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            BinaryOperatorExpression operatorExpression;
            DelimiterExpression delimiterExpression;
            if ((functionExpression = expression as FunctionExpression) != null)
            {
                if (functionExpression.Function == "sqrt")
                {
                    if ((delimiterExpression = functionExpression.Expression as DelimiterExpression) != null)
                    {
                        if ((operatorExpression = delimiterExpression.Expression as BinaryOperatorExpression) != null)
                        {

                            if (operatorExpression.Type == OperatorType.Power && operatorExpression.Right.Value.Equals("2"))
                            {
                                identity = operatorExpression.Left;
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
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Multiply)
                {
                    BinaryOperatorExpression righthand;
                    // Skal der ikke tjekkes for:  && operatorExpression.Left is VariableExpression i nedenstående?
                    if ((righthand = operatorExpression.Right as BinaryOperatorExpression) != null)
                    {
                        if (righthand.Type == OperatorType.Divide)
                        {
                            identity = serializer.Deserialize("a/" + serializer.Serialize(righthand.Left));
                            identity.Replace(serializer.Deserialize("a"), serializer.Deserialize(serializer.Serialize(operatorExpression.Left) + "*" + serializer.Serialize(righthand.Left)));
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
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Multiply)
                {
                    BinaryOperatorExpression righthand;
                    if ((righthand = operatorExpression.Right as BinaryOperatorExpression) != null)
                    {
                        if (righthand.Type == OperatorType.Multiply)
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
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
            {
                if (operatorExpression.Type == OperatorType.Power)
                {
                    BinaryOperatorExpression lefthand;
                    if ((lefthand = operatorExpression.Right as BinaryOperatorExpression) != null)
                    {
                        if (lefthand.Type == OperatorType.Multiply)
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
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
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
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
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
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
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
            BinaryOperatorExpression operatorExpression;
            ExpressionSerializer serializer = new ExpressionSerializer();
            if ((operatorExpression = expression as BinaryOperatorExpression) != null)
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
                if ((unary1 = operatorExpression.Expression as UnaryMinusExpression) != null)
                {
                    identity = serializer.Deserialize(serializer.Serialize(unary1.Expression));
                    return true;
                }
            }
            identity = null;
            return false;
        }

        /*
        // abc / (cda) = b/d
        public static bool DivisionWithSelf(ExpressionBase expression, List<ExpressionBase> selection, out ExpressionBase identity)
        {
            // Husk at optimere, dvs. flyt ting til deres mindste scope!
            BinaryOperatorExpression operatorExpression = expression as BinaryOperatorExpression;
            var left = operatorExpression.Left as VariadicOperatorExpression;
            var right = operatorExpression.Right as VariadicOperatorExpression;

            if (operatorExpression != null && selection.Count > 1)
            {
                if(operatorExpression.Type == OperatorType.Divide && left != null && right != null)
                {
                    //sammenlign de to lister left og right
                    for (int i = 0; i < left.Count; i++)
                    {
                        for (int j = 0; j < right.Count; j++)
                        {
                            if (left[i].Equals(right[j]))
                            {
                                left.RemoveAt(i);
                                i--;
                                right.RemoveAt(j);
                                break;
                            }
                        }
                    }
                    ExpressionSerializer serializer = new ExpressionSerializer();
                    if (right.Count != 0 && left.Count != 0)
                    {
                        identity = null;
                        return false;
                    }
                    //hvis de er ens, så fjern dem, hvis expression == selection, skriv 1.
                    else if(left == null && right == null)
                    {
                        identity = serializer.Deserialize("1");
                        return true;
                    }
                    else
                    {
                        identity = serializer.Deserialize(left + "/" + right);
                        return true;
                    }
                }
            }
            identity = null;
            return false;
        }
        */
    }
}
