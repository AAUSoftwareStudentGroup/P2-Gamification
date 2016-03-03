(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Framework.Expressions.ConstantType', {
        statics: {
            pi: 0
        },
        $enum: true
    });
    
    Bridge.define('ThreeOneSevenBee.Framework.Expressions.OperatorType', {
        statics: {
            add: 0,
            subtract: 1,
            divide: 2,
            multiply: 3,
            power: 4
        },
        $enum: true
    });
    
    Bridge.define('ThreeOneSevenBee.Framework.Expressions.ConstantExpression', {
        inherits: [ThreeOneSevenBee.Framework.Expression],
        value: null,
        constructor: function (value) {
            ThreeOneSevenBee.Framework.Expression.prototype.$constructor.call(this);
    
            this.value = value;
        },
        getValue: function () {
            return Bridge.Enum.toString(ThreeOneSevenBee.Framework.Expressions.ConstantType, this.value);
        },
        getNodesRecursive: function () {
            var $yield = [];
            $yield.push(this);
            return Bridge.Array.toEnumerable($yield);
        },
        canCalculate: function () {
            switch (this.value) {
                case ThreeOneSevenBee.Framework.Expressions.ConstantType.pi: 
                    return true;
            }
    
            return ThreeOneSevenBee.Framework.Expression.prototype.canCalculate.call(this);
        },
        calculate: function () {
            switch (this.value) {
                case ThreeOneSevenBee.Framework.Expressions.ConstantType.pi: 
                    return Math.PI;
            }
    
            return ThreeOneSevenBee.Framework.Expression.prototype.calculate.call(this);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Framework.Expressions.DelimiterExpression', {
        inherits: [ThreeOneSevenBee.Framework.Expression],
        config: {
            properties: {
                Expression: null
            }
        },
        constructor: function (expression) {
            ThreeOneSevenBee.Framework.Expression.prototype.$constructor.call(this);
    
            this.setExpression(expression);
        },
        getValue: function () {
            return "(" + this.getExpression().toString() + ")";
        },
        canCalculate: function () {
            return this.getExpression().canCalculate();
        },
        calculate: function () {
            return this.getExpression().calculate();
        },
        getNodesRecursive: function () {
            var $t;
            var $yield = [];
            $yield.push(this);
    
            $t = Bridge.getEnumerator(this.getExpression().getNodesRecursive());
            while ($t.moveNext()) {
                var node = $t.getCurrent();
                $yield.push(node);
            }
            return Bridge.Array.toEnumerable($yield);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Framework.Expressions.FunctionExpression', {
        inherits: [ThreeOneSevenBee.Framework.Expression],
        statics: {
            config: {
                init: function () {
                    this.functions = Bridge.merge(new Bridge.Dictionary$2(String,Function)(), [
        ["sqrt", Math.sqrt],
        ["sin", Math.sin],
        ["cos", Math.cos],
        ["tan", Math.tan]
    ] ) || null;
                }
            }
        },
        config: {
            properties: {
                Expression: null,
                Function: null
            }
        },
        constructor: function (expression, $function) {
            ThreeOneSevenBee.Framework.Expression.prototype.$constructor.call(this);
    
            this.setExpression(expression);
            this.setFunction($function.toLowerCase());
        },
        getValue: function () {
            return this.getFunction() + this.getExpression();
        },
        canCalculate: function () {
            if (Bridge.get(ThreeOneSevenBee.Framework.Expressions.FunctionExpression).functions.containsKey(this.getFunction())) {
                return this.getExpression().canCalculate();
            }
            return ThreeOneSevenBee.Framework.Expression.prototype.canCalculate.call(this);
        },
        calculate: function () {
            var func = { };
            if (Bridge.get(ThreeOneSevenBee.Framework.Expressions.FunctionExpression).functions.tryGetValue(this.getFunction(), func)) {
                return func.v(Bridge.Nullable.getValue(this.getExpression().calculate()));
            }
            return ThreeOneSevenBee.Framework.Expression.prototype.calculate.call(this);
        },
        getNodesRecursive: function () {
            var $t;
            var $yield = [];
            $yield.push(this);
    
            $t = Bridge.getEnumerator(this.getExpression().getNodesRecursive());
            while ($t.moveNext()) {
                var node = $t.getCurrent();
                $yield.push(node);
            }
            return Bridge.Array.toEnumerable($yield);
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
            return Bridge.Int.format(this.value, 'G');
        },
        getNodesRecursive: function () {
            var $yield = [];
            $yield.push(this);
            return Bridge.Array.toEnumerable($yield);
        },
        canCalculate: function () {
            return true;
        },
        calculate: function () {
            return this.value;
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Framework.Expressions.OperatorExpression', {
        inherits: [ThreeOneSevenBee.Framework.Expression],
        statics: {
            config: {
                init: function () {
                    this.symbols = Bridge.merge(new Bridge.Dictionary$2(ThreeOneSevenBee.Framework.Expressions.OperatorType,String)(), [
        [ThreeOneSevenBee.Framework.Expressions.OperatorType.add, "+"],
        [ThreeOneSevenBee.Framework.Expressions.OperatorType.subtract, "-"],
        [ThreeOneSevenBee.Framework.Expressions.OperatorType.divide, "/"],
        [ThreeOneSevenBee.Framework.Expressions.OperatorType.multiply, "*"],
        [ThreeOneSevenBee.Framework.Expressions.OperatorType.power, "^"]
    ] ) || null;
                }
            }
        },
        config: {
            properties: {
                Left: null,
                Right: null,
                Type: null
            }
        },
        constructor: function (left, right, type) {
            ThreeOneSevenBee.Framework.Expression.prototype.$constructor.call(this);
    
            this.setLeft(left);
            this.setRight(right);
            this.setType(type);
        },
        getValue: function () {
            var symbol;
            if (Bridge.get(ThreeOneSevenBee.Framework.Expressions.OperatorExpression).symbols.tryGetValue(this.getType(), symbol)) {
                return this.getLeft().toString() + symbol + this.getRight().toString();
            }
            throw new Bridge.InvalidOperationException("Invalid operator type: " + this.getType());
        },
        canCalculate: function () {
            return this.getLeft().canCalculate() && this.getRight().canCalculate();
        },
        calculate: function () {
            var left = this.getLeft().calculate();
            var right = this.getRight().calculate();
    
            if (!Bridge.hasValue(left) || !Bridge.hasValue(right)) {
                return ThreeOneSevenBee.Framework.Expression.prototype.calculate.call(this);
            }
    
            switch (this.getType()) {
                case ThreeOneSevenBee.Framework.Expressions.OperatorType.add: 
                    return Bridge.Nullable.add(left, right);
                case ThreeOneSevenBee.Framework.Expressions.OperatorType.subtract: 
                    return Bridge.Nullable.sub(left, right);
                case ThreeOneSevenBee.Framework.Expressions.OperatorType.divide: 
                    return Bridge.Nullable.div(left, right);
                case ThreeOneSevenBee.Framework.Expressions.OperatorType.multiply: 
                    return Bridge.Nullable.mul(left, right);
                case ThreeOneSevenBee.Framework.Expressions.OperatorType.power: 
                    return Math.pow(Bridge.Nullable.getValue(left), Bridge.Nullable.getValue(right));
            }
    
            return ThreeOneSevenBee.Framework.Expression.prototype.calculate.call(this);
        },
        getNodesRecursive: function () {
            var $t, $t1;
            var $yield = [];
            $yield.push(this);
    
            $t = Bridge.getEnumerator(this.getLeft().getNodesRecursive());
            while ($t.moveNext()) {
                var node = $t.getCurrent();
                $yield.push(node);
            }
            $t1 = Bridge.getEnumerator(this.getRight().getNodesRecursive());
            while ($t1.moveNext()) {
                var node1 = $t1.getCurrent();
                $yield.push(node1);
            }
            return Bridge.Array.toEnumerable($yield);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Framework.Expressions.UnaryMinusExpression', {
        inherits: [ThreeOneSevenBee.Framework.Expression],
        config: {
            properties: {
                Expression: null
            }
        },
        constructor: function (expression) {
            ThreeOneSevenBee.Framework.Expression.prototype.$constructor.call(this);
    
            this.setExpression(expression);
        },
        getValue: function () {
            return "-" + this.getExpression().toString();
        },
        canCalculate: function () {
            return this.getExpression().canCalculate();
        },
        calculate: function () {
            return Bridge.Nullable.neg(this.getExpression().calculate());
        },
        getNodesRecursive: function () {
            var $t;
            var $yield = [];
            $yield.push(this);
    
            $t = Bridge.getEnumerator(this.getExpression().getNodesRecursive());
            while ($t.moveNext()) {
                var node = $t.getCurrent();
                $yield.push(node);
            }
            return Bridge.Array.toEnumerable($yield);
        }
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
        },
        setValue: function (value) {
            this.value = value;
        },
        getNodesRecursive: function () {
            var $yield = [];
            $yield.push(this);
            return Bridge.Array.toEnumerable($yield);
        }
    });
    
    
    
    Bridge.init();
})(this);
