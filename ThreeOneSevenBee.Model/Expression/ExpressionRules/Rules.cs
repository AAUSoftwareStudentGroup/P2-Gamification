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
        public static VariadicOperatorExpression InsertSuggestion(List<int> indexes, VariadicOperatorExpression result, ExpressionBase suggestion)
        {
            for (int i = 0; i < indexes.Count; i++)
            {
                result.RemoveAt(indexes[i] - i);
            }
            result.Insert(indexes[0], suggestion);
            return result;
        }

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
                        result = InsertSuggestion(indexes, result, suggestion);
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
                result = InsertSuggestion(indexes, result, new NumericExpression(sum));
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
        //Mangler result
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
                BinaryOperatorExpression firstFraction = selection[0].Clone() as BinaryOperatorExpression;
                BinaryOperatorExpression secondFraction = selection[1].Clone() as BinaryOperatorExpression;

                if (firstFraction != null && secondFraction != null && firstFraction.Type == OperatorType.Divide && secondFraction.Type == OperatorType.Divide)
                {
                    List<ExpressionBase> numeratorList = new List<ExpressionBase>();

                    foreach (ExpressionBase selected in selection)
                    {
                        BinaryOperatorExpression fraction = selected as BinaryOperatorExpression;
                        UnaryMinusExpression minusFraction = fraction.Parent as UnaryMinusExpression;

                        if (fraction != null && ReferenceEquals(fraction.Parent, variadicExpression) && fraction.Right == firstFraction.Right)
                        {
                            numeratorList.Add(fraction.Left.Clone());
                        }
                        else if (minusFraction != null && ReferenceEquals(minusFraction.Parent, variadicExpression) && fraction.Right == firstFraction.Right)
                        {
                            numeratorList.Add(new UnaryMinusExpression(fraction.Left.Clone()));
                        }
                    }
                    VariadicOperatorExpression suggestionNumerator = new VariadicOperatorExpression(OperatorType.Add, numeratorList[0], numeratorList[1]);
                    foreach (var i in numeratorList.Skip(2))
                    {
                        suggestionNumerator.Add(i);
                    }

                    BinaryOperatorExpression suggestion = new BinaryOperatorExpression(suggestionNumerator, firstFraction.Right.Clone(), OperatorType.Divide);
                    
                    if (variadicExpression.Count == selection.Count)
                    {
                        return new Identity(suggestion, suggestion);
                    }
                    else
                    {
                        var list = new List<ExpressionBase>();
                        BinaryOperatorExpression temp = null;
                        for (int i = 0; i < selection.Count; i++)
                        {
                            temp = selection[i] as BinaryOperatorExpression;
                            if (temp != null)
                            {
                                list.Add(temp);
                            }
                        }
                        var indexes = list.Select((s) => variadicExpression.IndexOfReference(s)).Where((i) => i != -1).ToList();
                        indexes.Sort();
                        var result = variadicExpression.Clone() as VariadicOperatorExpression;
                        result = InsertSuggestion(indexes, result, suggestion);
                        return new Identity(suggestion, result);
                    }

                }
            }
            return null;
        }

        public static Identity ExponentProduct(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count < 2)
                return null;
            var variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null && variadicExpression.Type == OperatorType.Multiply)
            {
                var baseSelection = selection[0].Clone();
                var variableIsPowerToOne = new List<NumericExpression>();

                if (selection.All((s) => s.Parent is BinaryOperatorExpression && ReferenceEquals((s.Parent as BinaryOperatorExpression).Left, s) || s.Parent is VariadicOperatorExpression))
                {
                    if (selection.Any((s) =>
                        s.Parent != null &&
                        s.Parent is VariadicOperatorExpression &&
                        s.Parent.Parent is BinaryOperatorExpression &&
                        ReferenceEquals((s.Parent.Parent as BinaryOperatorExpression).Left, s) == false))
                    {
                        return null;
                    }
                    var numeratorList = new List<ExpressionBase>();
                    for (int j = 0; j < selection.Count; j++)
                    {
                        if (baseSelection == selection[j])
                        {
                            var parent = selection[j].Parent as BinaryOperatorExpression;
                            if (parent != null && parent.Type == OperatorType.Power)
                            {
                                if (parent.Right is VariadicOperatorExpression)
                                {
                                    var numbers = (VariadicOperatorExpression)parent.Right.Clone();
                                    if (numbers.Type == OperatorType.Add)
                                    {
                                        for (int i = 0; i < numbers.Count; i++)
                                        {
                                            numeratorList.Add(numbers[i]);
                                        }
                                    }
                                }
                                else
                                {
                                    numeratorList.Add(parent.Right.Clone());
                                }
                            }
                            else if (selection[j].Parent is VariadicOperatorExpression)
                            {
                                numeratorList.Add(new NumericExpression(1));

                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    VariadicOperatorExpression suggestionNumerator = null;
                    suggestionNumerator = new VariadicOperatorExpression(OperatorType.Add, numeratorList[0], numeratorList[1]);

                    foreach (var i in numeratorList.Skip(2))
                    {
                        suggestionNumerator.Add(i);
                    }

                    var suggestion = new BinaryOperatorExpression(baseSelection.Clone(), suggestionNumerator, OperatorType.Power);
                    if (variadicExpression.Count == selection.Count)
                    {
                        return new Identity(suggestion, suggestion);
                    }
                    else
                    {
                        var list = new List<ExpressionBase>();
                        BinaryOperatorExpression temp = null;
                        VariadicOperatorExpression temp2 = null;
                        for (int i = 0; i < selection.Count; i++)
                        {
                            temp = selection[i].Parent as BinaryOperatorExpression;
                            if (temp != null)
                            {
                                list.Add(temp);
                            }
                            else
                            {
                                temp2 = selection[i].Parent as VariadicOperatorExpression;
                                if (temp2 != null)
                                {
                                    list.Add(selection[i]);
                                }
                            }
                        }
                        var indexes = list.Select((s) => variadicExpression.IndexOfReference(s)).Where((i) => i != -1).ToList();
                        indexes.Sort();
                        var result = variadicExpression.Clone() as VariadicOperatorExpression;

                        result = InsertSuggestion(indexes, result, suggestion);
                        return new Identity(suggestion, result);
                    }
                }
            }
            return null;
        }

        // a * b + a * c = a * (b + c)
        public static Identity ProductParenthesis(ExpressionBase expression, List<ExpressionBase> selection)
        {
            var variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression == null)
                return null;

            if (variadicExpression.Type != OperatorType.Add)
                return null;

            // at least 2 items must be selected or the rule doesn't apply
            if (selection.Count < 2)
                return null;

            // all selected items must be variadic multiply or the rule doesn't apply
            if (!selection.All(e => e is VariableExpression || e is NumericExpression))
            {
                return null;
            }
            var selectionBase = selection[0];

            if (selection.All(s => s == selectionBase))
            {

            }
            return null;
        }

        //    variadicExpression = (VariadicOperatorExpression)variadicExpression.Clone();
        //    var rest = new List<ExpressionBase>();
        //    foreach (var op in selection.Cast<VariadicOperatorExpression>())
        //    {
        //        foreach (var subOp in op)
        //        {
        //            if (subOp == common.First())
        //                continue;
        //            rest.Add(subOp);
        //            if (variadicExpression != null)
        //                variadicExpression.Remove(op);
        //}
        //    }

        //    var suggestion = new VariadicOperatorExpression(
        //        OperatorType.Multiply,
        //        common.First(),
        //        new DelimiterExpression(
        //            new VariadicOperatorExpression(
        //                OperatorType.Add,
        //                rest[0], rest[1], rest.Skip(2).ToArray())));

        //    var result = new VariadicOperatorExpression(
        //        OperatorType.Add,
        //        variadicExpression,
        //        suggestion);

        //    return new Identity(suggestion, result);
        //}

        //(a+b)/c = a/c + b/c
        //Selection is the vinculum, it is split into all possible fractions
        public static Identity SplittingFractions(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count != 1)
            {
                return null;
            }
            BinaryOperatorExpression binaryExpression = selection[0] as BinaryOperatorExpression;

            if (binaryExpression != null && binaryExpression.Type == OperatorType.Divide)
            {
                //Prøv at udskrive numerators, for at se hvad de er
                VariadicOperatorExpression numerators = binaryExpression.Left.Clone() as VariadicOperatorExpression;
                if (numerators != null && numerators.Count > 1)
                {
                    List<ExpressionBase> fractionList = new List<ExpressionBase>();
                    foreach (var i in numerators)
                    {
                        fractionList.Add(new BinaryOperatorExpression(i, binaryExpression.Right.Clone(), OperatorType.Divide));
                    }
                    VariadicOperatorExpression suggestion = new VariadicOperatorExpression(OperatorType.Add, fractionList[0], fractionList[1]);
                    foreach (var i in fractionList.Skip(2))
                    {
                        suggestion.Add(i);
                    }
                    //Den returnerer en anderledes rækkefølge, dette skal fikses
                    return new Identity(suggestion, suggestion);
                }
            }
            return null;
        }


        public static Identity CommonPowerParenthesisRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count < 2)
            {
                return null;
            }
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null && variadicExpression.Type == OperatorType.Multiply)
            {
                if (selection.All(s => s.Parent is BinaryOperatorExpression && (s.Parent as BinaryOperatorExpression).Type == OperatorType.Power))
                {
                    ExpressionBase commonPower = null;
                    var selectionOne = selection[0].Parent as BinaryOperatorExpression;
                    var selectionTwo = selection[1].Parent as BinaryOperatorExpression;
                    if (ReferenceEquals(selection[0], selectionOne.Right)
                        && ReferenceEquals(selection[1], selectionTwo.Right))
                    {
                        commonPower = selection[0].Clone();
                    }
                    else { return null; }

                    if (!selection.All((e) => e == commonPower))
                    {
                        return null;
                    }

                    VariadicOperatorExpression baselist = new VariadicOperatorExpression(OperatorType.Multiply, selectionOne.Left.Clone(), selectionTwo.Left.Clone());
                    foreach (var item in selection.Skip(2))
                    {
                        BinaryOperatorExpression selectionParent = item.Parent as BinaryOperatorExpression;
                        if (item == commonPower && ReferenceEquals(selectionParent.Right, item))
                        {
                            baselist.Add(selectionParent.Left.Clone());
                        }
                        else { return null; }
                    }
                    BinaryOperatorExpression suggestion = new BinaryOperatorExpression(new DelimiterExpression(baselist), commonPower, OperatorType.Power);
                    if (variadicExpression.Count == selection.Count)
                    {
                        return new Identity(suggestion, suggestion);
                    }
                    else
                    {
                        var list = new List<ExpressionBase>();
                        BinaryOperatorExpression temp = null;
                        for (int i = 0; i < selection.Count; i++)
                        {
                            temp = selection[i].Parent as BinaryOperatorExpression;
                            if (temp != null)
                            {
                                list.Add(temp);
                            }
                        }
                        var indexes = list.Select((s) => variadicExpression.IndexOfReference(s)).Where((i) => i != -1).ToList();
                        indexes.Sort();
                        var result = variadicExpression.Clone() as VariadicOperatorExpression;
                        result = InsertSuggestion(indexes, result, suggestion);
                        return new Identity(suggestion, result);
                    }
                }
            }
            return null;
        }

        public static Identity ReverseCommonPowerParenthesisRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count != 2)
            {
                return null;
            }
            var binaryExpression = expression as BinaryOperatorExpression;
            List<ExpressionBase> itemsInParenthesis = new List<ExpressionBase>();
            VariadicOperatorExpression suggestion;
            if (binaryExpression != null && binaryExpression.Type == OperatorType.Power)
            {
                var commonparrent = binaryExpression.Right.Clone();
                if (binaryExpression.Left is DelimiterExpression)
                {
                    var delimiterExpression = binaryExpression.Left as DelimiterExpression;
                    if (delimiterExpression.Expression is VariadicOperatorExpression)
                    {
                        var variadicExpression = delimiterExpression.Expression.Clone() as VariadicOperatorExpression;
                        if (variadicExpression.Type == OperatorType.Multiply)
                        {
                            foreach (var item in variadicExpression)
                            {
                                itemsInParenthesis.Add(new BinaryOperatorExpression(item, commonparrent, OperatorType.Power));
                            }
                            suggestion = new VariadicOperatorExpression(OperatorType.Multiply, itemsInParenthesis[0].Clone(), itemsInParenthesis[1].Clone());
                            if (itemsInParenthesis.Count > 2)
                            {
                                foreach (var item in itemsInParenthesis.Skip(2))
                                {
                                    suggestion.Add(item.Clone());
                                }
                                return new Identity(suggestion, suggestion);
                            }
                            else
                            {
                                return new Identity(suggestion, suggestion);
                            }
                        }
                    }
                }
            }
            return null;
        }


    }
}
