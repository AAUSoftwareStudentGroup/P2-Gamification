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
        //a*a = a^2
        public static Identity ProductToExponentRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count < 2)
                return null;

            VariadicExpression product = expression as VariadicOperatorExpression;

            if (product != null && product.Type == OperatorType.Multiply)
            {

                if (selection.TakeWhile((e) => { return selection[0] == e && ReferenceEquals(e.Parent, expression); }).Count() == selection.Count)
                {
                    BinaryExpression suggestion = new BinaryOperatorExpression(selection[0].Clone(), new NumericExpression(selection.Count), OperatorType.Power);

                    if (product.Count == selection.Count)
                    {
                        return new Identity(suggestion, suggestion);
                    }
                    else
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

        //a^2 = a*a
        public static Identity ExponentToProductRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count != 2)
                return null;

            BinaryOperatorExpression exponent = expression as BinaryOperatorExpression;

            if (exponent != null && exponent.Type == OperatorType.Power)
            {
                if (ReferenceEquals(selection[0].Parent, exponent) && ReferenceEquals(selection[1].Parent, exponent))
                {
                    NumericExpression numericExpression = exponent.Right as NumericExpression;
                    if(numericExpression == null)
                    {
                        return null;
                    }
                    int number = (int)numericExpression.Number;
                    if (number == 0)
                    {
                        return new Identity(new NumericExpression(1), new NumericExpression(1));
                    }
                    else if (number == 1)
                    {
                        return new Identity(exponent.Left.Clone(), exponent.Left.Clone());
                    }
                    else if (number > 1)
                    {
                        VariadicOperatorExpression result = new VariadicOperatorExpression(OperatorType.Multiply, exponent.Left.Clone(), exponent.Left.Clone());
                        for (int i = 2; i < number; i++)
                        {
                            result.Add(exponent.Left.Clone());
                        }
                        return new Identity(result, result);
                    }
                }
            }
            return null;
        }

        //2+2+2 = 6, 2*2*2 = 8
        public static Identity NumericVariadicRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count < 2)
                return null;

            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null)
            {
                int sum;
                Func<int, int, int> operation;

                if (variadicExpression.Type == OperatorType.Multiply)
                {
                    sum = 1;
                    operation = (a, b) => a * b;
                }
                else if (variadicExpression.Type == OperatorType.Add)
                {
                    sum = 0;
                    operation = (a, b) => a + b;
                }
                else
                {
                    return null;
                }
                foreach (ExpressionBase selected in selection)
                {
                    if (!ReferenceEquals(selected.Parent, variadicExpression))
                    {
                        return null;
                    }
                    else
                    {
                        NumericExpression numericExpression = selected as NumericExpression;
                        if (numericExpression != null)
                        {
                            sum = operation(sum, (int)numericExpression.Number);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                var indexes = selection.Select((s) => variadicExpression.IndexOfReference(s)).Where((i) => i != -1).ToList();
                indexes.Sort();
                VariadicOperatorExpression result = variadicExpression.Clone() as VariadicOperatorExpression;
                for (int i = 0; i < indexes.Count; i++)
                {
                    result.RemoveAt(indexes[i] - i);
                }
                result.Insert(indexes[0], new NumericExpression(sum));
                return new Identity(new NumericExpression(sum), result);

            }
            return null;
        }

        //10-5 = 5, 2^3 = 8
        public static Identity NumericBinaryRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count < 2)
                return null;

            BinaryOperatorExpression binaryExpression = expression as BinaryOperatorExpression;
            if (binaryExpression != null && ReferenceEquals(selection[0].Parent, binaryExpression) && ReferenceEquals(selection[1].Parent, binaryExpression))
            {
                NumericExpression numericLeft = binaryExpression.Left as NumericExpression;
                NumericExpression numericRight = binaryExpression.Right as NumericExpression;
                if (numericLeft != null && numericRight != null)
                {
                    NumericExpression result;
                    if (binaryExpression.Type == OperatorType.Subtract)
                    {
                        result = new NumericExpression(numericLeft.Number - numericRight.Number);
                    }
                    else if (binaryExpression.Type == OperatorType.Power)
                    {
                        result = new NumericExpression(Math.Pow(numericLeft.Number, numericRight.Number));
                    }
                    else
                    {
                        return null;
                    }
                    if (result.Number >= 0)
                    {
                        return new Identity(result, result);
                    }
                    else
                    {
                        result.Number *= -1;
                        UnaryMinusExpression positiveResult = new UnaryMinusExpression(result);
                        return new Identity(positiveResult, positiveResult);
                    }
                    
                }
            }
            return null;
        }

        // a^-n = 1/a^n
        public static Identity VariableWithNegativeExponent(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count < 2)
                return null;

            BinaryOperatorExpression binaryexpression = expression as BinaryOperatorExpression;
            if (binaryexpression != null && binaryexpression.Type == OperatorType.Power && selection[0].Parent == binaryexpression && selection[1].Parent == binaryexpression)
            {
                VariableExpression variableexpression = binaryexpression.Left as VariableExpression;
                UnaryMinusExpression unaryexpression = binaryexpression.Right as UnaryMinusExpression;
                NumericExpression numericexpression = unaryexpression.Expression as NumericExpression;
                if (variableexpression != null && unaryexpression != null && numericexpression != null)
                {
                    ExpressionSerializer serializer = new ExpressionSerializer();
                    BinaryExpression power = new BinaryOperatorExpression(variableexpression.Clone(), numericexpression.Clone(), OperatorType.Power);
                    BinaryExpression mysuggestion = new BinaryOperatorExpression(new NumericExpression(1), power, OperatorType.Divide);
                    return new Identity(mysuggestion, mysuggestion);
                }
            }
            return null;
            }

        //a/c + b/c = (a+b)/c
        public static Identity AddFractionsWithSameNumerators(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count < 2)
            {
            return null;
        }

            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null)
            {
                if (variadicExpression.Type == OperatorType.Add)
                {
                    //Makes a variable for the two first fractions, since it is needed to make a VariadicOperatorExpression later on (it has to take at least two elements).
                    BinaryOperatorExpression firstFraction = selection[0] as BinaryOperatorExpression;
                    BinaryOperatorExpression secondFraction = selection[1] as BinaryOperatorExpression;

                    if (firstFraction != null && secondFraction != null &&  firstFraction.Type == OperatorType.Divide && secondFraction.Type == OperatorType.Divide)
                    {
                        List<ExpressionBase> numeratorList = new List<ExpressionBase>();
                        numeratorList.Add(firstFraction.Left.Clone());
                        numeratorList.Add(secondFraction.Left.Clone());

                        foreach (ExpressionBase selected in selection.Skip(2))
                        {
                            BinaryOperatorExpression fraction = selected as BinaryOperatorExpression;
                            if (fraction != null && ReferenceEquals(fraction.Parent, variadicExpression) && fraction.Right == firstFraction.Right)
                            {
                                numeratorList.Add(fraction.Left.Clone());
                            }
                            else
                            {
                                return null;    
                            }
                        }

                        VariadicOperatorExpression suggestionNumerator = new VariadicOperatorExpression(OperatorType.Add, firstFraction, secondFraction);
                        foreach (var i in numeratorList.Skip(2))
                        {
                            suggestionNumerator.Add(i);
                        }
                        BinaryOperatorExpression suggestion = new BinaryOperatorExpression(suggestionNumerator, firstFraction.Right.Clone(), OperatorType.Divide);
                       
                        return new Identity(suggestion, suggestion);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }





        
    }
}
