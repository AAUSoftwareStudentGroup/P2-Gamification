﻿using System;
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
                    if (numericExpression == null)
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
                    if (ReferenceEquals(selected.Parent, variadicExpression) == false)
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
                if (variadicExpression.Count == selection.Count)
                {
                    return new Identity(new NumericExpression(sum), new NumericExpression(sum));
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
            if (binaryexpression != null && binaryexpression.Type == OperatorType.Power && ReferenceEquals(selection[0].Parent, binaryexpression) && ReferenceEquals(selection[1].Parent, binaryexpression))
            {
                UnaryMinusExpression unaryexpression = binaryexpression.Right as UnaryMinusExpression;
                if (unaryexpression != null && binaryexpression.Left != null && binaryexpression.Right != null)
                {
                    ExpressionSerializer serializer = new ExpressionSerializer();
                    BinaryExpression power = new BinaryOperatorExpression(binaryexpression.Left.Clone(), unaryexpression.Expression.Clone(), OperatorType.Power);
                    BinaryExpression mysuggestion = new BinaryOperatorExpression(new NumericExpression(1), power, OperatorType.Divide);
                    return new Identity(mysuggestion, mysuggestion);
                }
            }
            return null;
        }

        public static Identity ReverseVariableWithNegativeExponent(ExpressionBase expression, List<ExpressionBase> selection)
        {
            BinaryOperatorExpression binaryexpression = expression as BinaryOperatorExpression;
            if (binaryexpression != null && binaryexpression.Type == OperatorType.Divide)
            {
                NumericExpression numericexpression = binaryexpression.Left as NumericExpression;
                BinaryExpression power = binaryexpression.Right as BinaryExpression;
                if (numericexpression != null && numericexpression.Value == "1" && power != null)
                {
                    UnaryMinusExpression unaryminus = new UnaryMinusExpression(power.Right);
                    BinaryExpression mysuggestion = new BinaryOperatorExpression(power.Left.Clone(), unaryminus.Clone(), OperatorType.Power);
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
            if (variadicExpression != null && variadicExpression.Type == OperatorType.Add)
            {
                //Makes a variable for the two first fractions, since it is needed to make a VariadicOperatorExpression later on (it has to take at least two elements).
                BinaryOperatorExpression firstFraction = selection[0] as BinaryOperatorExpression;
                BinaryOperatorExpression secondFraction = selection[1] as BinaryOperatorExpression;

                if (firstFraction != null && secondFraction != null && firstFraction.Type == OperatorType.Divide && secondFraction.Type == OperatorType.Divide)
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

                    VariadicOperatorExpression suggestionNumerator = new VariadicOperatorExpression(OperatorType.Add, firstFraction.Left.Clone(), secondFraction.Left.Clone());
                    foreach (var i in numeratorList.Skip(2))
                    {
                        suggestionNumerator.Add(i);
                    }
                    BinaryOperatorExpression suggestion = new BinaryOperatorExpression(suggestionNumerator, firstFraction.Right.Clone(), OperatorType.Divide);
                    //SHOULD NOT BE SUGGESTION,SUGGESTION, BUT SUGGESTION,RESULT, THIS IS FIXED WHEN THE CORRECT FUNCTION IS IMPLEMENTED!!!!!
                    return new Identity(suggestion, suggestion);
                }
            }
            return null;
        }


        public static Identity ExponentProduct(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count < 2)
                return null;
            VariadicOperatorExpression variadicexpression = expression as VariadicOperatorExpression;
            if (variadicexpression != null && variadicexpression.Type == OperatorType.Multiply)
            {
                ExpressionBase baseSelection = null;
                ExpressionBase firstSelection = null;
                ExpressionBase secondSelection = null;
                baseSelection = selection[0].Clone();
                firstSelection = selection[0].Parent as BinaryOperatorExpression;
                secondSelection = selection[1].Parent as BinaryOperatorExpression;
                VariadicOperatorExpression numbers;
                List<NumericExpression> varialbeIsPowerToOne = new List<NumericExpression>();
                int powerToOneCount = 0;

                if (selection[0].Parent is VariadicOperatorExpression)
                {
                    powerToOneCount++;
                }
                if (selection[1].Parent is VariadicOperatorExpression)
                {
                    powerToOneCount++;
                }
                if (powerToOneCount == 2)
                {
                    return null;
                }
                if ((firstSelection != null && secondSelection != null) || powerToOneCount > 0)
                {

                    List<ExpressionBase> numeratorList = new List<ExpressionBase>();
                    for (int j = 0; j < selection.Count; j++)
                    {
                        if (baseSelection == selection[j])
                        {
                            BinaryOperatorExpression parent = selection[j].Parent as BinaryOperatorExpression;
                            //  ReferenceEquals(parent.Left, selection[j])
                            if (parent != null && parent.Type == OperatorType.Power)
                            {
                                if (parent.Right is NumericExpression)
                                {
                                    numeratorList.Add(parent.Right.Clone());
                                }
                                else if (parent.Right is VariadicOperatorExpression)
                                {
                                    numbers = (VariadicOperatorExpression)parent.Right.Clone();
                                    if (numbers.Type == OperatorType.Add)
                                    {
                                        for (int i = 0; i < numbers.Count; i++)
                                        {
                                            numeratorList.Add(numbers[i]);
                                        }
                                    }
                                }
                            }
                            else if (selection[j].Parent is VariadicOperatorExpression)
                            {
                                varialbeIsPowerToOne.Add(new NumericExpression(1));

                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    VariadicOperatorExpression suggestionNumerator = null;
                    if (selection[0].Parent is BinaryOperatorExpression && selection[1].Parent is BinaryOperatorExpression)
                    {
                        suggestionNumerator = new VariadicOperatorExpression(OperatorType.Add, numeratorList[0], numeratorList[1]);

                        foreach (var i in numeratorList.Skip(2))
                        {
                            suggestionNumerator.Add(i);
                        }

                        foreach (var i in varialbeIsPowerToOne)
                        {
                            suggestionNumerator.Add(i);
                        }

                    }
                    else if ((selection[0].Parent is BinaryOperatorExpression && powerToOneCount == 1) || (selection[1].Parent is BinaryOperatorExpression && powerToOneCount == 1))
                    {
                        suggestionNumerator = new VariadicOperatorExpression(OperatorType.Add, numeratorList[0], varialbeIsPowerToOne[0]);
                        foreach (var i in numeratorList.Skip(1))
                        {
                            suggestionNumerator.Add(i);
                        }
                        foreach (var i in varialbeIsPowerToOne.Skip(1))
                        {
                            suggestionNumerator.Add(i);
                        }
                    }
                    if (varialbeIsPowerToOne.Count == selection.Count)
                    {
                        return null;
                    }
                    BinaryOperatorExpression suggestion = new BinaryOperatorExpression(baseSelection.Clone(), suggestionNumerator, OperatorType.Power);
                    if (variadicexpression.Count == selection.Count)
                    {
                        return new Identity(suggestion, suggestion);
                    }
                    else
                    {
                        List<BinaryOperatorExpression> list = new List<BinaryOperatorExpression>();
                        BinaryOperatorExpression temp = null;
                        List<ExpressionBase> list2 = new List<ExpressionBase>();
                        VariadicOperatorExpression temp2 = null;
                        for (int i = 0; i < selection.Count; i++)
                        {
                            temp = selection[i].Parent as BinaryOperatorExpression;
                            if (temp != null )
                            {
                                list.Add(temp);
                            }
                            else
                            {
                                temp2 = selection[i].Parent as VariadicOperatorExpression;
                                if (temp2 != null)
                                {
                                    list2.Add(selection[i]);
                                }
                            }
                        }

                        var indexes = list.Select((s) => variadicexpression.IndexOfReference(s)).Where((i) => i != -1).ToList();
                        var indexes2 = list2.Select((s) => variadicexpression.IndexOfReference(s)).Where((i) => i != -1).ToList();
                        indexes = indexes.Concat(indexes2).ToList();
                        indexes.Sort();
                        VariadicOperatorExpression result = variadicexpression.Clone() as VariadicOperatorExpression;

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


        //(a+b)/c = a/c + b/c
        // Vælg nævner og tæller man ønsker at rykke ud. Husk at tjekke, at deres common parent er samme (ref equals)
        // Overvej! Hvad hvis man vil rykke flere ud?
        // Suggestion skal vel have ny brøk + gamle brøk
        // Først tjekkes kun hvis man vælger TO!!! tæller!
        public static Identity SplittingFractions(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count < 2)
            {
                return null;
            }
            BinaryOperatorExpression binaryExpression = expression as BinaryOperatorExpression;
            if (binaryExpression != null && binaryExpression.Type == OperatorType.Divide)
            {
                VariadicOperatorExpression numeratorType = binaryExpression.Left as VariadicOperatorExpression;
                if (numeratorType != null && numeratorType.Type == OperatorType.Add)
                {
                    List<ExpressionBase> selectedNumerators = new List<ExpressionBase>();
                    foreach (var selected in selection)
                    {
                        if (!ReferenceEquals(selected.Parent, binaryExpression))
                        {
                            return null;
                        }

                        if (ReferenceEquals(selected.Parent, binaryExpression.Left))
                        {
                            selectedNumerators.Add(selected.Clone());
                        }

                    }

                    if (selectedNumerators.Count < 2)
                    {
                        BinaryOperatorExpression newFraction = new BinaryOperatorExpression(selectedNumerators[0], binaryExpression.Right.Clone(), OperatorType.Divide);


                        //VariadicOperatorExpression result = new VariadicOperatorExpression(OperatorType.Add, newFraction, something);

                        return new Identity(newFraction, newFraction);
                    }
                    else if (selectedNumerators.Count >= 2)
                    {
                        VariadicOperatorExpression listOfNumerators = new VariadicOperatorExpression(OperatorType.Add, selectedNumerators[0], selectedNumerators[1]);
                        foreach (var i in selectedNumerators.Skip(2))
                        {
                            listOfNumerators.Add(i);
                        }

                        BinaryOperatorExpression newFraction = new BinaryOperatorExpression(listOfNumerators, binaryExpression.Right.Clone(), OperatorType.Divide);
                        //Is missing result instead of two times suggestion! The above is missing the same!
                        return new Identity(newFraction, newFraction);
                    }
                }
            }
            return null;
        }


        // a^n * a^p = a^n+p

    }
}
