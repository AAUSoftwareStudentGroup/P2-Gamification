(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules', {
        statics: {
            productToExponentRule: function (expression, selection) {
                if (selection.getCount() < 2) {
                    return null;
                }
    
                var product = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
    
                if (Bridge.hasValue(product) && product.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                    if (Bridge.Linq.Enumerable.from(selection).where(function (e) {
                        return ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(selection.getItem(0), e) && e.getParent() === expression;
                    }).count() === selection.getCount()) {
                        var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(selection.getItem(0).clone(), new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(selection.getCount()), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                        return suggestion;
                    }
                }
                return null;
            },
            exponentToProductRule: function (expression, selection) {
                var exponent = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
    
                if (Bridge.hasValue(exponent) && exponent.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                    if (selection.getItem(0).getParent() === exponent && selection.getItem(1).getParent() === exponent) {
                        var numericExpression = Bridge.as(exponent.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                        if (!Bridge.hasValue(numericExpression)) {
                            return null;
                        }
                        var number = numericExpression.number;
                        if (number === 0) {
                            return new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1);
                        }
                        else  {
                            if (number === 1) {
                                return exponent.getLeft().clone();
                            }
                            else  {
                                if (number > 1) {
                                    var result = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, exponent.getLeft().clone(), exponent.getLeft().clone());
                                    for (var i = 2; i < number; i++) {
                                        result.add(exponent.getLeft().clone());
                                    }
                                    return result;
                                }
                            }
                        }
                    }
                }
                return null;
            },
            fractionToProductRule: function (expression, selection) {
                var fraction = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryExpression);
                if (Bridge.hasValue(fraction) && fraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                    var exponent = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(fraction.getRight().clone()), new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1))), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                    var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(fraction.getLeft().clone()), exponent);
                    return suggestion;
                }
                else  {
                    return null;
                }
    
            },
            removeParenthesisRule: function (expression, selection) {
                var parenthesis = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                if (Bridge.hasValue(parenthesis)) {
                    return parenthesis.getExpression().clone();
                }
                return null;
            },
            numericCalculateRule: function (expression, selection) {
                if (expression.canCalculate() === true) {
                    var result = Bridge.Int.trunc(Bridge.Nullable.getValue(Bridge.Nullable.lift(expression.calculate())));
                    if (result >= 0) {
                        return new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(result);
                    }
                    else  {
                        return new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(Math.abs(result)));
                    }
                }
                return null;
            },
            variableWithNegativeExponent: function (expression, selection) {
                var binaryexpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(binaryexpression) && binaryexpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                    var unaryexpression = Bridge.as(binaryexpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                    if (Bridge.hasValue(unaryexpression) && Bridge.hasValue(binaryexpression.getLeft()) && Bridge.hasValue(binaryexpression.getRight())) {
                        var power = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(binaryexpression.getLeft().clone(), unaryexpression.getExpression().clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                        return new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1), power, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
                    }
                }
                return null;
            },
            reverseVariableWithNegativeExponent: function (expression, selection) {
                var binaryexpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(binaryexpression) && binaryexpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                    var numericexpression = Bridge.as(binaryexpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                    var power = Bridge.as(binaryexpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryExpression);
                    if (Bridge.hasValue(numericexpression) && numericexpression.getValue() === "1" && Bridge.hasValue(power)) {
                        var unaryminus = new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(power.getRight());
                        var mysuggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(power.getLeft().clone(), unaryminus.clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                        return mysuggestion;
                    }
                }
    
                return null;
            },
            addFractionWithCommonDenominatorRule: function (expression, selection) {
                var $t;
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                    var binaryOperand;
                    var terms = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                    var commonDenominator = null;
                    $t = Bridge.getEnumerator(variadicExpression);
                    while ($t.moveNext()) {
                        var operand = $t.getCurrent();
                        binaryOperand = Bridge.as(operand, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                        if (Bridge.hasValue(binaryOperand) && binaryOperand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                            if (terms.getCount() === 0) {
                                terms.add(binaryOperand.getLeft().clone());
                                commonDenominator = binaryOperand.getRight().clone();
                            }
                            else  {
                                if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(commonDenominator, binaryOperand.getRight())) {
                                    terms.add(binaryOperand.getLeft().clone());
                                }
                                else  {
                                    return null;
                                }
                            }
                        }
                        else  {
                            return null;
                        }
                    }
                    if (terms.getCount() > 1) {
                        var SuggestionNumerator = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, terms.getItem(0), terms.getItem(1));
                        SuggestionNumerator.add$1(Bridge.Linq.Enumerable.from(terms).skip(2).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
                        return new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(SuggestionNumerator, commonDenominator, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
                    }
                }
                return null;
            },
            exponentProduct: function (expression, selection) {
                var $t;
                var product = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(product) && product.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                    var sum = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                    var suggestionLeft = null;
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
                        if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(operandLeft, suggestionLeft) || !Bridge.hasValue(suggestionLeft)) {
                            suggestionLeft = operandLeft;
                            sum.add(operandRight.clone());
                        }
                        else  {
                            return null;
                        }
                    }
                    if (sum.getCount() > 1) {
                        var suggestionRight = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, sum.getItem(0), sum.getItem(1));
                        suggestionRight.add$1(Bridge.Linq.Enumerable.from(sum).skip(2).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
                        return new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(suggestionLeft, suggestionRight, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                    }
                }
                return null;
            },
            productParenthesis: function (expression, selection) {
                var $t;
                var sum = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(sum) && sum.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                    var suggestionSum = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                    var commonProductOperands = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                    $t = Bridge.getEnumerator(sum);
                    while ($t.moveNext()) {
                        var operand = $t.getCurrent();
                        console.log(operand);
                        var selectedProductOperands = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)(), [
                            [operand]
                        ] );
                        var nonSelectedProductOperands = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                        var product = Bridge.as(operand, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                        if (Bridge.hasValue(product) && product.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                            selectedProductOperands = Bridge.Linq.Enumerable.from(product).where($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f2).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase);
                            nonSelectedProductOperands = Bridge.Linq.Enumerable.from(product).where($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f3).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase);
                        }
                        console.log(selectedProductOperands);
                        console.log(nonSelectedProductOperands);
                        if (Bridge.Linq.Enumerable.from(commonProductOperands).union(selectedProductOperands).count() === commonProductOperands.getCount() || commonProductOperands.getCount() === 0) {
                            commonProductOperands = selectedProductOperands;
                            if (nonSelectedProductOperands.getCount() === 0) {
                                suggestionSum.add(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1));
                            }
                            else  {
                                if (nonSelectedProductOperands.getCount() === 1) {
                                    suggestionSum.add(nonSelectedProductOperands.getItem(0).clone());
                                }
                                else  {
                                    var nonSelectedProduct = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, nonSelectedProductOperands.getItem(0).clone(), nonSelectedProductOperands.getItem(1).clone());
                                    nonSelectedProduct.add$1(Bridge.Linq.Enumerable.from(nonSelectedProductOperands).skip(2).select($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f4).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
                                    suggestionSum.add(nonSelectedProduct);
                                }
                            }
                        }
                        else  {
                            console.log(Bridge.toArray(commonProductOperands).join(",") + " != " + Bridge.toArray(selectedProductOperands).join(","));
                            return null;
                        }
                    }
                    console.log("wat");
                    var sumExpression = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, suggestionSum.getItem(0), suggestionSum.getItem(1));
                    sumExpression.add$1(Bridge.Linq.Enumerable.from(suggestionSum).skip(2).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
                    var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(sumExpression), new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(-1));
                    suggestion.add$1(Bridge.Linq.Enumerable.from(commonProductOperands).select($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f4).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
                    suggestion.removeAt(1);
                    return suggestion;
                }
                return null;
            },
            reverseProductParenthesis: function (expression, selection) {
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply && variadicExpression.getCount() === 2) {
                    var delimiterExpression = Bridge.as(variadicExpression.getItem(1), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                    var other = variadicExpression.getItem(0);
                    var variadicContent;
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
                    if (Bridge.hasValue(variadicContent) && variadicContent.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                        var suggestion = Bridge.as(variadicContent.clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                        for (var i = 0; i < suggestion.getCount(); i++) {
                            suggestion.getItem(i).replace(new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, other, suggestion.getItem(i).clone()));
                        }
                        return suggestion;
                    }
                    else  {
                        return new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, other, delimiterExpression.getExpression());
                    }
                }
                return null;
            },
            splittingFractions: function (expression, selection) {
                var $t, $t1;
                var binaryExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
    
                if (Bridge.hasValue(binaryExpression) && binaryExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                    var numerators = Bridge.as(binaryExpression.getLeft().clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
    
                    if (Bridge.hasValue(numerators) && numerators.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add && numerators.getCount() > 1) {
                        var fractionList = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                        $t = Bridge.getEnumerator(numerators);
                        while ($t.moveNext()) {
                            var i = $t.getCurrent();
                            fractionList.add(new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(i, binaryExpression.getRight().clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide));
                        }
                        var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, fractionList.getItem(0), fractionList.getItem(1));
                        $t1 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(fractionList).skip(2));
                        while ($t1.moveNext()) {
                            var i1 = $t1.getCurrent();
                            suggestion.add(i1);
                        }
                        //Den returnerer en anderledes rækkefølge, dette skal fikses
                        return suggestion;
                    }
                }
                return null;
            },
            commonPowerParenthesisRule: function (expression, selection) {
                var $t;
                if (selection.getCount() < 2) {
                    return null;
                }
    
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                    if (Bridge.Linq.Enumerable.from(selection).all($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f5)) {
                        var commonPower = null;
                        var selectionOne = Bridge.as(selection.getItem(0).getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                        var selectionTwo = Bridge.as(selection.getItem(1).getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                        if (selection.getItem(0) === selectionOne.getRight() && selection.getItem(1) === selectionTwo.getRight()) {
                            commonPower = selection.getItem(0).clone();
                        }
                        else  {
                            return null;
                        }
    
                        if (!Bridge.Linq.Enumerable.from(selection).all(function (e) {
                            return ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(e, commonPower);
                        })) {
                            return null;
                        }
    
                        var baselist = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, selectionOne.getLeft().clone(), selectionTwo.getLeft().clone());
                        $t = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(selection).skip(2));
                        while ($t.moveNext()) {
                            var item = $t.getCurrent();
                            var selectionParent = Bridge.as(item.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                            if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(item, commonPower) && selectionParent.getRight() === item) {
                                baselist.add(selectionParent.getLeft().clone());
                            }
                            else  {
                                return null;
                            }
                        }
                        var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(baselist), commonPower, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                        if (variadicExpression.getCount() === selection.getCount()) {
                            return suggestion;
                        }
                    }
                }
                return null;
            },
            factorizationRule: function (expression, selection) {
                if (selection.getCount() !== 1) {
                    return null;
                }
    
                var suggestion;
    
                if (Bridge.is(expression, ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression) && Bridge.hasValue(expression)) {
                    var numericExpression = Bridge.as(selection.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                    var n = numericExpression.number;
                    for (var count = 2; count < n; count++) {
                        if (n % count === 0) {
                            var a = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(count);
                            var b = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(Bridge.Int.div(n, count));
    
                            suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, a, b);
    
                            return suggestion;
                        }
                    }
                }
                return null;
            },
            reverseCommonPowerParenthesisRule: function (expression, selection) {
                var $t, $t1;
                if (selection.getCount() !== 2) {
                    return null;
                }
                var binaryExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                var itemsInParenthesis = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                var suggestion;
                if (Bridge.hasValue(binaryExpression) && binaryExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                    var commonparrent = binaryExpression.getRight().clone();
                    if (Bridge.is(binaryExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression)) {
                        var delimiterExpression = Bridge.as(binaryExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                        if (Bridge.is(delimiterExpression.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)) {
                            var variadicExpression = Bridge.as(delimiterExpression.getExpression().clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                            if (variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                                $t = Bridge.getEnumerator(variadicExpression);
                                while ($t.moveNext()) {
                                    var item = $t.getCurrent();
                                    itemsInParenthesis.add(new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(item, commonparrent, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power));
                                }
                                suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(itemsInParenthesis.getItem(0).clone()), new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(itemsInParenthesis.getItem(1).clone()));
                                if (itemsInParenthesis.getCount() > 2) {
                                    $t1 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(itemsInParenthesis).skip(2));
                                    while ($t1.moveNext()) {
                                        var item1 = $t1.getCurrent();
                                        suggestion.add(new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(item1.clone()));
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
            parenthesisPowerRule: function (expression, selection) {
                var binaryOperatorExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(binaryOperatorExpression) && binaryOperatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                    var delimiterBase = Bridge.as(binaryOperatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                    if (Bridge.hasValue(delimiterBase)) {
                        var binaryBase = Bridge.as(delimiterBase.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                        if (Bridge.hasValue(binaryBase)) {
                            return new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(binaryBase.getLeft().clone(), new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(binaryBase.getRight().clone()), binaryOperatorExpression.getRight().clone()), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                        }
                    }
                }
                return null;
            },
            squareRootRule: function (expression, selection) {
                var sqrtExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression);
    
                if (!Bridge.hasValue(sqrtExpression)) {
                    return null;
                }
    
                if (sqrtExpression.getFunction() !== "sqrt") {
                    return null;
                }
    
                var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(sqrtExpression.getExpression().clone()), new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1), new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(2), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
    
                return suggestion;
            },
            productOfConstantAndFraction: function (expression, selection) {
                var $t, $t1;
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
    
                if (!Bridge.hasValue(variadicExpression) || variadicExpression.getType() !== ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply || variadicExpression.getCount() !== 2) {
                    return null;
                }
    
                var fraction = null;
                var constant = null;
    
                if (Bridge.hasValue((($t = Bridge.as(variadicExpression.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), fraction = $t, $t))) && fraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                    constant = variadicExpression.getItem(1);
                }
                else  {
                    if (Bridge.hasValue((($t1 = Bridge.as(variadicExpression.getItem(1), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), fraction = $t1, $t1))) && fraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                        constant = variadicExpression.getItem(0);
                    }
                    else  {
                        return null;
                    }
                }
    
                var numerators = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, fraction.getLeft().clone(), constant.clone());
    
                var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(numerators.clone(), fraction.getRight().clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
    
                return suggestion;
            },
            divisionEqualsOneRule: function (expression, selection) {
                var fraction = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(fraction) && fraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                    if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(fraction.getLeft(), fraction.getRight())) {
                        var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1);
                        return suggestion;
                    }
                }
                return null;
            },
            factorizeUnaryMinus: function (expression, selection) {
                var unaryMinusExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                if (Bridge.hasValue(unaryMinusExpression)) {
                    var numericExpression = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1);
                    var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(numericExpression)), unaryMinusExpression.getExpression().clone());
                    return suggestion;
                }
                return null;
            },
            multiplyOneRule: function (expression, selection) {
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply && variadicExpression.getCount() === 2) {
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
                if (selection.getCount() !== 2) {
                    return null;
                }
    
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
    
                if (!Bridge.hasValue(variadicExpression) || variadicExpression.getType() !== ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                    return null;
                }
    
                if (variadicExpression.getItem(0).toString() === "0" || variadicExpression.getItem(0).toString() === "-{0}") {
                    return variadicExpression.getItem(1);
                }
                else  {
                    if (variadicExpression.getItem(1).toString() === "0" || variadicExpression.getItem(1).toString() === "-{0}") {
                        return variadicExpression.getItem(0);
                    }
                    else  {
                        return null;
                    }
                }
            },
            multiplyByNull: function (expression, selection) {
                var $t;
                if (selection.getCount() < 2) {
                    return null;
                }
    
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
    
                if (!Bridge.hasValue(variadicExpression) || variadicExpression.getType() !== ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                    return null;
                }
    
                $t = Bridge.getEnumerator(variadicExpression);
                while ($t.moveNext()) {
                    var item = $t.getCurrent();
                    if (item.toString() === "0" || item.toString() === "-{0}") {
                        return new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(0);
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
        f5: function (s) {
            return Bridge.is(s.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression) && (Bridge.as(s.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression)).getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power;
        }
    });
    
    
    
    Bridge.init();
})(this);
