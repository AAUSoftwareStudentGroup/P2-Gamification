(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.UI.View', {
        onClick: null,
        config: {
            properties: {
                Width: 0,
                Height: 0,
                X: 0,
                Y: 0,
                Name: null,
                Selected: false
            }
        },
        click: function (x, y) {
            if (this.containsPoint(x, y) && Bridge.hasValue(this.onClick)) {
                this.onClick();
            }
        },
        containsPoint: function (x, y) {
            return x >= this.getX() && y >= this.getY() && x <= this.getX() + this.getWidth() && y <= this.getY() + this.getHeight();
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.Context', {
        _contentView: null,
        config: {
            properties: {
                Width: 0,
                Height: 0
            }
        },
        constructor: function (width, height) {
            this.setWidth(width);
            this.setHeight(height);
        },
        setContentView: function (view) {
            this._contentView = view;
        },
        draw: function () {
            this.clear();
            this._contentView.drawWithContext(this, 0, 0);
        },
        draw$1: function (view, offsetX, offsetY) {
            this.draw$2(Bridge.as(view, ThreeOneSevenBee.Model.UI.LabelView), offsetX, offsetY);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ProgressbarCircle', {
        starLevels: null,
        constructor: function (levels) {
            if (levels === void 0) { levels = []; }
            this.starLevels = new Bridge.List$1(ThreeOneSevenBee.Model.UI.ProgressbarStar)(levels);
        },
        add: function (level) {
            if (!this.starLevels.contains(level)) {
                this.starLevels.add(level);
            }
        },
        remove: function (level) {
            if (this.starLevels.contains(level)) {
                this.starLevels.remove(level);
            }
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ProgressbarStar', {
        _maxProgress: 0,
        _currentProgress: 0,
        _stars: null,
        constructor: function (progress, maxValue, stars) {
            if (stars === void 0) { stars = []; }
            this._currentProgress = progress;
            this._maxProgress = maxValue;
            this._stars = new Bridge.List$1(Bridge.Int)(stars);
            this.getStars();
        },
        getProgress: function () {
            return this._currentProgress;
        },
        setProgress: function (value) {
            this._currentProgress = value;
        },
        getPercentage: function () {
            return Bridge.cast(this._currentProgress, Number) / this._maxProgress;
        },
        getMaxProgress: function () {
            return this._maxProgress;
        },
        setMaxProgress: function (value) {
            this._maxProgress = value;
        },
        add: function (star) {
            if (!this._stars.contains(star)) {
                this._stars.add(star);
            }
        },
        remove: function (star) {
            if (this._stars.contains(star)) {
                this._stars.remove(star);
            }
        },
        getStars: function () {
            var $t;
            var starsCount = 0;
            var totalStars = 0;
    
            $t = Bridge.getEnumerator(this._stars);
            while ($t.moveNext()) {
                var i = $t.getCurrent();
                totalStars++;
                if (i <= this._currentProgress) {
                    starsCount++;
                }
            }
            // Returns amount of reached stars.
            return starsCount;
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.LabelView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        config: {
            properties: {
                Text: null
            }
        },
        constructor: function (text) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this);
    
            this.setText(text);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.draw$2(this, offsetX, offsetY);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.CompositeView', {
        inherits: [ThreeOneSevenBee.Model.UI.View,Bridge.IEnumerable$1(ThreeOneSevenBee.Model.UI.View)],
        children: null,
        config: {
            properties: {
                PropagateClick: false
            }
        },
        constructor: function (width, height) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this);
    
            this.setWidth(width);
            this.setHeight(height);
            this.children = new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)();
            this.setPropagateClick(true);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            var $t;
            $t = Bridge.getEnumerator(this.children);
            while ($t.moveNext()) {
                var child = $t.getCurrent();
                child.drawWithContext(context, offsetX + this.getX(), offsetY + this.getY());
            }
        },
        click: function (x, y) {
            var $t;
            if (ThreeOneSevenBee.Model.UI.View.prototype.containsPoint.call(this, x, y)) {
                if (this.getPropagateClick()) {
                    $t = Bridge.getEnumerator(this.children);
                    while ($t.moveNext()) {
                        var child = $t.getCurrent();
                        child.click(x - this.getX(), y - this.getY());
                    }
                }
    
                if (Bridge.hasValue(this.onClick)) {
                    this.onClick();
                }
            }
        },
        getEnumerator$1: function () {
            return this.children.getEnumerator();
        },
        getEnumerator: function () {
            return this.getEnumerator$1();
        },
        add: function (view) {
            this.children.add(view);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ProgressbarStarView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        progressbar: null,
        constructor: function (progressbar) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this);
    
            this.progressbar = new ThreeOneSevenBee.Model.UI.ProgressbarStar(50, 100);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.draw$4(this, offsetX, offsetY);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ButtonView', {
        inherits: [ThreeOneSevenBee.Model.UI.LabelView],
        constructor: function (text, onClick) {
            ThreeOneSevenBee.Model.UI.LabelView.prototype.$constructor.call(this, text);
    
            this.onClick = onClick;
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.draw$1(this, offsetX, offsetY);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ExpressionView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        statics: {
            build: function (expression, model) {
                var $t;
                if (Bridge.is(expression, ThreeOneSevenBee.Model.Expression.Expressions.NumericExpression) || Bridge.is(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariableExpression)) {
                    return Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView(expression.toString(), function () {
                        model.select(expression);
                    }), {
                        setWidth: 20,
                        setHeight: 20,
                        setSelected: model.selectionIndex(expression) !== -1
                    } );
                }
                var operatorExpression;
                if (Bridge.hasValue((($t = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression), operatorExpression = $t, $t)))) {
                    var left;
                    var operatorSign;
                    var right;
    
                    if (operatorExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                        // Move right expression up
                        // Move left expression down
                        // Draw line in the middle with max width of left and right
    
                        left = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(operatorExpression.getLeft(), model);
                        operatorSign = new ThreeOneSevenBee.Model.UI.OperatorButtonView(operatorExpression.getType(), null);
                        right = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(operatorExpression.getRight(), model);
                        left.setY(left.getY()-left.getHeight() / 2);
                        right.setY(right.getY()+right.getHeight() / 2);
                        right.setX(left.getX() + left.getWidth() / 2 - right.getWidth() / 2);
                    }
                    else  {
                        left = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(operatorExpression.getLeft(), model);
                        operatorSign = Bridge.merge(new ThreeOneSevenBee.Model.UI.OperatorButtonView(operatorExpression.getType(), null), {
                            setX: left.getWidth(),
                            setWidth: 20,
                            setHeight: 20
                        } );
                        right = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(operatorExpression.getRight(), model);
                        right.setX(operatorSign.getWidth() + left.getWidth());
                    }
                    return Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(left.getWidth() + operatorSign.getWidth() + right.getWidth(), Math.max(Math.max(left.getHeight(), operatorSign.getHeight()), right.getHeight())), [
                        [left],
                        [operatorSign],
                        [right]
                    ] );
                }
                return null;
            }
        },
        constructor: function (model, width, height) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, width, height);
    
            var view = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(model.getExpression(), model);
            this.children.add(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(model.getExpression(), model));
            model.addOnChanged(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.ExpressionView.f1));
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Model.UI.ExpressionView", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.UI.ExpressionView, {
        f1: function (m) {
            this.children.clear();
            this.children.add(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(m.getExpression(), m));
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.IdentityMenuView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        constructor: function (model, width, height) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, width, height);
    
            this.children = this.build(model.getIdentities(), model);
            model.addOnChanged(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.IdentityMenuView.f1));
        },
        build: function (identities, model) {
            var views = new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)();
            var x = 0;
            for (var index = 0; index < Bridge.Linq.Enumerable.from(identities).count(); index++) {
                (function () {
                    var indexCopy = index;
                    var view = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(identities.getItem(index), model);
                    var Container = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(view.getWidth(), view.getHeight()), {
                        setPropagateClick: false
                    } );
                    Container.add(view);
                    Container.setX(x);
                    x += Container.getWidth() + 20;
                    Container.onClick = function () {
                        model.applyIdentity(identities.getItem(indexCopy));
                    };
                    views.add(Container);
                }).call(this);
            }
            return views;
        },
        click: function (x, y) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.click.call(this, x, y);
        }
    });
    
    Bridge.ns("ThreeOneSevenBee.Model.UI.IdentityMenuView", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.UI.IdentityMenuView, {
        f1: function (m) {
            this.children = this.build(m.getIdentities(), m);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.OperatorButtonView', {
        inherits: [ThreeOneSevenBee.Model.UI.ButtonView],
        config: {
            properties: {
                type: null
            }
        },
        constructor: function (type, onClick) {
            ThreeOneSevenBee.Model.UI.ButtonView.prototype.$constructor.call(this, "", onClick);
    
            this.settype(type);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.draw$3(this, offsetX, offsetY);
        }
    });
    
    Bridge.init();
})(this);
