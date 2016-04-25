(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.UI.View', {
        onClick: null,
        onKeyPressed: null,
        onChanged: null,
        config: {
            properties: {
                Width: 0,
                Height: 0,
                X: 0,
                Y: 0,
                Baseline: 0,
                BackgroundColor: null,
                Visible: false
            }
        },
        constructor: function (x, y, width, height) {
            this.setX(x);
            this.setY(y);
            this.setWidth(width);
            this.setHeight(height);
            this.setBaseline(height / 2);
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor"));
            this.setVisible(true);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.drawRectangle(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), this.getBackgroundColor());
        },
        click: function (x, y) {
            if (this.containsPoint(x, y) && Bridge.hasValue(this.onClick)) {
                this.onClick();
            }
        },
        keyPressed: function (key, lastClick) {
            if (Bridge.hasValue(this.onKeyPressed)) {
                this.onKeyPressed(key, lastClick.$clone());
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
    
    Bridge.define('ThreeOneSevenBee.Model.UI.Color', {
        red: 0,
        green: 0,
        blue: 0,
        alpha: 0,
        constructor: function () {
            ThreeOneSevenBee.Model.UI.Color.prototype.constructor$2.call(this, 0, 0, 0, 0);
    
        },
        constructor$1: function (red, green, blue) {
            ThreeOneSevenBee.Model.UI.Color.prototype.constructor$2.call(this, red, green, blue, 1);
    
        },
        constructor$2: function (red, green, blue, alpha) {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.IContext');
    
    Bridge.define('ThreeOneSevenBee.Model.UI.Direction', {
        statics: {
            top: 0,
            right: 1,
            left: 2,
            bottom: 3
        },
        $enum: true
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
                TextColor: null,
                Text: null
            }
        },
        constructor: function (text) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, 10, 10);
    
            this.setText(text);
            this.setTextColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
        },
        drawWithContext: function (context, offsetX, offsetY) {
            ThreeOneSevenBee.Model.UI.View.prototype.drawWithContext.call(this, context, offsetX, offsetY);
            context.drawText(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), this.getText(), this.getTextColor());
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.CompositeView', {
        inherits: [ThreeOneSevenBee.Model.UI.View,Bridge.IEnumerable$1(ThreeOneSevenBee.Model.UI.View)],
        children: null,
        config: {
            properties: {
                PropagateClick: false,
                PropagateKeypress: false
            }
        },
        constructor: function (width, height) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, width, height);
    
            this.children = new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)();
            this.setPropagateClick(true);
            this.setPropagateKeypress(true);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            var $t;
            if (this.getVisible() === true) {
                ThreeOneSevenBee.Model.UI.View.prototype.drawWithContext.call(this, context, offsetX, offsetY);
                $t = Bridge.getEnumerator(this.children);
                while ($t.moveNext()) {
                    var child = $t.getCurrent();
                    child.drawWithContext(context, offsetX + this.getX(), offsetY + this.getY());
                }
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
        keyPressed: function (key, lastClick) {
            var $t;
            if (this.getPropagateKeypress()) {
                $t = Bridge.getEnumerator(this.children);
                while ($t.moveNext()) {
                    var child = $t.getCurrent();
                    child.keyPressed(key, lastClick.$clone());
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
        },
        clear: function () {
            this.children.clear();
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.Context', {
        inherits: [ThreeOneSevenBee.Model.UI.IContext],
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
        drawRectangle: function (x, y, width, height, fillColor) {
            this.drawRectangle$1(x, y, width, height, fillColor, new ThreeOneSevenBee.Model.UI.Color("constructor"), 0);
        },
        drawRectangle$1: function (x, y, width, height, fillColor, lineColor, lineWidth) {
            this.drawPolygon$1([new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", x, y), new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", x + width, y), new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", x + width, y + height), new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", x, y + height)], fillColor, lineColor, lineWidth);
        },
        drawLine: function (first, second, lineColor, lineWidth) {
            this.drawPolygon$1([first.$clone(), second.$clone()], new ThreeOneSevenBee.Model.UI.Color("constructor"), lineColor, lineWidth);
        },
        drawPolygon: function (path, fillColor) {
            this.drawPolygon$1(path, fillColor, new ThreeOneSevenBee.Model.UI.Color("constructor"), 1);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.FrameView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        padding: 0,
        maxScale: 0,
        config: {
            properties: {
                Content$1: null,
                PropagateClick: false,
                PropagateKeypress: false
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
            this.setPropagateKeypress(true);
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
        keyPressed: function (key, lastClick) {
            if (this.getPropagateKeypress()) {
                this.getContent$1().keyPressed(key, lastClick.$clone());
            }
        },
        drawWithContext: function (context, offsetX, offsetY) {
            ThreeOneSevenBee.Model.UI.View.prototype.drawWithContext.call(this, context, offsetX, offsetY);
            this.getContent$1().drawWithContext(context, offsetX + this.getInnerX(), offsetY + this.getInnerY());
        },
        align: function (view) {
            view.setX((this.getInnerWidth() - view.getWidth()) / 2);
            view.setY((this.getInnerHeight() - view.getHeight()) / 2);
            return view;
        },
        fit: function (view) {
            return view.scale(Math.min(this.getInnerWidth() / view.getWidth(), Math.min(this.getInnerHeight() / view.getHeight(), this.maxScale === 0 ? Number.MAX_VALUE : this.maxScale)));
        },
        scale: function (factor) {
            this.getContent$1().scale(factor);
            return ThreeOneSevenBee.Model.UI.View.prototype.scale.call(this, factor);
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
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor"));
        },
        drawWithContext: function (context, offsetX, offsetY) {
            ThreeOneSevenBee.Model.UI.View.prototype.drawWithContext.call(this, context, offsetX, offsetY);
            context.drawPNGImage(this.getImage(), this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight());
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.OperatorView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        config: {
            properties: {
                Type: null,
                LineWidth: 0,
                LineColor: null
            }
        },
        constructor: function (type) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, 10, 10);
    
            this.setType(type);
            this.setLineWidth(1);
            this.setLineColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
        },
        drawWithContext: function (context, offsetX, offsetY) {
            ThreeOneSevenBee.Model.UI.View.prototype.drawWithContext.call(this, context, offsetX, offsetY);
            switch (this.getType()) {
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add: 
                    context.drawText(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), "+", this.getLineColor());
                    break;
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract: 
                    context.drawText(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), "-", this.getLineColor());
                    break;
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.minus: 
                    context.drawText(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), "-", this.getLineColor());
                    break;
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide: 
                    context.drawLine(new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", this.getX() + offsetX, this.getY() + offsetY + this.getHeight() / 2), new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", this.getX() + offsetX + this.getWidth(), this.getY() + offsetY + this.getHeight() / 2), this.getLineColor(), this.getLineWidth());
                    break;
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply: 
                    context.drawText(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), "·", this.getLineColor());
                    break;
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power: 
                    break;
                default: 
                    break;
            }
        },
        scale: function (factor) {
            this.setLineWidth(this.getLineWidth()*factor);
            return ThreeOneSevenBee.Model.UI.View.prototype.scale.call(this, factor);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ParenthesisView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        config: {
            properties: {
                Type: null,
                LineColor: null,
                LineWidth: 0
            }
        },
        constructor: function (type) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, 10, 10);
    
            this.setType(type);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.drawText(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), this.getType() === ThreeOneSevenBee.Model.UI.ParenthesisType.left ? "(" : ")", this.getLineColor());
        },
        scale: function (factor) {
            this.setLineWidth(this.getLineWidth()*factor);
            return ThreeOneSevenBee.Model.UI.View.prototype.scale.call(this, factor);
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
    
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.SqrtView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        config: {
            properties: {
                SignWidth: 0,
                TopHeight: 0,
                LineColor: null,
                LineWidth: 0
            }
        },
        constructor: function () {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, 30, 20);
    
            this.setLineColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
            this.setLineWidth(5);
            this.setSignWidth(10);
            this.setTopHeight(5);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.drawPolygon$1([new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", this.getX() + offsetX + this.getSignWidth() / 8, this.getY() + offsetY + this.getHeight() - this.getSignWidth() / 2), new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", this.getX() + offsetX + this.getSignWidth() / 4, this.getY() + offsetY + this.getHeight() - this.getSignWidth() / 2), new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", this.getX() + offsetX + this.getSignWidth() / 2 - this.getLineWidth() / 2, this.getY() + offsetY + this.getHeight()), new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", this.getX() + offsetX + this.getSignWidth() / 2, this.getY() + offsetY + this.getHeight()), new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", this.getX() + offsetX + this.getSignWidth(), this.getY() + offsetY + this.getTopHeight() / 2), new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", this.getX() + offsetX + this.getWidth(), this.getY() + offsetY + this.getTopHeight() / 2)], new ThreeOneSevenBee.Model.UI.Color("constructor"), this.getLineColor(), this.getLineWidth());
        },
        scale: function (factor) {
            this.setSignWidth(this.getSignWidth()*factor);
            this.setTopHeight(this.getTopHeight()*factor);
            this.setLineWidth(this.getLineWidth()*factor);
            return ThreeOneSevenBee.Model.UI.View.prototype.scale.call(this, factor);
        },
        containsPoint: function (x, y) {
            return ThreeOneSevenBee.Model.UI.View.prototype.containsPoint.call(this, x, y) && (this.getX() + this.getSignWidth() > x || this.getY() + this.getTopHeight() > y);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.VectorImageView', {
        inherits: [ThreeOneSevenBee.Model.UI.View,Bridge.IEnumerable$1(ThreeOneSevenBee.Model.Euclidean.Vector2)],
        config: {
            properties: {
                Path: null
            }
        },
        constructor: function (x, y, width, height) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, x, y, width, height);
    
            this.setPath(new Bridge.List$1(ThreeOneSevenBee.Model.Euclidean.Vector2)());
        },
        add: function (x, y) {
            this.getPath().add(new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", x, y));
        },
        scale: function (factor) {
            for (var i = 0; i < this.getPath().getCount(); i++) {
                this.getPath().setItem(i, ThreeOneSevenBee.Model.Euclidean.Vector2.op_Multiply$1(this.getPath().getItem(i), factor));
            }
            return ThreeOneSevenBee.Model.UI.View.prototype.scale.call(this, factor);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            if (this.getVisible() === true) {
                context.drawPolygon(Bridge.Linq.Enumerable.from(this.getPath()).select(Bridge.fn.bind(this, function (p) {
                    return new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", this.getX() + p.x + offsetX, this.getY() + p.y + offsetY);
                })).toArray(), this.getBackgroundColor());
            }
        },
        getEnumerator$1: function () {
            return this.getPath().getEnumerator();
        },
        getEnumerator: function () {
            return this.getEnumerator$1();
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ButtonView', {
        inherits: [ThreeOneSevenBee.Model.UI.LabelView],
        constructor: function (text, onClick) {
            ThreeOneSevenBee.Model.UI.LabelView.prototype.$constructor.call(this, " " + text + " ");
    
            this.onClick = onClick;
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
                operatorView.setWidth(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                operatorView.setHeight(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                operatorView.setBaseline(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                operatorView.onClick = function () {
                    model.select(minusExpression);
                };
    
                operatorView.setLineColor(expression.getSelected() ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 39, 174, 97) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
                operatorView.setLineWidth(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 15);
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
                        operatorView1.setHeight(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 1.5);
                        operatorView1.setY(left.getHeight());
                        operatorView1.setBaseline(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                        operatorView1.onClick = function () {
                            model.select(expression);
                        };
                        operatorView1.setLineWidth(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 12);
                        operatorView1.setLineColor(expression.getSelected() ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 39, 174, 97) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
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
                }
            }
            var variadicExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.VariadicOperatorExpression);
            if (Bridge.hasValue(variadicExpression)) {
                var views = new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)();
                var offsetX = 0;
                var height = 0;
                var maxBaseline = Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2;
                $t = Bridge.getEnumerator(variadicExpression);
                while ($t.moveNext()) {
                    (function () {
                        var expr = $t.getCurrent();
                        var operand;
                        if (views.getCount() !== 0) {
                            var minus = Bridge.as(expr, ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
                            var operatorView2;
                            if (variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add && Bridge.hasValue(minus)) {
                                operatorView2 = new ThreeOneSevenBee.Model.UI.OperatorView(ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract);
                                operand = this.buildView(minus.getExpression(), model);
                                operatorView2.onClick = function () {
                                    model.select(minus);
                                };
                                operatorView2.setLineColor(minus.getSelected() ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 175, 100) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
                            }
                            else  {
                                operatorView2 = new ThreeOneSevenBee.Model.UI.OperatorView(variadicExpression.getType());
                                operand = this.buildView(expr, model);
                            }
                            operatorView2.setX(offsetX);
                            operatorView2.setWidth((variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply ? 0.5 : 1.5) * Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                            operatorView2.setHeight(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE);
                            operatorView2.setBaseline(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                            operatorView2.setLineWidth(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / (variadicExpression.getType() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply ? 25 : 15));
                            views.add(operatorView2);
                            offsetX += operatorView2.getWidth();
                        }
                        else  {
                            operand = this.buildView(expr, model);
                        }
    
                        maxBaseline = Math.max(maxBaseline, operand.getBaseline());
                        operand.setX(offsetX);
                        offsetX += operand.getWidth();
                        views.add(operand);
                    }).call(this);
                }
                $t1 = Bridge.getEnumerator(views);
                while ($t1.moveNext()) {
                    var view1 = $t1.getCurrent();
                    view1.setY(maxBaseline - view1.getBaseline());
                    height = Math.max(height, view1.getY() + view1.getHeight());
                }
                return Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(offsetX, height), {
                    children: views,
                    setBaseline: maxBaseline
                } );
            }
            var delimiterExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.DelimiterExpression);
            if (Bridge.hasValue(delimiterExpression)) {
                var view2 = this.buildView(delimiterExpression.getExpression(), model);
                view2.setX(view2.getHeight() / 3);
                view2.setY(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 8);
                var left1 = Bridge.merge(new ThreeOneSevenBee.Model.UI.ParenthesisView(ThreeOneSevenBee.Model.UI.ParenthesisType.left), {
                    onClick: function () {
                        model.select(expression);
                    },
                    setWidth: view2.getHeight() / 3,
                    setHeight: view2.getHeight() + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 4
                } );
                var right1 = Bridge.merge(new ThreeOneSevenBee.Model.UI.ParenthesisView(ThreeOneSevenBee.Model.UI.ParenthesisType.right), {
                    onClick: function () {
                        model.select(expression);
                    },
                    setX: view2.getWidth() + view2.getHeight() / 3,
                    setWidth: view2.getHeight() / 3,
                    setHeight: view2.getHeight() + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 4
                } );
    
                var compositeView = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(view2.getWidth() + view2.getHeight() / 1.5, view2.getHeight() + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 4), [
                    [left1],
                    [view2],
                    [right1]
                ] );
                left1.setLineWidth(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 15);
                right1.setLineWidth(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 15);
                left1.setLineColor(expression.getSelected() ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 175, 100) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
                right1.setLineColor(expression.getSelected() ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 175, 100) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
                compositeView.setBaseline(view2.getY() + view2.getBaseline());
                return compositeView;
            }
            var functionExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.FunctionExpression);
            if (Bridge.hasValue(functionExpression) && functionExpression.getFunction() === "sqrt") {
                var view3 = this.buildView(functionExpression.getExpression(), model);
                var sqrtView = new ThreeOneSevenBee.Model.UI.SqrtView();
                sqrtView.onClick = function () {
                    model.select(expression);
                };
                sqrtView.setSignWidth(view3.getHeight() / 2);
                sqrtView.setTopHeight(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                sqrtView.setLineWidth(Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 12);
                sqrtView.setWidth(view3.getWidth() + sqrtView.getSignWidth() + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 4);
                sqrtView.setLineColor(expression.getSelected() ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 175, 100) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
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
            return Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView(expression.toString()), {
                onClick: function () {
                    model.select(expression);
                },
                setWidth: 3 * Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 5 * (expression.toString().length + 0.25),
                setHeight: Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE,
                setBaseline: Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2,
                setTextColor: expression.getSelected() ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 39, 174, 97) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0)
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
        levelSelectView: null,
        context: null,
        constructor: function (game, context) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.$constructor.call(this, context.getWidth(), context.getHeight());
    
            this.context = context;
    
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
    
            this.titleView = new ThreeOneSevenBee.Model.UI.TitleView(game.getUser(), game.getPlayers());
    
            this.levelView = Bridge.merge(new ThreeOneSevenBee.Model.UI.LevelView(game, context.getWidth(), context.getHeight()), {
                setOnExit: Bridge.fn.bind(this, function () {
                    game.saveLevel();
                    this.setContent(this.titleView);
                }),
                setOnNextLevel: function () {
                    game.saveLevel();
                    game.nextLevel();
                }
            } );
    
            this.levelSelectView = Bridge.merge(new ThreeOneSevenBee.Model.UI.LevelSelectView(game.getUser()), {
                onChanged: Bridge.fn.bind(this, function () {
                    this.setContent(this.levelSelectView);
                    this.update(game);
                    this.levelSelectView.update(game.getUser());
                }),
                onLevelSelect: Bridge.fn.bind(this, function (level) {
                    this.setContent(this.levelView);
                    this.update(game);
                    game.setLevel(level.levelIndex, level.categoryIndex);
                }),
                setOnExit: Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.GameView.f1)
            } );
    
            this.titleView.playButton.onClick = Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.GameView.f2);
    
            this.titleView.levelButton.onClick = Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.GameView.f3);
    
            game.onChanged = Bridge.fn.bind(this, this.update);
    
            this.setContent(this.titleView);
    
            context.setContentView(this);
        },
        update: function (game) {
            this.levelView.update(game);
            this.levelSelectView.update(game.getUser());
            this.context.draw();
        },
        setContent: function (content) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.setContent.call(this, content);
            this.context.draw();
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Model.UI.GameView", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.UI.GameView, {
        f1: function () {
            this.setContent(this.titleView);
        },
        f2: function () {
            this.setContent(this.levelView);
        },
        f3: function () {
            this.setContent(this.levelSelectView);
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
                    var frameView = Bridge.merge(new ThreeOneSevenBee.Model.UI.FrameView("constructor$3", this.getWidth() / identities.getCount(), this.getHeight(), view, 4), {
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
    
    Bridge.define('ThreeOneSevenBee.Model.UI.Inputbox', {
        inherits: [ThreeOneSevenBee.Model.UI.LabelView],
        emptyString: null,
        constructor: function (emptyString) {
            ThreeOneSevenBee.Model.UI.LabelView.prototype.$constructor.call(this, emptyString);
    
            this.emptyString = emptyString;
        },
        keyPressed: function (key, lastClick) {
            console.log("Key: " + key);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.LevelSelectView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        onLevelSelect: null,
        config: {
            properties: {
                OnExit: null,
                Category: 0,
                MenuButton: null,
                ArrowLeft: null,
                ArrowRight: null,
                Levels: null,
                CategoryName: null
            }
        },
        constructor: function (user) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, 400, 300);
    
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
            this.setCategory(user.currentCategoryIndex);
            this.build(user);
        },
        build: function (user) {
            this.setMenuButton(Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView("Menu", Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.LevelSelectView.f1)), {
                setWidth: 75,
                setHeight: 30,
                setBackgroundColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 193, 57, 43),
                setTextColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255)
            } ));
    
            this.setCategoryName(Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView(user.categories.getItem(this.getCategory()).name), {
                setX: 100,
                setY: 20,
                setWidth: 200,
                setHeight: 40
            } ));
    
            this.setLevels(Bridge.merge(new ThreeOneSevenBee.Model.UI.FrameView("constructor", this.getWidth() - 100, this.getHeight() - (this.getCategoryName().getY() + this.getCategoryName().getHeight())), {
                setX: 50,
                setY: this.getCategoryName().getY() + this.getCategoryName().getHeight()
            } ));
    
            this.setArrowRight(Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(345, this.getLevels().getY() + this.getLevels().getHeight() / 2 - 37, 50, 75), [
                [0, 0],
                [25, 37],
                [0, 75]
            ] ));
    
            this.getArrowRight().setVisible(this.getCategory() < user.categories.getCount() - 1);
            this.getArrowRight().setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 44, 119, 130));
            this.getArrowRight().onClick = Bridge.fn.bind(this, function () {
                if (this.getCategory() < user.categories.getCount() - 1) {
                    this.setCategory(this.getCategory()+1);
                    this.onChanged();
                }
            });
    
            this.setArrowLeft(Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(5, this.getLevels().getY() + this.getLevels().getHeight() / 2 - 37, 50, 75), [
                [50, 0],
                [25, 37],
                [50, 75]
            ] ));
    
            this.getArrowLeft().setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 44, 119, 130));
            this.getArrowLeft().onClick = Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.LevelSelectView.f2);
            this.getArrowLeft().setVisible(this.getCategory() > 0);
    
            this.children.add(this.getMenuButton());
            this.children.add(this.getCategoryName());
            this.children.add(this.getArrowLeft());
            this.children.add(this.getArrowRight());
            this.children.add(this.getLevels());
    
            this.update(user);
        },
        update: function (user) {
            var $t;
            this.getCategoryName().setText(user.categories.getItem(this.getCategory()).name);
            this.getArrowLeft().setVisible(this.getCategory() > 0);
            this.getArrowRight().setVisible(this.getCategory() < user.categories.getCount() - 1);
    
            var levelButtons = new ThreeOneSevenBee.Model.UI.CompositeView(400, 400);
    
            var levelNumber = 0;
            var numberOfLevels = user.categories.getItem(this.getCategory()).getCount();
            $t = Bridge.getEnumerator(user.categories.getItem(this.getCategory()));
            while ($t.moveNext()) {
                (function () {
                    var level = $t.getCurrent();
                    levelButtons.add(Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView((levelNumber + 1).toString(), Bridge.fn.bind(this, function () {
                        this.onLevelSelect(level);
                    })), {
                        setWidth: 40,
                        setHeight: 40,
                        setX: levelNumber % Bridge.Int.trunc(Math.sqrt(numberOfLevels)) * 50 + 5,
                        setY: Bridge.Int.div(levelNumber, Bridge.Int.trunc(Math.sqrt(numberOfLevels))) * 50 + 5,
                        setBackgroundColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 130, 120),
                        setTextColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255)
                    } ));
                    levelNumber += 1;
    
                }).call(this);
            }
            levelButtons.setWidth(Bridge.Int.trunc(Math.sqrt(numberOfLevels)) * 50);
            levelButtons.setHeight(Bridge.Int.div(levelNumber, Bridge.Int.trunc(Math.sqrt(numberOfLevels))) * 50);
            this.getLevels().setContent(levelButtons);
        }
    });
    
    Bridge.ns("ThreeOneSevenBee.Model.UI.LevelSelectView", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.UI.LevelSelectView, {
        f1: function () {
            if (Bridge.hasValue(this.getOnExit())) {
                this.getOnExit()();
            }
        },
        f2: function () {
            if (this.getCategory() > 0) {
                this.setCategory(this.getCategory()-1);
                this.onChanged();
            }
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.LevelView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        menuButton: null,
        nextButton: null,
        restartButton: null,
        progressbar: null,
        identityMenu: null,
        expression: null,
        toolTipView: null,
        toolTipView2: null,
        toolTipView3: null,
        config: {
            properties: {
                OnExit: null,
                OnNextLevel: null
            }
        },
        constructor: function (game, width, height) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, width, height);
    
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
            this.build(game);
        },
        build: function (game) {
            this.menuButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView("Menu", Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.LevelView.f1)), {
                setWidth: 100,
                setHeight: 50,
                setBackgroundColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 192, 57, 43),
                setTextColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255)
            } );
    
            this.nextButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView("Næste", Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.LevelView.f2)), {
                setX: this.getWidth() - 100,
                setWidth: 100,
                setHeight: 50,
                setBackgroundColor: game.getIsLevelCompleted() ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 22, 160, 134) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 190, 190, 190),
                setTextColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255)
            } );
    
            this.restartButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView("Forfra", function () {
                game.restartLevel();
            }), {
                setX: this.getWidth() / 2 - 50,
                setY: 50,
                setWidth: 100,
                setHeight: 50,
                setBackgroundColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 192, 57, 43),
                setTextColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255)
            } );
    
            this.progressbar = Bridge.merge(new ThreeOneSevenBee.Model.UI.ProgressbarStarView(game.progressBar, this.getWidth() - 220, 30), {
                setX: 110,
                setY: 10
            } );
    
            this.identityMenu = Bridge.merge(new ThreeOneSevenBee.Model.UI.IdentityMenuView(game.getExprModel(), this.getWidth(), 100), {
                setY: this.getHeight() - 125
            } );
    
            this.expression = Bridge.merge(new ThreeOneSevenBee.Model.UI.ExpressionView("constructor$2", game.getExprModel(), this.getWidth(), this.getHeight() - 175, 8), {
                setX: 0,
                setY: 50
            } );
    
            this.toolTipView = Bridge.merge(new ThreeOneSevenBee.Model.UI.ToolTipView("Denne bar viser hvor langt du er nået.", 300, 75), {
                setVisible: game.getIsFirstLevel(),
                setX: this.progressbar.getX(),
                setY: this.progressbar.getY() + this.progressbar.getHeight() + 10
            } );
    
            this.toolTipView2 = Bridge.merge(new ThreeOneSevenBee.Model.UI.ToolTipView("Når knappen bliver grøn kan du gå videre til næste bane", 400, 75), {
                setVisible: game.getIsFirstLevel(),
                setX: this.nextButton.getX() - 380,
                setY: this.nextButton.getY() + this.nextButton.getHeight() + 10,
                setArrowPosition: 380
            } );
    
            this.toolTipView3 = Bridge.merge(new ThreeOneSevenBee.Model.UI.ToolTipView("Dit mål er at reducere ovenstående udtryk. Dette gøres ved at markere de dele i udtrykket som skal reduceres.\nMarkér [a] og [a]. Klik derefter på den ønskede omskrivning nedenfor for at reducere udtrykket", 800, 90), {
                setVisible: game.getIsFirstLevel(),
                setX: this.getWidth() / 2 - 400,
                setY: this.getHeight() / 2 + 30,
                setArrowPosition: 390
            } );
    
            this.children = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)(), [
                [this.menuButton],
                [this.nextButton],
                [this.restartButton],
                [this.progressbar],
                [this.identityMenu],
                [this.expression],
                [this.toolTipView],
                [this.toolTipView2],
                [this.toolTipView3]
            ] );
    
            if (Bridge.hasValue(this.onChanged)) {
                this.onChanged();
            }
        },
        update: function (game) {
            this.progressbar.update(game.progressBar);
            this.identityMenu.update(game.getExprModel().getIdentities(), game.getExprModel());
            this.expression.update(game.getExprModel());
            this.nextButton.setBackgroundColor(game.getIsLevelCompleted() ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 120, 130) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 190, 190, 190));
            this.toolTipView.setVisible(game.getIsFirstLevel());
            this.toolTipView2.setVisible(game.getIsFirstLevel());
            this.toolTipView3.setVisible(game.getIsFirstLevel());
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
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 209, 209, 209));
        },
        build: function (players) {
            var $t;
            this.children = new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)();
            var offsetY = 5;
            $t = Bridge.getEnumerator(players);
            while ($t.moveNext()) {
                var player = $t.getCurrent();
                this.children.add(Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(this.getWidth(), this.getHeight()), [
                    [Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView(player.getPlayerName()), {
                        setX: 0,
                        setY: offsetY,
                        setWidth: this.getWidth() - 20,
                        setHeight: 20,
                        setBackgroundColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 239, 239, 239)
                    } )],
                    [Bridge.merge(new ThreeOneSevenBee.Model.UI.ImageView("spildonebadge.png", 20, 20), {
                        setX: this.getWidth() - 20
                    } )]
                ] ));
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
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 225, 225, 225));
            this.build(progressbar);
        },
        build: function (progressbar) {
            this.progress = Bridge.merge(new ThreeOneSevenBee.Model.UI.View(0, 0, Math.max(0, this.getWidth() * progressbar.getPercentage()), this.getHeight()), {
                setBackgroundColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 175, 100)
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
        welcomeText: null,
        levelButton: null,
        playButton: null,
        playerList: null,
        constructor: function (user, players) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, 600, 300);
    
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
            this.welcomeText = Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView("Velkommen " + user.getPlayerName()), {
                setX: 100,
                setY: 50,
                setHeight: 50,
                setWidth: 220
            } );
    
            var playIcon = Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(0, 0, 100, 100), [
                [25, 25],
                [75, 50],
                [25, 75]
            ] );
            playIcon.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
            this.playButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(100, 100), [
                [playIcon]
            ] );
            this.playButton.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 39, 174, 97));
            this.playButton.setX(100);
            this.playButton.setY(100);
    
            var levelIcon1 = Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(0, 0, 100, 100), [
                [20, 20],
                [45, 20],
                [45, 45],
                [20, 45],
                [20, 20]
            ] );
            var levelIcon2 = Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(0, 0, 100, 100), [
                [80, 20],
                [80, 45],
                [55, 45],
                [55, 20],
                [80, 20]
            ] );
            var levelIcon3 = Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(0, 0, 100, 100), [
                [80, 80],
                [55, 80],
                [55, 55],
                [80, 55],
                [80, 80]
            ] );
            var levelIcon4 = Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(0, 0, 100, 100), [
                [20, 80],
                [20, 55],
                [45, 55],
                [45, 80],
                [20, 80]
            ] );
            levelIcon1.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
            levelIcon2.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
            levelIcon3.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
            levelIcon4.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
            this.levelButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(100, 100), [
                [levelIcon1],
                [levelIcon2],
                [levelIcon3],
                [levelIcon4]
            ] );
            this.levelButton.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 42, 128, 185));
            this.levelButton.setX(220);
            this.levelButton.setY(100);
    
            this.playerList = Bridge.merge(new ThreeOneSevenBee.Model.UI.PlayerListView(players, 160, 200), {
                setX: 340,
                setY: 50
            } );
    
            this.children = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)(), [
                [this.welcomeText],
                [this.levelButton],
                [this.playerList],
                [this.playButton]
            ] );
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ToolTipView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        arrowPosition: 0,
        arrowDirection: null,
        config: {
            properties: {
                labelView: null,
                arrow: null
            }
        },
        constructor: function (text, width, height) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, width, height);
    
            this.setarrow(Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(0, 0, 20, 11), [
                [0, 11],
                [10, 0],
                [20, 11]
            ] ));
            this.setlabelView(Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView(text), {
                setY: 10,
                setHeight: height - 10,
                setWidth: width
            } ));
            this.setText(text);
            this.setBoxColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 130, 120));
            this.setTextColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
            this.setArrowDirection(ThreeOneSevenBee.Model.UI.Direction.top);
            this.setArrowPosition(0);
            this.children.add(this.getarrow());
            this.children.add(this.getlabelView());
        },
        getBoxColor: function () {
            return this.getlabelView().getBackgroundColor();
        },
        setBoxColor: function (value) {
            this.getlabelView().setBackgroundColor(value);
            this.getarrow().setBackgroundColor(value);
        },
        getTextColor: function () {
            return this.getlabelView().getTextColor();
        },
        setTextColor: function (value) {
            this.getlabelView().setTextColor(value);
        },
        getText: function () {
            return this.getlabelView().getText();
        },
        setText: function (value) {
            this.getlabelView().setText(value);
        },
        getArrowPosition: function () {
            return this.arrowPosition;
        },
        setArrowPosition: function (value) {
            this.arrowPosition = value;
            switch (this.getArrowDirection()) {
                case ThreeOneSevenBee.Model.UI.Direction.top: 
                    this.getarrow().setX(this.arrowPosition);
                    break;
                case ThreeOneSevenBee.Model.UI.Direction.right: 
                    this.getarrow().setY(this.arrowPosition);
                    break;
                case ThreeOneSevenBee.Model.UI.Direction.left: 
                    this.getarrow().setY(this.arrowPosition);
                    break;
                case ThreeOneSevenBee.Model.UI.Direction.bottom: 
                    this.getarrow().setX(this.arrowPosition);
                    break;
                default: 
                    break;
            }
        },
        getArrowDirection: function () {
            return this.arrowDirection;
        },
        setArrowDirection: function (value) {
            this.arrowDirection = value;
            this.arrowPosition = 0;
            this.getlabelView().setX(0);
            this.getlabelView().setY(0);
            this.getlabelView().setHeight(this.getHeight());
            this.getlabelView().setWidth(this.getWidth());
            switch (this.arrowDirection) {
                case ThreeOneSevenBee.Model.UI.Direction.top: 
                    this.setarrow(Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(0, 0, 20, 10), [
                        [0, 10],
                        [10, 0],
                        [20, 10]
                    ] ));
                    this.getlabelView().setY(10);
                    this.getlabelView().setHeight(this.getHeight() - 10);
                    break;
                case ThreeOneSevenBee.Model.UI.Direction.right: 
                    this.setarrow(Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(this.getWidth() - 10, 0, 10, 20), [
                        [0, 0],
                        [10, 10],
                        [0, 20]
                    ] ));
                    this.getlabelView().setWidth(this.getWidth() - 10);
                    break;
                case ThreeOneSevenBee.Model.UI.Direction.left: 
                    this.setarrow(Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(0, 0, 10, 20), [
                        [10, 0],
                        [0, 10],
                        [10, 20]
                    ] ));
                    this.getlabelView().setX(10);
                    this.getlabelView().setWidth(this.getWidth() - 10);
                    break;
                case ThreeOneSevenBee.Model.UI.Direction.bottom: 
                    this.setarrow(Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(0, this.getHeight() - 10, 20, 10), [
                        [0, 0],
                        [20, 0],
                        [10, 10]
                    ] ));
                    this.getlabelView().setHeight(this.getHeight() - 10);
                    break;
                default: 
                    break;
            }
            this.getarrow().setBackgroundColor(this.getBoxColor());
        }
    });
    
    
    
    Bridge.init();
})(this);
