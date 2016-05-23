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

        //a*a*...*a = a^n where n is an integer.
        public static ExpressionBase ProductToExponentRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a product.
            VariadicExpression product = expression as VariadicOperatorExpression;
            if (product != null && product.Type == OperatorType.Multiply)
            {
                //If all operands are equal.
                if (product.Where((operand) => product[0] == operand).Count() == product.Count)
                {
                    //Return power.
                    BinaryExpression suggestion = new BinaryOperatorExpression(product[0].Clone(), new NumericExpression(product.Count), OperatorType.Power);
                    return suggestion;
                }
            }
            return null;
        }

        //a^n = a*a*...*a where n is an integer.
        public static ExpressionBase ExponentToProductRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a power.
            BinaryOperatorExpression exponent = expression as BinaryOperatorExpression;
            if (exponent != null && exponent.Type == OperatorType.Power)
            {
                //If exponent is a number.
                NumericExpression numericExpression = exponent.Right as NumericExpression;
                if (numericExpression != null)
                {
                    int number = (int)numericExpression.Number;
                    if (number == 0)
                    {
                        //a^0 = 1
                        return new NumericExpression(1);
                    }
                    else if (number == 1)
                    {
                        //a^1 = a
                        return exponent.Left.Clone();
                    }
                    else if (number > 1)
                    {
                        //a^n = a*a*..*a
                        VariadicOperatorExpression result = new VariadicOperatorExpression(OperatorType.Multiply, exponent.Left.Clone(), exponent.Left.Clone());
                        for (int i = 2; i < number; i++)
                        {
                            result.Add(exponent.Left.Clone());
                        }
                        return result;
                    }
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        //a/b = a*b^-1
        public static ExpressionBase FractionToProductRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression a fraction. e.g. a/b
            BinaryExpression fraction = expression as BinaryExpression;
            if (fraction != null && fraction.Type == OperatorType.Divide)
            {
                //Convert denominator to power of -1. b => b^-1
                ExpressionBase exponent = new BinaryOperatorExpression(
                    fraction.Right.Clone(),
                    new UnaryMinusExpression(new NumericExpression(1)),
                    OperatorType.Power
                );
                //Return product of numerator and power. a*b^-1
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

        //(15) = 15
        public static ExpressionBase RemoveParenthesisRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is parenthesis
            DelimiterExpression parenthesis = expression as DelimiterExpression;
            if (parenthesis != null)
            {
                //return content of parenthesis
                return parenthesis.Expression.Clone();
            }
            return null;
        }

        //a^-n = 1/a^n
        public static ExpressionBase VariableWithNegativeExponent(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a power. e.g. a^-n
            BinaryOperatorExpression binaryexpression = expression as BinaryOperatorExpression;
            if (binaryexpression != null && binaryexpression.Type == OperatorType.Power)
            {
                //If exponent of power is unary minus. e.g. -n
                UnaryMinusExpression unaryexpression = binaryexpression.Right as UnaryMinusExpression;
                if (unaryexpression != null && binaryexpression.Left != null && binaryexpression.Right != null)
                {
                    //Return reciprocal of power with unary minus removed in exponent. e.g. 1/a^n
                    BinaryExpression power = new BinaryOperatorExpression(binaryexpression.Left.Clone(), unaryexpression.Expression.Clone(), OperatorType.Power);
                    return new BinaryOperatorExpression(new NumericExpression(1), power, OperatorType.Divide);
                }
            }
            return null;
        }

        //1/a^n = a^-n
        public static ExpressionBase ReverseVariableWithNegativeExponent(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a fraction. e.g. 1/a^n
            BinaryOperatorExpression binaryexpression = expression as BinaryOperatorExpression;
            if (binaryexpression != null && binaryexpression.Type == OperatorType.Divide)
            {
                //If numerator is 1 and denominator is power. e.g. 1 and a^n
                NumericExpression numericexpression = binaryexpression.Left as NumericExpression;
                BinaryExpression power = binaryexpression.Right as BinaryExpression;
                if (numericexpression != null && numericexpression.Value == "1" && power != null && power.Type == OperatorType.Power)
                {
                    //return power with minus exponent. e.g. a^-n
                    UnaryMinusExpression unaryminus = new UnaryMinusExpression(power.Right.Clone());
                    BinaryExpression mysuggestion = new BinaryOperatorExpression(power.Left.Clone(), unaryminus.Clone(), OperatorType.Power);
                    return mysuggestion;
                }
            }

            return null;
        }

        //a/c + b/c = (a+b)/c
        public static ExpressionBase AddFractionWithCommonDenominatorRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a sum.
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null && variadicExpression.Type == OperatorType.Add)
            {
                BinaryOperatorExpression binaryOperand;
                List<ExpressionBase> terms = new List<ExpressionBase>();
                ExpressionBase commonDenominator = null;

                //Foreach operand in sum.
                foreach (ExpressionBase operand in variadicExpression)
                {
                    //If operand is a fraction.
                    binaryOperand = operand as BinaryOperatorExpression;
                    if (binaryOperand != null && binaryOperand.Type == OperatorType.Divide)
                    {
                        //If operand is the first fraction in sum
                        if (terms.Count == 0)
                        {
                            //Add numerator to list of numerators.
                            terms.Add(binaryOperand.Left.Clone());

                            //Set commonDenominator to operand's denominator.
                            commonDenominator = binaryOperand.Right.Clone();
                        }
                        else if (commonDenominator == binaryOperand.Right)
                        {
                            //Add fraction's numerator to list if its denominator equals commenDenominator.
                            terms.Add(binaryOperand.Left.Clone());
                        }
                        else
                        {
                            //Return null if an fraction's denominator does not equals commenDenominator.
                            return null;
                        }
                    }
                    else
                    {
                        //Return if operand is not a fraction.
                        return null;
                    }
                }
                if (terms.Count > 1)
                {
                    //Initialize sum of all fractions' numerators.
                    VariadicOperatorExpression SuggestionNumerator = new VariadicOperatorExpression(OperatorType.Add, terms[0], terms[1]);
                    SuggestionNumerator.Add(terms.Skip(2).ToList());

                    //Return fraction with sum of all fractions' numerators and with their common denominator.
                    return new BinaryOperatorExpression(SuggestionNumerator, commonDenominator, OperatorType.Divide);
                }
            }
            return null;
        }

        //a^n*a^m = a^{n+m}
        public static ExpressionBase ExponentProduct(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a product. e.g. a*a^2
            VariadicOperatorExpression product = expression as VariadicOperatorExpression;
            if (product != null && product.Type == OperatorType.Multiply)
            {
                List<ExpressionBase> sum = new List<ExpressionBase>();
                ExpressionBase commonBase = null;

                //Foreach operand in product
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
                    //If operand's base equals commonBase.
                    if (operandLeft == commonBase || commonBase == null)
                    {
                        //Add operands exponent to sum.
                        commonBase = operandLeft;
                        sum.Add(operandRight.Clone());
                    }
                    else
                    {
                        return null;
                    }
                }
                if (sum.Count > 1)
                {
                    //Return power with common base and sum of all operands' exponents.
                    VariadicOperatorExpression suggestionRight = new VariadicOperatorExpression(OperatorType.Add, sum[0], sum[1]);
                    suggestionRight.Add(sum.Skip(2).ToList());
                    return new BinaryOperatorExpression(commonBase, suggestionRight, OperatorType.Power);
                }
            }
            return null;
        }

        //a*b+a*c = (b+c)*a
        //a*b-a*c = (b-c)*a
        public static ExpressionBase ProductParenthesis(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a sum. e.g. a*b*c - a*b*d
            VariadicOperatorExpression sum = expression as VariadicOperatorExpression;
            if (sum != null && sum.Type == OperatorType.Add)
            {
                List<ExpressionBase> suggestionSum = new List<ExpressionBase>();
                List<ExpressionBase> commonProductOperands = new List<ExpressionBase>();
                //Foreach operand in sum e.g. a*b*c and a*b*d
                foreach (var actualOperand in sum)
                {
                    ExpressionBase operand;
                    bool negative;

                    //If operand is unary minus take its operand and set negative = true. e.g. -a*b*d => operand = a*b*d, negative = true
                    UnaryMinusExpression minus = actualOperand as UnaryMinusExpression;
                    if (minus != null)
                    {
                        operand = minus.Expression;
                        negative = true;
                    }
                    else
                    {
                        operand = actualOperand;
                        negative = false;
                    }
                    List<ExpressionBase> selectedProductOperands = new List<ExpressionBase>() { operand };
                    List<ExpressionBase> nonSelectedProductOperands = new List<ExpressionBase>() { };
                    VariadicOperatorExpression product = operand as VariadicOperatorExpression;
                    //If operand in the sum is a product. e.g. a*b*d
                    if (product != null && product.Type == OperatorType.Multiply)
                    {
                        //Store all selected and nonselected operands in the product.
                        selectedProductOperands = product.Where((o) => (o.Selected == true || o.GetNodesRecursive().Any((n) => n.Selected))).ToList();
                        nonSelectedProductOperands = product.Where((o) => o.Selected == false && o.GetNodesRecursive().Any((n) => n.Selected) == false).ToList();
                    }
                    bool allEqual = true;
                    //Determine if all selected operands of the product are equal and in same order to the commen product operands.
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

                    //If all selected operands in the product are in the common products operands.
                    if (allEqual || commonProductOperands.Count == 0)
                    {
                        commonProductOperands = selectedProductOperands;
                        //If the whole operand of the sum is selected leave 1 or -1 at its placing when factor outside parenthesis.
                        if (nonSelectedProductOperands.Count == 0)
                        {
                            suggestionSum.Add(negative ? ToNumeric(-1) : ToNumeric(1));
                        }
                        else if (nonSelectedProductOperands.Count == 1)
                        {
                            //If one operand of the product is not selected leave it when factorize the rest outside.
                            ExpressionBase clone = nonSelectedProductOperands[0].Clone();
                            suggestionSum.Add(negative ? new UnaryMinusExpression(clone) : clone);
                        }
                        else
                        {
                            //If two or more operands of the product is not selected leave these when factorize the rest outside.
                            VariadicOperatorExpression nonSelectedProduct = new VariadicOperatorExpression(OperatorType.Multiply, nonSelectedProductOperands[0].Clone(), nonSelectedProductOperands[1].Clone());
                            nonSelectedProduct.Add(nonSelectedProductOperands.Skip(2).Select((o) => o.Clone()).ToList());
                            suggestionSum.Add(negative ? (ExpressionBase)new UnaryMinusExpression(nonSelectedProduct) : nonSelectedProduct);
                        }
                    }
                    else
                    {
                        //If two operands in the sum has two different selections return null
                        return null;
                    }
                }
                //Return product where the common product selection is factorized outside a parenthesis. e.g. (c - d)*a*b
                VariadicOperatorExpression sumExpression = new VariadicOperatorExpression(OperatorType.Add, suggestionSum[0], suggestionSum[1]);
                sumExpression.Add(suggestionSum.Skip(2).ToList());
                VariadicOperatorExpression suggestion = new VariadicOperatorExpression(OperatorType.Multiply, sumExpression, new NumericExpression(-1));
                suggestion.Add(commonProductOperands.Select((o) => o.Clone()).ToList());
                suggestion.RemoveAt(1);
                return suggestion;
            }
            return null;
        }

        // a*(b + c) = a*b + a*c
        public static ExpressionBase ReverseProductParenthesis(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a product
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null && variadicExpression.Type == OperatorType.Multiply && variadicExpression.Count == 2)
            {
                DelimiterExpression delimiterExpression = variadicExpression[1] as DelimiterExpression;
                ExpressionBase other = variadicExpression[0];
                VariadicOperatorExpression variadicContent;
                //Find which of the two operands in the product is a parenthesis
                if (delimiterExpression != null)
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
                    }
                    else
                    {
                        return null;
                    }
                }
                //If one of the two operands is a parenthesis and its content is a sum
                if (variadicContent != null && variadicContent.Type == OperatorType.Add)
                {
                    VariadicOperatorExpression suggestion = variadicContent.Clone() as VariadicOperatorExpression;

                    //Foreach operand in the parenthesis' sum.
                    for (int i = 0; i < suggestion.Count; i++)
                    {
                        //Multiply the operand with expression multiplied with the parenthesis
                        UnaryMinusExpression minusOperand = suggestion[i] as UnaryMinusExpression;
                        if (minusOperand != null)
                        {
                            minusOperand.Expression.Replace(new VariadicOperatorExpression(OperatorType.Multiply, other, minusOperand.Expression.Clone()));
                        }
                        else
                        {
                            suggestion[i].Replace(new VariadicOperatorExpression(OperatorType.Multiply, other, suggestion[i].Clone()));
                        }
                    }
                    //return product. e.g. a*b + a*c
                    return suggestion;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        //{a*b}/c = a/c * b/c
        public static ExpressionBase SplittingFractions(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a fraction. e.g. {a*b}/c
            BinaryOperatorExpression binaryExpression = expression as BinaryOperatorExpression;
            if (binaryExpression != null && binaryExpression.Type == OperatorType.Divide)
            {
                //If the fraction's numerator is a sum e.g. a*b in fraction {a*b}/c
                VariadicOperatorExpression numerators = binaryExpression.Left.Clone() as VariadicOperatorExpression;
                if (numerators != null && numerators.Type == OperatorType.Add && numerators.Count > 1)
                {
                    List<ExpressionBase> fractionList = new List<ExpressionBase>();
                    //Foeach operand in the numerator's sum. e.g. a and b in fraction {a*b}/c
                    foreach (var i in numerators)
                    {
                        //Initialize a fraction with the operand as numerator and add to list. e.g. a/c and b/c
                        fractionList.Add(new BinaryOperatorExpression(i, binaryExpression.Right.Clone(), OperatorType.Divide));
                    }
                    //Initialize and return the product of all the fractions in the list. e.g. a/c * b/c
                    VariadicOperatorExpression suggestion = new VariadicOperatorExpression(OperatorType.Add, fractionList[0], fractionList[1]);
                    foreach (var i in fractionList.Skip(2))
                    {
                        suggestion.Add(i);
                    }
                    return suggestion;
                }
            }
            return null;
        }

        //a^c * b^c = (a*b)^c
        public static ExpressionBase CommonPowerParenthesisRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a product. e.g. a^c * b^c
            VariadicOperatorExpression product = expression as VariadicOperatorExpression;
            if (product != null && product.Type == OperatorType.Multiply)
            {
                ExpressionBase commonPower = null;
                List<ExpressionBase> baseList = new List<ExpressionBase>();

                //Foreach operand in product
                foreach (ExpressionBase operand in product)
                {
                    //If operand is a power
                    BinaryOperatorExpression power = operand as BinaryOperatorExpression;
                    if (power != null && power.Type == OperatorType.Power)
                    {
                        //If power's exponent equals the common exponent
                        if (power.Right == commonPower || commonPower == null)
                        {
                            //Add power's base to list
                            commonPower = power.Right;
                            baseList.Add(power.Left);
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
                if (baseList.Count > 1)
                {
                    //Initialize product of all bases in the list.
                    VariadicOperatorExpression resultProduct = new VariadicOperatorExpression(OperatorType.Multiply, baseList[0].Clone(), baseList[1].Clone());
                    resultProduct.Add(baseList.Skip(2).Select((b) => b.Clone()).ToList());
                    //return exponent with product as base and common exponent. e.g. (a*b)^c
                    BinaryOperatorExpression result = new BinaryOperatorExpression(resultProduct, commonPower.Clone(), OperatorType.Power);
                    return result;
                }
            }
            return null;
        }

        //(a*b)^c = a^c * b^c
        public static ExpressionBase ReverseCommonPowerParenthesisRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            List<ExpressionBase> itemsInParenthesis = new List<ExpressionBase>();
            VariadicOperatorExpression suggestion;
            //If expression is a power
            var binaryExpression = expression as BinaryOperatorExpression;
            if (binaryExpression != null && binaryExpression.Type == OperatorType.Power)
            {
                var commonparrent = binaryExpression.Right.Clone();
                //If power's base is a parenthesis
                if (binaryExpression.Left is DelimiterExpression)
                {
                    var delimiterExpression = binaryExpression.Left as DelimiterExpression;
                    //If the parenthesis' content is a product
                    if (delimiterExpression.Expression is VariadicOperatorExpression)
                    {
                        var variadicExpression = delimiterExpression.Expression.Clone() as VariadicOperatorExpression;
                        if (variadicExpression.Type == OperatorType.Multiply)
                        {
                            //Raise every operand to the power's exponent.
                            foreach (var item in variadicExpression)
                            {
                                itemsInParenthesis.Add(new BinaryOperatorExpression(item, commonparrent, OperatorType.Power));
                            }
                            //Initialize and reutrn product of exponents
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

        //6 = 2*3
        public static ExpressionBase FactorizationRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            VariadicOperatorExpression suggestion;
            //If expression is a number
            if (expression is NumericExpression && expression != null)
            {
                var numericExpression = selection[0] as NumericExpression;
                var n = numericExpression.Number;
                //Find integers a != 1 and b != 1 such that n = a*b
                for (int count = 2; count < n; count++)
                {
                    if (n % count == 0)
                    {
                        NumericExpression a = new NumericExpression(count);
                        NumericExpression b = new NumericExpression(n / count);

                        //Return product of a and b
                        suggestion = new VariadicOperatorExpression(OperatorType.Multiply, a, b);
                        return suggestion;
                    }
                }
            }
            return null;
        }

        //
        public static ExpressionBase ParenthesisPowerRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a power
            BinaryOperatorExpression binaryOperatorExpression = expression as BinaryOperatorExpression;
            if (binaryOperatorExpression != null && binaryOperatorExpression.Type == OperatorType.Power)
            {
                //If power's base is a parenthesis
                DelimiterExpression delimiterBase = binaryOperatorExpression.Left as DelimiterExpression;
                if (delimiterBase != null)
                {
                    //If the parentheses content is a power
                    BinaryOperatorExpression binaryBase = delimiterBase.Expression as BinaryOperatorExpression;
                    if (binaryBase != null && binaryBase.Type == OperatorType.Power)
                    {
                        //Return power with exponent as product of both exponents
                        return new BinaryOperatorExpression(binaryBase.Left.Clone(), new VariadicOperatorExpression(OperatorType.Multiply, binaryBase.Right.Clone(), binaryOperatorExpression.Right.Clone()), OperatorType.Power);
                    }
                }
            }
            return null;
        }

        //sqrt(a) = a^{1/2}
        public static ExpressionBase SquareRootRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a square root
            FunctionExpression sqrtExpression = expression as FunctionExpression;
            if (sqrtExpression != null && sqrtExpression.Function == "sqrt")
            {
                //Return power with square root's content as base and 1/2 as exponent.
                return new BinaryOperatorExpression(sqrtExpression.Expression.Clone(), new BinaryOperatorExpression(new NumericExpression(1), new NumericExpression(2), OperatorType.Divide), OperatorType.Power);
            }
            else
            {
                return null;
            }
        }

        // a*{b/c} = {a*b}/c
        public static ExpressionBase ProductOfConstantAndFraction(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a product with to operands
            var variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null && variadicExpression.Type == OperatorType.Multiply && variadicExpression.Count == 2)
            {
                BinaryOperatorExpression fraction = null;
                ExpressionBase other = null;
                //Find which operand is a fraction
                if ((fraction = variadicExpression[0] as BinaryOperatorExpression) != null && fraction.Type == OperatorType.Divide)
                {
                    other = variadicExpression[1];
                }
                else if ((fraction = variadicExpression[1] as BinaryOperatorExpression) != null && fraction.Type == OperatorType.Divide)
                {
                    other = variadicExpression[0];
                }
                else
                {
                    //Return null of non of the operands is a fraction
                    return null;
                }

                //Multiply numerator with other operand
                VariadicOperatorExpression numerators = new VariadicOperatorExpression(OperatorType.Multiply, fraction.Left.Clone(), other.Clone());

                //Return fraction
                BinaryOperatorExpression suggestion = new BinaryOperatorExpression(numerators.Clone(), fraction.Right.Clone(), OperatorType.Divide);
                return suggestion;
            }
            return null;
        }

        //a/a = 1
        public static ExpressionBase DivisionEqualsOneRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a fraction
            var fraction = expression as BinaryOperatorExpression;
            if (fraction != null && fraction.Type == OperatorType.Divide)
            {
                //If numerator and denominator are equal
                if (fraction.Left == fraction.Right)
                {
                    //return 1
                    NumericExpression suggestion = new NumericExpression(1);
                    return suggestion;
                }
            }
            return null;
        }

        //-a = (-1)*a
        public static ExpressionBase FactorizeUnaryMinus(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is unary minus
            var unaryMinusExpression = expression as UnaryMinusExpression;
            if (unaryMinusExpression != null)
            {
                //Return product of operand and (-1)
                var numericExpression = new NumericExpression(1);
                var suggestion = new VariadicOperatorExpression(OperatorType.Multiply, new UnaryMinusExpression(numericExpression), unaryMinusExpression.Expression.Clone());
                return suggestion;
            }
            return null;
        }

        public static int? FromNumeric(ExpressionBase expression)
        {
            NumericExpression numeric = expression as NumericExpression;
            if (numeric != null)
            {
                return numeric.Number;
            }
            DelimiterExpression delimiter = expression as DelimiterExpression;
            if (delimiter != null)
            {
                return FromMinusNumeric(delimiter.Expression);
            }
            else
            {
                return FromMinusNumeric(expression);
            }
        }

        public static int? FromMinusNumeric(ExpressionBase expression)
        {
            UnaryMinusExpression minus = expression as UnaryMinusExpression;
            if (minus != null)
            {
                NumericExpression numeric = minus.Expression as NumericExpression;
                if (numeric != null)
                {
                    return -numeric.Number;
                }
            }
            return null;
        }

        public static ExpressionBase ToNumeric(int number)
        {
            if (number < 0)
            {
                return new UnaryMinusExpression(new NumericExpression(-number));
            }
            else
            {
                return new NumericExpression(number);
            }
        }

        //4+5 = 9, 4*5 = 20
        public static ExpressionBase CalculateVariadicRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            VariadicOperatorExpression variadic = expression as VariadicOperatorExpression;
            if (variadic != null)
            {
                int sum;
                Func<int, int, int> operation;

                //Set start value of sum and operation to either multiplication or addition.
                if (variadic.Type == OperatorType.Multiply)
                {
                    sum = 1;
                    operation = (a, b) => a * b;
                }
                else if (variadic.Type == OperatorType.Add)
                {
                    sum = 0;
                    operation = (a, b) => a + b;
                }
                else
                {
                    return null;
                }

                //Calculate sum using operation on operands
                foreach (ExpressionBase operand in variadic)
                {
                    int? number = FromNumeric(operand);
                    if (number != null)
                    {
                        sum = operation((int)number, sum);
                    }
                    else
                    {
                        return null;
                    }
                }

                //Return sum
                return ToNumeric(sum);
            }
            return null;
        }

        // 4^2 = 16, 4/2 = 2
        public static ExpressionBase CalculateBinaryRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            BinaryOperatorExpression binary = expression as BinaryOperatorExpression;
            if (binary != null)
            {
                int? leftNumber, rightNumber;
                leftNumber = FromNumeric(binary.Left);
                rightNumber = FromNumeric(binary.Right);
                //If left and right operands are numbers
                if (leftNumber != null && rightNumber != null)
                {
                    //If expression is a power
                    if (binary.Type == OperatorType.Power)
                    {
                        if ((int)rightNumber > 0)
                        {
                            //Calculate and return left^right
                            int sum = 1;
                            for (int n = 0; n < (int)rightNumber; n++)
                            {
                                sum *= (int)leftNumber;
                            }
                            return ToNumeric(sum);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (binary.Type == OperatorType.Divide && (int)leftNumber % (int)rightNumber == 0)
                    {
                        //If expression is a fraction return left / right
                        return ToNumeric((int)leftNumber / (int)rightNumber);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        // (-a)*b = -{a*b}
        public static ExpressionBase MultiplyMinusRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is a product of two operands
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null && variadicExpression.Type == OperatorType.Multiply && variadicExpression.Count == 2)
            {
                for (int index = 0; index < 2; index++)
                {
                    //If operand is a parenthesis
                    DelimiterExpression delimiter = variadicExpression[index == 0 ? 1 : 0] as DelimiterExpression;
                    if (delimiter != null)
                    {
                        //If parenthesis' content is unary minus
                        UnaryMinusExpression minus = delimiter.Expression as UnaryMinusExpression;
                        if (minus != null)
                        {
                            //Multiply and unary minus' operand with other operand
                            ExpressionBase result = minus.Clone();
                            ExpressionBase content = (result as UnaryExpression).Expression;
                            content.Replace(new VariadicOperatorExpression(OperatorType.Multiply, content.Clone(), variadicExpression[index == 0 ? 0 : 1]));
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        // 1 * a = a
        public static ExpressionBase MultiplyOneRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is product of two operands
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null && variadicExpression.Type == OperatorType.Multiply && variadicExpression.Count == 2)
            {
                //If one operand is 1 return the other operand.
                NumericExpression one = variadicExpression[0] as NumericExpression;
                if (variadicExpression[0].ToString() == "1")
                {
                    return variadicExpression[1].Clone();
                }
                else if (variadicExpression[1].ToString() == "1")
                {
                    return variadicExpression[0].Clone();
                }
            }
            return null;
        }

        //a + 0 = a
        public static ExpressionBase RemoveNull(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is sum of two operands
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null && variadicExpression.Type == OperatorType.Add && variadicExpression.Count == 2)
            {
                //If one operand is 0 return other operand
                if (variadicExpression[0].ToString() == "0" || variadicExpression[0].ToString() == "-{0}")
                {
                    return variadicExpression[1].Clone();
                }
                else if (variadicExpression[1].ToString() == "0" || variadicExpression[1].ToString() == "-{0}")
                {
                    return variadicExpression[0].Clone();
                }
            }
            return null;
        }

        //a*0 = 0
        public static ExpressionBase MultiplyByNull(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is product
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null && variadicExpression.Type == OperatorType.Multiply)
            {
                //If one of the operands is 0 then return 0
                foreach (ExpressionBase item in variadicExpression)
                {
                    if (item.ToString() == "0" || item.ToString() == "-{0}")
                    {
                        return new NumericExpression(0);
                    }
                }
            }
            return null;
        }

        //a*b = b*a, a+b = b+a
        public static ExpressionBase CommutativeRule(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is sum or product of two operands
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null && variadicExpression.Count == 2)
            {
                //Return product where operands er swapped place
                return new VariadicOperatorExpression(variadicExpression.Type, variadicExpression[1].Clone(), variadicExpression[0].Clone());
            }
            return null;
        }

        // a/b * c/d = {a*c}/{b*d}
        public static ExpressionBase ProductOfFractions(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is product if two or more operands
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null && variadicExpression.Type == OperatorType.Multiply && variadicExpression.Count >= 2)
            {
                List<ExpressionBase> listOfNumerators = new List<ExpressionBase>();
                List<ExpressionBase> listOfDenominators = new List<ExpressionBase>();
                //Foreach operand in product
                foreach (ExpressionBase operand in variadicExpression)
                {
                    //If operand is a fraction
                    BinaryOperatorExpression binaryExpression = operand as BinaryOperatorExpression;
                    if(binaryExpression != null && binaryExpression.Type == OperatorType.Divide)
                    {
                        //Add numerator and donimator to list
                        listOfNumerators.Add(binaryExpression.Left.Clone());
                        listOfDenominators.Add(binaryExpression.Right.Clone());
                    }
                    else
                    {
                        //If operands i not a fraction return null
                        return null;
                    }                
                }

                //Convert list to product of numerators
                VariadicOperatorExpression suggestionNumerator = new VariadicOperatorExpression(OperatorType.Multiply, listOfNumerators[0], listOfNumerators[1]);
                foreach (var item in listOfNumerators.Skip(2))
                {
                    suggestionNumerator.Add(item);
                }
                //Convert list to product of denominators
                VariadicOperatorExpression suggestionDenominator = new VariadicOperatorExpression(OperatorType.Multiply, listOfDenominators[0], listOfDenominators[1]);
                foreach (var item in listOfDenominators.Skip(2))
                {
                    suggestionDenominator.Add(item);
                }
                //Return fraction with product of numerators and denominators
                return new BinaryOperatorExpression(suggestionNumerator, suggestionDenominator, OperatorType.Divide);
            }
            return null;
        }

        // y - y = 0
        public static ExpressionBase VariablesEqualNull(ExpressionBase expression, List<ExpressionBase> selection)
        {
            //If expression is sum if two operands
            VariadicOperatorExpression variadicExpression = expression as VariadicOperatorExpression;
            if (variadicExpression != null && variadicExpression.Type == OperatorType.Add && variadicExpression.Count == 2)
            {
                //Return 0 if one operand is unary minus and its operand equals the other operand.
                UnaryMinusExpression minusExpression;
                if (variadicExpression[0] is UnaryMinusExpression)
                {
                    minusExpression = variadicExpression[0] as UnaryMinusExpression;

                    if (minusExpression.Expression == variadicExpression[1])
                    {
                        return new NumericExpression(0);
                    }
                }
                else if (variadicExpression[1] is UnaryMinusExpression)
                {
                    minusExpression = variadicExpression[1] as UnaryMinusExpression;

                    if (minusExpression.Expression == variadicExpression[0])
                    {
                        return new NumericExpression(0);
                    }
                }
            }
            return null;
        }
    }
}
