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
                            for (var i = 0; i < indexes.getCount(); i++) {
                                result.removeAt(indexes.getItem(i) - i);
                            }
                            result.insert(indexes.getItem(0), suggestion);
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
                    for (var i = 0; i < indexes.getCount(); i++) {
                        result.removeAt(indexes.getItem(i) - i);
                    }
                    result.insert(indexes.getItem(0), new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(sum));
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
                var $t, $t1;
                if (selection.getCount() < 2) {
                    return null;
                }
    
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression) && variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                    //Makes a variable for the two first fractions, since it is needed to make a VariadicOperatorExpression later on (it has to take at least two elements).
                    var firstFraction = Bridge.as(selection.getItem(0), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                    var secondFraction = Bridge.as(selection.getItem(1), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
    
                    if (Bridge.hasValue(firstFraction) && Bridge.hasValue(secondFraction) && firstFraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide && secondFraction.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                        var numeratorList = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                        numeratorList.add(firstFraction.getLeft().clone());
                        numeratorList.add(secondFraction.getLeft().clone());
    
                        $t = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(selection).skip(2));
                        while ($t.moveNext()) {
                            var selected = $t.getCurrent();
                            var fraction = Bridge.as(selected, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                            if (Bridge.hasValue(fraction) && fraction.getParent() === variadicExpression && ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(fraction.getRight(), firstFraction.getRight())) {
                                numeratorList.add(fraction.getLeft().clone());
                            }
                            else  {
                                return null;
                            }
                        }
    
                        var suggestionNumerator = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, firstFraction.getLeft().clone(), secondFraction.getLeft().clone());
                        $t1 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(numeratorList).skip(2));
                        while ($t1.moveNext()) {
                            var i = $t1.getCurrent();
                            suggestionNumerator.add(i);
                        }
                        var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(suggestionNumerator, firstFraction.getRight().clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
                        //SHOULD NOT BE SUGGESTION,SUGGESTION, BUT SUGGESTION,RESULT, THIS IS FIXED WHEN THE CORRECT FUNCTION IS IMPLEMENTED!!!!!
                        return new ThreeOneSevenBee.Model.Expression.Identity(suggestion, suggestion);
                    }
                }
                return null;
            },
            exponentProduct: function (expression, selection) {
                var $t, $t1;
                if (selection.getCount() < 2) {
                    return null;
                }
                var variadicexpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicexpression) && variadicexpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                    var baseSelection = null;
                    var firstSelection = null;
                    var secondSelection = null;
    
                    baseSelection = selection.getItem(0);
                    firstSelection = Bridge.as(selection.getItem(0).getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryExpression);
                    secondSelection = Bridge.as(selection.getItem(1).getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryExpression);
                    var numbers;
                    if (Bridge.hasValue(firstSelection) && Bridge.hasValue(secondSelection)) {
                        var numeratorList = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                        $t = Bridge.getEnumerator(selection);
                        while ($t.moveNext()) {
                            var selected = $t.getCurrent();
                            if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(baseSelection, selected)) {
                                var parent = Bridge.as(selected.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryExpression);
                                if (Bridge.hasValue(parent) && parent.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                                    if (Bridge.is(parent.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression)) {
                                        numeratorList.add(parent.getRight().clone());
                                    }
                                    else  {
                                        if (Bridge.is(parent.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)) {
                                            numbers = Bridge.cast(parent.getRight().clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                                            for (var i = 0; i < numbers.getCount(); i++) {
                                                numeratorList.add(numbers.getItem(i));
                                            }
                                        }
                                    }
                                }
                            }
                            else  {
                                return null;
                            }
                        }
                        var suggestionNumerator = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, numeratorList.getItem(0), numeratorList.getItem(1));
                        $t1 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(numeratorList).skip(2));
                        while ($t1.moveNext()) {
                            var i1 = $t1.getCurrent();
                            suggestionNumerator.add(i1);
                        }
                        var suggestion = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(baseSelection.clone(), suggestionNumerator, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                        if (variadicexpression.getCount() === selection.getCount()) {
                            return new ThreeOneSevenBee.Model.Expression.Identity(suggestion, suggestion);
                        }
                        else  {
                            var list = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression)();
                            var temp = null;
                            for (var i2 = 0; i2 < selection.getCount(); i2++) {
                                temp = Bridge.as(selection.getItem(i2).getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                                if (Bridge.hasValue(temp)) {
                                    list.add(temp);
                                }
                            }
                            var indexes = Bridge.Linq.Enumerable.from(list).select(function (s) {
                                return variadicexpression.indexOfReference(s);
                            }).where($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f4).toList(Bridge.Int);
                            indexes.sort();
                            var result = Bridge.as(variadicexpression.clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
    
                            for (var i3 = 0; i3 < indexes.getCount(); i3++) {
                                result.removeAt(indexes.getItem(i3) - i3);
                            }
                            result.insert(indexes.getItem(0), suggestion);
                            return new ThreeOneSevenBee.Model.Expression.Identity(suggestion, result);
                        }
                    }
                }
                return null;
            },
            splittingFractions: function (expression, selection) {
                var $t, $t1;
                if (selection.getCount() < 2) {
                    return null;
                }
                var binaryExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(binaryExpression) && binaryExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                    var numeratorType = Bridge.as(binaryExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                    if (Bridge.hasValue(numeratorType) && numeratorType.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                        var selectedNumerators = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                        $t = Bridge.getEnumerator(selection);
                        while ($t.moveNext()) {
                            var selected = $t.getCurrent();
                            if (!selected.getParent() === binaryExpression) {
                                return null;
                            }
    
                            if (selected.getParent() === binaryExpression.getLeft()) {
                                selectedNumerators.add(selected.clone());
                            }
    
                        }
    
                        if (selectedNumerators.getCount() < 2) {
                            var newFraction = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(selectedNumerators.getItem(0), binaryExpression.getRight().clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
    
    
                            //VariadicOperatorExpression result = new VariadicOperatorExpression(OperatorType.Add, newFraction, something);
    
                            return new ThreeOneSevenBee.Model.Expression.Identity(newFraction, newFraction);
                        }
                        else  {
                            if (selectedNumerators.getCount() >= 2) {
                                var listOfNumerators = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, selectedNumerators.getItem(0), selectedNumerators.getItem(1));
                                $t1 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(selectedNumerators).skip(2));
                                while ($t1.moveNext()) {
                                    var i = $t1.getCurrent();
                                    listOfNumerators.add(i);
                                }
    
                                var newFraction1 = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(listOfNumerators, binaryExpression.getRight().clone(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
                                //Is missing result instead of two times suggestion! The above is missing the same!
                                return new ThreeOneSevenBee.Model.Expression.Identity(newFraction1, newFraction1);
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
        f4: function (i3) {
            return i3 !== -1;
        }
    });
    
    
    
    Bridge.init();
})(this);
