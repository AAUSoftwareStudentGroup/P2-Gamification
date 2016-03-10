(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules', {
        statics: {
            itselfRule: function (expression, selection, identity) {
                identity.v = expression;
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
            }
        }
    });
    
    
    
    Bridge.init();
})(this);
