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
        public static ExpressionBase ProductToExponentRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count < 2)
            {
                return null;
            }

            VariadicExpression product = expression as VariadicOperatorExpression;

            if (product != null && product.Type == OperatorType.Multiply)
            {
                if (selection.Where((e) => { return selection[0] == e && ReferenceEquals(e.Parent, expression); }).Count() == selection.Count)
                {
                    BinaryExpression suggestion = new BinaryOperatorExpression(selection[0].Clone(), new NumericExpression(selection.Count), OperatorType.Power);
                    return suggestion;
                }
            }
            return null;
        }

        //a^2 = a*a
        public static ExpressionBase ExponentToProductRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
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
                        return new NumericExpression(1);
                    }
                    else if (number == 1)
                    {
                        return exponent.Left.Clone();
                    }
                    else if (number > 1)
                    {
                        VariadicOperatorExpression result = new VariadicOperatorExpression(OperatorType.Multiply, exponent.Left.Clone(), exponent.Left.Clone());
                        for (int i = 2; i < number; i++)
                        {
                            result.Add(exponent.Left.Clone());
                        }
                        return result;
                    }
                }
            }
            return null;
        }


        public static ExpressionBase FractionToProductRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            BinaryExpression fraction = expression as BinaryExpression;
            if (selection.Count == 1
                && ReferenceEquals(selection[0], expression)
                && fraction != null
                && fraction.Type == OperatorType.Divide)
            {
                ExpressionBase exponent = new BinaryOperatorExpression(
                    new DelimiterExpression(fraction.Right.Clone()),
                    new UnaryMinusExpression(new NumericExpression(1)),
                    OperatorType.Power
                );
                ExpressionBase suggestion = new VariadicOperatorExpression(
                    OperatorType.Multiply,
                    new DelimiterExpression(fraction.Left.Clone()),
                    exponent
                );
                return suggestion;
            }
            else
            {
                return null;
            }

        }

        // (15) = 15
        public static ExpressionBase RemoveParenthesisRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            DelimiterExpression parenthesis = expression as DelimiterExpression;
            if(parenthesis != null)
            {
                return parenthesis.Expression;
            }
            return null;
        }

        //2+2+2 = 6, 2*2*2 = 8
        public static ExpressionBase NumericCalculateRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if(expression.CanCalculate() == true)
            {
                int result = (int)expression.Calculate();
                if(result >= 0)
                {
                    return new NumericExpression(result);
                }
                else
                {
                    return new UnaryMinusExpression(new NumericExpression(Math.Abs(result)));
                }
            }
            return null;
        }

        //10-5 = 5, 2^3 = 8
        public static ExpressionBase NumericBinaryRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count < 2)
            {
                return null;
            }

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
                        result = new NumericExpression((int)Math.Pow(numericLeft.Number, numericRight.Number));
                    }
                    else
                    {
                        return null;
                    }
                    if (result.Number >= 0)
                    {
                        return result;
                    }
                    else
                    {
                        result.Number *= -1;
                        UnaryMinusExpression negativeResult = new UnaryMinusExpression(result);
                        return negativeResult;
                    }

                }
            }
            return null;
        }

        // a^-n = 1/a^n
        public static ExpressionBase VariableWithNegativeExponent(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count < 2)
            {
                return null;
            }

            BinaryOperatorExpression binaryexpression = expression as BinaryOperatorExpression;
            if (binaryexpression != null && binaryexpression.Type == OperatorType.Power && ReferenceEquals(selection[0].Parent, binaryexpression) && ReferenceEquals(selection[1].Parent, binaryexpression))
            {
                UnaryMinusExpression unaryexpression = binaryexpression.Right as UnaryMinusExpression;
                if (unaryexpression != null && binaryexpression.Left != null && binaryexpression.Right != null)
                {
                    ExpressionSerializer serializer = new ExpressionSerializer();
                    BinaryExpression power = new BinaryOperatorExpression(binaryexpression.Left.Clone(), unaryexpression.Expression.Clone(), OperatorType.Power);
                    BinaryExpression mysuggestion = new BinaryOperatorExpression(new NumericExpression(1), power, OperatorType.Divide);
                    return mysuggestion;
                }
            }
            return null;
        }

        public static ExpressionBase ReverseVariableWithNegativeExponent(ExpressionBase expression, List<ExpressionBase> selection)
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
                    return mysuggestion;
                }
            }

            return null;
        }

        //a/c + b/c = (a+b)/c
        public static ExpressionBase AddFractionsWithSameNumerators(ExpressionBase expression, List<ExpressionBase> selection)
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
                        return suggestion;
                    }
                }
            }
            return null;
        }

        public static ExpressionBase ExponentProduct(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count < 2)
            {
                return null;
            }

            var variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null && variadicExpression.Type == OperatorType.Multiply)
            {
                var baseSelection = selection[0].Clone();
                var variableIsPowerToOne = new List<NumericExpression>();

                if (selection.All((s) => (s.Parent is BinaryOperatorExpression && ReferenceEquals((s.Parent as BinaryOperatorExpression).Left, s) && ReferenceEquals(s.Parent.Parent, expression)) || ReferenceEquals(s.Parent, expression)))
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
                        return suggestion;
                    }
                }
            }
            return null;
        }

        // a * b + a * c = a * (b + c)
        // a * b - a * c = a * (b - c)
        // a * b + a*c + 3
        public static ExpressionBase ProductParenthesis(ExpressionBase expression, List<ExpressionBase> selection)
        {
            var variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression == null)
            {
                return null;
            }

            if (variadicExpression.Type != OperatorType.Add)
            {
                return null;
            }

            // at least 2 items must be selected or the rule doesn't apply
            if (selection.Count < 2)
            {
                return null;
            }

            // all selected items must be v
            if (!selection.All(e => e is VariableExpression || e is NumericExpression || e is ConstantExpression))
            {
                return null;
            }
            var selectionBase = selection[0].Clone();

            if (selection.Any(s => s != selectionBase))
            {
                return null;
            }
            List<ExpressionBase> list = new List<ExpressionBase>();

            foreach (var selected in selection)
            {
                var selectedParent = selected.Parent.Clone() as VariadicOperatorExpression;
                if (selectedParent != null && selectedParent.Type == OperatorType.Multiply)
                {
                    selectedParent.Remove(selected);
                    if (selectedParent.Count < 2)
                    {
                        list.Add(selectedParent[0]);
                    }
                    else
                    {
                        list.Add(selectedParent);
                    }
                }
                else
                {
                    return null;
                }
            }
            if (list.Count < 2)
            {
                return null;
            }
            VariadicOperatorExpression withinDelimiter = new VariadicOperatorExpression(OperatorType.Add, list[0],
                list[1]);
            if (list.Count > 2)
            {
                foreach (var item in list.Skip((2)))
                {
                    withinDelimiter.Add(item);
                }
            }
            DelimiterExpression DelimiterExpression = new DelimiterExpression(withinDelimiter);
            VariadicOperatorExpression suggestion = new VariadicOperatorExpression(OperatorType.Multiply, selectionBase, DelimiterExpression);
            if (selection.Count == variadicExpression.Count)
            {
                return suggestion;
            }
            return null;
        }

        // a*(b+c) = a*b + a*c   ---  Select a and parenthesis
        //This rule is only activated if there is at least two items in the parenthesis and if the items are added or subtracted
        public static ExpressionBase ReverseProductParenthesis(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count != 2)
            {
                return null;
            }
            var variadicExpression = expression as VariadicOperatorExpression;
            if (!(variadicExpression != null && variadicExpression.Type == OperatorType.Multiply))
            {
                return null;
            }
            if (!(selection[0] is DelimiterExpression || selection[1] is DelimiterExpression))
            {
                return null;
            }
            DelimiterExpression delimiterExpression;
            ExpressionBase notParenthesis;
            if (selection[0] is DelimiterExpression)
            {
                delimiterExpression = selection[0] as DelimiterExpression;
                notParenthesis = selection[1] as ExpressionBase;
            }
            else
            {
                delimiterExpression = selection[1] as DelimiterExpression;
                notParenthesis = selection[0] as ExpressionBase;
            }

            List<ExpressionBase> suggestionList = new List<ExpressionBase>();
            var expressionType = delimiterExpression.Expression as VariadicOperatorExpression;
            if (!(expressionType != null && expressionType.Type == OperatorType.Add))
            {
                return null;
            }

            foreach (var item in expressionType)
            {
                suggestionList.Add(item);
            }

            List<VariadicOperatorExpression> tempList = new List<VariadicOperatorExpression>();

            foreach (var item in suggestionList)
            {
                var temp = new VariadicOperatorExpression(OperatorType.Multiply, notParenthesis.Clone(), item.Clone());
                tempList.Add(temp);
            }

            VariadicOperatorExpression suggestion = new VariadicOperatorExpression(OperatorType.Add, tempList[0], tempList[1]);

            foreach (var item in tempList.Skip(2))
            {
                suggestion.Add(item);
            }
            return suggestion;
        }

        public static ExpressionBase SplittingFractions(ExpressionBase expression, List<ExpressionBase> selection)
        {
            BinaryOperatorExpression binaryExpression = expression as BinaryOperatorExpression;

            if (binaryExpression != null && binaryExpression.Type == OperatorType.Divide)
            {
                VariadicOperatorExpression numerators = binaryExpression.Left.Clone() as VariadicOperatorExpression;

                if (numerators != null && numerators.Type == OperatorType.Add && numerators.Count > 1)
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
                    return suggestion;
                }
            }
            return null;
        }

        public static ExpressionBase CommonPowerParenthesisRule(ExpressionBase expression, List<ExpressionBase> selection)
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
                        else
                        {
                            return null;
                        }
                    }
                    BinaryOperatorExpression suggestion = new BinaryOperatorExpression(new DelimiterExpression(baselist), commonPower, OperatorType.Power);
                    if (variadicExpression.Count == selection.Count)
                    {
                        return suggestion;
                    }
                }
            }
            return null;
        }

        // 6*2 = 3*2*2
        public static ExpressionBase FactorizationRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if (selection.Count != 1)
            {
                return null;
            }

            VariadicOperatorExpression suggestion;

            if (expression is NumericExpression && expression != null)
            {
                var numericExpression = selection[0] as NumericExpression;
                var n = numericExpression.Number;
                for (int count = 2; count < n; count++)
                {
                    if (n % count == 0)
                    {
                        NumericExpression a = new NumericExpression(count);
                        NumericExpression b = new NumericExpression(n / count);

                        suggestion = new VariadicOperatorExpression(OperatorType.Multiply, a, b);

                        return suggestion;
                    }
                }
            }
            return null;
        }

        public static ExpressionBase ReverseCommonPowerParenthesisRule(ExpressionBase expression, List<ExpressionBase> selection)
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
                                return suggestion;
                            }
                            else
                            {
                                return suggestion;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static ExpressionBase ParenthesisPowerRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            BinaryOperatorExpression binaryOperatorExpression = expression as BinaryOperatorExpression;
            if(binaryOperatorExpression != null && binaryOperatorExpression.Type == OperatorType.Power)
            {
                DelimiterExpression delimiterBase = binaryOperatorExpression.Left as DelimiterExpression;
                if(delimiterBase != null)
                {
                    BinaryOperatorExpression binaryBase = delimiterBase.Expression as BinaryOperatorExpression;
                    if(binaryBase != null)
                    {
                        return new BinaryOperatorExpression(binaryBase.Left, new VariadicOperatorExpression(OperatorType.Multiply, binaryBase.Right, binaryOperatorExpression.Right), OperatorType.Power);
                    }
                }
            }
            return null;
        }

        // sqrt(c^4) = (c^4)^1/2
        public static ExpressionBase SquareRootRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            FunctionExpression sqrtExpression = expression as FunctionExpression;

            if (sqrtExpression == null)
            {
                return null;
            }

            if (sqrtExpression.Function != "sqrt")
            {
                return null;
            }

            BinaryOperatorExpression suggestion = new BinaryOperatorExpression(new DelimiterExpression(sqrtExpression.Expression.Clone()), new BinaryOperatorExpression(new NumericExpression(1), new NumericExpression(2), OperatorType.Divide), OperatorType.Power);

            return suggestion;
        }

        // 4 * (a/b) = 4a/b
        //Select constant and vinculum
        public static ExpressionBase ProductOfConstantAndFraction(ExpressionBase expression, List<ExpressionBase> selection)
        {
            var variadicExpression = expression as VariadicOperatorExpression;

            if (variadicExpression == null || variadicExpression.Type != OperatorType.Multiply || variadicExpression.Count != 2)
            {
                return null;
            }

            BinaryOperatorExpression fraction = null;
            ExpressionBase constant = null;

            if ((fraction = variadicExpression[0] as BinaryOperatorExpression) != null && fraction.Type == OperatorType.Divide && ReferenceEquals(variadicExpression[0].Parent, expression))
            {
                constant = variadicExpression[1].GetParentPath().First((e) => ReferenceEquals(e.Parent, expression)).Clone();
            }
            else if((fraction = variadicExpression[1] as BinaryOperatorExpression) != null && fraction.Type == OperatorType.Divide && ReferenceEquals(variadicExpression[1].Parent, expression))
            {
                constant = variadicExpression[0].GetParentPath().First((e) => ReferenceEquals(e.Parent, expression)).Clone();
            }else
            {
                return null;
            }

            VariadicOperatorExpression numerators = new VariadicOperatorExpression(OperatorType.Multiply, fraction.Left, constant);

            BinaryOperatorExpression suggestion = new BinaryOperatorExpression(numerators, fraction.Right, OperatorType.Divide);

            return suggestion;
        }

        // 15/15 = 1 , a/a = 1
        public static ExpressionBase DivisionEqualsOneRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            var fraction = expression as BinaryOperatorExpression;
            if (fraction != null && fraction.Type == OperatorType.Divide)
            {
                if (fraction.Left == fraction.Right)
                {
                    NumericExpression suggestion = new NumericExpression(1);
                    return suggestion;
                }
            }
            return null;
        }

        public static ExpressionBase FactorizeUnaryMinus(ExpressionBase expression, List<ExpressionBase> selection)
        {
            var unaryMinusExpression = expression as UnaryMinusExpression;
            if (unaryMinusExpression != null)
            {
                var numericExpression = new NumericExpression(1);
                var suggestion = new VariadicOperatorExpression(OperatorType.Multiply, new UnaryMinusExpression(numericExpression), unaryMinusExpression.Expression.Clone());
                return suggestion;
            }
            return null;
        }


        // 1 * a = a
        public static ExpressionBase ProductOfOneAndSomethingRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            var variadicExpression = expression as VariadicOperatorExpression;
            NumericExpression one;

            if (variadicExpression != null && variadicExpression.Type == OperatorType.Multiply && variadicExpression.Count == 2)
            {
                if (variadicExpression[0] is NumericExpression)
                {
                    one = variadicExpression[0] as NumericExpression;
                    if (one.Number == 1)
                    {
                        ExpressionBase suggestion = variadicExpression[1].GetParentPath().First((e) => ReferenceEquals(e.Parent, expression));
                        return suggestion.Clone();
                    }
                }
                else if (variadicExpression[1] is NumericExpression)
                {
                    one = variadicExpression[1] as NumericExpression;
                    if (one.Number == 1)
                    {
                        ExpressionBase suggestion = variadicExpression[0].GetParentPath().First((e) => ReferenceEquals(e.Parent, expression)).Clone();
                        return suggestion.Clone();
                    }
                }
                else
                {
                    return null;
                }

                // Der er en bug med power
            }
            return null;
        }

        // a + 4 + 0 + k = a + 4 + k, - Select anything (only one thing other than null) AND null
        public static ExpressionBase RemoveNull(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if(selection.Count != 2)
            {
                return null;
            }

            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;

            if(variadicExpression == null || variadicExpression.Type != OperatorType.Add)
            {
                return null;
            }

            if(variadicExpression[0].ToString() == "0" || variadicExpression[0].ToString() == "-{0}")
            {
                return variadicExpression[1];
            }
            else if(variadicExpression[1].ToString() == "0" || variadicExpression[1].ToString() == "-{0}")
            {
                return variadicExpression[0];
            }
            else
            {
                return null;
            }
        }

        // 2 + 3 * 5 * (-7) * 0 + 1 = 3
        public static ExpressionBase MultiplyByNull(ExpressionBase expression, List<ExpressionBase> selection)
        {
            if(selection.Count < 2)
            {
                return null;
            }

            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;

            if(variadicExpression == null ||variadicExpression.Type != OperatorType.Multiply)
            {
                return null;
            }

            foreach (ExpressionBase item in variadicExpression)
            {
                if(item.ToString() == "0" || item.ToString() == "-{0}")
                {
                    return new NumericExpression(0);
                }
            }
            return null;
        }
    }
}
