﻿(function (globals) {
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
            minus: 2,
            divide: 3,
            multiply: 4,
            power: 5
        },
        $enum: true
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
                Type: null
            }
        },
        constructor: function (type) {
            ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.$constructor.call(this);
    
            this.setType(type);
        },
        getSymbol: function () {
            if (Bridge.get(ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression).symbols.containsKey(this.getType())) {
                return Bridge.get(ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression).symbols.get(this.getType());
            }
            throw new Bridge.InvalidOperationException("Invalid Type: " + this.getType());
        },
        getTypeString: function () {
            return Bridge.get(ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression).symbols.get(this.getType());
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.ConstantExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.ExpressionBase,ThreeOneSevenBee.Model.Expression.ILeaf],
        config: {
            properties: {
                Type: null
            }
        },
        constructor: function (type) {
            ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.$constructor.call(this);
    
            this.setType(type);
        },
        getValue: function () {
            return Bridge.Enum.toString(ThreeOneSevenBee.Model.Expression.Expressions.ConstantType, this.getType());
        },
        getSize: function () {
            return 1;
        },
        clone: function () {
            return Bridge.merge(new ThreeOneSevenBee.Model.Expression.Expressions.ConstantExpression(this.getType()), {
                setSelected: this.getSelected()
            } );
        },
        replace$1: function (old, replacement, doRecursively) {
            return false;
        },
        getNodesRecursive: function () {
            var $yield = [];
            return Bridge.Array.toEnumerable($yield);
            return Bridge.Array.toEnumerable($yield);
        },
        canCalculate: function () {
            switch (this.getType()) {
                case ThreeOneSevenBee.Model.Expression.Expressions.ConstantType.pi: 
                    return true;
            }
    
            return false;
        },
        calculate: function () {
            switch (this.getType()) {
                case ThreeOneSevenBee.Model.Expression.Expressions.ConstantType.pi: 
                    return Math.PI;
            }
    
            return null;
        },
        equalsT: function (otherBase) {
            var other = (Bridge.as(otherBase, ThreeOneSevenBee.Model.Expression.Expressions.ConstantExpression));
    
            if (!Bridge.hasValue(other)) {
                return false;
            }
    
            return this.getType() === other.getType();
        },
        toString: function () {
            return this.getValue();
        },
        treePrint: function (indent, isLast) {
    
            console.log(indent + "|-" + this.getValue());
            return indent + (isLast ? "  " : "| ");
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.ExpressionBase],
        expression: null,
        constructor: function (expression) {
            ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.$constructor.call(this);
    
            this.setExpression(expression);
            this.getExpression().setParent(this);
        },
        getExpression: function () {
            return this.expression;
        },
        setExpression: function (value) {
            this.expression = value;
            this.expression.setParent(this);
        },
        getValue: function () {
            return "(" + this.getExpression().toString() + ")";
        },
        getSize: function () {
            return 1 + this.getExpression().getSize();
        },
        replace$1: function (old, replacement, doRecursively) {
            var hasReplaced = false;
    
            if (this.getExpression() === old) {
                this.setExpression(replacement.clone());
                this.getExpression().setParent(this);
                hasReplaced = hasReplaced || true;
            }
            else  {
                if (doRecursively) {
                    hasReplaced = hasReplaced || this.getExpression().replace$1(old, replacement, true);
                }
            }
    
            return hasReplaced;
        },
        canCalculate: function () {
            return this.getExpression().canCalculate();
        },
        calculate: function () {
            return this.getExpression().calculate();
        },
        equalsT: function (otherBase) {
            var other = (Bridge.as(otherBase, ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression));
    
            if (!Bridge.hasValue(other)) {
                return false;
            }
    
            return ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(this.getExpression(), other.getExpression());
        },
        clone: function () {
            return Bridge.merge(new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(this.getExpression().clone()), {
                setSelected: this.getSelected()
            } );
        },
        getNodesRecursive: function () {
            var $t;
            var $yield = [];
            $yield.push(this.getExpression());
    
            $t = Bridge.getEnumerator(this.getExpression().getNodesRecursive());
            while ($t.moveNext()) {
                var node = $t.getCurrent();
                $yield.push(node);
            }
            return Bridge.Array.toEnumerable($yield);
        },
        toString: function () {
            return "(" + this.getExpression().toString() + ")";
        },
        treePrint: function (indent, isLast) {
            console.log(indent + "|-" + "()");
            indent += (isLast ? "  " : "| ");
            this.getExpression().treePrint(indent, true);
            return indent;
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
        expression: null,
        config: {
            properties: {
                Function: null
            }
        },
        constructor: function (expression, $function) {
            ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.$constructor.call(this);
    
            this.setExpression(expression);
            this.getExpression().setParent(this);
            this.setFunction($function.toLowerCase());
        },
        getExpression: function () {
            return this.expression;
        },
        setExpression: function (value) {
            this.expression = value;
            this.expression.setParent(this);
        },
        getValue: function () {
            return this.getFunction() + this.getExpression();
        },
        getSize: function () {
            return 1 + this.getExpression().getSize();
        },
        canCalculate: function () {
            if (Bridge.get(ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression).functions.containsKey(this.getFunction())) {
                return this.getExpression().canCalculate();
            }
            return false;
        },
        calculate: function () {
            var func = { };
            if (Bridge.get(ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression).functions.tryGetValue(this.getFunction(), func)) {
                return func.v(Bridge.Nullable.getValue(this.getExpression().calculate()));
            }
            return null;
        },
        equalsT: function (otherBase) {
            var other = (Bridge.as(otherBase, ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression));
    
            if (!Bridge.hasValue(other)) {
                return false;
            }
    
            return this.getFunction() === other.getFunction() && ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(this.getExpression(), other.getExpression());
        },
        clone: function () {
            return Bridge.merge(new ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression(this.getExpression().clone(), this.getFunction()), {
                setSelected: this.getSelected()
            } );
        },
        replace$1: function (old, replacement, doRecursively) {
            var hasReplaced = false;
    
            if (this.getExpression() === old) {
                this.setExpression(replacement.clone());
                this.getExpression().setParent(this);
                hasReplaced = hasReplaced || true;
            }
            else  {
                if (doRecursively) {
                    hasReplaced = hasReplaced || this.getExpression().replace$1(old, replacement, true);
                }
            }
    
            return hasReplaced;
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
        },
        toString: function () {
            return this.getFunction() + "{" + this.getExpression() + "}";
        },
        treePrint: function (indent, isLast) {
            console.log(indent + "|-" + this.getFunction());
            indent += (isLast ? "  " : "| ");
            this.getExpression().treePrint(indent, true);
            return indent;
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.ExpressionBase,ThreeOneSevenBee.Model.Expression.ILeaf],
        number: 0,
        constructor: function (value) {
            ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.$constructor.call(this);
    
            this.number = value;
        },
        getValue: function () {
            return this.number.toString();
        },
        getSize: function () {
            return 1;
        },
        clone: function () {
            return Bridge.merge(new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(this.number), {
                setSelected: this.getSelected()
            } );
        },
        replace$1: function (old, replacement, doRecursively) {
            return false;
        },
        getNodesRecursive: function () {
            var $yield = [];
            return Bridge.Array.toEnumerable($yield);
            return Bridge.Array.toEnumerable($yield);
        },
        canCalculate: function () {
            return true;
        },
        calculate: function () {
            return this.number;
        },
        equalsT: function (otherBase) {
            var other = (Bridge.as(otherBase, ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression));
    
            if (!Bridge.hasValue(other)) {
                return false;
            }
    
            return (Math.abs(this.number - other.number) < 4.94065645841247E-324);
        },
        toString: function () {
            return this.getValue();
        },
        treePrint: function (indent, isLast) {
            console.log(indent + "|-" + this.getValue());
            return indent + (isLast ? "  " : "| ");
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.VariableExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.ExpressionBase,ThreeOneSevenBee.Model.Expression.ILeaf],
        value: null,
        constructor: function (value) {
            ThreeOneSevenBee.Model.Expression.ExpressionBase.prototype.$constructor.call(this);
    
            this.value = value;
        },
        getValue: function () {
            return this.value;
        },
        getSize: function () {
            return 2;
        },
        setValue: function (value) {
            this.value = value;
        },
        canCalculate: function () {
            return false;
        },
        calculate: function () {
            return null;
        },
        equalsT: function (otherBase) {
            var other = (Bridge.as(otherBase, ThreeOneSevenBee.Model.Expression.Expressions.VariableExpression));
    
            if (!Bridge.hasValue(other)) {
                return false;
            }
    
            return this.value === other.value;
        },
        clone: function () {
            return Bridge.merge(new ThreeOneSevenBee.Model.Expression.Expressions.VariableExpression(this.getValue()), {
                setSelected: this.getSelected()
            } );
        },
        replace$1: function (old, replacement, doRecursively) {
            return false;
        },
        getNodesRecursive: function () {
            var $yield = [];
            return Bridge.Array.toEnumerable($yield);
            return Bridge.Array.toEnumerable($yield);
        },
        toString: function () {
            return this.getValue();
        },
        treePrint: function (indent, isLast) {
            console.log(Bridge.String.format(indent + "|-" + this.getValue(), true));
            return indent + (isLast ? "  " : "| ");
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.BinaryExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression],
        left: null,
        right: null,
        constructor: function (left, right, type) {
            ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression.prototype.$constructor.call(this, type);
    
            if (!Bridge.hasValue(left)) {
                throw new Bridge.ArgumentNullException("left");
            }
            if (!Bridge.hasValue(right)) {
                throw new Bridge.ArgumentNullException("right");
            }
    
            this.setLeft(left);
            this.setRight(right);
        },
        getLeft: function () {
            return this.left;
        },
        setLeft: function (value) {
            var wrapped = new ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer().wrapInDelimiterIfNeccessary(value, this);
            this.left = wrapped;
            this.left.setParent(this);
        },
        getRight: function () {
            return this.right;
        },
        setRight: function (value) {
            this.right = value;
            this.right.setParent(this);
        },
        replace$1: function (old, replacement, doRecursively) {
            var hasReplaced = false;
    
            if (this.getLeft() === old) {
                this.setLeft(replacement.clone());
                this.getLeft().setParent(this);
                hasReplaced = hasReplaced || true;
            }
            else  {
                if (doRecursively) {
                    hasReplaced = hasReplaced || this.getLeft().replace$1(old, replacement, true);
                }
            }
    
            if (this.getRight() === old) {
                this.setRight(replacement.clone());
                this.getRight().setParent(this);
                hasReplaced = hasReplaced || true;
            }
            else  {
                if (doRecursively) {
                    hasReplaced = hasReplaced || this.getRight().replace$1(old, replacement, true);
                }
            }
    
            return hasReplaced;
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.UnaryExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression],
        config: {
            properties: {
                Expression: null
            }
        },
        constructor: function (type, expression) {
            ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression.prototype.$constructor.call(this, type);
    
            if (type !== ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.minus) {
                throw new Bridge.ArgumentException("Invalid Type: " + type, "type");
            }
            if (!Bridge.hasValue(expression)) {
                throw new Bridge.ArgumentNullException("expression");
            }
    
            this.setExpression(expression);
            this.getExpression().setParent(this);
        },
        replace$1: function (old, replacement, doRecursively) {
            var hasReplaced = false;
    
            if (this.getExpression() === old) {
                this.setExpression(replacement.clone());
                this.getExpression().setParent(this);
                hasReplaced = hasReplaced || true;
            }
            else  {
                if (doRecursively) {
                    hasReplaced = hasReplaced || this.getExpression().replace$1(old, replacement, true);
                }
            }
    
            return hasReplaced;
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.VariadicExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression,Bridge.ICollection$1(ThreeOneSevenBee.Model.Expression.ExpressionBase),Bridge.IList$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)],
        expressions: null,
        constructor: function (type, first, second, expressions) {
            ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression.prototype.$constructor.call(this, type);
            var $t;
    
            if (expressions === void 0) { expressions = []; }
    
            if (type !== ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add && type !== ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                throw new Bridge.ArgumentException("Invalid Type: " + type, "type");
            }
            if (!Bridge.hasValue(first)) {
                throw new Bridge.ArgumentNullException("first");
            }
            if (!Bridge.hasValue(second)) {
                throw new Bridge.ArgumentNullException("second");
            }
    
            this.expressions = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
            this.add(first);
            this.add(second);
            $t = Bridge.getEnumerator(expressions);
            while ($t.moveNext()) {
                var expression = $t.getCurrent();
                this.add(expression);
            }
    },
    constructor$1: function (type, expressions) {
        ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression.prototype.$constructor.call(this, type);
        var $t;
    
        if (expressions === void 0) { expressions = []; }
        if (type !== ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add && type !== ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
            throw new Bridge.ArgumentException("Invalid Type: " + type, "type");
        }
        if (!Bridge.hasValue(expressions)) {
            throw new Bridge.ArgumentNullException("expressions");
        }
        if (expressions.length < 2) {
            throw new Bridge.ArgumentOutOfRangeException("Must give at least two expressions.", "expression");
        }
    
        this.expressions = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
        $t = Bridge.getEnumerator(expressions);
        while ($t.moveNext()) {
            var expression = $t.getCurrent();
            this.add(expression);
        }
    },
    getCount: function () {
        return this.expressions.getCount();
    },
    getIsReadOnly: function () {
        return false;
    },
    getItem: function (index) {
        return this.expressions.getItem(index);
    },
    setItem: function (index, value) {
        this.removeAt(index);
        this.insert(index, value);
    },
    replace$1: function (old, replacement, doRecursively) {
        var $t;
        var hasReplaced = false;
    
        var expressionArray = this.expressions.toArray();
    
        for (var i = 0; i < expressionArray.length; i++) {
            if (expressionArray[i] === old) {
                expressionArray[i] = replacement.clone();
                expressionArray[i].setParent(this);
                hasReplaced = hasReplaced || true;
            }
            else  {
                if (doRecursively) {
                    hasReplaced = hasReplaced || expressionArray[i].replace$1(old, replacement, true);
                }
            }
        }
    
        if (hasReplaced) {
            this.expressions.clear();
            $t = Bridge.getEnumerator(expressionArray);
            while ($t.moveNext()) {
                var expr = $t.getCurrent();
                this.add(expr);
            }
        }
    
        return hasReplaced;
    },
    add$1: function (items) {
        var $t;
        $t = Bridge.getEnumerator(items);
        while ($t.moveNext()) {
            var item = $t.getCurrent();
            this.add(item);
        }
    },
    add$2: function (item) {
        var $t;
        if (item.getType() === this.getType()) {
            $t = Bridge.getEnumerator(item);
            while ($t.moveNext()) {
                var subItem = $t.getCurrent();
                this.add(subItem);
            }
        }
        else  {
            var wrapped = new ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer().wrapInDelimiterIfNeccessary(item, this);
            this.expressions.add(wrapped);
            wrapped.setParent(this);
        }
    },
    add: function (item) {
        if (Bridge.is(item, ThreeOneSevenBee.Model.Expression.Expressions.VariadicExpression)) {
            this.add$2(Bridge.cast(item, ThreeOneSevenBee.Model.Expression.Expressions.VariadicExpression));
        }
        else  {
            var wrapped = new ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer().wrapInDelimiterIfNeccessary(item, this);
            this.expressions.add(wrapped);
            wrapped.setParent(this);
        }
    },
    remove: function (item) {
        return this.expressions.remove(item);
    },
    clear: function () {
        this.expressions.clear();
    },
    contains: function (item) {
        return this.expressions.contains(item);
    },
    getEnumerator$1: function () {
        return this.expressions.getEnumerator();
    },
    getEnumerator: function () {
        return this.expressions.getEnumerator();
    },
    indexOf: function (item) {
        return this.expressions.indexOf(item);
    },
    insert: function (index, item) {
        var $t;
        var variadicExpression = Bridge.as(item, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
        if (!Bridge.hasValue(variadicExpression) || variadicExpression.getType() !== this.getType()) {
            var wrapped = new ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer().wrapInDelimiterIfNeccessary(item, this);
            this.expressions.insert(index, wrapped);
            wrapped.setParent(this);
        }
        else  {
            var offset = 0;
            $t = Bridge.getEnumerator(variadicExpression);
            while ($t.moveNext()) {
                var operand = $t.getCurrent();
                var wrapped1 = new ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer().wrapInDelimiterIfNeccessary(item, this);
                this.expressions.insert(index + offset++, wrapped1);
                wrapped1.setParent(this);
            }
        }
    },
    removeAt: function (index) {
        this.expressions.removeAt(index);
    },
    removeReference: function (expression) {
        this.removeAt(this.indexOfReference(expression));
    },
    indexOfReference: function (expression) {
        for (var i = 0; i < this.getCount(); i++) {
            if (this.getItem(i) === expression) {
                return i;
            }
        }
        return -1;
    }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.Expressions.BinaryExpression],
        constructor: function (left, right, type) {
            ThreeOneSevenBee.Model.Expression.Expressions.BinaryExpression.prototype.$constructor.call(this, left, right, type);
    
        },
        getValue: function () {
            return this.getLeft().getValue() + this.getSymbol() + this.getRight().getValue();
        },
        getSize: function () {
            return 1 + this.getLeft().getSize() + this.getRight().getSize();
        },
        canCalculate: function () {
            switch (this.getType()) {
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add: 
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract: 
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide: 
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply: 
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power: 
                    return this.getLeft().canCalculate() && this.getRight().canCalculate();
            }
            return false;
        },
        calculate: function () {
            switch (this.getType()) {
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add: 
                    return Bridge.Nullable.add(this.getLeft().calculate(), this.getRight().calculate());
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract: 
                    return Bridge.Nullable.sub(this.getLeft().calculate(), this.getRight().calculate());
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide: 
                    return Bridge.Nullable.div(this.getLeft().calculate(), this.getRight().calculate());
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply: 
                    return Bridge.Nullable.mul(this.getLeft().calculate(), this.getRight().calculate());
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power: 
                    return Math.pow(Bridge.Nullable.getValue(this.getLeft().calculate()), Bridge.Nullable.getValue(this.getRight().calculate()));
            }
            return null;
        },
        equalsT: function (otherBase) {
            var other = (Bridge.as(otherBase, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression));
    
            if (!Bridge.hasValue(other)) {
                return false;
            }
    
            if (other.getType() !== this.getType()) {
                return false;
            }
    
            switch (this.getType()) {
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add: 
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply: 
                    if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(this.getLeft(), other.getLeft()) && ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(this.getRight(), other.getRight()) || ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(this.getLeft(), other.getRight()) && ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(this.getRight(), other.getLeft())) {
                        return true;
                    }
                    break;
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract: 
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide: 
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power: 
                    if (this.getLeft().equalsT(other.getLeft()) && this.getRight().equalsT(other.getRight())) {
                        return true;
                    }
                    break;
            }
            return false;
        },
        clone: function () {
            return Bridge.merge(new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(this.getLeft().clone(), this.getRight().clone(), this.getType()), {
                setSelected: this.getSelected()
            } );
        },
        getNodesRecursive: function () {
            var $t, $t1;
            var $yield = [];
            $yield.push(this.getLeft());
            $t = Bridge.getEnumerator(this.getLeft().getNodesRecursive());
            while ($t.moveNext()) {
                var node = $t.getCurrent();
                $yield.push(node);
            }
            $yield.push(this.getRight());
            $t1 = Bridge.getEnumerator(this.getRight().getNodesRecursive());
            while ($t1.moveNext()) {
                var node1 = $t1.getCurrent();
                $yield.push(node1);
            }
            return Bridge.Array.toEnumerable($yield);
        },
        toString: function () {
            return "{" + this.getLeft() + "}" + this.getSymbol() + "{" + this.getRight() + "}";
        },
        treePrint: function (indent, isLast) {
    
            console.log(indent + "|-" + this.getSymbol());
            indent += (isLast ? "  " : "| ");
            this.getLeft().treePrint(indent, false);
            this.getRight().treePrint(indent, true);
            return indent;
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.Expressions.UnaryExpression],
        constructor: function (expression) {
            ThreeOneSevenBee.Model.Expression.Expressions.UnaryExpression.prototype.$constructor.call(this, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.minus, expression);
    
        },
        getValue: function () {
            return "-" + this.getExpression();
        },
        getSize: function () {
            return 1 + this.getExpression().getSize();
        },
        canCalculate: function () {
            return this.getExpression().canCalculate();
        },
        calculate: function () {
            return Bridge.Nullable.neg(this.getExpression().calculate());
        },
        equalsT: function (otherBase) {
            var other = (Bridge.as(otherBase, ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression));
    
            if (!Bridge.hasValue(other)) {
                return false;
            }
    
            return ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(this.getExpression(), other.getExpression());
        },
        clone: function () {
            return Bridge.merge(new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(this.getExpression().clone()), {
                setSelected: this.getSelected()
            } );
        },
        getNodesRecursive: function () {
            var $t;
            var $yield = [];
            $yield.push(this.getExpression());
    
            $t = Bridge.getEnumerator(this.getExpression().getNodesRecursive());
            while ($t.moveNext()) {
                var node = $t.getCurrent();
                $yield.push(node);
            }
            return Bridge.Array.toEnumerable($yield);
        },
        toString: function () {
            return "-{" + this.getExpression() + "}";
        },
        treePrint: function (indent, isLast) {
            console.log(indent + "|-" + "-");
            indent += (isLast ? "  " : "| ");
            this.getExpression().treePrint(indent, true);
            return indent;
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression', {
        inherits: [ThreeOneSevenBee.Model.Expression.Expressions.VariadicExpression],
        constructor: function (type, firstExpression, secondExpression, expressions) {
            ThreeOneSevenBee.Model.Expression.Expressions.VariadicExpression.prototype.$constructor.call(this, type, firstExpression, secondExpression, expressions);
    
            if (expressions === void 0) { expressions = []; }
        },
        constructor$1: function (type, expressions) {
            ThreeOneSevenBee.Model.Expression.Expressions.VariadicExpression.prototype.constructor$1.call(this, type, expressions);
    
            if (expressions === void 0) { expressions = []; }
        },
        getValue: function () {
            return Bridge.Linq.Enumerable.from(this).skip(1).aggregate(Bridge.Linq.Enumerable.from(this).first().getValue(), Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression.f1));
        },
        getSize: function () {
            var $t;
            // Count-1 gets number of operators in the variadic expression
            var result = (this.getCount() - 1);
            $t = Bridge.getEnumerator(this);
            while ($t.moveNext()) {
                var expression = $t.getCurrent();
                result += expression.getSize();
            }
            return result;
        },
        canCalculate: function () {
            var $t;
            switch (this.getType()) {
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add: 
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply: 
                    $t = Bridge.getEnumerator(this);
                    while ($t.moveNext()) {
                        var expression = $t.getCurrent();
                        if (expression.canCalculate() === false) {
                            return false;
                        }
                    }
                    return true;
            }
            return false;
        },
        calculate: function () {
            if (!this.canCalculate()) {
                return null;
            } // break out early if this cannot be calculated
    
            var result = null;
            switch (this.getType()) {
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add: 
                    // read out the first index
                    result = this.getItem(0).calculate();
                    // skip the first, and apply the operation
                    for (var i = 1; i < this.getCount(); i++) {
                        result = Bridge.Nullable.add(result, this.getItem(i).calculate());
                    }
                    break;
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply: 
                    // read out the first index
                    result = this.getItem(0).calculate();
                    // skip the first, and apply the operation
                    for (var i1 = 1; i1 < this.getCount(); i1++) {
                        result = Bridge.Nullable.mul(result, this.getItem(i1).calculate());
                    }
                    break;
            }
            return result;
        },
        equalsT: function (otherBase) {
            var other = Bridge.as(otherBase, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
            if (Bridge.hasValue(other) && other.getType() === this.getType() && other.getCount() === this.getCount()) {
                for (var index = 0; index < other.getCount(); index++) {
                    if (ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Inequality(this.getItem(index), other.getItem(index))) {
                        return false;
                    }
                }
                return true;
            }
            return false;
        },
        clone: function () {
            var $t;
            if (this.getCount() < 2) {
                throw new Bridge.InvalidOperationException("Tried to clone invalid VariadicExpression.");
            }
            var expression = Bridge.merge(new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", this.getType(), this.getItem(0).clone(), this.getItem(1).clone()), {
                setSelected: this.getSelected()
            } );
            $t = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(this).skip(2));
            while ($t.moveNext()) {
                var expr = $t.getCurrent();
                expression.add(expr.clone());
            }
            return expression;
        },
        getNodesRecursive: function () {
            var $t, $t1;
            var $yield = [];
            $t = Bridge.getEnumerator(this);
            while ($t.moveNext()) {
                var node = $t.getCurrent();
                $yield.push(node);
                $t1 = Bridge.getEnumerator(node.getNodesRecursive());
                while ($t1.moveNext()) {
                    var childNode = $t1.getCurrent();
                    $yield.push(childNode);
                }
            }
            return Bridge.Array.toEnumerable($yield);
        },
        toString: function () {
            return Bridge.Linq.Enumerable.from(this).skip(1).aggregate("{" + Bridge.Linq.Enumerable.from(this).first() + "}", Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression.f2));
        },
        treePrint: function (indent, isLast) {
            console.log(indent + "|-" + this.getSymbol());
            indent += (isLast ? "  " : "| ");
            for (var i = 0; i < this.getCount() - 1; i++) {
                this.getItem(i).treePrint(indent, false);
            }
            this.getItem(this.getCount() - 1).treePrint(indent, true);
            return indent;
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression, {
        f1: function (s, e) {
            return s + this.getSymbol() + e.getValue();
        },
        f2: function (s, e) {
            return s + this.getSymbol() + "{" + e + "}";
        }
    });
    
    Bridge.init();
})(this);
