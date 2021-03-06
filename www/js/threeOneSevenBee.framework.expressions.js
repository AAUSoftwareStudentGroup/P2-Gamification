﻿/* global Bridge */

"use strict";

Bridge.define('ThreeOneSevenBee.Framework.Expressions.DelimiterType', {
    statics: {
        parenthesis: 0,
        curly: 1,
        bracket: 2
    },
    $enum: true
});

Bridge.define('ThreeOneSevenBee.Framework.Expressions.OperatorType', {
    statics: {
        add: 0,
        subtract: 1,
        divide: 2,
        multiply: 3
    },
    $enum: true
});

Bridge.define('ThreeOneSevenBee.Framework.Expressions.VariableExpression', {
    inherits: [ThreeOneSevenBee.Framework.Expression],
    value: null,
    constructor: function (value) {
        ThreeOneSevenBee.Framework.Expression.prototype.$constructor.call(this);

        this.value = value;
    },
    getValue: function () {
        return this.value;
    }
});

Bridge.define('ThreeOneSevenBee.Framework.Expressions.NumericExpression', {
    inherits: [ThreeOneSevenBee.Framework.Expression],
    value: 0,
    constructor: function (value) {
        ThreeOneSevenBee.Framework.Expression.prototype.$constructor.call(this);

        this.value = value;
    },
    getValue: function () {
        return this.value.toString();
    }
});

Bridge.define('ThreeOneSevenBee.Framework.Expressions.DelimiterExpression', {
    inherits: [ThreeOneSevenBee.Framework.Expression],
    config: {
        properties: {
            Expression: null,
            Type: 0
        }
    },
    constructor: function (expression, type) {
        ThreeOneSevenBee.Framework.Expression.prototype.$constructor.call(this);

        this.setExpression(expression);
        this.setType(type);
    },
    getValue: function () {
        switch (this.getType()) {
            case Bridge.get(ThreeOneSevenBee.Framework.Expressions.DelimiterType).parenthesis: 
                return "(" + this.getExpression().toString() + ")";
            case Bridge.get(ThreeOneSevenBee.Framework.Expressions.DelimiterType).curly: 
                return "{" + this.getExpression().toString() + "}";
            case Bridge.get(ThreeOneSevenBee.Framework.Expressions.DelimiterType).bracket: 
                return "[" + this.getExpression().toString() + "]";
            default: 
                throw new Bridge.InvalidOperationException("Invalid expression type: " + this.getType());
        }
    }
});

Bridge.define('ThreeOneSevenBee.Framework.Expressions.BinaryExpression', {
    inherits: [ThreeOneSevenBee.Framework.Expression],
    config: {
        properties: {
            Left: null,
            Right: null
        }
    },
    constructor: function (left, right) {
        ThreeOneSevenBee.Framework.Expression.prototype.$constructor.call(this);

        this.setLeft(left);
        this.setRight(right);
    }
});

Bridge.define('ThreeOneSevenBee.Framework.Expressions.OperatorExpression', {
    inherits: [ThreeOneSevenBee.Framework.Expressions.BinaryExpression],
    statics: {
        config: {
            init: function () {
                this.symbols = Bridge.merge(new Bridge.Dictionary$2(ThreeOneSevenBee.Framework.Expressions.OperatorType,String)(), [
    [Bridge.get(ThreeOneSevenBee.Framework.Expressions.OperatorType).add, "+"],
    [Bridge.get(ThreeOneSevenBee.Framework.Expressions.OperatorType).subtract, "-"],
    [Bridge.get(ThreeOneSevenBee.Framework.Expressions.OperatorType).divide, "/"],
    [Bridge.get(ThreeOneSevenBee.Framework.Expressions.OperatorType).multiply, "*"]
] );
            }
        }
    },
    config: {
        properties: {
            Type: 0
        }
    },
    constructor: function (left, right, type) {
        ThreeOneSevenBee.Framework.Expressions.BinaryExpression.prototype.$constructor.call(this, left, right);

        this.setType(type);
    },
    getValue: function () {
        var symbol;
        if (Bridge.get(ThreeOneSevenBee.Framework.Expressions.OperatorExpression).symbols.tryGetValue(this.getType(), symbol)) {
            return "{" + this.getLeft().toString() + "} " + symbol + " {" + this.getRight().toString() + "}";
        }
        throw new Bridge.InvalidOperationException("Invalid operator type: " + this.getType());
    }
});



Bridge.init();