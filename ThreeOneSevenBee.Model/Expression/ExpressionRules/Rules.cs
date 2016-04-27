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
            if (fraction != null && fraction.Type == OperatorType.Divide)
            {
                ExpressionBase exponent = new BinaryOperatorExpression(
                    fraction.Right.Clone(),
                    new UnaryMinusExpression(new NumericExpression(1)),
                    OperatorType.Power
                );
                ExpressionBase suggestion = new VariadicOperatorExpression(
                    OperatorType.Multiply,
                    fraction.Left.Clone(),
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
                return parenthesis.Expression.Clone();
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

        // a^-n = 1/a^n
        public static ExpressionBase VariableWithNegativeExponent(ExpressionBase expression, List<ExpressionBase> selection)
        {
            BinaryOperatorExpression binaryexpression = expression as BinaryOperatorExpression;
            if (binaryexpression != null && binaryexpression.Type == OperatorType.Power)
            {
                UnaryMinusExpression unaryexpression = binaryexpression.Right as UnaryMinusExpression;
                if (unaryexpression != null && binaryexpression.Left != null && binaryexpression.Right != null)
                {
                    BinaryExpression power = new BinaryOperatorExpression(binaryexpression.Left.Clone(), unaryexpression.Expression.Clone(), OperatorType.Power);
                    return new BinaryOperatorExpression(new NumericExpression(1), power, OperatorType.Divide);
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
        public static ExpressionBase AddFractionWithCommonDenominatorRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if(variadicExpression != null && variadicExpression.Type == OperatorType.Add)
            {
                BinaryOperatorExpression binaryOperand;
                List<ExpressionBase> terms = new List<ExpressionBase>();
                ExpressionBase commonDenominator = null;
                foreach (ExpressionBase operand in variadicExpression)
                {
                    binaryOperand = operand as BinaryOperatorExpression;
                    if(binaryOperand != null && binaryOperand.Type == OperatorType.Divide)
                    {
                        if(terms.Count == 0)
                        {
                            terms.Add(binaryOperand.Left.Clone());
                            commonDenominator = binaryOperand.Right.Clone();
                        }
                        else if(commonDenominator == binaryOperand.Right)
                        {
                            terms.Add(binaryOperand.Left.Clone());
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
                if(terms.Count > 1)
                {
                    VariadicOperatorExpression SuggestionNumerator = new VariadicOperatorExpression(OperatorType.Add, terms[0], terms[1]);
                    SuggestionNumerator.Add(terms.Skip(2).ToList());
                    return new BinaryOperatorExpression(SuggestionNumerator, commonDenominator, OperatorType.Divide);
                }
            }
            return null;
        }

        public static ExpressionBase ExponentProduct(ExpressionBase expression, List<ExpressionBase> selection)
        {
            VariadicOperatorExpression product = expression as VariadicOperatorExpression;
            if(product != null && product.Type == OperatorType.Multiply)
            {
                List<ExpressionBase> sum = new List<ExpressionBase>();
                ExpressionBase suggestionLeft = null;
                foreach (var operand in product)
                {
                    ExpressionBase operandLeft = operand;
                    ExpressionBase operandRight = new NumericExpression(1);
                    BinaryOperatorExpression power = operand as BinaryOperatorExpression;
                    if (power != null && power.Type == OperatorType.Power)
                    {
                        operandLeft = power.Left;
                        operandRight = power.Right;
                    }
                    if (operandLeft == suggestionLeft || suggestionLeft == null)
                    {
                        suggestionLeft = operandLeft;
                        sum.Add(operandRight.Clone());
                    }
                    else
                    {
                        return null;
                    }
                }
                if(sum.Count > 1)
                {
                    VariadicOperatorExpression suggestionRight = new VariadicOperatorExpression(OperatorType.Add, sum[0], sum[1]);
                    suggestionRight.Add(sum.Skip(2).ToList());
                    return new BinaryOperatorExpression(suggestionLeft, suggestionRight, OperatorType.Power);
                }
            }
            return null;
        }

        // a * b + a * c = a * (b + c)
        // a * b - a * c = a * (b - c)
        // a * b + a*c + 3
        public static ExpressionBase ProductParenthesis(ExpressionBase expression, List<ExpressionBase> selection)
        {
            VariadicOperatorExpression sum = expression as VariadicOperatorExpression;
            if(sum != null && sum.Type == OperatorType.Add)
            {
                List<ExpressionBase> suggestionSum = new List<ExpressionBase>();
                List<ExpressionBase> commonProductOperands = new List<ExpressionBase>();
                foreach (var operand in sum)
                {
                    List<ExpressionBase> selectedProductOperands = new List<ExpressionBase>() { operand };
                    List<ExpressionBase> nonSelectedProductOperands = new List<ExpressionBase>() { };
                    VariadicOperatorExpression product = operand as VariadicOperatorExpression;
                    if (product != null && product.Type == OperatorType.Multiply)
                    {
                        selectedProductOperands = product.Where((o) => (o.Selected == true || o.GetNodesRecursive().Any((n) => n.Selected))).ToList();
                        nonSelectedProductOperands = product.Where((o) => o.Selected == false && o.GetNodesRecursive().Any((n) => n.Selected) == false).ToList();
                    }
                    bool allEqual = true;
                    if (commonProductOperands.Count == 0 || commonProductOperands.Count == selectedProductOperands.Count)
                    {
                        for (int index = 0; index < commonProductOperands.Count; index++)
                        {
                            if (commonProductOperands[index] != selectedProductOperands[index])
                            {
                                allEqual = false;
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                    if (allEqual || commonProductOperands.Count == 0)
                    {
                        commonProductOperands = selectedProductOperands;
                        if (nonSelectedProductOperands.Count == 0)
                        {
                            suggestionSum.Add(new NumericExpression(1));
                        }
                        else if (nonSelectedProductOperands.Count == 1)
                        {
                            suggestionSum.Add(nonSelectedProductOperands[0].Clone());
                        }
                        else
                        {
                            VariadicOperatorExpression nonSelectedProduct = new VariadicOperatorExpression(OperatorType.Multiply, nonSelectedProductOperands[0].Clone(), nonSelectedProductOperands[1].Clone());
                            nonSelectedProduct.Add(nonSelectedProductOperands.Skip(2).Select((o) => o.Clone()).ToList());
                            suggestionSum.Add(nonSelectedProduct);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                VariadicOperatorExpression sumExpression = new VariadicOperatorExpression(OperatorType.Add, suggestionSum[0], suggestionSum[1]);
                sumExpression.Add(suggestionSum.Skip(2).ToList());
                VariadicOperatorExpression suggestion = new VariadicOperatorExpression(OperatorType.Multiply, sumExpression, new NumericExpression(-1));
                suggestion.Add(commonProductOperands.Select((o) => o.Clone()).ToList());
                suggestion.RemoveAt(1);
                return suggestion;
            }
            return null;
        }

        // a*(b+c) = a*b + a*c   ---  Select a and parenthesis
        //This rule is only activated if there is at least two items in the parenthesis and if the items are added or subtracted
        public static ExpressionBase ReverseProductParenthesis(ExpressionBase expression, List<ExpressionBase> selection)
        {
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if(variadicExpression != null && variadicExpression.Type == OperatorType.Multiply && variadicExpression.Count == 2)
            {
                DelimiterExpression delimiterExpression = variadicExpression[1] as DelimiterExpression;
                ExpressionBase other = variadicExpression[0];
                VariadicOperatorExpression variadicContent;
                if(delimiterExpression != null)
                {
                    variadicContent = delimiterExpression.Expression as VariadicOperatorExpression;
                }
                else
                {
                    delimiterExpression = variadicExpression[0] as DelimiterExpression;
                    other = variadicExpression[1];
                    if (delimiterExpression != null)
                    {
                        variadicContent = delimiterExpression.Expression as VariadicOperatorExpression;
                    }else
                    {
                        return null;
                    }
                }
                if(variadicContent != null && variadicContent.Type == OperatorType.Add)
                {
                    VariadicOperatorExpression suggestion = variadicContent.Clone() as VariadicOperatorExpression;
					for (int i = 0; i < suggestion.Count; i++) {
						suggestion[i].Replace(new VariadicOperatorExpression(OperatorType.Multiply, other, suggestion[i].Clone()));
					}
                    return suggestion;
                }
                else
                {
                    return null;
                }
            }
            return null;
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
                    BinaryOperatorExpression suggestion = new BinaryOperatorExpression(baselist, commonPower, OperatorType.Power);
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
                        return new BinaryOperatorExpression(binaryBase.Left.Clone(), new VariadicOperatorExpression(OperatorType.Multiply, binaryBase.Right.Clone(), binaryOperatorExpression.Right.Clone()), OperatorType.Power);
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

            BinaryOperatorExpression suggestion = new BinaryOperatorExpression(sqrtExpression.Expression.Clone(), new BinaryOperatorExpression(new NumericExpression(1), new NumericExpression(2), OperatorType.Divide), OperatorType.Power);

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

            if ((fraction = variadicExpression[0] as BinaryOperatorExpression) != null && fraction.Type == OperatorType.Divide)
            {
                constant = variadicExpression[1];
            }
            else if((fraction = variadicExpression[1] as BinaryOperatorExpression) != null && fraction.Type == OperatorType.Divide)
            {
                constant = variadicExpression[0];
            }else
            {
                return null;
            }

            VariadicOperatorExpression numerators = new VariadicOperatorExpression(OperatorType.Multiply, fraction.Left.Clone(), constant.Clone());

            BinaryOperatorExpression suggestion = new BinaryOperatorExpression(numerators.Clone(), fraction.Right.Clone(), OperatorType.Divide);

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
        public static ExpressionBase MultiplyOneRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if(variadicExpression != null && variadicExpression.Type == OperatorType.Multiply && variadicExpression.Count == 2)
            {
                NumericExpression one = variadicExpression[0] as NumericExpression;
                if(variadicExpression[0].ToString() == "1")
                {
                    return variadicExpression[1].Clone();
                }
                else if(variadicExpression[1].ToString() == "1")
                {
                    return variadicExpression[0].Clone();
                }
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
