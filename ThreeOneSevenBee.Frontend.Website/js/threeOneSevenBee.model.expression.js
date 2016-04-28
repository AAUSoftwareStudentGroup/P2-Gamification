(function (globals) {
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
        config: {
            init: function () {
                this.wrapInParenthesis = Bridge.merge(new Bridge.Dictionary$2(ThreeOneSevenBee.Model.Expression.Expressions.OperatorType,Bridge.Dictionary$2(ThreeOneSevenBee.Model.Expression.Expressions.OperatorType,Boolean))(), [
        [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.minus, Bridge.merge(new Bridge.Dictionary$2(ThreeOneSevenBee.Model.Expression.Expressions.OperatorType,Boolean)(), [
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.minus, true],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, true],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power, false]
        ] )],
        [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, Bridge.merge(new Bridge.Dictionary$2(ThreeOneSevenBee.Model.Expression.Expressions.OperatorType,Boolean)(), [
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.minus, true],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, true],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power, true]
        ] )],
        [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide, Bridge.merge(new Bridge.Dictionary$2(ThreeOneSevenBee.Model.Expression.Expressions.OperatorType,Boolean)(), [
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.minus, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power, false]
        ] )],
        [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, Bridge.merge(new Bridge.Dictionary$2(ThreeOneSevenBee.Model.Expression.Expressions.OperatorType,Boolean)(), [
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.minus, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power, true]
        ] )],
        [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power, Bridge.merge(new Bridge.Dictionary$2(ThreeOneSevenBee.Model.Expression.Expressions.OperatorType,Boolean)(), [
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.minus, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, false],
            [ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power, true]
        ] )]
    ] ) || null;
            }
        },
        constructor: function () {
            this.rules = new Bridge.List$1(Function)();
        },
        add: function (rule) {
            this.rules.add(rule);
        },
        remove: function (rule) {
            this.rules.remove(rule);
        },
        getCommonParent$1: function (selection) {
            if (selection.getCount() === 0) {
                return null;
            }
            else  {
                if (selection.getCount() === 1) {
                    return selection.getItem(0);
                }
                else  {
                    var parentPaths = new Bridge.List$1(Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase))();
                    for (var index = 0; index < selection.getCount(); index++) {
                        parentPaths.add(Bridge.Linq.Enumerable.from(selection.getItem(index).getParentPath()).reverse().toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
                    }
                    return this.getCommonParent(parentPaths);
                }
            }
        },
        getCommonParent: function (parentPaths) {
            var intersection = this.pathIntersection(parentPaths.getItem(0), parentPaths.getItem(1));
            for (var index = 2; index < parentPaths.getCount(); index++) {
                intersection = this.pathIntersection(intersection, parentPaths.getItem(index));
            }
            return intersection.getItem(intersection.getCount() - 1);
        },
        pathIntersection: function (first, second) {
            var secondIndex = 0;
            return Bridge.Linq.Enumerable.from(first).takeWhile(function (expr) {
                return secondIndex === second.getCount() || expr === second.getItem(secondIndex++);
            }).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase);
        },
        wrapInDelimiterIfNeccessary$1: function (expression, parentType) {
            var isNeccessary = this.wrapInParenthesis.get(expression.getType()).get(parentType);
    
            if (isNeccessary) {
                return new ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression(expression);
            }
            else  {
                return expression;
            }
        },
        wrapInDelimiterIfNeccessary: function (expression, parent) {
            if (!Bridge.hasValue(parent)) {
                return expression;
            }
            var operatorExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression);
            var parentExpression = Bridge.as(parent, ThreeOneSevenBee.Model.Expression.Expressions.OperatorExpression);
    
            if (!Bridge.hasValue(operatorExpression) || !Bridge.hasValue(parentExpression)) {
                return expression;
            }
            else  {
                return this.wrapInDelimiterIfNeccessary$1(operatorExpression, parentExpression.getType());
            }
        },
        getIdentities: function (selection) {
            var $t, $t1, $t2, $t3;
            var identities = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.Identity)();
    
            if (selection.getCount() === 0) {
                return identities;
            }
    
            var commonParent = this.getCommonParent$1(selection);
            if (Bridge.is(commonParent, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)) {
                var variadicParent = Bridge.as(commonParent, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
    
                var operandsLeftOfSelection = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                var operandsRightOfSelection = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                var selectedOperands = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
    
                $t = Bridge.getEnumerator(variadicParent);
                while ($t.moveNext()) {
                    var operand = $t.getCurrent();
                    if (operand.getSelected() === false && Bridge.Linq.Enumerable.from(operand.getNodesRecursive()).any($_.ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer.f1) === false) {
                        if (selectedOperands.getCount() === 0) {
                            operandsLeftOfSelection.add(operand);
                        }
                        else  {
                            operandsRightOfSelection.add(operand);
                        }
                    }
                    else  {
                        selectedOperands.add(operand);
                    }
                }
    
                var toBeReplaced = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", variadicParent.getType(), selectedOperands.getItem(0).clone(), selectedOperands.getItem(1).clone());
                var toBeReplacedSelection = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
                $t1 = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(selectedOperands).skip(2));
                while ($t1.moveNext()) {
                    var operand1 = $t1.getCurrent();
                    toBeReplaced.add(operand1.clone());
                }
    
                toBeReplacedSelection = Bridge.Linq.Enumerable.from(toBeReplaced.getNodesRecursive()).where($_.ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer.f1).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase);
                var c = 0;
                $t2 = Bridge.getEnumerator(this.rules);
                while ($t2.moveNext()) {
                    var rule = $t2.getCurrent();
                    var suggestion = rule(toBeReplaced, toBeReplacedSelection);
    
                    if (Bridge.hasValue(suggestion)) {
                        var result;
                        if (variadicParent.getCount() === selectedOperands.getCount()) {
                            result = suggestion;
                        }
                        else  {
                            var variadicResult = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", variadicParent.getType(), new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(-1), new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(-1));
                            variadicResult.add$1(Bridge.Linq.Enumerable.from(operandsLeftOfSelection).select($_.ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer.f2).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
                            variadicResult.add(this.wrapInDelimiterIfNeccessary(suggestion.clone(), variadicResult));
                            variadicResult.add$1(Bridge.Linq.Enumerable.from(operandsRightOfSelection).select($_.ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer.f2).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase));
                            variadicResult.removeAt(0);
                            variadicResult.removeAt(0);
                            result = variadicResult;
                        }
                        identities.add(new ThreeOneSevenBee.Model.Expression.Identity(suggestion, this.wrapInDelimiterIfNeccessary(result, commonParent.getParent())));
                    }
                }
            }
            else  {
                var c1 = 0;
                $t3 = Bridge.getEnumerator(this.rules);
                while ($t3.moveNext()) {
                    var rule1 = $t3.getCurrent();
                    var suggestion1 = this.wrapInDelimiterIfNeccessary(rule1(commonParent, selection), commonParent.getParent());
                    if (Bridge.hasValue(suggestion1)) {
                        identities.add(new ThreeOneSevenBee.Model.Expression.Identity(suggestion1, suggestion1));
                    }
                }
            }
    
            return identities;
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer, {
        f1: function (n) {
            return n.getSelected() === true;
        },
        f2: function (o) {
            return o.clone();
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.ExpressionBase', {
        inherits: function () { return [Bridge.IEquatable$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)]; },
        statics: {
            op_Equality: function (left, right) {
                if (left === null) {
                    // ...and right hand side is null...
                    if (right === null) {
                        //...both are null and are Equal.
                        return true;
                    }
    
                    // ...right hand side is not null, therefore not Equal.
                    return false;
                }
                return left.equalsT(right);
            },
            op_Inequality: function (left, right) {
                return !(ThreeOneSevenBee.Model.Expression.ExpressionBase.op_Equality(left, right));
            }
        },
        config: {
            properties: {
                /**
                 * The parent of this node. If this is null, this node is the root of the tree.
                 *
                 * @instance
                 * @public
                 * @this ThreeOneSevenBee.Model.Expression.ExpressionBase
                 * @memberof ThreeOneSevenBee.Model.Expression.ExpressionBase
                 * @function getParent
                 * @return  {ThreeOneSevenBee.Model.Expression.ExpressionBase}
                 */
                /**
                 * The parent of this node. If this is null, this node is the root of the tree.
                 *
                 * @instance
                 * @public
                 * @this ThreeOneSevenBee.Model.Expression.ExpressionBase
                 * @memberof ThreeOneSevenBee.Model.Expression.ExpressionBase
                 * @function setParent
                 * @param   {ThreeOneSevenBee.Model.Expression.ExpressionBase}    value
                 * @return  {void}
                 */
                Parent: null,
                Selected: false,
                ToBeReplaced: false
            },
            init: function () {
                this.analyzer = new ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer() || null;
            }
        },
        /**
         * Replaces this expression with the replacement expression if possible.
         *
         * @instance
         * @public
         * @this ThreeOneSevenBee.Model.Expression.ExpressionBase
         * @memberof ThreeOneSevenBee.Model.Expression.ExpressionBase
         * @param   {ThreeOneSevenBee.Model.Expression.ExpressionBase}    replacement
         * @return  {boolean}                                                            True if a replacement took place.
         */
        replace: function (replacement) {
            if (Bridge.hasValue(this.getParent())) {
                this.getParent().replace$1(this, replacement, false);
            }
            return false;
        },
        /**
         * Returns the path of nodes to get to the root of the tree.
         *
         * @instance
         * @public
         * @this ThreeOneSevenBee.Model.Expression.ExpressionBase
         * @memberof ThreeOneSevenBee.Model.Expression.ExpressionBase
         * @return  {Bridge.IEnumerable$1}
         */
        getParentPath: function () {
            var $yield = [];
            var currentParent = this;
            while (Bridge.hasValue(currentParent)) {
                $yield.push(currentParent);
                currentParent = currentParent.getParent();
            }
            return Bridge.Array.toEnumerable($yield);
        },
        /**
         * Determines if the other expression has the same value, but not reference.
         *
         * @instance
         * @public
         * @override
         * @this ThreeOneSevenBee.Model.Expression.ExpressionBase
         * @memberof ThreeOneSevenBee.Model.Expression.ExpressionBase
         * @param   {Object}     other
         * @return  {boolean}
         */
        equals: function (other) {
            return (Bridge.is(other, ThreeOneSevenBee.Model.Expression.ExpressionBase)) && this.equalsT(Bridge.cast(other, ThreeOneSevenBee.Model.Expression.ExpressionBase));
        },
        getHashCode: function () {
            return Bridge.getHashCode(this.getValue());
        },
        toString: function () {
            return "{" + this.getValue() + "}";
        },
        /**
         * Prints the tree to the standard output stream.
         *
         * @instance
         * @public
         * @this ThreeOneSevenBee.Model.Expression.ExpressionBase
         * @memberof ThreeOneSevenBee.Model.Expression.ExpressionBase
         * @return  {void}
         */
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
        onChanged: null,
        constructor: function (expression, onChange, rules) {
            var $t;
            if (rules === void 0) { rules = []; }
            this.selectionParent = null;
            this.selection = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)();
            this.identities = new Bridge.List$1(ThreeOneSevenBee.Model.Expression.Identity)();
            this.serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
            this.analyzer = new ThreeOneSevenBee.Model.Expression.ExpressionAnalyzer();
            this.expression = this.serializer.deserialize(expression);
            $t = Bridge.getEnumerator(rules);
            while ($t.moveNext()) {
                var rule = $t.getCurrent();
                this.analyzer.add(rule);
            }
            this.onChanged = onChange;
            this.callOnChanged();
    },
    constructor$1: function (expression, rules) {
        ThreeOneSevenBee.Model.Expression.ExpressionModel.prototype.$constructor.call(this, expression, null, rules);
    
        if (rules === void 0) { rules = []; }
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
    callOnChanged: function () {
        if (Bridge.hasValue(this.onChanged)) {
            this.onChanged(this);
        }
    },
    select: function (expression) {
        expression.setSelected(expression.getSelected() === false);
    
        this.updateSelection();
    
        this.updateIdentities();
    },
    updateSelection: function () {
        this.selection = Bridge.Linq.Enumerable.from(this.getExpression().getNodesRecursive()).where($_.ThreeOneSevenBee.Model.Expression.ExpressionModel.f1).toList(ThreeOneSevenBee.Model.Expression.ExpressionBase);
    
        if (this.getExpression().getSelected() === true) {
            this.selection.insert(0, this.getExpression());
        }
    },
    updateIdentities: function () {
        this.updateSelection();
        this.selectionParent = this.analyzer.getCommonParent$1(this.selection);
        this.identities = this.analyzer.getIdentities(this.selection);
        console.log("Identities updated");
        if (Bridge.hasValue(this.onChanged)) {
            this.onChanged(this);
        }
    },
    unSelectAll: function () {
        for (var index = 0; index < this.selection.getCount(); index++) {
            this.selection.getItem(index).setSelected(false);
        }
        this.selection.clear();
        this.identities.clear();
        this.selectionParent = null;
        this.callOnChanged();
    },
    applyIdentity: function (identity) {
        var parent = this.getSelected().getParent();
        if (!Bridge.hasValue(parent)) {
            this.expression = identity;
        }
        else  {
            this.getSelected().replace(identity);
        }
        this.getSelected().setParent(parent);
        this.updateSelection();
        this.unSelectAll();
        this.updateIdentities();
    }
    });
    
    Bridge.ns("ThreeOneSevenBee.Model.Expression.ExpressionModel", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.Expression.ExpressionModel, {
        f1: function (e) {
            return e.getSelected();
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
        treatMinusAsUnaryMinusInVariadicPlus: true,
        config: {
            init: function () {
                this.ops = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.Expression.Operator)(), [
        [new ThreeOneSevenBee.Model.Expression.Operator("constructor$1", "^", 4, ThreeOneSevenBee.Model.Expression.OperatorAssociativity.right)],
        [new ThreeOneSevenBee.Model.Expression.Operator("constructor$1", "*", 3, ThreeOneSevenBee.Model.Expression.OperatorAssociativity.left)],
        [new ThreeOneSevenBee.Model.Expression.Operator("constructor$1", "/", 3, ThreeOneSevenBee.Model.Expression.OperatorAssociativity.left)],
        [new ThreeOneSevenBee.Model.Expression.Operator("constructor$1", "-", 2, ThreeOneSevenBee.Model.Expression.OperatorAssociativity.left)],
        [new ThreeOneSevenBee.Model.Expression.Operator("constructor$1", "+", 1, ThreeOneSevenBee.Model.Expression.OperatorAssociativity.left)]
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
    
            var validLetters = "abcdefghijklmnopqrstuvwxyz";
    
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
            var left = null;
            var right = null;
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
                        root = new ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression(Bridge.Int.parseInt(token.getData().toString(), -2147483648, 2147483647));
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
                                right = stack.pop();
                                left = stack.pop();
                                if (Bridge.is(left, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression) && (Bridge.as(left, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)).getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                                    (Bridge.as(left, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)).add(right);
                                    root = left;
                                    stack.push(root);
                                }
                                else  {
                                    root = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, left, right);
                                    stack.push(root);
                                }
                                break;
                            case "-": 
                                if (this.treatMinusAsUnaryMinusInVariadicPlus) {
                                    right = new ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression(stack.pop());
                                    left = stack.pop();
                                    if (Bridge.is(left, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression) && (Bridge.as(left, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)).getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                                        (Bridge.as(left, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)).add(right);
                                        root = left;
                                        stack.push(root);
                                    }
                                    else  {
                                        root = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add, left, right);
                                        stack.push(root);
                                    }
                                }
                                else  {
                                    right = stack.pop();
                                    left = stack.pop();
                                    root = new ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression(left, right, ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract);
                                    stack.push(root);
                                }
                                break;
                            case "*": 
                                right = stack.pop();
                                left = stack.pop();
                                if (Bridge.is(left, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression) && (Bridge.as(left, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)).getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                                    (Bridge.as(left, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression)).add(right);
                                    root = left;
                                    stack.push(root);
                                }
                                else  {
                                    root = new ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression("constructor", ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply, left, right);
                                    stack.push(root);
                                }
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
    
    Bridge.define('ThreeOneSevenBee.Model.Expression.Identity', {
        suggestion: null,
        result: null,
        constructor: function (suggestion, result) {
            this.suggestion = suggestion;
            this.result = result;
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
