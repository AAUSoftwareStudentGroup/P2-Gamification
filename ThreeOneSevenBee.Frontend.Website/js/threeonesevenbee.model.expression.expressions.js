(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.ConstantType', {
        statics: {
            pi: 0
        },
        $enum: true
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.OperatorType', {
        statics: {
            add: 0,
            subtract: 1,
            divide: 2,
            multiply: 3,
            power: 4
        },
        $enum: true
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.ConstantExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.ExpressionBase],
        value: null,
        constructor: function (value) {
            ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.$constructor.call(this);
    
            this.value = value;
        },
        getValue: function () {
            return Bridge.Enum.toString(ThreeOneSevenBee.Model.Expression.Expressions.ConstantType, this.value);
        },
        clone: function () {
            return new ThreeOneSevenBee.Model.Expression.Expressions.ConstantExpression(this.value);
        },
        replace: function (old, replacement) {
            return false;
        },
        getNodesRecursive: function () {
            var $yield = [];
            $yield.push(this);
            return Bridge.Array.toEnumerable($yield);
        },
        canCalculate: function () {
            switch (this.value) {
                case ThreeOneSevenBee.Model.Expression.Expressions.ConstantType.pi: 
                    return true;
            }
    
            return ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.canCalculate.call(this);
        },
        calculate: function () {
            switch (this.value) {
                case ThreeOneSevenBee.Model.Expression.Expressions.ConstantType.pi: 
                    return Math.PI;
            }
    
            return ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.calculate.call(this);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.ExpressionBase],
        config: {
            properties: {
                Expression: null
            }
        },
        constructor: function (expression) {
            ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.$constructor.call(this);
    
            this.setExpression(expression);
            this.getExpression().setParent(this);
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
        clone: function () {
            return new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(this.getExpression().clone());
        },
        replace: function (old, replacement) {
            if (this.getExpression() === old) {
                this.setExpression(replacement);
                return true;
            }
            return this.getExpression().replace(old, replacement);
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
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.ExpressionBase],
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
            ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.$constructor.call(this);
    
            this.setExpression(expression);
            this.getExpression().setParent(this);
            this.setFunction($function.toLowerCase());
        },
        getValue: function () {
            return this.getFunction() + this.getExpression();
        },
        canCalculate: function () {
            if (Bridge.get(ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression).functions.containsKey(this.getFunction())) {
                return this.getExpression().canCalculate();
            }
            return ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.canCalculate.call(this);
        },
        calculate: function () {
            var func = { };
            if (Bridge.get(ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression).functions.tryGetValue(this.getFunction(), func)) {
                return func.v(Bridge.Nullable.getValue(this.getExpression().calculate()));
            }
            return ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.calculate.call(this);
        },
        clone: function () {
            return new ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression(this.getExpression().clone(), this.getFunction());
        },
        replace: function (old, replacement) {
            if (this.getExpression() === old) {
                this.setExpression(replacement);
                return true;
            }
            return this.getExpression().replace(old, replacement);
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
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.ExpressionBase],
        value: 0,
        constructor: function (value) {
            ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.$constructor.call(this);
    
            this.value = value;
        },
        getValue: function () {
            return Bridge.Int.format(this.value, 'G');
        },
        clone: function () {
            return new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(this.value);
        },
        replace: function (old, replacement) {
            return false;
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
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.ExpressionBase],
        statics: {
            config: {
                init: function () {
                    this.symbols = Bridge.merge(new Bridge.Dictionary$2(ThreeOneSevenBee.Model.Expression.Expressions.OperatorType,String)(), [
        [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, "+"],
        [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract, "-"],
        [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide, "/"],
        [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, "*"],
        [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power, "^"]
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
            ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.$constructor.call(this);
    
            this.setLeft(left);
            this.getLeft().setParent(this);
            this.setRight(right);
            this.getRight().setParent(this);
            this.setType(type);
        },
        getValue: function () {
            if (Bridge.get(ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression).symbols.containsKey(this.getType())) {
                return this.getLeft().toString() + Bridge.get(ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression).symbols.get(this.getType()) + this.getRight().toString();
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
                return ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.calculate.call(this);
            }
    
            switch (this.getType()) {
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add: 
                    return Bridge.Nullable.add(left, right);
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract: 
                    return Bridge.Nullable.sub(left, right);
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide: 
                    return Bridge.Nullable.div(left, right);
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply: 
                    return Bridge.Nullable.mul(left, right);
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power: 
                    return Math.pow(Bridge.Nullable.getValue(left), Bridge.Nullable.getValue(right));
            }
    
            return ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.calculate.call(this);
        },
        clone: function () {
            return new ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression(this.getLeft().clone(), this.getRight().clone(), this.getType());
        },
        replace: function (old, replacement) {
            var leftReplaced = false;
            var rightReplaced = false;
    
            if (this.getLeft() === old) {
                this.setLeft(replacement);
                leftReplaced = true;
            }
            if (this.getRight() === old) {
                this.setRight(replacement);
                rightReplaced = true;
            }
    
            if (leftReplaced || rightReplaced) {
                return true;
            }
    
            leftReplaced = leftReplaced || this.getLeft().replace(old, replacement);
            rightReplaced = rightReplaced || this.getRight().replace(old, replacement);
    
            return leftReplaced || rightReplaced;
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
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.ExpressionBase],
        config: {
            properties: {
                Expression: null
            }
        },
        constructor: function (expression) {
            ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.$constructor.call(this);
    
            this.setExpression(expression);
            this.getExpression().setParent(this);
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
        clone: function () {
            return new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(this.getExpression().clone());
        },
        replace: function (old, replacement) {
            if (this.getExpression() === old) {
                this.setExpression(replacement);
                return true;
            }
            return this.getExpression().replace(old, replacement);
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
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.VariableExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.ExpressionBase],
        value: null,
        constructor: function (value) {
            ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.$constructor.call(this);
    
            this.value = value;
        },
        getValue: function () {
            return this.value;
        },
        setValue: function (value) {
            this.value = value;
        },
        clone: function () {
            return new ThreeOneSevenBee.Model.Expression.Expressions.VariableExpression(this.getValue());
        },
        replace: function (old, replacement) {
            return false;
        },
        getNodesRecursive: function () {
            var $yield = [];
            $yield.push(this);
            return Bridge.Array.toEnumerable($yield);
        }
    });
    
    
    
    Bridge.init();
})(this);
