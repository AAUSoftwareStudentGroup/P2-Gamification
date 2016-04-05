﻿(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules', {
        statics: {
            insertSuggestion: function (indexes, result, suggestion) {
                for (var i = 0; i < indexes.getCount(); i++) {
                    result.removeAt(indexes.getItem(i) - i);
                }
                result.insert(indexes.getItem(0), suggestion);
                return result;
            },
            productToExponentRule: function (expression, selection) {
                if (selection.getCount() < 2) {
                    return null;
                }
    
                var product = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
    
                if (Bridge.hasValue(product) && product.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
    
                    if (Bridge.Linq.Enumerable.from(selection).takeWhile(function (e) {
                        return ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(selection.getItem(0), e) && e.getParent() === expression;
                    }).count() === selection.getCount()) {
                        var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(selection.getItem(0).clone(), new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(selection.getCount()), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
    
                        if (product.getCount() === selection.getCount()) {
                            return new ThreeOneSevenBee.Model.Expression.Identity(suggestion, suggestion);
                        }
                        else  {
                            var indexes = Bridge.Linq.Enumerable.from(selection).select(function (s) {
                                return product.indexOfReference(s);
                            }).where($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f1).toList(Bridge.Int);
                            indexes.sort();
                            var result = Bridge.as(product.clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                            result = Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).insertSuggestion(indexes, result, suggestion);
                            return new ThreeOneSevenBee.Model.Expression.Identity(suggestion, result);
                        }
                    }
                }
                return null;
            },
            exponentToProductRule: function (expression, selection) {
                if (selection.getCount() !== 2) {
                    return null;
                }
    
                var exponent = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
    
                if (Bridge.hasValue(exponent) && exponent.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                    if (selection.getItem(0).getParent() === exponent && selection.getItem(1).getParent() === exponent) {
                        var numericExpression = Bridge.as(exponent.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                        if (!Bridge.hasValue(numericExpression)) {
                            return null;
                        }
                        var number = Bridge.Int.trunc(numericExpression.number);
                        if (number === 0) {
                            return new ThreeOneSevenBee.Model.Expression.Identity(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1), new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1));
                        }
                        else  {
                            if (number === 1) {
                                return new ThreeOneSevenBee.Model.Expression.Identity(exponent.getLeft().clone(), exponent.getLeft().clone());
                            }
                            else  {
                                if (number > 1) {
                                    var result = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, exponent.getLeft().clone(), exponent.getLeft().clone());
                                    for (var i = 2; i < number; i++) {
                                        result.add(exponent.getLeft().clone());
                                    }
                                    return new ThreeOneSevenBee.Model.Expression.Identity(result, result);
                                }
                            }
                        }
                    }
                }
                return null;
            },
            numericVariadicRule: function (expression, selection) {
                var $t;
                if (selection.getCount() < 2) {
                    return null;
                }
    
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression)) {
                    var sum;
                    var operation;
    
                    if (variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                        sum = 1;
                        operation = $_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f2;
                    }
                    else  {
                        if (variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                            sum = 0;
                            operation = $_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f3;
                        }
                        else  {
                            return null;
                        }
                    }
                    $t = Bridge.getEnumerator(selection);
                    while ($t.moveNext()) {
                        var selected = $t.getCurrent();
                        if (selected.getParent() === variadicExpression === false) {
                            return null;
                        }
                        else  {
                            var numericExpression = Bridge.as(selected, ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                            if (Bridge.hasValue(numericExpression)) {
                                sum = operation(sum, Bridge.Int.trunc(numericExpression.number));
                            }
                            else  {
                                return null;
                            }
                        }
                    }
                    if (variadicExpression.getCount() === selection.getCount()) {
                        return new ThreeOneSevenBee.Model.Expression.Identity(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(sum), new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(sum));
                    }
                    var indexes = Bridge.Linq.Enumerable.from(selection).select(function (s) {
                        return variadicExpression.indexOfReference(s);
                    }).where($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f1).toList(Bridge.Int);
                    indexes.sort();
                    var result = Bridge.as(variadicExpression.clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                    result = Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).insertSuggestion(indexes, result, new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(sum));
                    return new ThreeOneSevenBee.Model.Expression.Identity(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(sum), result);
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
                                result = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(Math.pow(numericLeft.number, numericRight.number));
                            }
                            else  {
                                return null;
                            }
                        }
                        if (result.number >= 0) {
                            return new ThreeOneSevenBee.Model.Expression.Identity(result, result);
                        }
                        else  {
                            result.number *= -1;
                            var positiveResult = new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(result);
                            return new ThreeOneSevenBee.Model.Expression.Identity(positiveResult, positiveResult);
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
                        return new ThreeOneSevenBee.Model.Expression.Identity(mysuggestion, mysuggestion);
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
                        return new ThreeOneSevenBee.Model.Expression.Identity(mysuggestion, mysuggestion);
                    }
                }
    
                return null;
            },
            addFractionsWithSameNumerators: function (expression, selection) {
                var $t, $t1, $t2;
                if (selection.getCount() < 2) {
                    return null;
                }
    
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && (variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add || variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract)) {
                    //Makes a variable for the two first fractions, since it is needed to make a VariadicOperatorExpression later on (it has to take at least two elements).
                    var firstFraction = Bridge.as(selection.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                    var secondFraction = Bridge.as(selection.getItem(1), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
    
                    if (Bridge.hasValue(firstFraction) && Bridge.hasValue(secondFraction) && firstFraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide && secondFraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                        var numeratorList = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                        numeratorList.add(firstFraction.getLeft().clone());
                        numeratorList.add(secondFraction.getLeft().clone());
    
                        $t = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(selection).skip(2));
                        while ($t.moveNext()) {
                            var $t1 = (function () {
                                var selected = $t.getCurrent();
                                var fraction = Bridge.as(selected, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                                var test = Bridge.as(fraction.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
    
                                if (Bridge.hasValue(fraction) && fraction.getParent() === variadicExpression && ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(fraction.getRight(), firstFraction.getRight())) {
                                    // Kan det gøres på denne måde?
                                    if (Bridge.hasValue(Bridge.as(fraction.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression))) {
                                        numeratorList.add(Bridge.as(fraction.getLeft().clone(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression));
                                    }
                                    else  {
                                        if (Bridge.hasValue(fraction) && fraction.getParent().getParent() === variadicExpression && ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(fraction.getRight(), firstFraction.getRight())) {
                                            numeratorList.add(new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(fraction.getLeft().clone()));
                                        }
                                        else  {
                                            return {jump: 3, v: null};
                                        }
                                    }
                                }
    
                                var suggestionNumerator = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, firstFraction.getLeft().clone(), secondFraction.getLeft().clone());
                                $t2 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(numeratorList).skip(2));
                                while ($t2.moveNext()) {
                                    var i = $t2.getCurrent();
                                    suggestionNumerator.add(i);
                                }
                                var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(suggestionNumerator, firstFraction.getRight().clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
                                if (variadicExpression.getCount() === selection.getCount()) {
                                    return {jump: 3, v: new ThreeOneSevenBee.Model.Expression.Identity(suggestion, suggestion)};
                                }
                                else  {
                                    var list = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                                    var temp = null;
                                    for (var i1 = 0; i1 < selection.getCount(); i1++) {
                                        temp = Bridge.as(selection.getItem(i1), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                                        if (Bridge.hasValue(temp)) {
                                            list.add(temp);
                                        }
                                    }
                                    var indexes = Bridge.Linq.Enumerable.from(list).select(function (s) {
                                        return variadicExpression.indexOfReference(s);
                                    }).where($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f4).toList(Bridge.Int);
                                    indexes.sort();
                                    var result = Bridge.as(variadicExpression.clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                                    result = Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).insertSuggestion(indexes, result, suggestion);
                                    return {jump: 3, v: new ThreeOneSevenBee.Model.Expression.Identity(suggestion, result)};
                                }
                            }).call(this) || {};
                            if($t1.jump == 3) return $t1.v;
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
                    var powerToOneCount = 0;
    
                    if (Bridge.Linq.Enumerable.from(selection).all($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f5)) {
                        if (Bridge.Linq.Enumerable.from(selection).any($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f6)) {
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
                            return new ThreeOneSevenBee.Model.Expression.Identity(suggestion, suggestion);
                        }
                        else  {
                            var list = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                            var temp = null;
                            var temp2 = null;
                            for (var i2 = 0; i2 < selection.getCount(); i2++) {
                                temp = Bridge.as(selection.getItem(i2).getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                                if (Bridge.hasValue(temp)) {
                                    list.add(temp);
                                }
                                else  {
                                    temp2 = Bridge.as(selection.getItem(i2).getParent(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                                    if (Bridge.hasValue(temp2)) {
                                        list.add(selection.getItem(i2));
                                    }
                                }
                            }
                            var indexes = Bridge.Linq.Enumerable.from(list).select(function (s) {
                                return variadicExpression.indexOfReference(s);
                            }).where($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f7).toList(Bridge.Int);
                            indexes.sort();
                            var result = Bridge.as(variadicExpression.clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
    
                            result = Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).insertSuggestion(indexes, result, suggestion);
                            return new ThreeOneSevenBee.Model.Expression.Identity(suggestion, result);
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
    
                // all selected items must be variadic multiply or the rule doesn't apply
                if (!Bridge.Linq.Enumerable.from(selection).all($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f8)) {
                    return null;
                }
    
                // find common multiplicator
                var common = Bridge.Linq.Enumerable.from(Bridge.Linq.Enumerable.from(selection).select(function(x) { return Bridge.cast(x, Bridge.IEnumerable$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)); }).aggregate($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f9)).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase);
    
                // if no common multiplicators are found, the rule doesn't apply
                if (!Bridge.Linq.Enumerable.from(common).any()) {
                    return null;
                }
    
                variadicExpression = Bridge.cast(variadicExpression.clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                var rest = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                $t = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(selection).select(function(x) { return Bridge.cast(x, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression); }));
                while ($t.moveNext()) {
                    var op = $t.getCurrent();
                    $t1 = Bridge.getEnumerator(op);
                    while ($t1.moveNext()) {
                        var subOp = $t1.getCurrent();
                        if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(subOp, Bridge.Linq.Enumerable.from(common).first())) {
                            continue;
                        }
                        rest.add(subOp);
                        if (Bridge.hasValue(variadicExpression)) {
                            variadicExpression.remove(op);
                        }
                    }
                }
    
                var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, Bridge.Linq.Enumerable.from(common).first(), new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, rest.getItem(0), rest.getItem(1), [Bridge.Linq.Enumerable.from(rest).skip(2).toArray()])));
    
                var result = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, variadicExpression, suggestion);
    
                return new ThreeOneSevenBee.Model.Expression.Identity(suggestion, result);
            },
            splittingFractions: function (expression, selection) {
                var $t, $t1;
                if (selection.getCount() !== 1) {
                    return null;
                }
                var binaryExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                var test = Bridge.as(selection.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (!binaryExpression === test) {
                    return null;
                }
                if (Bridge.hasValue(binaryExpression) && binaryExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                    var numerators = Bridge.as(binaryExpression.getLeft().clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                    if (Bridge.hasValue(numerators) && numerators.getCount() > 1) {
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
                        return new ThreeOneSevenBee.Model.Expression.Identity(suggestion, suggestion);
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
                    if (Bridge.Linq.Enumerable.from(selection).all($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f10)) {
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
                            return new ThreeOneSevenBee.Model.Expression.Identity(suggestion, suggestion);
                        }
                        else  {
                            var list = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                            var temp = null;
                            for (var i = 0; i < selection.getCount(); i++) {
                                temp = Bridge.as(selection.getItem(i).getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                                if (Bridge.hasValue(temp)) {
                                    list.add(temp);
                                }
                            }
                            var indexes = Bridge.Linq.Enumerable.from(list).select(function (s) {
                                return variadicExpression.indexOfReference(s);
                            }).where($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f11).toList(Bridge.Int);
                            indexes.sort();
                            var result = Bridge.as(variadicExpression.clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                            result = Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).insertSuggestion(indexes, result, suggestion);
                            return new ThreeOneSevenBee.Model.Expression.Identity(suggestion, result);
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
                    var commonparrent = binaryExpression.getRight();
                    if (Bridge.is(binaryExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression)) {
                        var delimiterExpression = Bridge.as(binaryExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                        if (Bridge.is(delimiterExpression.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)) {
                            var variadicExpression = Bridge.as(delimiterExpression.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                            if (variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                                $t = Bridge.getEnumerator(variadicExpression);
                                while ($t.moveNext()) {
                                    var item = $t.getCurrent();
                                    itemsInParenthesis.add(new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(item, commonparrent, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power));
                                }
                                suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, itemsInParenthesis.getItem(0), itemsInParenthesis.getItem(1));
                                if (itemsInParenthesis.getCount() > 2) {
                                    $t1 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(itemsInParenthesis).skip(2));
                                    while ($t1.moveNext()) {
                                        var item1 = $t1.getCurrent();
                                        suggestion.add(item1);
                                    }
                                    return new ThreeOneSevenBee.Model.Expression.Identity(suggestion, suggestion);
                                }
                                else  {
                                    return new ThreeOneSevenBee.Model.Expression.Identity(suggestion, suggestion);
                                }
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
        f1: function (i) {
            return i !== -1;
        },
        f2: function (a, b) {
            return a * b;
        },
        f3: function (a, b) {
            return a + b;
        },
        f4: function (i2) {
            return i2 !== -1;
        },
        f5: function (s) {
            return Bridge.is(s.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression) && (Bridge.as(s.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression)).getLeft() === s || Bridge.is(s.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
        },
        f6: function (s) {
            return Bridge.hasValue(s.getParent()) && Bridge.is(s.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression) && Bridge.is(s.getParent().getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression) && (Bridge.as(s.getParent().getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression)).getLeft() === s === false;
        },
        f7: function (i3) {
            return i3 !== -1;
        },
        f8: function (e) {
            return Bridge.is(e, ThreeOneSevenBee.Model.Expression.Expressions.VariadicExpression) && (Bridge.as(e, ThreeOneSevenBee.Model.Expression.Expressions.VariadicExpression)).getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply;
        },
        f9: function (x, y) {
            return Bridge.Linq.Enumerable.from(x).intersect(y);
        },
        f10: function (s) {
            return Bridge.is(s.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression) && (Bridge.as(s.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression)).getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power;
        },
        f11: function (i1) {
            return i1 !== -1;
        }
    });
    
    
    
    Bridge.init();
})(this);
