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
                if (selection.getCount() === 1 && selection.getItem(0) === expression && Bridge.hasValue(fraction) && fraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                    var exponent = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(fraction.getRight().clone()), new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1)), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
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
                    return parenthesis.getExpression();
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
            numericBinaryRule: function (expression, selection) {
                if (selection.getCount() < 2) {
                    return null;
                }
    
                var binaryExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(binaryExpression) && selection.getItem(0).getParent() === binaryExpression && selection.getItem(1).getParent() === binaryExpression) {
                    var numericLeft = Bridge.as(binaryExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                    var numericRight = Bridge.as(binaryExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                    if (Bridge.hasValue(numericLeft) && Bridge.hasValue(numericRight)) {
                        var result;
                        if (binaryExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract) {
                            result = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(numericLeft.number - numericRight.number);
                        }
                        else  {
                            if (binaryExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                                result = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(Bridge.Int.trunc(Math.pow(numericLeft.number, numericRight.number)));
                            }
                            else  {
                                return null;
                            }
                        }
                        if (result.number >= 0) {
                            return result;
                        }
                        else  {
                            result.number *= -1;
                            var negativeResult = new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(result);
                            return negativeResult;
                        }
    
                    }
                }
                return null;
            },
            variableWithNegativeExponent: function (expression, selection) {
                if (selection.getCount() < 2) {
                    return null;
                }
    
                var binaryexpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(binaryexpression) && binaryexpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power && selection.getItem(0).getParent() === binaryexpression && selection.getItem(1).getParent() === binaryexpression) {
                    var unaryexpression = Bridge.as(binaryexpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                    if (Bridge.hasValue(unaryexpression) && Bridge.hasValue(binaryexpression.getLeft()) && Bridge.hasValue(binaryexpression.getRight())) {
                        var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                        var power = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(binaryexpression.getLeft().clone(), unaryexpression.getExpression().clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                        var mysuggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1), power, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
                        return mysuggestion;
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
            addFractionsWithSameNumerators: function (expression, selection) {
                var $t, $t1;
                if (selection.getCount() < 2) {
                    return null;
                }
    
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                    //Makes a variable for the two first fractions, since it is needed to make a VariadicOperatorExpression later on (it has to take at least two elements).
    
                    var firstFraction = Bridge.as(selection.getItem(0).clone(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                    var secondFraction = Bridge.as(selection.getItem(1).clone(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
    
                    if (Bridge.hasValue(firstFraction) && Bridge.hasValue(secondFraction) && firstFraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide && secondFraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                        var numeratorList = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
    
                        $t = Bridge.getEnumerator(selection);
                        while ($t.moveNext()) {
                            var selected = $t.getCurrent();
                            var fraction = Bridge.as(selected, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                            var minusFraction = Bridge.as(fraction.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
    
                            if (Bridge.hasValue(fraction) && fraction.getParent() === variadicExpression && ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(fraction.getRight(), firstFraction.getRight())) {
                                numeratorList.add(fraction.getLeft().clone());
                            }
                            else  {
                                if (Bridge.hasValue(minusFraction) && minusFraction.getParent() === variadicExpression && ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(fraction.getRight(), firstFraction.getRight())) {
                                    numeratorList.add(new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(fraction.getLeft().clone()));
                                }
                            }
                        }
                        var suggestionNumerator = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, numeratorList.getItem(0), numeratorList.getItem(1));
                        $t1 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(numeratorList).skip(2));
                        while ($t1.moveNext()) {
                            var i = $t1.getCurrent();
                            suggestionNumerator.add(i);
                        }
    
                        var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(suggestionNumerator, firstFraction.getRight().clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
    
                        if (variadicExpression.getCount() === selection.getCount()) {
                            return suggestion;
                        }
                    }
                }
                return null;
            },
            exponentProduct: function (expression, selection) {
                var $t;
                if (selection.getCount() < 2) {
                    return null;
                }
    
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                    var baseSelection = selection.getItem(0).clone();
                    var variableIsPowerToOne = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression)();
    
                    if (Bridge.Linq.Enumerable.from(selection).all(function (s) {
                        return (Bridge.is(s.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression) && (Bridge.as(s.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression)).getLeft() === s && s.getParent().getParent() === expression) || s.getParent() === expression;
                    })) {
                        if (Bridge.Linq.Enumerable.from(selection).any($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f1)) {
                            return null;
                        }
                        var numeratorList = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                        for (var j = 0; j < selection.getCount(); j++) {
                            if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(baseSelection, selection.getItem(j))) {
                                var parent = Bridge.as(selection.getItem(j).getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                                if (Bridge.hasValue(parent) && parent.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                                    if (Bridge.is(parent.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)) {
                                        var numbers = Bridge.cast(parent.getRight().clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                                        if (numbers.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                                            for (var i = 0; i < numbers.getCount(); i++) {
                                                numeratorList.add(numbers.getItem(i));
                                            }
                                        }
                                    }
                                    else  {
                                        numeratorList.add(parent.getRight().clone());
                                    }
                                }
                                else  {
                                    if (Bridge.is(selection.getItem(j).getParent(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)) {
                                        numeratorList.add(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1));
    
                                    }
                                }
                            }
                            else  {
                                return null;
                            }
                        }
                        var suggestionNumerator = null;
                        suggestionNumerator = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, numeratorList.getItem(0), numeratorList.getItem(1));
    
                        $t = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(numeratorList).skip(2));
                        while ($t.moveNext()) {
                            var i1 = $t.getCurrent();
                            suggestionNumerator.add(i1);
                        }
    
                        var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(baseSelection.clone(), suggestionNumerator, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                        if (variadicExpression.getCount() === selection.getCount()) {
                            return suggestion;
                        }
                    }
                }
                return null;
            },
            productParenthesis: function (expression, selection) {
                var $t, $t1;
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (!Bridge.hasValue(variadicExpression)) {
                    return null;
                }
    
                if (variadicExpression.getType() !== ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                    return null;
                }
    
                // at least 2 items must be selected or the rule doesn't apply
                if (selection.getCount() < 2) {
                    return null;
                }
    
                // all selected items must be v
                if (!Bridge.Linq.Enumerable.from(selection).all($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f2)) {
                    return null;
                }
                var selectionBase = selection.getItem(0).clone();
    
                if (Bridge.Linq.Enumerable.from(selection).any(function (s) {
                    return ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Inequality(s, selectionBase);
                })) {
                    return null;
                }
                var list = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
    
                $t = Bridge.getEnumerator(selection);
                while ($t.moveNext()) {
                    var selected = $t.getCurrent();
                    var selectedParent = Bridge.as(selected.getParent().clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                    if (Bridge.hasValue(selectedParent) && selectedParent.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                        selectedParent.remove(selected);
                        if (selectedParent.getCount() < 2) {
                            list.add(selectedParent.getItem(0));
                        }
                        else  {
                            list.add(selectedParent);
                        }
                    }
                    else  {
                        return null;
                    }
                }
                if (list.getCount() < 2) {
                    return null;
                }
                var withinDelimiter = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, list.getItem(0), list.getItem(1));
                if (list.getCount() > 2) {
                    $t1 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(list).skip((2)));
                    while ($t1.moveNext()) {
                        var item = $t1.getCurrent();
                        withinDelimiter.add(item);
                    }
                }
                var DelimiterExpression = new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(withinDelimiter);
                var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, selectionBase, DelimiterExpression);
                if (selection.getCount() === variadicExpression.getCount()) {
                    return suggestion;
                }
                return null;
            },
            reverseProductParenthesis: function (expression, selection) {
                var $t, $t1, $t2;
                if (selection.getCount() !== 2) {
                    return null;
                }
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (!(Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply)) {
                    return null;
                }
                if (!(Bridge.is(selection.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression) || Bridge.is(selection.getItem(1), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression))) {
                    return null;
                }
                var delimiterExpression;
                var notParenthesis;
                if (Bridge.is(selection.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression)) {
                    delimiterExpression = Bridge.as(selection.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                    notParenthesis = selection.getItem(1);
                }
                else  {
                    delimiterExpression = Bridge.as(selection.getItem(1), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                    notParenthesis = selection.getItem(0);
                }
    
                var suggestionList = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                var expressionType = Bridge.as(delimiterExpression.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (!(Bridge.hasValue(expressionType) && expressionType.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add)) {
                    return null;
                }
    
                $t = Bridge.getEnumerator(expressionType);
                while ($t.moveNext()) {
                    var item = $t.getCurrent();
                    suggestionList.add(item);
                }
    
                var tempList = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)();
    
                $t1 = Bridge.getEnumerator(suggestionList);
                while ($t1.moveNext()) {
                    var item1 = $t1.getCurrent();
                    var temp = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, notParenthesis.clone(), item1.clone());
                    tempList.add(temp);
                }
    
                var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, tempList.getItem(0), tempList.getItem(1));
    
                $t2 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(tempList).skip(2));
                while ($t2.moveNext()) {
                    var item2 = $t2.getCurrent();
                    suggestion.add$3(item2);
                }
                return suggestion;
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
                    if (Bridge.Linq.Enumerable.from(selection).all($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f3)) {
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
            parenthesisPowerRule: function (expression, selection) {
                var binaryOperatorExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(binaryOperatorExpression) && binaryOperatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                    var delimiterBase = Bridge.as(binaryOperatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                    if (Bridge.hasValue(delimiterBase)) {
                        var binaryBase = Bridge.as(delimiterBase.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                        if (Bridge.hasValue(binaryBase)) {
                            return new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(binaryBase.getLeft(), new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, binaryBase.getRight(), binaryOperatorExpression.getRight()), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
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
    
                if (Bridge.hasValue((($t = Bridge.as(variadicExpression.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), fraction = $t, $t))) && fraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide && variadicExpression.getItem(0).getParent() === expression) {
                    constant = Bridge.Linq.Enumerable.from(variadicExpression.getItem(1).getParentPath()).first(function (e) {
                        return e.getParent() === expression;
                    }).clone();
                }
                else  {
                    if (Bridge.hasValue((($t1 = Bridge.as(variadicExpression.getItem(1), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), fraction = $t1, $t1))) && fraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide && variadicExpression.getItem(1).getParent() === expression) {
                        constant = Bridge.Linq.Enumerable.from(variadicExpression.getItem(0).getParentPath()).first(function (e) {
                            return e.getParent() === expression;
                        }).clone();
                    }
                    else  {
                        return null;
                    }
                }
    
                var numerators = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, fraction.getLeft(), constant);
    
                var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(numerators, fraction.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
    
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
                    var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(numericExpression), unaryMinusExpression.getExpression().clone());
                    return suggestion;
                }
                return null;
            },
            productOfOneAndSomethingRule: function (expression, selection) {
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                var one;
    
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply && variadicExpression.getCount() === 2) {
                    if (Bridge.is(variadicExpression.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression)) {
                        one = Bridge.as(variadicExpression.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                        if (one.number === 1) {
                            var suggestion = Bridge.Linq.Enumerable.from(variadicExpression.getItem(1).getParentPath()).first(function (e) {
                                return e.getParent() === expression;
                            });
                            return suggestion.clone();
                        }
                    }
                    else  {
                        if (Bridge.is(variadicExpression.getItem(1), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression)) {
                            one = Bridge.as(variadicExpression.getItem(1), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                            if (one.number === 1) {
                                var suggestion1 = Bridge.Linq.Enumerable.from(variadicExpression.getItem(0).getParentPath()).first(function (e) {
                                    return e.getParent() === expression;
                                }).clone();
                                return suggestion1.clone();
                            }
                        }
                        else  {
                            return null;
                        }
                    }
    
                    // Der er en bug med power
                }
                return null;
            }
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules, {
        f1: function (s) {
            return Bridge.hasValue(s.getParent()) && Bridge.is(s.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression) && Bridge.is(s.getParent().getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression) && (Bridge.as(s.getParent().getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression)).getLeft() === s === false;
        },
        f2: function (e) {
            return Bridge.is(e, ThreeOneSevenBee.Model.Expression.Expressions.VariableExpression) || Bridge.is(e, ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression) || Bridge.is(e, ThreeOneSevenBee.Model.Expression.Expressions.ConstantExpression);
        },
        f3: function (s) {
            return Bridge.is(s.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression) && (Bridge.as(s.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression)).getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power;
        }
    });
    
    
    
    Bridge.init();
})(this);
