(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.UI.View', {
        onClick: null,
        onChanged: null,
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
            context.draw$9(this, offsetX, offsetY);
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
        contentView: null,
        config: {
            properties: {
                Width: 0,
                Height: 0
            }
        },
        constructor: function (width, height) {
            this.contentView = new ThreeOneSevenBee.Model.UI.View(0, 0, 0, 0);
            this.setWidth(width);
            this.setHeight(height);
        },
        setContentView: function (view) {
            this.contentView = view;
        },
        draw: function () {
            this.clear();
            this.contentView.drawWithContext(this, 0, 0);
        },
        draw$2: function (view, offsetX, offsetY) {
            this.draw$9(Bridge.as(view, ThreeOneSevenBee.Model.UI.View), offsetX, offsetY);
        },
        draw$3: function (view, offsetX, offsetY) {
            this.draw$9(Bridge.as(view, ThreeOneSevenBee.Model.UI.View), offsetX, offsetY);
        },
        draw$1: function (view, offsetX, offsetY) {
            this.draw$3(Bridge.as(view, ThreeOneSevenBee.Model.UI.LabelView), offsetX, offsetY);
        },
        draw$7: function (view, offsetX, offsetY) {
            this.draw$9(Bridge.as(view, ThreeOneSevenBee.Model.UI.View), offsetX, offsetY);
        },
        draw$4: function (view, offsetX, offsetY) {
            this.draw$9(Bridge.as(view, ThreeOneSevenBee.Model.UI.View), offsetX, offsetY);
        },
        draw$5: function (view, offsetX, offsetY) {
            this.draw$9(Bridge.as(view, ThreeOneSevenBee.Model.UI.View), offsetX, offsetY);
        },
        draw$8: function (view, offsetX, offsetY) {
            this.draw$9(Bridge.as(view, ThreeOneSevenBee.Model.UI.View), offsetX, offsetY);
        },
        draw$6: function (view, offsetX, offsetY) {
            this.draw$9(Bridge.as(view, ThreeOneSevenBee.Model.UI.View), offsetX, offsetY);
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
            this.starLevels = new Bridge.List$1(ThreeOneSevenBee.Model.Game.ProgressbarStar)(levels);
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
    
    Bridge.define('ThreeOneSevenBee.Model.UI.LabelView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        config: {
            properties: {
                FontColor: null,
                Font: null,
                FontSize: 0,
                Align: null,
                Text: null
            }
        },
        constructor: function (text) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, 10, 10);
    
            this.setText(text);
            this.setFontColor("#000000");
            this.setFont("Cambria Math");
            this.setFontSize(this.getHeight());
            this.setAlign("center");
        },
        scale: function (factor) {
            this.setFontSize(this.getFontSize()*factor);
            return ThreeOneSevenBee.Model.UI.View.prototype.scale.call(this, factor);
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
            context.draw$9(this, offsetX, offsetY);
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
        constructor: function (width, height) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.constructor$2.call(this, width, height, new ThreeOneSevenBee.Model.UI.View(0, 0, 0, 0), true, 0);
    
        },
        constructor$1: function (width, height, content) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.constructor$2.call(this, width, height, content, true, height / content.getHeight());
    
        },
        constructor$3: function (width, height, content, maxScale) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.constructor$2.call(this, width, height, content, true, maxScale);
    
        },
        constructor$2: function (width, height, content, propagateClick, maxScale) {
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
            context.draw$9(this, offsetX, offsetY);
            this.getContent$1().drawWithContext(context, offsetX + this.getInnerX(), offsetY + this.getInnerY());
        },
        align: function (view) {
            view.setX((this.getInnerWidth() - view.getWidth()) / 2);
            view.setY((this.getInnerHeight() - view.getHeight()) / 2);
            return view;
        },
        fit: function (view) {
            return view.scale(Math.min(this.getInnerWidth() / view.getWidth(), Math.min(this.getInnerHeight() / view.getHeight(), this.maxScale === 0 ? Number.MAX_VALUE : this.maxScale)));
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
            this.setBackgroundColor("transparent");
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
    
    Bridge.define('ThreeOneSevenBee.Model.UI.PolygonView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        fillStyle: null,
        config: {
            properties: {
                model: null,
                cornerPositions: null
            }
        },
        constructor$1: function (model, fillStyle, x, y, width, height) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, x, y, width, height);
    
            this.fillStyle = fillStyle;
            this.setcornerPositions(new Bridge.List$1(ThreeOneSevenBee.Model.Euclidean.Vector2)());
            var vector = new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", 0, 0);
            if (Bridge.Linq.Enumerable.from(model.getcorners()).count() < 3) {
                throw new Bridge.Exception("[Polygonview]: Model has less than 3 corners");
            }
            var totalAngle = Math.PI * (Bridge.Linq.Enumerable.from(model.getcorners()).count() - 2);
            var angle = totalAngle / Bridge.Linq.Enumerable.from(model.getcorners()).count();
            angle = Math.PI - angle;
            console.log("Angle: " + angle);
            this.getcornerPositions().add(vector.$clone());
            for (var i = 1; i < Bridge.Linq.Enumerable.from(model.getcorners()).count(); i++) {
                vector.x += Math.cos(angle * i);
                vector.y += Math.sin(angle * i);
                this.getcornerPositions().add(vector.$clone());
            }
            this.setcornerPositions(this.getcornerPositions());
            this.centerAndScale(width, height);
        },
        constructor: function (model, cornerPositions, fillStyle, x, y, width, height) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, x, y, width, height);
    
            this.fillStyle = fillStyle;
            // Draw model per specifications
            if (Bridge.Linq.Enumerable.from(model.getcorners()).count() !== Bridge.Linq.Enumerable.from(cornerPositions).count()) {
                throw new Bridge.ArgumentException("Non-equal amounts of cornerpositions(" + Bridge.Linq.Enumerable.from(cornerPositions).count() + ") and corners(" + Bridge.Linq.Enumerable.from(model.getcorners()).count() + ")!");
            }
    
            this.setcornerPositions(cornerPositions);
            this.centerAndScale(width, height);
        },
        centerAndScale: function (width, height) {
            var $t;
            var min = new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", Number.MAX_VALUE, Number.MAX_VALUE);
            var max = new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", Number.MIN_VALUE, Number.MIN_VALUE);
            $t = Bridge.getEnumerator(this.getcornerPositions());
            while ($t.moveNext()) {
                var corner = $t.getCurrent();
                if (corner.x < min.x) {
                    min.x = corner.x;
                }
                if (corner.x > max.x) {
                    max.x = corner.x;
                }
                if (corner.y > max.y) {
                    max.y = corner.y;
                }
                if (corner.y < min.y) {
                    min.y = corner.y;
                }
            }
            max = ThreeOneSevenBee.Model.Euclidean.Vector2.op_Subtraction(max.$clone(), min.$clone());
            var scale = (max.x - width < max.y - height) ? height / max.y : width / max.x;
            for (var i = 0; i < Bridge.Linq.Enumerable.from(this.getcornerPositions()).count(); i++) {
                this.getcornerPositions().setItem(i, ThreeOneSevenBee.Model.Euclidean.Vector2.op_Subtraction(this.getcornerPositions().getItem(i), min.$clone()));
                this.getcornerPositions().setItem(i, ThreeOneSevenBee.Model.Euclidean.Vector2.op_Multiply$1(this.getcornerPositions().getItem(i), scale)); //  Take the biggest offset and scale accordingly
            }
    
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.draw$6(this, offsetX + this.getX(), offsetY + this.getX());
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.SqrtView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        config: {
            properties: {
                SignWidth: 0,
                TopHeight: 0
            }
        },
        constructor: function () {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, 30, 20);
    
            this.setSignWidth(10);
            this.setTopHeight(5);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.draw$8(this, offsetX, offsetY);
        },
        scale: function (factor) {
            this.setSignWidth(this.getSignWidth()*factor);
            this.setTopHeight(this.getTopHeight()*factor);
            return ThreeOneSevenBee.Model.UI.View.prototype.scale.call(this, factor);
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
            nUMVAR_SIZE: 20
        },
        constructor: function () {
            ThreeOneSevenBee.Model.UI.ExpressionView.prototype.constructor$1.call(this, 0, 0);
    
        },
        constructor$1: function (width, height) {
            ThreeOneSevenBee.Model.UI.ExpressionView.prototype.constructor$2.call(this, null, width, height, 0);
    
        },
        constructor$2: function (model, width, height, maxScale) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.$constructor.call(this, width, height);
    
            this.maxScale = maxScale;
            this.build(model);
        },
        buildView: function (expression, model) {
            var $t, $t1;
            var minusExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
            if (Bridge.hasValue(minusExpression)) {
                var view = this.buildView(minusExpression.getExpression(), model);
                var operatorView = new ThreeOneSevenBee.Model.UI.OperatorView(minusExpression.getType());
                operatorView.setWidth(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                operatorView.setHeight(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                operatorView.setBaseline(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                operatorView.onClick = function () {
                    model.select(expression);
                };
                operatorView.setBackgroundColor(model.selectionIndex(expression) !== -1 ? "#cccccc" : "transparent");
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
                var left = this.buildView(operatorExpression.getLeft(), model);
                var right = this.buildView(operatorExpression.getRight(), model);
                var operatorView1 = new ThreeOneSevenBee.Model.UI.OperatorView(operatorExpression.getType());
                switch (operatorExpression.getType()) {
                    case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide: 
                        var width = Math.max(left.getWidth(), right.getWidth()) + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE;
                        operatorView1.setWidth(width);
                        operatorView1.setHeight(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                        operatorView1.setY(left.getHeight());
                        operatorView1.setBaseline(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                        operatorView1.onClick = function () {
                            model.select(expression);
                        };
                        operatorView1.setBackgroundColor(model.selectionIndex(expression) !== -1 ? "#cccccc" : "transparent");
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
                        left.setY(right.getHeight() - Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                        var exponent = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(right.getX() + right.getWidth(), left.getY() + left.getHeight()), [
                            [left],
                            [right]
                        ] );
                        exponent.setBaseline(left.getY() + left.getBaseline());
                        return exponent;
                    case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract: 
                        var baseline = Math.max(operatorView1.getBaseline(), Math.max(left.getBaseline(), right.getBaseline()));
                        operatorView1.setX(left.getWidth());
                        operatorView1.setWidth(2 * Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
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
                        operatorView2.setWidth((variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply ? 0.5 : 1.5) * Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                        operatorView2.setHeight(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                        operatorView2.setBaseline(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                        views.add(operatorView2);
                        offsetX += operatorView2.getWidth();
                    }
                    var operand = this.buildView(expr, model);
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
                var view2 = this.buildView(delimiterExpression.getExpression(), model);
                view2.setX(view2.getHeight() / 4);
                view2.setY(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 8);
                var compositeView = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(view2.getWidth() + view2.getHeight() / 2, view2.getHeight() + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 4), [
                    [Bridge.merge(new ThreeOneSevenBee.Model.UI.ParenthesisView(ThreeOneSevenBee.Model.UI.ParenthesisType.left), {
                        onClick: function () {
                            model.select(expression);
                        },
                        setWidth: view2.getHeight() / 4,
                        setHeight: view2.getHeight() + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 4
                    } )],
                    [view2],
                    [Bridge.merge(new ThreeOneSevenBee.Model.UI.ParenthesisView(ThreeOneSevenBee.Model.UI.ParenthesisType.right), {
                        onClick: function () {
                            model.select(expression);
                        },
                        setX: view2.getWidth() + view2.getHeight() / 4,
                        setWidth: view2.getHeight() / 4,
                        setHeight: view2.getHeight() + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 4
                    } )]
                ] );
                compositeView.setBaseline(view2.getY() + view2.getBaseline());
                return compositeView;
            }
            var functionExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression);
            if (Bridge.hasValue(functionExpression) && functionExpression.getFunction() === "sqrt") {
                var view3 = this.buildView(functionExpression.getExpression(), model);
                var sqrtView = new ThreeOneSevenBee.Model.UI.SqrtView();
                sqrtView.setSignWidth(view3.getHeight() / 2);
                sqrtView.setTopHeight(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                sqrtView.setWidth(view3.getWidth() + sqrtView.getSignWidth() + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 4);
                sqrtView.setHeight(view3.getHeight() + sqrtView.getTopHeight());
                view3.setX(sqrtView.getSignWidth() + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 8);
                view3.setY(sqrtView.getTopHeight());
                var compositeView1 = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(sqrtView.getWidth(), sqrtView.getHeight()), [
                    [sqrtView],
                    [view3]
                ] );
                compositeView1.setBaseline(view3.getBaseline() + sqrtView.getTopHeight());
                return compositeView1;
            }
            return Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView(expression.toString(), function () {
                model.select(expression);
            }), {
                setWidth: 3 * Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 5 * expression.toString().length,
                setHeight: Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE,
                setBaseline: Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2,
                setFontSize: Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE,
                setBackgroundColor: model.selectionIndex(expression) !== -1 ? "#cccccc" : "transparent"
            } );
    
        },
        build: function (model) {
            if (Bridge.hasValue(model)) {
                this.setContent(this.buildView(model.getExpression(), model));
            }
        },
        update: function (model) {
            this.build(model);
            if (Bridge.hasValue(this.onChanged)) {
                this.onChanged();
            }
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.GameView', {
        inherits: [ThreeOneSevenBee.Model.UI.FrameView],
        titleView: null,
        levelView: null,
        context: null,
        constructor: function (game, context) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.$constructor.call(this, context.getWidth(), context.getHeight());
    
            this.context = context;
    
            this.titleView = new ThreeOneSevenBee.Model.UI.TitleView(game.getPlayers());
    
            this.levelView = Bridge.merge(new ThreeOneSevenBee.Model.UI.LevelView(game, context.getWidth(), context.getHeight()), {
                setOnExit: Bridge.fn.bind(this, function () {
                    game.save();
                    this.setContent(this.titleView);
                }),
                setOnNextLevel: function () {
                    game.save();
                    game.nextLevel();
                }
            } );
    
            this.titleView.playButton.onClick = Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.GameView.f1);
    
            this.titleView.levelButton.onClick = Bridge.fn.bind(this, function () {
                game.setLevel(Bridge.Int.parseInt(prompt(), -2147483648, 2147483647) - 1, 0);
                this.setContent(this.levelView);
            });
    
            game.onChanged = Bridge.fn.bind(this, this.update);
    
            this.setContent(this.titleView);
        },
        update: function (game) {
            this.levelView.update(game);
    
            this.context.draw();
        },
        setContent: function (content) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.setContent.call(this, content);
            this.context.setContentView(this.getContent$1());
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Model.UI.GameView", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.UI.GameView, {
        f1: function () {
            this.setContent(this.levelView);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.IdentityMenuView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        constructor: function (model, width, height) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, width, height);
    
            this.update(model.getIdentities(), model);
        },
        update: function (identities, model) {
            var views = new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)();
            var x = 0;
            for (var index = 0; index < identities.getCount(); index++) {
                (function () {
                    var indexCopy = index;
                    var expressionView = new ThreeOneSevenBee.Model.UI.ExpressionView("constructor");
                    var view = expressionView.buildView(identities.getItem(index).suggestion, model);
                    var frameView = Bridge.merge(new ThreeOneSevenBee.Model.UI.FrameView("constructor$3", this.getWidth() / identities.getCount(), this.getHeight(), view, 2), {
                        setPropagateClick: false
                    } );
                    frameView.setX(x);
                    x += frameView.getWidth();
                    frameView.onClick = function () {
                        model.applyIdentity(identities.getItem(indexCopy).result);
                    };
                    views.add(frameView);
                }).call(this);
            }
            this.children = views;
        },
        click: function (x, y) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.click.call(this, x, y);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.LevelSelectView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        constructor: function (game) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, 600, 300);
    
    
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.LevelView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        menuButton: null,
        nextButton: null,
        progressbar: null,
        identityMenu: null,
        expression: null,
        config: {
            properties: {
                OnExit: null,
                OnNextLevel: null
            }
        },
        constructor: function (game, width, height) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, width, height);
    
            this.build(game);
        },
        build: function (game) {
            this.menuButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView("Menu", Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.LevelView.f1)), {
                setWidth: 100,
                setHeight: 50,
                setBackgroundColor: "#C1392B",
                setFontColor: "#FFFFFF",
                setFont: "Segoe UI",
                setFontSize: 25
            } );
    
            this.nextButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView("Næste", Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.LevelView.f2)), {
                setX: this.getWidth() - 100,
                setWidth: 100,
                setHeight: 50,
                setBackgroundColor: game.getLevelCompleted() ? "#16A086" : "#BEC3C7",
                setFontColor: "#FFFFFF",
                setFont: "Segoe UI",
                setFontSize: 25
            } );
    
            this.progressbar = Bridge.merge(new ThreeOneSevenBee.Model.UI.ProgressbarStarView(game.progressBar, this.getWidth() - 220, 30), {
                setX: 110,
                setY: 10
            } );
    
            this.identityMenu = Bridge.merge(new ThreeOneSevenBee.Model.UI.IdentityMenuView(game.getExprModel(), this.getWidth(), 100), {
                setY: this.getHeight() - 100
            } );
    
            this.expression = Bridge.merge(new ThreeOneSevenBee.Model.UI.ExpressionView("constructor$2", game.getExprModel(), this.getWidth(), this.getHeight() - 150, 4), {
                setX: 0,
                setY: 50
            } );
    
            this.children = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)(), [
                [this.menuButton],
                [this.nextButton],
                [this.progressbar],
                [this.identityMenu],
                [this.expression]
            ] );
    
            if (Bridge.hasValue(this.onChanged)) {
                this.onChanged();
            }
        },
        update: function (game) {
            this.progressbar.update(game.progressBar);
            this.identityMenu.update(game.getExprModel().getIdentities(), game.getExprModel());
            this.expression.update(game.getExprModel());
            this.nextButton.setBackgroundColor(game.getLevelCompleted() ? "#16A086" : "#BEC3C7");
    
            if (Bridge.hasValue(this.onChanged)) {
                this.onChanged();
            }
        }
    });
    
    Bridge.ns("ThreeOneSevenBee.Model.UI.LevelView", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.UI.LevelView, {
        f1: function () {
            this.getOnExit()();
        },
        f2: function () {
            this.getOnNextLevel()();
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.PlayerListView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        constructor: function (players, width, height) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, width, height);
    
            this.build(players);
            this.setBackgroundColor("#D1D5D8");
        },
        build: function (players) {
            var $t;
            this.children = new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)();
            var offsetY = 5;
            $t = Bridge.getEnumerator(players);
            while ($t.moveNext()) {
                var player = $t.getCurrent();
                this.children.add(Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView(player.getPlayerName()), {
                    setX: 5,
                    setY: offsetY,
                    setWidth: this.getWidth() - 10,
                    setHeight: 20,
                    setFontSize: 15,
                    setFont: "Segoe UI",
                    setBackgroundColor: "#EFEFEF",
                    setAlign: "left"
                } ));
                offsetY += 25;
            }
        },
        update: function (players) {
            this.build(players);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ProgressbarStarView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        progress: null,
        stars: null,
        constructor: function (progressbar, width, height) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, width, height);
    
            this.setPropagateClick(false);
            this.setBackgroundColor("#E2E2E2");
            this.build(progressbar);
        },
        build: function (progressbar) {
            this.progress = Bridge.merge(new ThreeOneSevenBee.Model.UI.View(0, 0, this.getWidth() * progressbar.getPercentage(), this.getHeight()), {
                setBackgroundColor: "#27AE61"
            } );
            this.stars = new Bridge.List$1(ThreeOneSevenBee.Model.UI.ImageView)();
            this.stars.addRange(Bridge.Linq.Enumerable.from(progressbar.activatedStarPercentages()).select(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.ProgressbarStarView.f1)));
            this.stars.addRange(Bridge.Linq.Enumerable.from(progressbar.deactivatedStarPercentages()).select(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.ProgressbarStarView.f2)));
            this.children = new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)();
            this.children.add(this.progress);
            this.children.addRange(this.stars);
        },
        update: function (progressbar) {
            this.build(progressbar);
        }
    });
    
    Bridge.ns("ThreeOneSevenBee.Model.UI.ProgressbarStarView", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.UI.ProgressbarStarView, {
        f1: function (starPercentage) {
            return Bridge.merge(new ThreeOneSevenBee.Model.UI.ImageView("star_activated.png", this.getHeight(), this.getHeight()), {
                setX: starPercentage * this.getWidth() - this.getHeight()
            } );
        },
        f2: function (starPercentage) {
            return Bridge.merge(new ThreeOneSevenBee.Model.UI.ImageView("star.png", this.getHeight(), this.getHeight()), {
                setX: starPercentage * this.getWidth() - this.getHeight()
            } );
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.TitleView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        playButton: null,
        levelButton: null,
        playerList: null,
        constructor: function (players) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, 600, 300);
    
            this.playButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.ImageView("playbutton.png", 100, 100), {
                setX: 100,
                setY: 100,
                setBackgroundColor: "#27AE61"
            } );
            this.levelButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.ImageView("levelbutton.png", 100, 100), {
                setX: 220,
                setY: 100,
                setBackgroundColor: "#2A80B9"
            } );
            this.playerList = Bridge.merge(new ThreeOneSevenBee.Model.UI.PlayerListView(players, 160, 200), {
                setX: 340,
                setY: 50
            } );
            this.children = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)(), [
                [this.playButton],
                [this.levelButton],
                [this.playerList]
            ] );
        }
    });
    
    
    
    Bridge.init();
})(this);
