(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules', {
        statics: {
            checkParentPaths: function (checkString, expressions) {
                var $t;
                if (expressions === void 0) { expressions = []; }
                $t = Bridge.getEnumerator(expressions);
                while ($t.moveNext()) {
                    var expression = $t.getCurrent();
                    var parentPath = Bridge.Linq.Enumerable.from(expression.getParentPath()).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase);
                    if (parentPath.getCount() !== checkString.length) {
                        return false;
                    }
                    for (var index = 0; index < parentPath.getCount(); index++) {
                        var expr = parentPath.getItem(index);
                        var c = checkString.charCodeAt(index);
    
                        var numExpr = Bridge.as(expr, ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                        if (Bridge.hasValue(numExpr) && c !== 110 && c !== 108) {
                            return false;
                        }
    
                        var varExpr = Bridge.as(expr, ThreeOneSevenBee.Model.Expression.Expressions.VariableExpression);
                        if (Bridge.hasValue(varExpr) && c !== 118 && c !== 108) {
                            return false;
                        }
    
                        var minusExpr = Bridge.as(expr, ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                        if (Bridge.hasValue(minusExpr) && c !== 95) {
                            return false;
                        }
    
                        var variadicExpr = Bridge.as(expr, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                        if (Bridge.hasValue(variadicExpr)) {
                            if (variadicExpr.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add && c !== 43) {
                                return false;
                            }
                            if (variadicExpr.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply && c !== 42) {
                                return false;
                            }
                        }
    
                        var binaryExpr = Bridge.as(expr, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                        if (Bridge.hasValue(binaryExpr)) {
                            if (binaryExpr.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide && c !== 47) {
                                return false;
                            }
                            if (binaryExpr.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power && c !== 94) {
                                return false;
                            }
                            if (binaryExpr.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract && c !== 45) {
                                return false;
                            }
                        }
                    }
                }
                return true;
            },
            allEqual: function (expressions) {
                if (expressions.getCount() < 1) {
                    return false;
                }
                for (var i = 1; i < expressions.getCount(); i++) {
                    if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Inequality(expressions.getItem(i - 1), expressions.getItem(i))) {
                        return false;
                    }
                }
                return true;
            },
            divideRule: function (expression, selection, identity) {
                if (selection.getCount() > 0 && selection.getCount() % 2 === 0 && Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).allEqual(selection) && Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).checkParentPaths("l*/", selection.toArray())) {
                    var fraction = Bridge.as(selection.getItem(0).getParent().getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryExpression);
                    var left = Bridge.as(fraction.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                    var right = Bridge.as(fraction.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                    var leftIndexes = Bridge.Linq.Enumerable.from(selection).select(function (e) {
                        return left.indexOfReference(e);
                    }).where($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f1).toList(Bridge.Int);
                    var rightIndexes = Bridge.Linq.Enumerable.from(selection).select(function (e) {
                        return right.indexOfReference(e);
                    }).where($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules.f1).toList(Bridge.Int);
                    if (leftIndexes.getCount() === rightIndexes.getCount()) {
                        leftIndexes.sort();
                        rightIndexes.sort();
                        var temp = Bridge.as(expression.clone(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryExpression);
                        var tempLeft = Bridge.as(temp.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                        var tempRight = Bridge.as(temp.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                        for (var index = 0; index < leftIndexes.getCount(); index++) {
                            tempLeft.removeAt(leftIndexes.getItem(index) - index);
                        }
                        for (var index1 = 0; index1 < rightIndexes.getCount(); index1++) {
                            tempRight.removeAt(rightIndexes.getItem(index1) - index1);
                        }
                        identity.v = temp;
                        return true;
                    }
                }
                identity.v = null;
                return false;
    
            },
            itselfRule: function (expression, selection, identity) {
                identity.v = expression.clone();
                return true;
            },
            productToExponentRule: function (expression, selection, identity) {
                var operatorExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(operatorExpression) && selection.getCount() > 0 && operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                    for (var index = 0; index < selection.getCount(); index++) {
                        if (selection.getItem(index).getParent() === operatorExpression === false) {
                            identity.v = null;
                            return false;
                        }
                        if (index > 0 && ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Inequality(selection.getItem(index - 1), selection.getItem(index))) {
                            identity.v = null;
                            return false;
                        }
                    }
                    var indexes = new Bridge.List$1(Bridge.Int)();
                    for (var index1 = 0; index1 < operatorExpression.getCount(); index1++) {
                        for (var selctionIndex = 0; selctionIndex < selection.getCount(); selctionIndex++) {
                            if (operatorExpression.getItem(index1) === selection.getItem(selctionIndex)) {
                                indexes.add(index1);
                            }
                        }
                    }
                    var temp = Bridge.as(operatorExpression.clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                    for (var index2 = 1; index2 < indexes.getCount(); index2++) {
                        temp.removeAt(indexes.getItem(index2) - index2 + 1);
                    }
    
                    temp.setItem(indexes.getItem(0), new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(selection.getItem(0).clone(), new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(selection.getCount()), ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power));
                    temp.getItem(indexes.getItem(0)).setParent(temp);
                    identity.v = temp;
                    return true;
                }
                identity.v = null;
                return false;
            },
            exponentToProductRule: function (expression, selection, identity) {
                var binaryExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryExpression);
                if (Bridge.hasValue(binaryExpression)) {
                    if (selection.getCount() === 2 && (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(selection.getItem(0), binaryExpression.getLeft()) && ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(selection.getItem(1), binaryExpression.getRight())) || (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(selection.getItem(1), binaryExpression.getLeft()) && ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(selection.getItem(0), binaryExpression.getRight()))) {
                        var numericExpression = Bridge.as(binaryExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression);
                        if (Bridge.hasValue(numericExpression)) {
                            if (numericExpression.getValue() === "0") {
                                identity.v = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(1);
                            }
                            else  {
                                if (numericExpression.getValue() === "1") {
                                    identity.v = binaryExpression.getLeft().clone();
                                }
                                else  {
                                    var temp = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, binaryExpression.getLeft().clone(), binaryExpression.getLeft().clone());
                                    for (var n = 2; n < numericExpression.number; n++) {
                                        temp.add(binaryExpression.getLeft().clone());
                                    }
                                    identity.v = temp;
                                }
                            }
                            return true;
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            commutativeRule: function (expression, selection, identity) {
                var operatorExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue(operatorExpression) && selection.getCount() === 2) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add || operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                        if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(selection.getItem(0).getParent(), expression) && ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(selection.getItem(1).getParent(), expression)) {
                            var temp = Bridge.as(operatorExpression.clone(), ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                            for (var index = 0; index < operatorExpression.getCount(); index++) {
                                if (operatorExpression.getItem(index) === selection.getItem(0)) {
                                    temp.setItem(index, selection.getItem(1).clone());
                                }
                                if (operatorExpression.getItem(index) === selection.getItem(1)) {
                                    temp.setItem(index, selection.getItem(0).clone());
                                }
                                temp.getItem(index).setParent(temp);
                            }
                            identity.v = temp;
                            return true;
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            inversePowerRule: function (expression, selection, identity) {
                var $t;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                identity.v = null;
    
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                        if (Bridge.is(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression)) {
                            var power = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                            var newDivision = Bridge.as(serializer.deserialize("1/b"), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                            var newPower = Bridge.as(serializer.deserialize("a^b"), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                            newPower.setLeft(operatorExpression.getLeft());
                            newPower.setRight(power.getExpression());
                            newDivision.setRight(newPower);
                            identity.v = newDivision;
                            return true;
                        }
                    }
                }
                return false;
            },
            powerZeroRule: function (expression, selection, identity) {
                var $t;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                        if (Bridge.String.equals(operatorExpression.getRight().getValue(), "0")) {
                            identity.v = serializer.deserialize("1");
                            return true;
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            fractionAddRule: function (expression, selection, identity) {
                var $t, $t1, $t2;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                        var lefthand, righthand;
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), lefthand = $t1, $t1))) && Bridge.hasValue((($t2 = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), righthand = $t2, $t2)))) {
                            if (lefthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide && righthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                                if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(lefthand.getRight(), righthand.getRight())) {
                                    var newDivision = Bridge.as(serializer.deserialize("a/b"), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                                    var newAddition = Bridge.as(serializer.deserialize("a+b"), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                                    newAddition.setLeft(lefthand.getLeft());
                                    newAddition.setRight(righthand.getLeft());
                                    newDivision.setLeft(newAddition);
                                    newDivision.setRight(lefthand.getRight());
                                    identity.v = newDivision;
                                    return true;
                                }
                            }
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            fractionMultiplyRule: function (expression, selection, identity) {
                var $t, $t1, $t2;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                        var lefthand, righthand;
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), lefthand = $t1, $t1))) && Bridge.hasValue((($t2 = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), righthand = $t2, $t2)))) {
                            if (lefthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide && righthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                                var division = Bridge.as(serializer.deserialize("a/b"), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
    
                                division.setLeft((serializer.deserialize(serializer.serialize(lefthand.getLeft()) + "*" + serializer.serialize(righthand.getLeft()))));
                                division.setRight((serializer.deserialize(serializer.serialize(lefthand.getRight()) + "*" + serializer.serialize(righthand.getRight()))));
                                identity.v = division;
                                return true;
                            }
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            sameVariableDifferentExpMultiplyRule: function (expression, selection, identity) {
                var $t, $t1, $t2;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                        var lefthand, righthand;
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), lefthand = $t1, $t1))) && Bridge.hasValue((($t2 = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), righthand = $t2, $t2)))) {
                            if (lefthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power && righthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                                if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(lefthand.getLeft(), righthand.getLeft())) {
                                    // May be missing parenthesis 
                                    identity.v = serializer.deserialize(serializer.serialize(lefthand.getLeft()) + "^" + serializer.serialize(lefthand.getRight()) + "+" + serializer.serialize(righthand.getRight()));
                                    return true;
                                }
                            }
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            variableWithTwoExponent: function (expression, selection, identity) {
                var $t, $t1;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                        var lefthand;
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), lefthand = $t1, $t1)))) {
                            if (lefthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                                identity.v = serializer.deserialize(serializer.serialize(lefthand.getLeft()) + "^" + serializer.serialize(lefthand.getRight()) + "*" + serializer.serialize(operatorExpression.getRight()));
                                return true;
                            }
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            squareSentenceRule: function (expression, selection, identity) {
                var $t, $t1;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power && Bridge.String.equals(operatorExpression.getRight().getValue(), "2")) {
                        var lefthand;
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), lefthand = $t1, $t1)))) {
                            if (lefthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                                identity.v = serializer.deserialize(serializer.serialize(lefthand.getLeft()) + "^" + serializer.serialize(operatorExpression.getRight()) + "+" + serializer.serialize(lefthand.getRight()) + "^" + serializer.serialize(operatorExpression.getRight()) + "+" + serializer.serialize(operatorExpression.getRight()) + "*" + serializer.serialize(lefthand.getLeft()) + "*" + serializer.serialize(lefthand.getRight()));
                                return true;
                            }
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            squareRootAndPowerRule: function (expression, selection, identity) {
                var $t, $t1, $t2;
                var functionExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                var operatorExpression;
                var delimiterExpression;
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression), functionExpression = $t, $t)))) {
                    if (functionExpression.getFunction() === "sqrt") {
                        if (Bridge.hasValue((($t1 = Bridge.as(functionExpression.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression), delimiterExpression = $t1, $t1)))) {
                            if (Bridge.hasValue((($t2 = Bridge.as(delimiterExpression.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t2, $t2)))) {
    
                                if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power && Bridge.String.equals(operatorExpression.getRight().getValue(), "2")) {
                                    identity.v = operatorExpression.getLeft();
                                    return true;
                                }
                            }
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            fractionVariableMultiplyRule: function (expression, selection, identity) {
                var $t, $t1;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                        var righthand;
                        // Skal der ikke tjekkes for:  && operatorExpression.Left is VariableExpression i nedenstående?
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), righthand = $t1, $t1)))) {
                            if (righthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                                identity.v = serializer.deserialize("a/" + serializer.serialize(righthand.getLeft()));
                                identity.v.replace(serializer.deserialize("a"), serializer.deserialize(serializer.serialize(operatorExpression.getLeft()) + "*" + serializer.serialize(righthand.getLeft())));
                                return true;
                            }
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            multiplyVariableIntoParentheses: function (expression, selection, identity) {
                var $t, $t1;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                        var righthand;
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), righthand = $t1, $t1)))) {
                            if (righthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                                identity.v = serializer.deserialize(serializer.serialize(operatorExpression.getLeft()) + "*" + serializer.serialize(righthand.getLeft()) + "+" + serializer.serialize(operatorExpression.getLeft()) + "*" + serializer.serialize(righthand.getRight()));
                                return true;
                            }
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            powerOfVariablesMultiplied: function (expression, selection, identity) {
                var $t, $t1;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                        var lefthand;
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), lefthand = $t1, $t1)))) {
                            if (lefthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                                identity.v = serializer.deserialize(serializer.serialize(lefthand.getLeft()) + "^" + serializer.serialize(operatorExpression.getRight()) + "+" + serializer.serialize(lefthand.getRight()) + "^" + serializer.serialize(operatorExpression.getRight()));
                                return true;
                            }
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            multiplyingWithOneRule: function (expression, selection, identity) {
                var $t;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply && Bridge.String.equals(operatorExpression.getLeft().getValue(), "1")) {
                        identity.v = operatorExpression.getRight();
                        return true;
                    }
                    else  {
                        if ((operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply && Bridge.String.equals(operatorExpression.getRight().getValue(), "1"))) {
                            identity.v = operatorExpression.getLeft();
                            return true;
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            denumeratorIsOneRule: function (expression, selection, identity) {
                var $t;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                        if (Bridge.String.equals(operatorExpression.getRight().getValue(), "1")) {
                            identity.v = operatorExpression.getLeft();
                            return true;
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            numeratorIsZero: function (expression, selection, identity) {
                var $t;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                        if (Bridge.String.equals(operatorExpression.getLeft().getValue(), "0")) {
                            identity.v = serializer.deserialize("0");
                            return true;
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            removingUnaryMinusInDivisionRule: function (expression, selection, identity) {
                var $t;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                        if ((Bridge.is(operatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression)) && Bridge.is(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression)) {
                            var terminal = Bridge.as(operatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                            var terminal2 = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                            identity.v = serializer.deserialize(serializer.serialize(terminal.getExpression()) + "/" + terminal2.getExpression());
                            return true;
                        }
                    }
                }
                identity.v = null;
                return false;
            },
            doubleMinusEqualsPlus: function (expression, selection, identity) {
                var $t, $t1;
                var operatorExpression;
                var unary1;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression), operatorExpression = $t, $t)))) {
                    if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression), unary1 = $t1, $t1)))) {
                        identity.v = serializer.deserialize(serializer.serialize(unary1.getExpression()));
                        return true;
                    }
                }
                identity.v = null;
                return false;
            }
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules, {
        f1: function (e) {
            return e !== -1;
        }
    });
    
    
    
    Bridge.init();
})(this);
