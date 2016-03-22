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
                Baseline: 0,
                BackgroundColor: null
            }
        },
        constructor: function (x, y, width, height) {
            this.setX(x);
            this.setY(y);
            this.setWidth(width);
            this.setHeight(height);
            this.setBaseline(height / 2);
            this.setBackgroundColor("transparent");
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.draw$7(this, offsetX, offsetY);
        },
        click: function (x, y) {
            if (this.containsPoint(x, y) && Bridge.hasValue(this.onClick)) {
                this.onClick();
            }
        },
        scale: function (factor) {
            this.setX(this.getX()*factor);
            this.setY(this.getY()*factor);
            this.setWidth(this.getWidth()*factor);
            this.setHeight(this.getHeight()*factor);
            return this;
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
        draw$2: function (view, offsetX, offsetY) {
            this.draw$7(Bridge.as(view, ThreeOneSevenBee.Model.UI.View), offsetX, offsetY);
        },
        draw$3: function (view, offsetX, offsetY) {
            this.draw$7(Bridge.as(view, ThreeOneSevenBee.Model.UI.View), offsetX, offsetY);
        },
        draw$1: function (view, offsetX, offsetY) {
            this.draw$3(Bridge.as(view, ThreeOneSevenBee.Model.UI.LabelView), offsetX, offsetY);
        },
        draw$6: function (view, offsetX, offsetY) {
            this.draw$7(Bridge.as(view, ThreeOneSevenBee.Model.UI.View), offsetX, offsetY);
        },
        draw$4: function (view, offsetX, offsetY) {
            this.draw$7(Bridge.as(view, ThreeOneSevenBee.Model.UI.View), offsetX, offsetY);
        },
        draw$5: function (view, offsetX, offsetY) {
            this.draw$7(Bridge.as(view, ThreeOneSevenBee.Model.UI.View), offsetX, offsetY);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ParenthesisType', {
        statics: {
            left: 0,
            right: 1
        },
        $enum: true
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
        stars: null,
        constructor: function (progress, maxValue, stars) {
            if (stars === void 0) { stars = []; }
            this._currentProgress = progress;
            this._maxProgress = maxValue;
            this.stars = new Bridge.List$1(Bridge.Int)(stars);
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
            if (!this.stars.contains(star)) {
                this.stars.add(star);
            }
        },
        remove: function (star) {
            if (this.stars.contains(star)) {
                this.stars.remove(star);
            }
        },
        getStars: function () {
            var $t;
            var starsCount = 0;
            var totalStars = 0;
    
            $t = Bridge.getEnumerator(this.stars);
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
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, 10, 10);
    
            this.setText(text);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.draw$3(this, offsetX, offsetY);
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
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, width, height);
    
            this.children = new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)();
            this.setPropagateClick(true);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            var $t;
            context.draw$7(this, offsetX, offsetY);
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
        scale: function (factor) {
            var $t;
            $t = Bridge.getEnumerator(this.children);
            while ($t.moveNext()) {
                var child = $t.getCurrent();
                child.scale(factor);
            }
            return ThreeOneSevenBee.Model.UI.View.prototype.scale.call(this, factor);
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
    
    Bridge.define('ThreeOneSevenBee.Model.UI.FrameView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        padding: 0,
        maxScale: 0,
        config: {
            properties: {
                Content$1: null,
                PropagateClick: false
            }
        },
        constructor: function (width, height, content) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.constructor$1.call(this, width, height, content, true, height / content.getHeight());
    
        },
        constructor$2: function (width, height, content, maxScale) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.constructor$1.call(this, width, height, content, true, maxScale);
    
        },
        constructor$1: function (width, height, content, propagateClick, maxScale) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, width, height);
    
            this.setWidth(width);
            this.setHeight(height);
            this.setPropagateClick(propagateClick);
            this.maxScale = maxScale;
            this.padding = 0;
            this.setContent$1(this.align(this.fit(content)));
        },
        getInnerX: function () {
            return this.getX() - this.padding;
        },
        getInnerY: function () {
            return this.getY() - this.padding;
        },
        getInnerWidth: function () {
            return this.getWidth() - this.padding;
        },
        getInnerHeight: function () {
            return this.getHeight() - this.padding;
        },
        setContent: function (content) {
            this.setContent$1(this.align(this.fit(content)));
        },
        click: function (x, y) {
    
            if (ThreeOneSevenBee.Model.UI.View.prototype.containsPoint.call(this, x, y)) {
                if (this.getPropagateClick()) {
                    this.getContent$1().click(x - this.getX(), y - this.getY());
                }
    
                if (Bridge.hasValue(this.onClick)) {
                    this.onClick();
                }
            }
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.draw$7(this, offsetX, offsetY);
            this.getContent$1().drawWithContext(context, offsetX + this.getInnerX(), offsetY + this.getInnerY());
        },
        align: function (view) {
            view.setX((this.getInnerWidth() - view.getWidth()) / 2);
            view.setY((this.getInnerHeight() - view.getHeight()) / 2);
            return view;
        },
        fit: function (view) {
            return view.scale(Math.min(this.getInnerWidth() / view.getWidth(), Math.min(this.getInnerHeight() / view.getHeight(), this.maxScale)));
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ImageView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        config: {
            properties: {
                Image: null
            }
        },
        constructor: function (image, width, height) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, width, height);
    
            this.setImage(image);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.draw$2(this, offsetX, offsetY);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.OperatorView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        config: {
            properties: {
                type: null
            }
        },
        constructor: function (type) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, 10, 10);
    
            this.settype(type);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.draw$4(this, offsetX, offsetY);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ParenthesisView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        config: {
            properties: {
                Type: null
            }
        },
        constructor: function (type) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, 10, 10);
    
            this.setType(type);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.draw$5(this, offsetX, offsetY);
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
        inherits: [ThreeOneSevenBee.Model.UI.FrameView],
        statics: {
            nUMVAR_SIZE: 20,
            build: function (expression, model) {
                var $t, $t1;
                var minusExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                if (Bridge.hasValue(minusExpression)) {
                    var view = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(minusExpression.getExpression(), model);
                    var operatorView = new ThreeOneSevenBee.Model.UI.OperatorView(minusExpression.getType());
                    operatorView.setWidth(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                    operatorView.setHeight(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                    operatorView.setBaseline(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                    view.setX(operatorView.getWidth());
                    operatorView.setY(view.getBaseline() - operatorView.getBaseline());
                    var minusView = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(operatorView.getWidth() + view.getWidth(), view.getHeight()), [
                        [operatorView],
                        [view]
                    ] );
                    minusView.setBaseline(view.getBaseline());
                    return minusView;
                }
                var operatorExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.BinaryOperatorExpression);
                if (Bridge.hasValue(operatorExpression)) {
                    var left = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(operatorExpression.getLeft(), model);
                    var right = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(operatorExpression.getRight(), model);
                    var operatorView1 = new ThreeOneSevenBee.Model.UI.OperatorView(operatorExpression.getType());
                    switch (operatorExpression.getType()) {
                        case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide: 
                            var width = Math.max(left.getWidth(), right.getWidth()) + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE;
                            operatorView1.setWidth(width);
                            operatorView1.setHeight(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                            operatorView1.setY(left.getHeight());
                            operatorView1.setBaseline(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                            right.setY(left.getHeight() + operatorView1.getHeight());
                            left.setX((width - left.getWidth()) / 2);
                            right.setX((width - right.getWidth()) / 2);
                            var fraction = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(width, left.getHeight() + operatorView1.getHeight() + right.getHeight()), [
                                [left],
                                [operatorView1],
                                [right]
                            ] );
                            fraction.setBaseline(operatorView1.getY() + operatorView1.getHeight() / 2);
                            return fraction;
                        case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power: 
                            right.setX(left.getWidth());
                            left.setY(right.getHeight());
                            var exponent = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(right.getX() + right.getWidth(), left.getY() + left.getHeight()), [
                                [left],
                                [right]
                            ] );
                            exponent.setBaseline(exponent.getHeight() - left.getBaseline());
                            return exponent;
                        case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract: 
                            var baseline = Math.max(operatorView1.getBaseline(), Math.max(left.getBaseline(), right.getBaseline()));
                            operatorView1.setX(left.getWidth());
                            operatorView1.setWidth(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                            operatorView1.setHeight(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                            operatorView1.setBaseline(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                            right.setX(left.getWidth() + operatorView1.getWidth());
                            left.setY(baseline - left.getBaseline());
                            operatorView1.setY(baseline - operatorView1.getBaseline());
                            right.setY(baseline - right.getBaseline());
                            var height = Math.max(left.getY() + left.getHeight(), Math.max(operatorView1.getY() + operatorView1.getHeight(), right.getY() + right.getHeight()));
                            var subtraction = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(right.getX() + right.getWidth(), height), [
                                [left],
                                [operatorView1],
                                [right]
                            ] );
                            subtraction.setBaseline(baseline);
                            return subtraction;
                    }
                }
                var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
                if (Bridge.hasValue(variadicExpression)) {
                    var views = new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)();
                    var offsetX = 0;
                    var height1 = 0;
                    var maxBaseline = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2;
                    $t = Bridge.getEnumerator(variadicExpression);
                    while ($t.moveNext()) {
                        var expr = $t.getCurrent();
                        if (views.getCount() !== 0) {
                            var operatorView2 = new ThreeOneSevenBee.Model.UI.OperatorView(variadicExpression.getType());
                            operatorView2.setX(offsetX);
                            operatorView2.setWidth(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                            operatorView2.setHeight(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                            operatorView2.setBaseline(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                            views.add(operatorView2);
                            offsetX += operatorView2.getWidth();
                        }
                        var operand = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(expr, model);
                        maxBaseline = Math.max(maxBaseline, operand.getBaseline());
                        operand.setX(offsetX);
                        offsetX += operand.getWidth();
                        views.add(operand);
                    }
                    $t1 = Bridge.getEnumerator(views);
                    while ($t1.moveNext()) {
                        var view1 = $t1.getCurrent();
                        view1.setY(maxBaseline - view1.getBaseline());
                        height1 = Math.max(height1, view1.getY() + view1.getHeight());
                    }
                    return Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(offsetX, height1), {
                        children: views,
                        setBaseline: maxBaseline
                    } );
                }
                var delimiterExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
                if (Bridge.hasValue(delimiterExpression)) {
                    var view2 = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(delimiterExpression.getExpression(), model);
                    view2.setX(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                    var compositeView = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(view2.getWidth() + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE, view2.getHeight()), [
                        [Bridge.merge(new ThreeOneSevenBee.Model.UI.ParenthesisView(ThreeOneSevenBee.Model.UI.ParenthesisType.left), {
                            setWidth: Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2,
                            setHeight: view2.getHeight()
                        } )],
                        [view2],
                        [Bridge.merge(new ThreeOneSevenBee.Model.UI.ParenthesisView(ThreeOneSevenBee.Model.UI.ParenthesisType.right), {
                            setX: view2.getWidth() + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2,
                            setWidth: Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2,
                            setHeight: view2.getHeight()
                        } )]
                    ] );
                    compositeView.setBaseline(view2.getBaseline());
                    return compositeView;
                }
                return Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView(expression.toString(), function () {
                    model.select(expression);
                }), {
                    setWidth: Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE,
                    setHeight: Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE,
                    setBaseline: Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2,
                    setBackgroundColor: model.selectionIndex(expression) !== -1 ? "#cccccc" : "transparent"
                } );
    
            }
        },
        constructor: function (model, width, height) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.constructor$2.call(this, width, height, Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(model.getExpression(), model), 2);
    
            model.addOnChanged(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.ExpressionView.f1));
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Model.UI.ExpressionView", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.UI.ExpressionView, {
        f1: function (m) {
            this.setContent(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(m.getExpression(), m));
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
            for (var index = 0; index < identities.getCount(); index++) {
                (function () {
                    var indexCopy = index;
                    var view = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).build(identities.getItem(index), model);
                    var frameView = Bridge.merge(new ThreeOneSevenBee.Model.UI.FrameView("constructor$2", this.getWidth() / identities.getCount(), this.getHeight(), view, 1), {
                        setPropagateClick: false
                    } );
                    frameView.setX(x);
                    x += frameView.getWidth();
                    frameView.onClick = function () {
                        model.applyIdentity(identities.getItem(indexCopy));
                    };
                    views.add(frameView);
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
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ProgressbarStarView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        constructor: function (progressbar, width, height) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, width, height);
            var $t;
    
            this.setPropagateClick(false);
            this.setBackgroundColor("#E2E2E2");
            this.children = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)(), [
                [Bridge.merge(new ThreeOneSevenBee.Model.UI.View(0, 0, this.getWidth() * progressbar.getPercentage(), height), {
                    setBackgroundColor: "#2A9300"
                } )]
            ] );
            $t = Bridge.getEnumerator(progressbar.stars);
            while ($t.moveNext()) {
                var star = $t.getCurrent();
                this.children.add(Bridge.merge(new ThreeOneSevenBee.Model.UI.ImageView(star < progressbar.getProgress() ? "star_activated.png" : "star.png", height, height), {
                    setX: Bridge.cast(star, Number) / progressbar.getMaxProgress() * this.getWidth() - this.getHeight() / 2,
                    setBackgroundColor: "#000000"
                } ));
            }
    }
    });
    
    
    
    Bridge.init();
})(this);
