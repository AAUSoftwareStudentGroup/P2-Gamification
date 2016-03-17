﻿(function (globals) {
    "use strict";

    /** @namespace ThreeOneSevenBee.Model.Expression */
    
    /**
     * Provides functionality to analyze expressions and provide identical alternatives based on {@link }.
     *
     * @public
     * @class ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer
     */
    Bridge.define('ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer', {
        rules: null,
        constructor: function () {
            this.rules = new Bridge.List$1(Function)();
        },
        add: function (rule) {
            this.rules.add(rule);
        },
        remove: function (rule) {
            this.rules.remove(rule);
        },
        getCommonParent: function (selection) {
            var $t;
            if (selection.getCount() === 0) {
                return null;
            }
            else  {
                if (selection.getCount() === 1) {
                    return selection.getItem(0);
                }
                else  {
                    var intersection = selection.getItem(0).getParentPath();
    
                    $t = Bridge.getEnumerator(selection);
                    while ($t.moveNext()) {
                        var expression = $t.getCurrent();
                        var path = expression.getParentPath();
                        if (Bridge.Linq.Enumerable.from(path).count() === 0) {
                            return expression;
                        }
                        intersection = Bridge.Linq.Enumerable.from(intersection).intersect(path);
                    }
    
                    return Bridge.Linq.Enumerable.from(intersection).first();
                }
            }
        },
        getIdentities: function (expression, selection) {
            var $t;
            var identities = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
    
            if (Bridge.Linq.Enumerable.from(selection).count() === 0) {
                return identities;
            }
    
            $t = Bridge.getEnumerator(this.rules);
            while ($t.moveNext()) {
                var rule = $t.getCurrent();
                var commonParent = this.getCommonParent(selection);
                var identity = { };
                if (rule(commonParent, selection, identity)) {
                    identities.add(identity.v);
                }
            }
    
            return identities;
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.ExpressionBase', {
        inherits: function () { return [Bridge.IEquatable$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)]; },
        config: {
            properties: {
                Parent: null
            }
        },
        getParentPath: function () {
            var $yield = [];
            var currentParent = this.getParent();
            while (Bridge.hasValue(currentParent)) {
                $yield.push(currentParent);
                currentParent = currentParent.getParent();
            }
            return Bridge.Array.toEnumerable($yield);
        },
        equals: function (other) {
            return (Bridge.is(other, ThreeOneSevenBee.Model.Expression.ExpressionBase)) && this.equalsT(Bridge.cast(other, ThreeOneSevenBee.Model.Expression.ExpressionBase));
        },
        getHashCode: function () {
            return Bridge.getHashCode(this.getValue());
        },
        toString: function () {
            return this.getValue();
        },
        prettyPrint: function () {
            this.treePrint("", true);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.ExpressionModel', {
        expression: null,
        selectionParent: null,
        selection: null,
        identities: null,
        analyzer: null,
        serializer: null,
        config: {
            events: {
                OnChanged: null
            }
        },
        constructor: function (expression, rules) {
            var $t;
            if (rules === void 0) { rules = []; }
            this.selectionParent = null;
            this.selection = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
            this.identities = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
            this.serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
            this.analyzer = new ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer();
            this.expression = this.serializer.deserialize(expression);
            $t = Bridge.getEnumerator(rules);
            while ($t.moveNext()) {
                var rule = $t.getCurrent();
                this.analyzer.add(rule);
            }
    },
    getExpression: function () {
        return this.expression;
    },
    getIdentities: function () {
        return this.identities;
    },
    getSelection: function () {
        return this.selection;
    },
    getSelected: function () {
        return this.selectionParent;
    },
    selectionIndex: function (expression) {
        for (var i = 0; i < this.selection.getCount(); i++) {
            if (this.selection.getItem(i) === expression) {
                return i;
            }
        }
        return -1;
    },
    select: function (expression) {
        var $t, $t1;
        var index = this.selectionIndex(expression);
        if (index === -1) {
    
            $t = Bridge.getEnumerator(expression.getNodesRecursive());
            while ($t.moveNext()) {
                var descendant = $t.getCurrent();
                var i;
                if ((($t1 = this.selectionIndex(descendant), i = $t1, $t1)) !== -1) {
                    this.selection.removeAt(i);
                }
            }
            this.selection.add(expression);
        }
        else  {
            this.selection.removeAt(index);
        }
        this.selectionParent = this.analyzer.getCommonParent(this.selection);
        this.identities = this.analyzer.getIdentities(expression, this.selection);
        this.OnChanged(this);
    },
    unSelectAll: function () {
        this.selection.clear();
        this.identities.clear();
        this.selectionParent = null;
        this.OnChanged(this);
    },
    applyIdentity: function (identity) {
        if (this.identities.contains(identity)) {
            if (!Bridge.hasValue(this.selectionParent.getParent())) {
                this.expression = identity;
            }
            else  {
                var parent = (Bridge.as(this.selectionParent.getParent(), ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression));
                if (parent.getLeft() === this.selectionParent) {
                    parent.setLeft(identity);
                }
                else  {
                    parent.setRight(identity);
                }
                identity.setParent(parent);
            }
        }
        this.unSelectAll();
    }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.ExpressionParser', {
        statics: {
            config: {
                init: function () {
                    this.decimalSeparator = Bridge.Int.format(((1.1)), 'G').charCodeAt(1) || new Bridge.Int();
                }
            }
        },
        config: {
            init: function () {
                this.ops = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.Expression.Operator)(), [
        [new ThreeOneSevenBee.Model.Expression.Operator("constructor$1", "^", 4, ThreeOneSevenBee.Model.Expression.OperatorAssociativity.right)],
        [new ThreeOneSevenBee.Model.Expression.Operator("constructor$1", "*", 3, ThreeOneSevenBee.Model.Expression.OperatorAssociativity.left)],
        [new ThreeOneSevenBee.Model.Expression.Operator("constructor$1", "/", 3, ThreeOneSevenBee.Model.Expression.OperatorAssociativity.left)],
        [new ThreeOneSevenBee.Model.Expression.Operator("constructor$1", "+", 2, ThreeOneSevenBee.Model.Expression.OperatorAssociativity.left)],
        [new ThreeOneSevenBee.Model.Expression.Operator("constructor$1", "-", 2, ThreeOneSevenBee.Model.Expression.OperatorAssociativity.left)]
    ] ) || null;
                this.functions = Bridge.merge(new Bridge.List$1(String)(), [
        ["sqrt"]
    ] ) || null;
                this.output = new ThreeOneSevenBee.Model.Collections.Queue$1(ThreeOneSevenBee.Model.Expression.Token)() || null;
                this.operators = new ThreeOneSevenBee.Model.Collections.Stack$1(ThreeOneSevenBee.Model.Expression.Token)() || null;
            }
        },
        /**
         * Checks if the i'th position of input is whitespace.
         *
         * @instance
         * @private
         * @this ThreeOneSevenBee.Model.Expression.ExpressionParser
         * @memberof ThreeOneSevenBee.Model.Expression.ExpressionParser
         * @param   {string}           input    
         * @param   {System.Int32&}    i
         * @return  {boolean}
         */
        isWhiteSpace: function (input, i) {
            if (i.v >= input.length) {
                return false;
            }
    
            if (!Bridge.Char.isWhiteSpace(String.fromCharCode(input.charCodeAt(i.v)))) {
                return false;
            }
            i.v++;
            return true;
        },
        /**
         * Checks if the i'th position of input is a number or a decimal point.
         *
         * @instance
         * @private
         * @this ThreeOneSevenBee.Model.Expression.ExpressionParser
         * @memberof ThreeOneSevenBee.Model.Expression.ExpressionParser
         * @param   {string}           input    
         * @param   {System.Int32&}    i
         * @return  {boolean}
         */
        isNumberOrDecimal: function (input, i) {
            if (i.v >= input.length) {
                return false;
            }
    
            return (Bridge.Char.isDigit(input.charCodeAt(i.v)) || input.charCodeAt(i.v) === Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionParser).decimalSeparator);
        },
        /**
         * Checks if the i'th position and onwards contains a number. The number is returned.
         *
         * @instance
         * @private
         * @this ThreeOneSevenBee.Model.Expression.ExpressionParser
         * @memberof ThreeOneSevenBee.Model.Expression.ExpressionParser
         * @param   {string}            input     
         * @param   {System.Int32&}     i         
         * @param   {System.Double&}    number
         * @return  {boolean}
         */
        isNumber: function (input, i, number) {
            number.v = Number.NaN;
            if (i.v >= input.length) {
                return false;
            }
    
            if (!this.isNumberOrDecimal(input, i)) {
                return false;
            }
    
            var numberString = "";
            while (this.isNumberOrDecimal(input, i)) {
                numberString += String.fromCharCode(input.charCodeAt(i.v));
                i.v++;
            }
    
            return Bridge.Int.tryParseFloat(numberString, null, number);
        },
        isFunction: function (input, i, $function) {
            var $t;
            $function.v = null;
            if (i.v >= input.length) {
                return false;
            }
    
            $t = Bridge.getEnumerator(this.functions);
            while ($t.moveNext()) {
                var func = $t.getCurrent();
                if (i.v + func.length <= input.length && input.substr(i.v, func.length).toLowerCase() === func) {
                    $function.v = func;
                    i.v += func.length;
                    return true;
                }
            }
    
            return false;
        },
        /**
         * Checks if the i'th position and onwards is pi.
         *
         * @instance
         * @private
         * @this ThreeOneSevenBee.Model.Expression.ExpressionParser
         * @memberof ThreeOneSevenBee.Model.Expression.ExpressionParser
         * @param   {string}           input    
         * @param   {System.Int32&}    i
         * @return  {boolean}
         */
        isPi: function (input, i) {
            if (i.v >= input.length) {
                return false;
            }
    
            if (i.v + 1 < input.length && input.substr(i.v, 2).toLowerCase() === "pi") {
                i.v += 2;
                return true;
            }
    
            return false;
        },
        /**
         * Checks if the i'th position is a valid variable. The variable is returned.
         *
         * @instance
         * @private
         * @this ThreeOneSevenBee.Model.Expression.ExpressionParser
         * @memberof ThreeOneSevenBee.Model.Expression.ExpressionParser
         * @param   {string}            input       
         * @param   {System.Int32&}     i           
         * @param   {System.String&}    variable
         * @return  {boolean}
         */
        isVariable: function (input, i, variable) {
            var $t;
            variable.v = null;
            if (i.v >= input.length) {
                return false;
            }
    
            var validLetters = "abcdefghijklmnopqrstuvwz";
    
            $t = Bridge.getEnumerator(validLetters);
            while ($t.moveNext()) {
                var validLeter = $t.getCurrent();
                if (input.charCodeAt(i.v) === validLeter) {
                    variable.v = String.fromCharCode(input.charCodeAt(i.v));
                    i.v++;
                    return true;
                }
            }
    
            return false;
        },
        /**
         * Checks if the i'th postion is an operator. The operator is returned.
         *
         * @instance
         * @private
         * @this ThreeOneSevenBee.Model.Expression.ExpressionParser
         * @memberof ThreeOneSevenBee.Model.Expression.ExpressionParser
         * @param   {string}                                         input    
         * @param   {System.Int32&}                                  i        
         * @param   {ThreeOneSevenBee.Model.Expression.Operator&}    op
         * @return  {boolean}
         */
        isOperator: function (input, i, op) {
            op.v = null;
            if (i.v >= input.length) {
                return false;
            }
    
            var opSymbol = input.charCodeAt(i.v);
            op.v = Bridge.Linq.Enumerable.from(this.ops).firstOrDefault(function (o) {
                return o.getSymbol() === String.fromCharCode(opSymbol);
            }, Bridge.getDefaultValue(ThreeOneSevenBee.Model.Expression.Operator));
            if (!Bridge.hasValue(op.v)) {
                return false;
            }
            i.v++;
            return true;
        },
        inFixToPostFix: function (inFix) {
            var i = { v : 0 };
            var lastToken = null;
            while (i.v < inFix.length) {
                if (this.isWhiteSpace(inFix, i)) {
                    continue;
                }
                var number = { };
                if (this.isNumber(inFix, i, number)) {
                    lastToken = Bridge.get(ThreeOneSevenBee.Model.Expression.Token).number(number.v);
                    this.output.enqueue(lastToken);
                    continue;
                }
    
                var func = { };
                if (this.isFunction(inFix, i, func)) {
                    lastToken = Bridge.get(ThreeOneSevenBee.Model.Expression.Token).$function(func.v);
                    this.operators.push(lastToken);
                    continue;
                }
    
                if (this.isPi(inFix, i)) {
                    lastToken = Bridge.get(ThreeOneSevenBee.Model.Expression.Token).constant(ThreeOneSevenBee.Model.Expression.Expressions.ConstantType.pi);
                    this.output.enqueue(lastToken);
                    continue;
                }
    
                var variable = { };
                if (this.isVariable(inFix, i, variable)) {
                    lastToken = Bridge.get(ThreeOneSevenBee.Model.Expression.Token).variable(variable.v);
                    this.output.enqueue(lastToken);
                    continue;
                }
    
                var op1 = { };
                if (this.isOperator(inFix, i, op1)) {
                    // unary check
                    if (op1.v.getSymbol() === "-" && (!Bridge.hasValue(lastToken) || lastToken.getType() === ThreeOneSevenBee.Model.Expression.TokenType.operator)) {
                        lastToken = Bridge.get(ThreeOneSevenBee.Model.Expression.Token).operator(new ThreeOneSevenBee.Model.Expression.Operator("constructor$1", "~", 5, ThreeOneSevenBee.Model.Expression.OperatorAssociativity.right));
                        this.operators.push(lastToken);
                    }
                    else  {
                        while (Bridge.Linq.Enumerable.from(this.operators).any()) {
                            var precedence = 0;
                            if (Bridge.is(this.operators.peek().getData(), ThreeOneSevenBee.Model.Expression.Operator)) {
                                precedence = (Bridge.as(this.operators.peek().getData(), ThreeOneSevenBee.Model.Expression.Operator)).getPrecedence();
                            }
                            if ((op1.v.getAssociativity() === ThreeOneSevenBee.Model.Expression.OperatorAssociativity.left && op1.v.getPrecedence() <= precedence) || (op1.v.getAssociativity() === ThreeOneSevenBee.Model.Expression.OperatorAssociativity.right && op1.v.getPrecedence() < precedence)) {
                                this.output.enqueue(this.operators.pop());
                                continue;
                            }
                            else  {
                                if (this.operators.peek().getType() === ThreeOneSevenBee.Model.Expression.TokenType.$function) {
                                    this.output.enqueue(this.operators.pop());
                                    continue;
                                }
                            }
    
                            break;
                        }
                        lastToken = Bridge.get(ThreeOneSevenBee.Model.Expression.Token).operator(op1.v);
                        this.operators.push(lastToken);
                    }
    
                    continue;
                }
    
                if (inFix.charCodeAt(i.v) === 40) {
                    lastToken = Bridge.get(ThreeOneSevenBee.Model.Expression.Token).operator(new ThreeOneSevenBee.Model.Expression.Operator("constructor", "("));
                    this.operators.push(lastToken);
                    i.v++;
                    continue;
                }
    
                if (inFix.charCodeAt(i.v) === 41) {
                    while (Bridge.Linq.Enumerable.from(this.operators).any()) {
                        var tok = this.operators.pop();
                        if (tok.getType() === ThreeOneSevenBee.Model.Expression.TokenType.operator && Bridge.String.equals((Bridge.as(tok.getData(), ThreeOneSevenBee.Model.Expression.Operator)).getSymbol(), "(")) {
                            break;
                        }
                        this.output.enqueue(tok);
                    }
                    lastToken = Bridge.get(ThreeOneSevenBee.Model.Expression.Token).delimiter();
                    lastToken.setData("()");
                    this.output.enqueue(lastToken);
                    i.v++;
                    continue;
                }
    
    
                if (inFix.charCodeAt(i.v) === 123) {
                    lastToken = Bridge.get(ThreeOneSevenBee.Model.Expression.Token).operator(new ThreeOneSevenBee.Model.Expression.Operator("constructor", "{"));
                    this.operators.push(lastToken);
                    i.v++;
                    continue;
                }
    
                if (inFix.charCodeAt(i.v) === 125) {
                    while (Bridge.Linq.Enumerable.from(this.operators).any()) {
                        var tok1 = this.operators.pop();
                        if (tok1.getType() === ThreeOneSevenBee.Model.Expression.TokenType.operator && Bridge.String.equals((Bridge.as(tok1.getData(), ThreeOneSevenBee.Model.Expression.Operator)).getSymbol(), "{")) {
                            break;
                        }
                        this.output.enqueue(tok1);
                    }
                    lastToken = Bridge.get(ThreeOneSevenBee.Model.Expression.Token).delimiter();
                    lastToken.setData("{}");
                    this.output.enqueue(lastToken);
                    i.v++;
                    continue;
                }
    
                throw new Bridge.InvalidOperationException("Unexpected token: " + String.fromCharCode(inFix.charCodeAt(i.v)));
            }
    
            var output = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.Token)();
            while (Bridge.Linq.Enumerable.from(this.output).any()) {
                output.add(this.output.dequeue());
            }
            while (Bridge.Linq.Enumerable.from(this.operators).any()) {
                output.add(this.operators.pop());
            }
    
            return output;
        },
        parse: function (postFix) {
            var $t;
            var stack = new ThreeOneSevenBee.Model.Collections.Stack$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
            var root = null;
            $t = Bridge.getEnumerator(postFix);
            while ($t.moveNext()) {
                var token = $t.getCurrent();
                switch (token.getType()) {
                    case ThreeOneSevenBee.Model.Expression.TokenType.$function: 
                        root = new ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression(stack.pop(), Bridge.cast(token.getData(), String));
                        stack.push(root);
                        break;
                    case ThreeOneSevenBee.Model.Expression.TokenType.delimiter: 
                        switch (token.getData().toString()) {
                            case "()": 
                                root = new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(stack.pop());
                                stack.push(root);
                                break;
                            case "{}": 
                                root = stack.pop();
                                stack.push(root);
                                break;
                        }
                        break;
                    case ThreeOneSevenBee.Model.Expression.TokenType.number: 
                        root = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(Bridge.cast(token.getData(), Number));
                        stack.push(root);
                        break;
                    case ThreeOneSevenBee.Model.Expression.TokenType.constant: 
                        root = new ThreeOneSevenBee.Model.Expression.Expressions.ConstantExpression(token.getData());
                        stack.push(root);
                        break;
                    case ThreeOneSevenBee.Model.Expression.TokenType.variable: 
                        root = new ThreeOneSevenBee.Model.Expression.Expressions.VariableExpression(Bridge.cast(token.getData(), String));
                        stack.push(root);
                        break;
                    case ThreeOneSevenBee.Model.Expression.TokenType.operator: 
                        switch (token.getData().toString()) {
                            case "~": 
                                root = new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(stack.pop());
                                stack.push(root);
                                break;
                            case "+": 
                                var right = stack.pop();
                                var left = stack.pop();
                                root = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(left, right, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add);
                                stack.push(root);
                                break;
                            case "-": 
                                right = stack.pop();
                                left = stack.pop();
                                root = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(left, right, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract);
                                stack.push(root);
                                break;
                            case "*": 
                                right = stack.pop();
                                left = stack.pop();
                                root = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(left, right, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply);
                                stack.push(root);
                                break;
                            case "/": 
                                right = stack.pop();
                                left = stack.pop();
                                root = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(left, right, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide);
                                stack.push(root);
                                break;
                            case "^": 
                                right = stack.pop();
                                left = stack.pop();
                                root = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(left, right, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power);
                                stack.push(root);
                                break;
                        }
                        break;
                }
            }
            return root;
        },
        parse$1: function (inFix) {
            return this.parse(this.inFixToPostFix(inFix));
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.ILeaf');
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.ExpressionSerializer', {
        config: {
            init: function () {
                this.parser = new ThreeOneSevenBee.Model.Expression.ExpressionParser() || null;
            }
        },
        serialize: function (expression) {
            return expression.toString();
        },
        deserialize: function (expression) {
            return this.parser.parse$1(expression);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.IExpression');
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Operator', {
        config: {
            properties: {
                Symbol: null,
                Precedence: 0,
                Associativity: null
            }
        },
        constructor$1: function (symbol, precedence, associativity) {
            this.setSymbol(symbol);
            this.setPrecedence(precedence);
            this.setAssociativity(associativity);
        },
        constructor: function (symbol) {
            ThreeOneSevenBee.Model.Expression.Operator.prototype.constructor$1.call(this, symbol, Bridge.getDefaultValue(Bridge.Int), Bridge.getDefaultValue(ThreeOneSevenBee.Model.Expression.OperatorAssociativity));
    
        },
        getHashCode: function () {
            return Bridge.getHashCode(this.getSymbol()) + Bridge.getHashCode(this.getPrecedence()) + Bridge.getHashCode(this.getAssociativity());
        },
        toString: function () {
            return this.getSymbol();
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.OperatorAssociativity', {
        statics: {
            left: 0,
            right: 1
        },
        $enum: true
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Token', {
        statics: {
            number: function (data) {
                return new ThreeOneSevenBee.Model.Expression.Token(ThreeOneSevenBee.Model.Expression.TokenType.number, data);
            },
            variable: function (data) {
                return new ThreeOneSevenBee.Model.Expression.Token(ThreeOneSevenBee.Model.Expression.TokenType.variable, data);
            },
            constant: function (type) {
                return new ThreeOneSevenBee.Model.Expression.Token(ThreeOneSevenBee.Model.Expression.TokenType.constant, type);
            },
            operator: function (op) {
                return new ThreeOneSevenBee.Model.Expression.Token(ThreeOneSevenBee.Model.Expression.TokenType.operator, op);
            },
            delimiter: function () {
                return new ThreeOneSevenBee.Model.Expression.Token(ThreeOneSevenBee.Model.Expression.TokenType.delimiter, "()");
            },
            $function: function ($function) {
                return new ThreeOneSevenBee.Model.Expression.Token(ThreeOneSevenBee.Model.Expression.TokenType.$function, $function);
            }
        },
        config: {
            properties: {
                Type: null,
                Data: null
            }
        },
        constructor: function (type, data) {
            this.setType(type);
            this.setData(data);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.TokenType', {
        statics: {
            number: 0,
            variable: 1,
            constant: 2,
            operator: 3,
            delimiter: 4,
            $function: 5,
            leftBracket: 6,
            rightBracket: 7
        },
        $enum: true
    });
    
    
    
    Bridge.init();
})(this);
