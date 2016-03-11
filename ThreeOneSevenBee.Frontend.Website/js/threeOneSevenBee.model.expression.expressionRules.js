(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules', {
        statics: {
            itselfRule: function (expression, selection, identity) {
                identity.v = expression.clone();
                return true;
            },
            commutativeRule: function (expression, selection, identity) {
                var $t;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                        identity.v = serializer.deserialize(serializer.serialize(operatorExpression.getRight()) + "+" + serializer.serialize(operatorExpression.getLeft()));
                        return true;
                    }
                    else  {
                        if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                            identity.v = serializer.deserialize(serializer.serialize(operatorExpression.getRight()) + "*" + serializer.serialize(operatorExpression.getLeft()));
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
    
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                        if (Bridge.is(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression)) {
                            var power = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                            var newDivision = Bridge.as(serializer.deserialize("1/b"), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression);
                            var newPower = Bridge.as(serializer.deserialize("a^b"), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression);
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
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
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
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                        var lefthand, righthand;
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), lefthand = $t1, $t1))) && Bridge.hasValue((($t2 = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), righthand = $t2, $t2)))) {
                            if (lefthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide && righthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                                if (lefthand.getRight() === righthand.getRight()) {
                                    var newDivision = Bridge.as(serializer.deserialize("a/b"), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression);
                                    var newAddition = Bridge.as(serializer.deserialize("a+b"), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression);
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
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                        var lefthand, righthand;
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), lefthand = $t1, $t1))) && Bridge.hasValue((($t2 = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), righthand = $t2, $t2)))) {
                            if (lefthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide && righthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                                var division = Bridge.as(serializer.deserialize("a/b"), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression);
    
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
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                        var lefthand, righthand;
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), lefthand = $t1, $t1))) && Bridge.hasValue((($t2 = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), righthand = $t2, $t2)))) {
                            if (lefthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power && righthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                                if (lefthand.getLeft() === righthand.getLeft()) {
                                    // May be missing parenthesis
                                    var newPower = Bridge.as(serializer.deserialize("a^b"), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression);
                                    var newAdd = Bridge.as(serializer.deserialize("a+b"), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression);
                                    newPower.setLeft(lefthand.getLeft());
                                    newAdd.setLeft(lefthand.getRight());
                                    newAdd.setRight(righthand.getRight());
                                    newPower.setRight(newAdd);
                                    identity.v = newPower;
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
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                        var righthand;
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), righthand = $t1, $t1)))) {
                            if (righthand.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power) {
                                identity.v = serializer.deserialize(serializer.serialize(operatorExpression.getLeft()) + "^" + serializer.serialize(righthand.getLeft()) + "*" + serializer.serialize(righthand.getRight()));
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
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power && Bridge.String.equals(operatorExpression.getRight().getValue(), "2")) {
                        var lefthand;
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getLeft(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), lefthand = $t1, $t1)))) {
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
                            if (Bridge.hasValue((($t2 = Bridge.as(delimiterExpression.getExpression(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t2, $t2)))) {
    
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
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                        var righthand;
                        // Skal der ikke tjekkes for:  && operatorExpression.Left is VariableExpression i nedenstående?
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), righthand = $t1, $t1)))) {
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
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                        var righthand;
                        if (Bridge.hasValue((($t1 = Bridge.as(operatorExpression.getRight(), ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), righthand = $t1, $t1)))) {
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
            multiplyingWithOneRule: function (expression, selection, identity) {
                var $t;
                var operatorExpression;
                var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
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
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
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
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
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
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression), operatorExpression = $t, $t)))) {
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
    
    
    
    Bridge.init();
})(this);
