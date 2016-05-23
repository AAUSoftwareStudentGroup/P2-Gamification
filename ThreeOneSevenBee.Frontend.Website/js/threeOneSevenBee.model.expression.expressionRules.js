(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules', {
        statics: {
            productToExponentRule: function (expression, selection) {
                //If expression is a product.
                var product = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(product) && product.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                    //If all operands are equal.
                    if (Bridge.Linq.Enumerable.from(product).where(function (operand) {
                        return ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(product.getItem(0), operand);
                    }).count() === product.getCount()) {
                        //Return power.
                        var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(product.getItem(0).clone(), new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(product.getCount()), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                        return suggestion;
                    }
                }
                return null;
            },
            exponentToProductRule: function (expression, selection) {
                //If expression is a power.
                var exponent = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(exponent) && exponent.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                    //If exponent is a number.
                    var numericExpression = Bridge.as(exponent.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                    if (Bridge.hasValue(numericExpression)) {
                        var number = numericExpression.number;
                        if (number === 0) {
                            //a^0 = 1
                            return new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1);
                        }
                        else  {
                            if (number === 1) {
                                //a^1 = a
                                return exponent.getLeft().clone();
                            }
                            else  {
                                if (number > 1) {
                                    //a^n = a*a*..*a
                                    var result = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, exponent.getLeft().clone(), exponent.getLeft().clone());
                                    for (var i = 2; i < number; i++) {
                                        result.add(exponent.getLeft().clone());
                                    }
                                    return result;
                                }
                            }
                        }
                    }
                    else  {
                        return null;
                    }
                }
                return null;
            },
            fractionToProductRule: function (expression, selection) {
                //If expression a fraction. e.g. a/b
                var fraction = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryExpression);
                if (Bridge.hasValue(fraction) && fraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                    //Convert denominator to power of -1. b => b^-1
                    var exponent = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(fraction.getRight().clone(), new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1)), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                    //Return product of numerator and power. a*b^-1
                    var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, fraction.getLeft().clone(), exponent);
                    return suggestion;
                }
                else  {
                    return null;
                }
    
            },
            removeParenthesisRule: function (expression, selection) {
                //If expression is parenthesis
                var parenthesis = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                if (Bridge.hasValue(parenthesis)) {
                    //return content of parenthesis
                    return parenthesis.getExpression().clone();
                }
                return null;
            },
            variableWithNegativeExponent: function (expression, selection) {
                //If expression is a power. e.g. a^-n
                var binaryexpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(binaryexpression) && binaryexpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                    //If exponent of power is unary minus. e.g. -n
                    var unaryexpression = Bridge.as(binaryexpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                    if (Bridge.hasValue(unaryexpression) && Bridge.hasValue(binaryexpression.getLeft()) && Bridge.hasValue(binaryexpression.getRight())) {
                        //Return reciprocal of power with unary minus removed in exponent. e.g. 1/a^n
                        var power = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(binaryexpression.getLeft().clone(), unaryexpression.getExpression().clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                        return new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1), power, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
                    }
                }
                return null;
            },
            reverseVariableWithNegativeExponent: function (expression, selection) {
                //If expression is a fraction. e.g. 1/a^n
                var binaryexpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(binaryexpression) && binaryexpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                    //If numerator is 1 and denominator is power. e.g. 1 and a^n
                    var numericexpression = Bridge.as(binaryexpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                    var power = Bridge.as(binaryexpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryExpression);
                    if (Bridge.hasValue(numericexpression) && numericexpression.getValue() === "1" && Bridge.hasValue(power) && power.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                        //return power with minus exponent. e.g. a^-n
                        var unaryminus = new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(power.getRight().clone());
                        var mysuggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(power.getLeft().clone(), unaryminus.clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                        return mysuggestion;
                    }
                }
    
                return null;
            },
            addFractionWithCommonDenominatorRule: function (expression, selection) {
                var $t;
                //If expression is a sum.
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                    var binaryOperand;
                    var terms = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                    var commonDenominator = null;
    
                    //Foreach operand in sum.
                    $t = Bridge.getEnumerator(variadicExpression);
                    while ($t.moveNext()) {
                        var operand = $t.getCurrent();
                        //If operand is a fraction.
                        binaryOperand = Bridge.as(operand, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                        if (Bridge.hasValue(binaryOperand) && binaryOperand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                            //If operand is the first fraction in sum
                            if (terms.getCount() === 0) {
                                //Add numerator to list of numerators.
                                terms.add(binaryOperand.getLeft().clone());
    
                                //Set commonDenominator to operand's denominator.
                                commonDenominator = binaryOperand.getRight().clone();
                            }
                            else  {
                                if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(commonDenominator, binaryOperand.getRight())) {
                                    //Add fraction's numerator to list if its denominator equals commenDenominator.
                                    terms.add(binaryOperand.getLeft().clone());
                                }
                                else  {
                                    //Return null if an fraction's denominator does not equals commenDenominator.
                                    return null;
                                }
                            }
                        }
                        else  {
                            //Return if operand is not a fraction.
                            return null;
                        }
                    }
                    if (terms.getCount() > 1) {
                        //Initialize sum of all fractions' numerators.
                        var SuggestionNumerator = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, terms.getItem(0), terms.getItem(1));
                        SuggestionNumerator.add$1(Bridge.Linq.Enumerable.from(terms).skip(2).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
    
                        //Return fraction with sum of all fractions' numerators and with their common denominator.
                        return new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(SuggestionNumerator, commonDenominator, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
                    }
                }
                return null;
            },
            exponentProduct: function (expression, selection) {
                var $t;
                //If expression is a product. e.g. a*a^2
                var product = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(product) && product.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                    var sum = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                    var commonBase = null;
    
                    //Foreach operand in product
                    $t = Bridge.getEnumerator(product);
                    while ($t.moveNext()) {
                        var operand = $t.getCurrent();
                        var operandLeft = operand;
                        var operandRight = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1);
                        var power = Bridge.as(operand, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                        if (Bridge.hasValue(power) && power.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                            operandLeft = power.getLeft();
                            operandRight = power.getRight();
                        }
                        //If operand's base equals commonBase.
                        if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(operandLeft, commonBase) || !Bridge.hasValue(commonBase)) {
                            //Add operands exponent to sum.
                            commonBase = operandLeft;
                            sum.add(operandRight.clone());
                        }
                        else  {
                            return null;
                        }
                    }
                    if (sum.getCount() > 1) {
                        //Return power with common base and sum of all operands' exponents.
                        var suggestionRight = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, sum.getItem(0), sum.getItem(1));
                        suggestionRight.add$1(Bridge.Linq.Enumerable.from(sum).skip(2).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
                        return new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(commonBase, suggestionRight, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                    }
                }
                return null;
            },
            productParenthesis: function (expression, selection) {
                var $t;
                //If expression is a sum. e.g. a*b*c - a*b*d
                var sum = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(sum) && sum.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                    var suggestionSum = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                    var commonProductOperands = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                    //Foreach operand in sum e.g. a*b*c and a*b*d
                    $t = Bridge.getEnumerator(sum);
                    while ($t.moveNext()) {
                        var actualOperand = $t.getCurrent();
                        var operand;
                        var negative;
    
                        //If operand is unary minus take its operand and set negative = true. e.g. -a*b*d => operand = a*b*d, negative = true
                        var minus = Bridge.as(actualOperand, ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                        if (Bridge.hasValue(minus)) {
                            operand = minus.getExpression();
                            negative = true;
                        }
                        else  {
                            operand = actualOperand;
                            negative = false;
                        }
                        var selectedProductOperands = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)(), [
                            [operand]
                        ] );
                        var nonSelectedProductOperands = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                        var product = Bridge.as(operand, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                        //If operand in the sum is a product. e.g. a*b*d
                        if (Bridge.hasValue(product) && product.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                            //Store all selected and nonselected operands in the product.
                            selectedProductOperands = Bridge.Linq.Enumerable.from(product).where($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f2).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase);
                            nonSelectedProductOperands = Bridge.Linq.Enumerable.from(product).where($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f3).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase);
                        }
                        var allEqual = true;
                        //Determine if all selected operands of the product are equal to the commen product operands.
                        if (commonProductOperands.getCount() === 0 || commonProductOperands.getCount() === selectedProductOperands.getCount()) {
                            for (var index = 0; index < commonProductOperands.getCount(); index++) {
                                if (selectedProductOperands.contains(commonProductOperands.getItem(index))) {
                                    allEqual = false;
                                }
                            }
                        }
                        else  {
                            return null;
                        }
    
                        //If all selected operands in the product are in the common products operands.
                        if (allEqual || commonProductOperands.getCount() === 0) {
                            commonProductOperands = selectedProductOperands;
                            //If the whole operand of the sum is selected leave 1 or -1 at its placing when factor outside parenthesis.
                            if (nonSelectedProductOperands.getCount() === 0) {
                                suggestionSum.add(negative ? Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).toNumeric(-1) : Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).toNumeric(1));
                            }
                            else  {
                                if (nonSelectedProductOperands.getCount() === 1) {
                                    //If one operand of the product is not selected leave it when factorize the rest outside.
                                    var clone = nonSelectedProductOperands.getItem(0).clone();
                                    suggestionSum.add(negative ? new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(clone) : clone);
                                }
                                else  {
                                    //If two or more operands of the product is not selected leave these when factorize the rest outside.
                                    var nonSelectedProduct = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, nonSelectedProductOperands.getItem(0).clone(), nonSelectedProductOperands.getItem(1).clone());
                                    nonSelectedProduct.add$1(Bridge.Linq.Enumerable.from(nonSelectedProductOperands).skip(2).select($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f4).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
                                    suggestionSum.add(negative ? Bridge.cast(new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(nonSelectedProduct), ThreeOneSevenBee.Model.Expression.ExpressionBase) : nonSelectedProduct);
                                }
                            }
                        }
                        else  {
                            //If two operands in the sum has two different selections return null
                            return null;
                        }
                    }
                    //Return product where the common product selection is factorized outside a parenthesis. e.g. (c - d)*a*b
                    var sumExpression = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, suggestionSum.getItem(0), suggestionSum.getItem(1));
                    sumExpression.add$1(Bridge.Linq.Enumerable.from(suggestionSum).skip(2).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
                    var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, sumExpression, new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(-1));
                    suggestion.add$1(Bridge.Linq.Enumerable.from(commonProductOperands).select($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f4).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
                    suggestion.removeAt(1);
                    return suggestion;
                }
                return null;
            },
            reverseProductParenthesis: function (expression, selection) {
                //If expression is a product
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply && variadicExpression.getCount() === 2) {
                    var delimiterExpression = Bridge.as(variadicExpression.getItem(1), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                    var other = variadicExpression.getItem(0);
                    var variadicContent;
                    //Find which of the two operands in the product is a parenthesis
                    if (Bridge.hasValue(delimiterExpression)) {
                        variadicContent = Bridge.as(delimiterExpression.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                    }
                    else  {
                        delimiterExpression = Bridge.as(variadicExpression.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                        other = variadicExpression.getItem(1);
                        if (Bridge.hasValue(delimiterExpression)) {
                            variadicContent = Bridge.as(delimiterExpression.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                        }
                        else  {
                            return null;
                        }
                    }
                    //If one of the two operands is a parenthesis and its content is a sum
                    if (Bridge.hasValue(variadicContent) && variadicContent.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                        var suggestion = Bridge.as(variadicContent.clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
    
                        //Foreach operand in the parenthesis' sum.
                        for (var i = 0; i < suggestion.getCount(); i++) {
                            //Multiply the operand with expression multiplied with the parenthesis
                            var minusOperand = Bridge.as(suggestion.getItem(i), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                            if (Bridge.hasValue(minusOperand)) {
                                minusOperand.getExpression().replace(new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, other, minusOperand.getExpression().clone()));
                            }
                            else  {
                                suggestion.getItem(i).replace(new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, other, suggestion.getItem(i).clone()));
                            }
                        }
                        //return product. e.g. a*b + a*c
                        return suggestion;
                    }
                    else  {
                        return null;
                    }
                }
                return null;
            },
            splittingFractions: function (expression, selection) {
                var $t, $t1;
                //If expression is a fraction. e.g. {a*b}/c
                var binaryExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(binaryExpression) && binaryExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                    //If the fraction's numerator is a sum e.g. a*b in fraction {a*b}/c
                    var numerators = Bridge.as(binaryExpression.getLeft().clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                    if (Bridge.hasValue(numerators) && numerators.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add && numerators.getCount() > 1) {
                        var fractionList = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                        //Foeach operand in the numerator's sum. e.g. a and b in fraction {a*b}/c
                        $t = Bridge.getEnumerator(numerators);
                        while ($t.moveNext()) {
                            var i = $t.getCurrent();
                            //Initialize a fraction with the operand as numerator and add to list. e.g. a/c and b/c
                            fractionList.add(new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(i, binaryExpression.getRight().clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide));
                        }
                        //Initialize and return the product of all the fractions in the list. e.g. a/c * b/c
                        var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, fractionList.getItem(0), fractionList.getItem(1));
                        $t1 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(fractionList).skip(2));
                        while ($t1.moveNext()) {
                            var i1 = $t1.getCurrent();
                            suggestion.add(i1);
                        }
                        return suggestion;
                    }
                }
                return null;
            },
            commonPowerParenthesisRule: function (expression, selection) {
                var $t;
                //If expression is a product. e.g. a^c * b^c
                var product = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(product) && product.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                    var commonPower = null;
                    var baseList = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
    
                    //Foreach operand in product
                    $t = Bridge.getEnumerator(product);
                    while ($t.moveNext()) {
                        var operand = $t.getCurrent();
                        //If operand is a power
                        var power = Bridge.as(operand, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                        if (Bridge.hasValue(power) && power.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                            //If power's exponent equals the common exponent
                            if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(power.getRight(), commonPower) || !Bridge.hasValue(commonPower)) {
                                //Add power's base to list
                                commonPower = power.getRight();
                                baseList.add(power.getLeft());
                            }
                            else  {
                                return null;
                            }
                        }
                        else  {
                            return null;
                        }
                    }
                    if (baseList.getCount() > 1) {
                        //Initialize product of all bases in the list.
                        var resultProduct = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, baseList.getItem(0).clone(), baseList.getItem(1).clone());
                        resultProduct.add$1(Bridge.Linq.Enumerable.from(baseList).skip(2).select($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f5).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
                        //return exponent with product as base and common exponent. e.g. (a*b)^c
                        var result = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(resultProduct, commonPower.clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                        return result;
                    }
                }
                return null;
            },
            reverseCommonPowerParenthesisRule: function (expression, selection) {
                var $t, $t1;
                var itemsInParenthesis = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                var suggestion;
                //If expression is a power
                var binaryExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(binaryExpression) && binaryExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                    var commonparrent = binaryExpression.getRight().clone();
                    //If power's base is a parenthesis
                    if (Bridge.is(binaryExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression)) {
                        var delimiterExpression = Bridge.as(binaryExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                        //If the parenthesis' content is a product
                        if (Bridge.is(delimiterExpression.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)) {
                            var variadicExpression = Bridge.as(delimiterExpression.getExpression().clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                            if (variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                                //Raise every operand to the power's exponent.
                                $t = Bridge.getEnumerator(variadicExpression);
                                while ($t.moveNext()) {
                                    var item = $t.getCurrent();
                                    itemsInParenthesis.add(new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(item, commonparrent, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power));
                                }
                                //Initialize and reutrn product of exponents
                                suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, itemsInParenthesis.getItem(0).clone(), itemsInParenthesis.getItem(1).clone());
                                if (itemsInParenthesis.getCount() > 2) {
                                    $t1 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(itemsInParenthesis).skip(2));
                                    while ($t1.moveNext()) {
                                        var item1 = $t1.getCurrent();
                                        suggestion.add(item1.clone());
                                    }
                                    return suggestion;
                                }
                                else  {
                                    return suggestion;
                                }
                            }
                        }
                    }
                }
                return null;
            },
            factorizationRule: function (expression, selection) {
                var suggestion;
                //If expression is a number
                if (Bridge.is(expression, ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression) && Bridge.hasValue(expression)) {
                    var numericExpression = Bridge.as(selection.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                    var n = numericExpression.number;
                    //Find integers a != 1 and b != 1 such that n = a*b
                    for (var count = 2; count < n; count++) {
                        if (n % count === 0) {
                            var a = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(count);
                            var b = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(Bridge.Int.div(n, count));
    
                            //Return product of a and b
                            suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, a, b);
                            return suggestion;
                        }
                    }
                }
                return null;
            },
            parenthesisPowerRule: function (expression, selection) {
                //If expression is a power
                var binaryOperatorExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(binaryOperatorExpression) && binaryOperatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                    //If power's base is a parenthesis
                    var delimiterBase = Bridge.as(binaryOperatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                    if (Bridge.hasValue(delimiterBase)) {
                        //If the parentheses content is a power
                        var binaryBase = Bridge.as(delimiterBase.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                        if (Bridge.hasValue(binaryBase) && binaryBase.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                            //Return power with exponent as product of both exponents
                            return new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(binaryBase.getLeft().clone(), new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, binaryBase.getRight().clone(), binaryOperatorExpression.getRight().clone()), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                        }
                    }
                }
                return null;
            },
            squareRootRule: function (expression, selection) {
                //If expression is a square root
                var sqrtExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression);
                if (Bridge.hasValue(sqrtExpression) && sqrtExpression.getFunction() === "sqrt") {
                    //Return power with square root's content as base and 1/2 as exponent.
                    return new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(sqrtExpression.getExpression().clone(), new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1), new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(2), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                }
                else  {
                    return null;
                }
            },
            productOfConstantAndFraction: function (expression, selection) {
                var $t, $t1;
                //If expression is a product with to operands
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply && variadicExpression.getCount() === 2) {
                    var fraction = null;
                    var other = null;
                    //Find which operand is a fraction
                    if (Bridge.hasValue((($t = Bridge.as(variadicExpression.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), fraction = $t, $t))) && fraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                        other = variadicExpression.getItem(1);
                    }
                    else  {
                        if (Bridge.hasValue((($t1 = Bridge.as(variadicExpression.getItem(1), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), fraction = $t1, $t1))) && fraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                            other = variadicExpression.getItem(0);
                        }
                        else  {
                            //Return null of non of the operands is a fraction
                            return null;
                        }
                    }
    
                    //Multiply numerator with other operand
                    var numerators = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, fraction.getLeft().clone(), other.clone());
    
                    //Return fraction
                    var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(numerators.clone(), fraction.getRight().clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
                    return suggestion;
                }
                return null;
            },
            divisionEqualsOneRule: function (expression, selection) {
                //If expression is a fraction
                var fraction = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(fraction) && fraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                    //If numerator and denominator are equal
                    if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(fraction.getLeft(), fraction.getRight())) {
                        //return 1
                        var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1);
                        return suggestion;
                    }
                }
                return null;
            },
            factorizeUnaryMinus: function (expression, selection) {
                //If expression is unary minus
                var unaryMinusExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                if (Bridge.hasValue(unaryMinusExpression)) {
                    //Return product of operand and (-1)
                    var numericExpression = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1);
                    var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(numericExpression), unaryMinusExpression.getExpression().clone());
                    return suggestion;
                }
                return null;
            },
            fromNumeric: function (expression) {
                var numeric = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                if (Bridge.hasValue(numeric)) {
                    return numeric.number;
                }
                var delimiter = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                if (Bridge.hasValue(delimiter)) {
                    return Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).fromMinusNumeric(delimiter.getExpression());
                }
                else  {
                    return Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).fromMinusNumeric(expression);
                }
            },
            fromMinusNumeric: function (expression) {
                var minus = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                if (Bridge.hasValue(minus)) {
                    var numeric = Bridge.as(minus.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                    if (Bridge.hasValue(numeric)) {
                        return -numeric.number;
                    }
                }
                return null;
            },
            toNumeric: function (number) {
                if (number < 0) {
                    return new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(-number));
                }
                else  {
                    return new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(number);
                }
            },
            calculateVariadicRule: function (expression, selection) {
                var $t;
                var variadic = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadic)) {
                    var sum;
                    var operation;
    
                    //Set start value of sum and operation to either multiplication or addition.
                    if (variadic.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                        sum = 1;
                        operation = $_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f6;
                    }
                    else  {
                        if (variadic.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                            sum = 0;
                            operation = $_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f7;
                        }
                        else  {
                            return null;
                        }
                    }
    
                    //Calculate sum using operation on operands
                    $t = Bridge.getEnumerator(variadic);
                    while ($t.moveNext()) {
                        var operand = $t.getCurrent();
                        var number = Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).fromNumeric(operand);
                        if (Bridge.hasValue(number)) {
                            sum = operation(Bridge.Nullable.getValue(number), sum);
                        }
                        else  {
                            return null;
                        }
                    }
    
                    //Return sum
                    return Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).toNumeric(sum);
                }
                return null;
            },
            calculateBinaryRule: function (expression, selection) {
                var binary = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(binary)) {
                    var leftNumber, rightNumber;
                    leftNumber = Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).fromNumeric(binary.getLeft());
                    rightNumber = Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).fromNumeric(binary.getRight());
                    //If left and right operands are numbers
                    if (Bridge.hasValue(leftNumber) && Bridge.hasValue(rightNumber)) {
                        //If expression is a power
                        if (binary.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                            if (Bridge.Nullable.getValue(rightNumber) > 0) {
                                //Calculate and return left^right
                                var sum = 1;
                                for (var n = 0; n < Bridge.Nullable.getValue(rightNumber); n++) {
                                    sum *= Bridge.Nullable.getValue(leftNumber);
                                }
                                return Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).toNumeric(sum);
                            }
                            else  {
                                return null;
                            }
                        }
                        else  {
                            if (binary.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide && Bridge.Nullable.getValue(leftNumber) % Bridge.Nullable.getValue(rightNumber) === 0) {
                                //If expression is a fraction return left / right
                                return Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).toNumeric(Bridge.Int.div(Bridge.Nullable.getValue(leftNumber), Bridge.Nullable.getValue(rightNumber)));
                            }
                            else  {
                                return null;
                            }
                        }
                    }
                }
                return null;
            },
            multiplyMinusRule: function (expression, selection) {
                //If expression is a product of two operands
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply && variadicExpression.getCount() === 2) {
                    for (var index = 0; index < 2; index++) {
                        //If operand is a parenthesis
                        var delimiter = Bridge.as(variadicExpression.getItem(index === 0 ? 1 : 0), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                        if (Bridge.hasValue(delimiter)) {
                            //If parenthesis' content is unary minus
                            var minus = Bridge.as(delimiter.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                            if (Bridge.hasValue(minus)) {
                                //Multiply and unary minus' operand with other operand
                                var result = minus.clone();
                                var content = (Bridge.as(result, ThreeOneSevenBee.Model.Expression.Expressions.UnaryExpression)).getExpression();
                                content.replace(new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, content.clone(), variadicExpression.getItem(index === 0 ? 0 : 1)));
                                return result;
                            }
                        }
                    }
                }
                return null;
            },
            multiplyOneRule: function (expression, selection) {
                //If expression is product of two operands
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply && variadicExpression.getCount() === 2) {
                    //If one operand is 1 return the other operand.
                    var one = Bridge.as(variadicExpression.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                    if (variadicExpression.getItem(0).toString() === "1") {
                        return variadicExpression.getItem(1).clone();
                    }
                    else  {
                        if (variadicExpression.getItem(1).toString() === "1") {
                            return variadicExpression.getItem(0).clone();
                        }
                    }
                }
                return null;
            },
            removeNull: function (expression, selection) {
                //If expression is sum of two operands
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add && variadicExpression.getCount() === 2) {
                    //If one operand is 0 return other operand
                    if (variadicExpression.getItem(0).toString() === "0" || variadicExpression.getItem(0).toString() === "-{0}") {
                        return variadicExpression.getItem(1).clone();
                    }
                    else  {
                        if (variadicExpression.getItem(1).toString() === "0" || variadicExpression.getItem(1).toString() === "-{0}") {
                            return variadicExpression.getItem(0).clone();
                        }
                    }
                }
                return null;
            },
            multiplyByNull: function (expression, selection) {
                var $t;
                //If expression is product
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                    //If one of the operands is 0 then return 0
                    $t = Bridge.getEnumerator(variadicExpression);
                    while ($t.moveNext()) {
                        var item = $t.getCurrent();
                        if (item.toString() === "0" || item.toString() === "-{0}") {
                            return new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(0);
                        }
                    }
                }
                return null;
            },
            commutativeRule: function (expression, selection) {
                //If expression is sum or product of two operands
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getCount() === 2) {
                    //Return product where operands er swapped place
                    return new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", variadicExpression.getType(), variadicExpression.getItem(1).clone(), variadicExpression.getItem(0).clone());
                }
                return null;
            },
            productOfFractions: function (expression, selection) {
                var $t, $t1, $t2;
                //If expression is product if two or more operands
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply && variadicExpression.getCount() >= 2) {
                    var listOfNumerators = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                    var listOfDenominators = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                    //Foreach operand in product
                    $t = Bridge.getEnumerator(variadicExpression);
                    while ($t.moveNext()) {
                        var operand = $t.getCurrent();
                        //If operand is a fraction
                        var binaryExpression = Bridge.as(operand, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                        if (Bridge.hasValue(binaryExpression) && binaryExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                            //Add numerator and donimator to list
                            listOfNumerators.add(binaryExpression.getLeft().clone());
                            listOfDenominators.add(binaryExpression.getRight().clone());
                        }
                        else  {
                            //If operands i not a fraction return null
                            return null;
                        }
                    }
    
                    //Convert list to product of numerators
                    var suggestionNumerator = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, listOfNumerators.getItem(0), listOfNumerators.getItem(1));
                    $t1 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(listOfNumerators).skip(2));
                    while ($t1.moveNext()) {
                        var item = $t1.getCurrent();
                        suggestionNumerator.add(item);
                    }
                    //Convert list to product of denominators
                    var suggestionDenominator = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, listOfDenominators.getItem(0), listOfDenominators.getItem(1));
                    $t2 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(listOfDenominators).skip(2));
                    while ($t2.moveNext()) {
                        var item1 = $t2.getCurrent();
                        suggestionDenominator.add(item1);
                    }
                    //Return fraction with product of numerators and denominators
                    return new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(suggestionNumerator, suggestionDenominator, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
                }
                return null;
            },
            variablesEqualNull: function (expression, selection) {
                //If expression is sum if two operands
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add && variadicExpression.getCount() === 2) {
                    //Return 0 if one operand is unary minus and its operand equals the other operand.
                    var minusExpression;
                    if (Bridge.is(variadicExpression.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression)) {
                        minusExpression = Bridge.as(variadicExpression.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
    
                        if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(minusExpression.getExpression(), variadicExpression.getItem(1))) {
                            return new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(0);
                        }
                    }
                    else  {
                        if (Bridge.is(variadicExpression.getItem(1), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression)) {
                            minusExpression = Bridge.as(variadicExpression.getItem(1), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
    
                            if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(minusExpression.getExpression(), variadicExpression.getItem(0))) {
                                return new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(0);
                            }
                        }
                    }
                }
                return null;
            }
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules, {
        f1: function (n) {
            return n.getSelected();
        },
        f2: function (o) {
            return (o.getSelected() === true || Bridge.Linq.Enumerable.from(o.getNodesRecursive()).any($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f1));
        },
        f3: function (o) {
            return o.getSelected() === false && Bridge.Linq.Enumerable.from(o.getNodesRecursive()).any($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f1) === false;
        },
        f4: function (o) {
            return o.clone();
        },
        f5: function (b) {
            return b.clone();
        },
        f6: function (a, b) {
            return a * b;
        },
        f7: function (a, b) {
            return a + b;
        }
    });
    
    
    
    Bridge.init();
})(this);
