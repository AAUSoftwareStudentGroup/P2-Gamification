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
                Active: false,
                Scaling: 0,
                Baseline: 0,
                BackgroundColor: null,
                Visible: false
            }
        },
        constructor: function (x, y, width, height) {
            this.setX(x);
            this.setY(y);
            this.setActive(false);
            this.setWidth(width);
            this.setHeight(height);
            this.setBaseline(height / 2);
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor"));
            this.setVisible(true);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            context.drawRectangle(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), this.getBackgroundColor());
        },
        click: function (x, y, context) {
            if (this.containsPoint(x, y)) {
                if (Bridge.hasValue(this.onClick)) {
                    this.onClick();
                }
            }
        },
        keyPressed: function (key, context) {
            if (Bridge.hasValue(this.onKeyPressed) && this.getActive()) {
                this.onKeyPressed(key, context);
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
    
    Bridge.define('ThreeOneSevenBee.Model.UI.TextAlignment', {
        statics: {
            left: 0,
            right: 1,
            centered: 2
        },
        $enum: true
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.LabelView', {
        inherits: [ThreeOneSevenBee.Model.UI.View],
        config: {
            properties: {
                TextColor: null,
                Align: null,
                Text: null
            }
        },
        constructor: function (text) {
            ThreeOneSevenBee.Model.UI.View.prototype.$constructor.call(this, 0, 0, 10, 10);
    
            this.setText(text);
            this.setTextColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
            this.setAlign(ThreeOneSevenBee.Model.UI.TextAlignment.centered);
        },
        drawWithContext: function (context, offsetX, offsetY) {
            ThreeOneSevenBee.Model.UI.View.prototype.drawWithContext.call(this, context, offsetX, offsetY);
    
            context.drawText(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), this.getText(), this.getTextColor(), this.getAlign());
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
        getActive: function () {
            var $t;
            if (Bridge.hasValue(this.children)) {
                $t = Bridge.getEnumerator(this.children);
                while ($t.moveNext()) {
                    var child = $t.getCurrent();
                    if (child.getActive()) {
                        return true;
                    }
                }
            }
            return false;
        },
        setActive: function (value) {
            var $t;
            if (value === false && Bridge.hasValue(this.children)) {
                $t = Bridge.getEnumerator(this.children);
                while ($t.moveNext()) {
                    var child = $t.getCurrent();
                    child.setActive(value);
                }
            }
            ThreeOneSevenBee.Model.UI.View.prototype.setActive.call(this, value);
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
        click: function (x, y, context) {
            var $t;
            if (ThreeOneSevenBee.Model.UI.View.prototype.containsPoint.call(this, x, y)) {
                this.setActive(true);
                if (this.getPropagateClick()) {
                    $t = Bridge.getEnumerator(this.children);
                    while ($t.moveNext()) {
                        var child = $t.getCurrent();
                        child.click(x - this.getX(), y - this.getY(), context);
                    }
                }
    
                if (Bridge.hasValue(this.onClick)) {
                    this.onClick();
                }
            }
            else  {
                this.setActive(false);
            }
        },
        keyPressed: function (key, context) {
            var $t;
            if (this.getPropagateKeypress()) {
                $t = Bridge.getEnumerator(this.children);
                while ($t.moveNext()) {
                    var child = $t.getCurrent();
                    child.keyPressed(key, context);
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
        config: {
            properties: {
                Width: 0,
                Height: 0,
                ContentView$1: null
            }
        },
        constructor: function (width, height) {
            this.setContentView$1(new ThreeOneSevenBee.Model.UI.FrameView("constructor", 0, 0));
            this.setWidth(width);
            this.setHeight(height);
        },
        setContentView: function (view) {
            this.setContentView$1(view);
        },
        draw: function () {
            this.clear();
            this.getContentView$1().drawWithContext(this, 0, 0);
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
        getActive: function () {
            return this.getContent$1().getActive();
        },
        setActive: function (value) {
            if (Bridge.hasValue(this.getContent$1())) {
                this.getContent$1().setActive(value);
            }
        },
        setContent: function (content) {
            this.setContent$1(this.align(this.fit(content)));
        },
        update: function () {
            this.setContent$1(this.align(this.fit(this.getContent$1())));
        },
        click: function (x, y, context) {
    
            if (ThreeOneSevenBee.Model.UI.View.prototype.containsPoint.call(this, x, y)) {
                if (this.getPropagateClick()) {
                    this.getContent$1().click(x - this.getX(), y - this.getY(), context);
                }
    
                if (Bridge.hasValue(this.onClick)) {
                    this.onClick();
                }
            }
            else  {
                this.setActive(false);
            }
        },
        keyPressed: function (key, context) {
            if (this.getPropagateKeypress()) {
                this.getContent$1().keyPressed(key, context);
            }
        },
        drawWithContext: function (context, offsetX, offsetY) {
            this.update();
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
                    context.drawText(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), "+", this.getLineColor(), ThreeOneSevenBee.Model.UI.TextAlignment.centered);
                    break;
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract: 
                    context.drawText(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), "-", this.getLineColor(), ThreeOneSevenBee.Model.UI.TextAlignment.centered);
                    break;
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.minus: 
                    context.drawText(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), "-", this.getLineColor(), ThreeOneSevenBee.Model.UI.TextAlignment.centered);
                    break;
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide: 
                    context.drawLine(new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", this.getX() + offsetX, this.getY() + offsetY + this.getHeight() / 2), new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", this.getX() + offsetX + this.getWidth(), this.getY() + offsetY + this.getHeight() / 2), this.getLineColor(), this.getLineWidth());
                    break;
                case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply: 
                    context.drawText(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), "·", this.getLineColor(), ThreeOneSevenBee.Model.UI.TextAlignment.centered);
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
            context.drawText(this.getX() + offsetX, this.getY() + offsetY, this.getWidth(), this.getHeight(), this.getType() === ThreeOneSevenBee.Model.UI.ParenthesisType.left ? "(" : ")", this.getLineColor(), ThreeOneSevenBee.Model.UI.TextAlignment.centered);
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
    
    Bridge.define('ThreeOneSevenBee.Model.UI.CategoryCompletionView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        congratulationView: null,
        descriptionView: null,
        badgeView: null,
        menuButton: null,
        nextCategory: null,
        isLastCategory: false,
        config: {
            properties: {
                Category: null,
                NextCategory: null,
                OnExit: null,
                OnNext: null
            }
        },
        constructor: function (category, isLastCategory) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, 600, 400);
    
            this.setCategory(category);
            this.isLastCategory = isLastCategory;
            this.build();
        },
        build: function () {
            var offSetY = 5;
            this.children = new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)();
            this.congratulationView = Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView("Tillykke !!!"), {
                setX: (this.getWidth() * 0.5) - ((this.getWidth() * 0.65) / 2),
                setY: offSetY,
                setWidth: this.getWidth() * 0.65,
                setHeight: this.getHeight() * 0.2
            } );
    
            this.descriptionView = Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView("Du har gennemført kategorien: " + this.getCategory().name + (this.isLastCategory ? " \nog dermed gennemført hele spillet!" : "")), {
                setX: (this.getWidth() * 0.5) - ((this.congratulationView.getWidth() * 0.75) * 0.5),
                setY: offSetY + this.congratulationView.getHeight(),
                setWidth: this.congratulationView.getWidth() * 0.75,
                setHeight: this.congratulationView.getHeight() / 2
            } );
            this.children.add(this.congratulationView);
            this.children.add(this.descriptionView);
    
    
    
            var badgeDic = new ThreeOneSevenBee.Model.UI.PlayerListView("constructor$1", this.getWidth(), this.getHeight());
    
            this.badgeView = null;
            if (badgeDic.badgeDictionary.containsKey(this.getCategory().getBadge())) {
                this.badgeView = Bridge.merge(new ThreeOneSevenBee.Model.UI.ImageView(badgeDic.badgeDictionary.get(this.getCategory().getBadge()), this.getWidth() * 0.25, this.getWidth() * 0.25), {
                    setX: this.getWidth() / 2 - ((this.getWidth() * 0.25) / 2),
                    setY: this.getHeight() * 0.4
                } );
                this.children.add(this.badgeView);
            }
    
            this.menuButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView("Menu", Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.CategoryCompletionView.f1)), {
                setX: this.badgeView.getX() + (this.badgeView.getWidth() / 2) - 115,
                setY: this.getHeight() - 50,
                setWidth: 105,
                setHeight: 35,
                setBackgroundColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 192, 57, 43),
                setTextColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255)
            } );
            if (this.isLastCategory === false) {
                this.nextCategory = Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView("Næste", Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.CategoryCompletionView.f2)), {
                    setX: this.badgeView.getX() + (this.badgeView.getWidth() / 2) + 10,
                    setY: this.getHeight() - 50,
                    setWidth: 105,
                    setHeight: 35,
                    setTextColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255),
                    setBackgroundColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 120, 130)
                } );
                this.children.add(this.nextCategory);
            }
            this.children.add(this.menuButton);
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Model.UI.CategoryCompletionView", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.UI.CategoryCompletionView, {
        f1: function () {
            if (Bridge.hasValue(this.getOnExit())) {
                this.getOnExit()();
            }
        },
        f2: function () {
            if (Bridge.hasValue(this.getOnNext())) {
                this.getOnNext()();
            }
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.ExpressionView', {
        inherits: [ThreeOneSevenBee.Model.UI.FrameView],
        statics: {
            nUMVAR_SIZE: 20
        },
        selectionBackgroundColor: null,
        selectionTextColor: null,
        constructor: function () {
            ThreeOneSevenBee.Model.UI.ExpressionView.prototype.constructor$1.call(this, 0, 0);
    
        },
        constructor$1: function (width, height) {
            ThreeOneSevenBee.Model.UI.ExpressionView.prototype.constructor$2.call(this, null, width, height, 0);
    
        },
        constructor$2: function (model, width, height, maxScale) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.$constructor.call(this, width, height);
    
            this.maxScale = maxScale;
            this.selectionTextColor = new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 175, 100);
            this.selectionBackgroundColor = new ThreeOneSevenBee.Model.UI.Color("constructor$1", 230, 230, 230);
            this.build(model);
        },
        buildView: function (expression, model) {
            var $t, $t1;
            var minusExpression = Bridge.as(expression, ThreeOneSevenBee.Model.Expression.Expressions.UnaryMinusExpression);
            if (Bridge.hasValue(minusExpression)) {
                var view = this.buildView(minusExpression.getExpression(), model);
                view.setBackgroundColor(model.getSelected() === expression ? this.selectionBackgroundColor : new ThreeOneSevenBee.Model.UI.Color("constructor"));
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
                minusView.setBackgroundColor(model.getSelected() === expression ? this.selectionBackgroundColor : new ThreeOneSevenBee.Model.UI.Color("constructor"));
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
                        fraction.setBackgroundColor(model.getSelected() === expression ? this.selectionBackgroundColor : new ThreeOneSevenBee.Model.UI.Color("constructor"));
                        return fraction;
                    case ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.power: 
                        right.setX(left.getWidth());
                        left.setY(right.getHeight() - Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2);
                        var exponent = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(right.getX() + right.getWidth(), left.getY() + left.getHeight()), [
                            [left],
                            [right]
                        ] );
                        exponent.setBaseline(left.getY() + left.getBaseline());
                        exponent.setBackgroundColor(model.getSelected() === expression ? this.selectionBackgroundColor : new ThreeOneSevenBee.Model.UI.Color("constructor"));
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
                                operand.setBackgroundColor(model.getSelected() === expr ? this.selectionBackgroundColor : new ThreeOneSevenBee.Model.UI.Color("constructor"));
                                operatorView2.setBackgroundColor(operand.getBackgroundColor());
                                operatorView2.onClick = function () {
                                    model.select(minus);
                                };
                                operatorView2.setLineColor(minus.getSelected() ? this.selectionTextColor : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
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
                        if (expr.getParent() === model.getSelected() && (expr.getSelected() || Bridge.Linq.Enumerable.from(expr.getNodesRecursive()).any($_.ThreeOneSevenBee.Model.UI.ExpressionView.f1))) {
                            operand.setBackgroundColor(this.selectionBackgroundColor);
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
                left1.setLineColor(expression.getSelected() ? this.selectionTextColor : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
                right1.setLineColor(expression.getSelected() ? this.selectionTextColor : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
                compositeView.setBaseline(view2.getY() + view2.getBaseline());
                compositeView.setBackgroundColor(model.getSelected() === expression ? this.selectionBackgroundColor : new ThreeOneSevenBee.Model.UI.Color("constructor"));
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
                sqrtView.setLineColor(expression.getSelected() ? this.selectionTextColor : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0));
                sqrtView.setHeight(view3.getHeight() + sqrtView.getTopHeight());
                view3.setX(sqrtView.getSignWidth() + Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 8);
                view3.setY(sqrtView.getTopHeight());
                var compositeView1 = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(sqrtView.getWidth(), sqrtView.getHeight()), [
                    [sqrtView],
                    [view3]
                ] );
                compositeView1.setBaseline(view3.getBaseline() + sqrtView.getTopHeight());
                compositeView1.setBackgroundColor(model.getSelected() === expression ? this.selectionBackgroundColor : new ThreeOneSevenBee.Model.UI.Color("constructor"));
                return compositeView1;
            }
            return Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView(expression.toString()), {
                onClick: function () {
                    model.select(expression);
                },
                setWidth: 3 * Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 5 * (expression.toString().length + 0.25),
                setHeight: Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE,
                setBaseline: Bridge.get(ThreeOneSevenBee.Model.UI.ExpressionView).nUMVAR_SIZE / 2,
                setTextColor: expression.getSelected() ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 39, 174, 97) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0),
                setBackgroundColor: model.getSelected() === expression ? this.selectionBackgroundColor : new ThreeOneSevenBee.Model.UI.Color("constructor")
            } );
        },
        build: function (model) {
            if (Bridge.hasValue(model)) {
                var content = this.buildView(model.getExpression(), model);
                this.setContent(content);
            }
        },
        update$1: function (model) {
            this.build(model);
            if (Bridge.hasValue(this.onChanged)) {
                this.onChanged();
            }
        }
    });
    
    Bridge.ns("ThreeOneSevenBee.Model.UI.ExpressionView", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.UI.ExpressionView, {
        f1: function (n) {
            return n.getSelected();
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.GameView', {
        inherits: [ThreeOneSevenBee.Model.UI.FrameView],
        titleView: null,
        levelView: null,
        categoryCompletionView: null,
        levelSelectView: null,
        config: {
            properties: {
                OnExit: null,
                ReloadGame: null
            }
        },
        constructor: function (game, width, height) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.$constructor.call(this, width, height);
    
            this.categoryCompletionView = null;
    
            game.onCategoryCompleted = Bridge.fn.combine(game.onCategoryCompleted, Bridge.fn.bind(this, function (c) {
                this.categoryCompletionView = Bridge.merge(new ThreeOneSevenBee.Model.UI.CategoryCompletionView(c, game.getIsLastCategory()), {
                    setOnNext: Bridge.fn.bind(this, function () {
                        if (game.getIsLastCategory() === false) {
                            game.setLevel(0, c.categoryIndex + 1);
                            this.setContent(this.levelView);
                        }
                    }),
                    setOnExit: Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.GameView.f1)
                } );
                this.setContent(this.categoryCompletionView);
            }));
    
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
    
            this.titleView = new ThreeOneSevenBee.Model.UI.TitleView(game);
    
            this.levelView = Bridge.merge(new ThreeOneSevenBee.Model.UI.LevelView(game), {
                setOnExit: Bridge.fn.bind(this, function () {
                    game.saveLevel();
                    this.setContent(this.levelSelectView);
                }),
                setOnNextLevel: function () {
                    if (game.getIsLastLevel() === false) {
                        game.saveLevel();
                        game.nextLevel();
                    }
                }
            } );
    
            this.levelSelectView = Bridge.merge(new ThreeOneSevenBee.Model.UI.LevelSelectView(game.getUser()), {
                onChanged: Bridge.fn.bind(this, function () {
                    this.setContent(this.levelSelectView);
                    this.update$1(game);
                    this.levelSelectView.update(game.getUser());
                }),
                onLevelSelect: Bridge.fn.bind(this, function (level) {
                    if (level.unlocked) {
                        this.setContent(this.levelView);
                        this.update$1(game);
                        game.setLevel(level.levelIndex, level.categoryIndex);
                    }
                }),
                setOnExit: Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.GameView.f1)
            } );
    
            this.titleView.playButton.onClick = Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.GameView.f2);
    
            this.titleView.levelButton.onClick = Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.GameView.f3);
    
            this.titleView.onLogout = Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.GameView.f4);
    
            game.onChanged = Bridge.fn.bind(this, this.update$1);
    
            this.setContent(this.titleView);
        },
        update$1: function (game) {
            this.levelView.update(game);
            this.levelSelectView.update(game.getUser());
            if (Bridge.hasValue(this.onChanged)) {
                this.onChanged();
            }
        },
        setContent: function (content) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.setContent.call(this, content);
            if (Bridge.hasValue(this.onChanged)) {
                this.onChanged();
            }
        }
    });
    
    Bridge.ns("ThreeOneSevenBee.Model.UI.GameView", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.UI.GameView, {
        f1: function () {
            this.getReloadGame()();
        },
        f2: function () {
            this.setContent(this.levelView);
        },
        f3: function () {
            this.setContent(this.levelSelectView);
        },
        f4: function () {
            if (Bridge.hasValue(this.getOnExit())) {
                this.getOnExit()();
            }
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
                    var frameView = Bridge.merge(new ThreeOneSevenBee.Model.UI.FrameView("constructor$3", (this.getWidth() - 20 * (identities.getCount() - 1)) / identities.getCount(), this.getHeight(), view, 4), {
                        setPropagateClick: false,
                        setBackgroundColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 230, 230, 230)
                    } );
                    frameView.setX(x);
                    x += frameView.getWidth() + 20;
                    frameView.onClick = function () {
                        model.applyIdentity(identities.getItem(indexCopy).result);
                    };
                    views.add(frameView);
                }).call(this);
            }
            this.children = views;
        },
        click: function (x, y, context) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.click.call(this, x, y, context);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.Inputbox', {
        inherits: [ThreeOneSevenBee.Model.UI.LabelView],
        placeholder: null,
        cursorPos: 0,
        hidden: false,
        placeholderColor: null,
        constructor$2: function (placeholder, hidden, placeholderColor) {
            ThreeOneSevenBee.Model.UI.LabelView.prototype.$constructor.call(this, placeholder);
    
            this.placeholderColor = placeholderColor;
            this.placeholder = placeholder;
            this.hidden = hidden;
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 200, 200, 200));
            this.cursorPos = 0;
        },
        constructor$1: function (placeholder, hidden) {
            ThreeOneSevenBee.Model.UI.Inputbox.prototype.constructor$2.call(this, placeholder, hidden, new ThreeOneSevenBee.Model.UI.Color("constructor$1", 100, 100, 100));
    
        },
        constructor: function (placeholder) {
            ThreeOneSevenBee.Model.UI.Inputbox.prototype.constructor$1.call(this, placeholder, false);
    
        },
        click: function (x, y, context) {
            if (this.containsPoint(x, y)) {
                if (Bridge.hasValue(this.onClick)) {
                    this.onClick();
                }
                this.setActive(true);
            }
            else  {
                this.setActive(false);
            }
            if (this.getActive() === false && this.getText().length === 0) {
                this.setText(this.placeholder);
            }
            if (this.containsPoint(x, y)) {
                if (this.getActive() === true && Bridge.String.compare(this.getText(), this.placeholder) === 0) {
                    this.setText("");
                }
                for (var i = 0; i <= this.getText().length; i++) {
                    if (context.getTextDimensions(this.hidden ? this.getHiddenText().substr(0, i) : this.getText().substr(0, i), this.getWidth(), this.getHeight()).x > x - this.getX()) {
                        this.cursorPos = i - 1;
                        break;
                    }
                    if (i === this.getText().length) {
                        this.cursorPos = i;
                    }
                }
            }
        },
        isDefault: function () {
            return Bridge.String.compare(this.getText(), this.placeholder) === 0;
        },
        keyPressed: function (key, context) {
            ThreeOneSevenBee.Model.UI.LabelView.prototype.keyPressed.call(this, key, context);
            if (this.getActive()) {
                switch (key) {
                    case "Back": 
                        if (this.cursorPos > 0) {
                            this.setText(Bridge.String.remove(this.getText(), --this.cursorPos, 1));
                        }
                        break;
                    case "Right": 
                        if (this.cursorPos < this.getText().length) {
                            this.cursorPos++;
                        }
                        break;
                    case "Space": 
                        this.setText(Bridge.String.insert(this.cursorPos, this.getText(), " "));
                        this.cursorPos++;
                        break;
                    case "Left": 
                        if (this.cursorPos > 0) {
                            this.cursorPos--;
                        }
                        break;
                    default: 
                        if (key.length === 1) {
                            this.setText(Bridge.String.insert(this.cursorPos, this.getText(), key));
                            this.cursorPos++;
                            if (context.getTextDimensions(this.hidden ? this.getHiddenText() : this.getText(), this.getWidth(), this.getHeight()).x > this.getWidth() - 5) {
                                this.keyPressed("Back", context);
                            }
                        }
                        else  {
                            console.log("Unknown key: " + key);
                        }
                        break;
                }
            }
        },
        getHiddenText: function () {
            var hiddenText = "";
            for (var i = 0; i < this.getText().length; i++) {
                hiddenText += "*";
            }
            return hiddenText;
        },
        drawWithContext: function (context, offsetX, offsetY) {
            var backupColor = this.getTextColor();
            if (this.isDefault()) {
                this.setTextColor(this.placeholderColor);
            }
    
            if (!this.hidden) {
                ThreeOneSevenBee.Model.UI.LabelView.prototype.drawWithContext.call(this, context, offsetX, offsetY);
            }
            else  {
                var TextBackup = this.getText();
    
                if (this.getText() === this.placeholder) {
                    this.setText(this.placeholder);
                }
                else  {
                    this.setText(this.getHiddenText());
                }
    
                ThreeOneSevenBee.Model.UI.LabelView.prototype.drawWithContext.call(this, context, offsetX, offsetY);
                this.setText(TextBackup);
            }
            if (this.getActive()) {
                var textWidth = 0;
                if (this.cursorPos > 0 && this.getText().length !== 0) {
                    textWidth = context.getTextDimensions(this.hidden ? this.getHiddenText().substr(0, this.cursorPos) : this.getText().substr(0, this.cursorPos), this.getWidth(), this.getHeight()).x;
                }
                context.drawLine(new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", textWidth + offsetX + this.getX() + 1, this.getY() + offsetY), new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", textWidth + offsetX + this.getX() + 1, offsetY + this.getY() + this.getHeight()), new ThreeOneSevenBee.Model.UI.Color("constructor$1", 0, 0, 0), 1);
            }
            if (this.isDefault()) {
                this.setTextColor(backupColor);
            }
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
                CategoryName: null,
                TitelView: null,
                StarTextView: null,
                StarView: null,
                BadgeView: null
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
    
            this.setTitelView(Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(200, 40), {
                setX: 100,
                setY: 10
            } ));
            this.setCategoryName(Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView(user.categories.getItem(this.getCategory()).name), {
                setWidth: this.getTitelView().getWidth() * 0.6,
                setHeight: this.getTitelView().getHeight(),
                setX: this.getTitelView().getX() - (this.getTitelView().getWidth() * 0.6) / 2
            } ));
    
            this.setStarTextView(Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView(""), {
                setWidth: this.getTitelView().getWidth() * 0.15,
                setHeight: 15,
                setX: this.getCategoryName().getX() + this.getCategoryName().getWidth() + 7.5,
                setY: this.getCategoryName().getY() + ((this.getCategoryName().getHeight()) / 2) - 7.5
            } ));
    
            this.setStarView(Bridge.merge(new ThreeOneSevenBee.Model.UI.ImageView("star_activated.png", this.getTitelView().getWidth() * 0.1, this.getTitelView().getWidth() * 0.1), {
                setX: this.getCategoryName().getX() + this.getCategoryName().getWidth() + this.getStarTextView().getWidth() + 10,
                setY: this.getCategoryName().getY() + 0.5 * (this.getTitelView().getWidth() * 0.1)
            } ));
    
            this.setBadgeView(Bridge.merge(new ThreeOneSevenBee.Model.UI.ImageView("tutorialbadge.png", this.getTitelView().getWidth() * 0.15, this.getTitelView().getWidth() * 0.15), {
                setX: 0 - ((this.getTitelView().getWidth() * 0.1) / 2) + 5,
                setY: this.getCategoryName().getY() + (0.5 * (this.getTitelView().getWidth() * 0.15)) - 10
            } ));
    
            this.getTitelView().add(this.getStarTextView());
            this.getTitelView().add(this.getCategoryName());
            this.getTitelView().add(this.getStarView());
            this.getTitelView().add(this.getBadgeView());
    
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
                [25, 37],
                [50, 0],
                [50, 75]
            ] ));
    
            this.getArrowLeft().setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 44, 119, 130));
            this.getArrowLeft().onClick = Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.LevelSelectView.f2);
            this.getArrowLeft().setVisible(this.getCategory() > 0);
    
            this.children.add(this.getMenuButton());
            this.children.add(this.getArrowLeft());
            this.children.add(this.getArrowRight());
            this.children.add(this.getLevels());
            this.children.add(this.getTitelView());
    
            this.update(user);
        },
        update: function (user) {
            var $t;
            this.getCategoryName().setText(user.categories.getItem(this.getCategory()).name);
            this.getArrowLeft().setVisible(this.getCategory() > 0);
            this.getArrowRight().setVisible(this.getCategory() < user.categories.getCount() - 1);
            var totalStars = user.categories.getItem(this.getCategory()).getCount() * 3;
            var userStarsInCategory = 0;
    
            var levelButtons = new ThreeOneSevenBee.Model.UI.CompositeView(400, 400);
    
            var levelNumber = 0;
            var numberOfLevels = user.categories.getItem(this.getCategory()).getCount();
            $t = Bridge.getEnumerator(user.categories.getItem(this.getCategory()));
            while ($t.moveNext()) {
                (function () {
                    var level = $t.getCurrent();
                    userStarsInCategory += level.stars;
                    var levelButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(40, 40), {
                        onClick: Bridge.fn.bind(this, function () {
                            this.onLevelSelect(level);
                        }),
                        setX: levelNumber % Bridge.Int.trunc(Math.sqrt(numberOfLevels)) * 50 + 5,
                        setY: Bridge.Int.div(levelNumber, Bridge.Int.trunc(Math.sqrt(numberOfLevels))) * 50 + 5,
                        setBackgroundColor: level.unlocked ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 130, 120) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 190, 190, 190)
                    } );
                    levelButton.add(Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView((levelNumber + 1).toString()), {
                        setWidth: levelButton.getWidth(),
                        setHeight: levelButton.getWidth() * 0.75,
                        setBackgroundColor: level.unlocked ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 130, 120) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 190, 190, 190),
                        setTextColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255)
                    } ));
                    var starsize = levelButton.getWidth() * 0.25;
                    var startPostition = (levelButton.getWidth() - (starsize)) / 2;
                    if (level.stars === 2) {
                        startPostition = levelButton.getWidth() / 2 - starsize;
                    }
                    else  {
                        if (level.stars === 3) {
                            startPostition = (levelButton.getWidth() - (starsize)) / 2 - starsize;
                        }
                    }
                    for (var n = 0; n < level.stars; n++) {
                        levelButton.add(Bridge.merge(new ThreeOneSevenBee.Model.UI.ImageView("star_activated.png", starsize, starsize), {
                            setY: levelButton.getHeight() - starsize,
                            setX: startPostition
                        } ));
                        startPostition += starsize;
                    }
                    levelButtons.add(levelButton);
                    levelNumber += 1;
    
                    switch (user.categories.getItem(this.getCategory()).name) {
                        case "Tutorial": 
                            this.getBadgeView().setImage("tutorial_badge.png");
                            break;
                        case "Potenser": 
                            this.getBadgeView().setImage("potens_badge.png");
                            break;
                        case "Brøker": 
                            this.getBadgeView().setImage("brøkbadge.png");
                            break;
                        case "Parenteser": 
                            this.getBadgeView().setImage("parenthesis_badge.png");
                            break;
                        case "Master of Algebra": 
                            this.getBadgeView().setImage("master_of_algebrabadge.png");
                            break;
                        default: 
                            break;
                    }
    
                }).call(this);
            }
            this.getStarTextView().setText(userStarsInCategory + " / " + totalStars);
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
        helpButton: null,
        progressbar: null,
        identityMenu: null,
        expression: null,
        toolTipView: null,
        config: {
            properties: {
                OnExit: null,
                OnNextLevel: null
            }
        },
        constructor: function (game) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, 700, 400);
    
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
                setBackgroundColor: game.getIsLastLevel() || game.getIsLevelCompleted() === false ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 190, 190, 190) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 120, 130),
                setTextColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255)
            } );
    
            this.restartButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.ImageView("restart.png", 50, 50), {
                onClick: function () {
                    game.restartLevel();
                },
                setX: 110,
                setY: 0,
                setBackgroundColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 192, 57, 43)
            } );
    
            this.toolTipView = Bridge.merge(new ThreeOneSevenBee.Model.UI.ToolTipView(game.getUser().getCurrentLevel().description, 300, 100), {
                setX: this.getWidth() - 300,
                setY: 50,
                setVisible: false,
                onClick: Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.LevelView.f3)
            } );
            this.toolTipView.setArrowPosition(155);
    
            this.helpButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView("?", Bridge.fn.bind(this, function () {
                if (game.getUser().getCurrentLevel().description !== "") {
                    if (this.toolTipView.getVisible() === true) {
                        this.helpButton.setText("?");
                        this.toolTipView.setVisible(false);
                    }
                    else  {
                        this.helpButton.setText(" Luk ");
                        this.toolTipView.setVisible(true);
                    }
                }
            })), {
                setX: this.getWidth() - 160,
                setY: 0,
                setWidth: 50,
                setHeight: 50,
                setBackgroundColor: game.getUser().getCurrentLevel().description === "" ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 190, 190, 190) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 120, 130),
                setTextColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255)
            } );
    
            this.progressbar = Bridge.merge(new ThreeOneSevenBee.Model.UI.ProgressbarStarView(game.progressBar, this.getWidth() - 340, 30), {
                setX: 170,
                setY: 10
            } );
    
            this.identityMenu = Bridge.merge(new ThreeOneSevenBee.Model.UI.IdentityMenuView(game.getExprModel(), this.getWidth(), 100), {
                setY: this.getHeight() - 125
            } );
    
            this.expression = Bridge.merge(new ThreeOneSevenBee.Model.UI.ExpressionView("constructor$2", game.getExprModel(), this.getWidth() - 100, this.getHeight() - 275, 8), {
                setX: 50,
                setY: 100
            } );
    
            this.children = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)(), [
                [this.menuButton],
                [this.nextButton],
                [this.restartButton],
                [this.helpButton],
                [this.progressbar],
                [this.identityMenu],
                [this.expression],
                [this.toolTipView]
            ] );
    
            if (Bridge.hasValue(this.onChanged)) {
                this.onChanged();
            }
        },
        update: function (game) {
            this.progressbar.update(game.progressBar);
            this.identityMenu.update(game.getExprModel().getIdentities(), game.getExprModel());
            this.expression.update$1(game.getExprModel());
            this.nextButton.setBackgroundColor(game.getIsLastLevel() || game.getIsLevelCompleted() === false ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 190, 190, 190) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 120, 130));
            this.helpButton.setBackgroundColor(game.getUser().getCurrentLevel().description === "" ? new ThreeOneSevenBee.Model.UI.Color("constructor$1", 190, 190, 190) : new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 120, 130));
            this.toolTipView.setText(game.getUser().getCurrentLevel().description);
            this.toolTipView.setVisible(false);
            this.helpButton.setText("?");
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
        },
        f3: function () {
            if (Bridge.hasValue(this.helpButton.onClick) && this.toolTipView.getVisible() === true) {
                this.helpButton.onClick();
            }
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.LoginView', {
        inherits: [ThreeOneSevenBee.Model.UI.FrameView],
        onLogin: null,
        status: null,
        constructor: function (width, height) {
            ThreeOneSevenBee.Model.UI.FrameView.prototype.$constructor.call(this, width, height);
    
    
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
    
            var username = new ThreeOneSevenBee.Model.UI.Inputbox("constructor", "Username");
            username.setX(75);
            username.setY(100);
            username.setWidth(450);
            username.setHeight(36);
            username.setAlign(ThreeOneSevenBee.Model.UI.TextAlignment.left);
    
            var password = new ThreeOneSevenBee.Model.UI.Inputbox("constructor$1", "Password", true);
            password.setX(75);
            password.setY(160);
            password.setWidth(450);
            password.setHeight(36);
            password.setAlign(ThreeOneSevenBee.Model.UI.TextAlignment.left);
    
            var submit = new ThreeOneSevenBee.Model.UI.ButtonView("Log ind", Bridge.fn.bind(this, function () {
                if (Bridge.hasValue(this.onLogin)) {
                    this.onLogin(username.getText(), password.getText());
                }
            }));
            submit.setX(220);
            submit.setY(250);
            submit.setWidth(160);
            submit.setHeight(60);
            submit.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 175, 100));
            submit.setTextColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
    
            this.status = new ThreeOneSevenBee.Model.UI.LabelView(" ");
            this.status.setX(75);
            this.status.setY(200);
            this.status.setWidth(200);
            this.status.setHeight(30);
            this.status.setTextColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 200, 0, 0));
    
            this.setContent(Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(600, 400), {
                children: Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)(), [
                    [username],
                    [password],
                    [submit],
                    [this.status]
                ] )
            } ));
        },
        showLoginError: function () {
            this.status.setText("Forkert brugernavn eller kode");
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.UI.PlayerListView', {
        inherits: [ThreeOneSevenBee.Model.UI.CompositeView],
        frameView: null,
        config: {
            init: function () {
                this.badgeDictionary = Bridge.merge(new Bridge.Dictionary$2(ThreeOneSevenBee.Model.Game.BadgeName,String)(), [
        [ThreeOneSevenBee.Model.Game.BadgeName.brokBadge, "brøkbadge.png"],
        [ThreeOneSevenBee.Model.Game.BadgeName.masterOfAlgebra, "master_of_algebrabadge.png"],
        [ThreeOneSevenBee.Model.Game.BadgeName.potensBadge, "potens_badge.png"],
        [ThreeOneSevenBee.Model.Game.BadgeName.tutorialBadge, "tutorial_badge.png"],
        [ThreeOneSevenBee.Model.Game.BadgeName.parenthesisBadge, "parenthesis_badge.png"]
    ] ) || null;
            }
        },
        constructor: function (players, width, height) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, width, height);
    
            this.build(players);
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 209, 209, 209));
        },
        constructor$1: function (width, height) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, width, height);
    
    
        },
        build: function (players) {
            var $t, $t1;
            this.children = new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)();
    
            var badgesWidth = (this.badgeDictionary.getCount() - 1) * 15;
            var labelWidth = this.getWidth() - badgesWidth - 25;
    
            var topListTextView = Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView("Top 7"), {
                setX: 5,
                setWidth: this.getWidth() - 10,
                setHeight: 15,
                setY: 5
            } );
    
            var offsetY = 25;
    
            $t = Bridge.getEnumerator(Bridge.Linq.Enumerable.from(players).take(7));
            while ($t.moveNext()) {
                var player = $t.getCurrent();
    
                var row = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(this.getWidth(), 20), [
                    [Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView(" " + player.getPlayerName() + " "), {
                        setWidth: labelWidth,
                        setHeight: 10,
                        setAlign: ThreeOneSevenBee.Model.UI.TextAlignment.left,
                        setY: 5
                    } )]
                ] );
                var spacing = 0;
                $t1 = Bridge.getEnumerator(player.badges);
                while ($t1.moveNext()) {
                    var badge = $t1.getCurrent();
                    row.add(Bridge.merge(new ThreeOneSevenBee.Model.UI.ImageView(this.badgeDictionary.get(badge), 15, 15), {
                        setX: labelWidth + spacing,
                        setY: 2.5
                    } ));
                    spacing += 15;
                }
                this.frameView = new ThreeOneSevenBee.Model.UI.FrameView("constructor$1", this.getWidth(), 20, row);
    
                row.setX(5);
                row.setY(offsetY);
                row.setWidth(this.getWidth() - 10);
                row.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 239, 239, 239));
                this.children.add(row);
    
    
                offsetY += 25;
            }
            this.children.add(topListTextView);
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
            var $t;
            this.progress = Bridge.merge(new ThreeOneSevenBee.Model.UI.View(0, 0, Math.min(this.getWidth(), Math.max(0, this.getWidth() * progressbar.getPercentage())), this.getHeight()), {
                setBackgroundColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 40, 175, 100)
            } );
            this.stars = new Bridge.List$1(ThreeOneSevenBee.Model.UI.ImageView)();
            this.stars.addRange(Bridge.Linq.Enumerable.from(progressbar.activatedStarPercentages()).select(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.ProgressbarStarView.f1)));
            this.stars.addRange(Bridge.Linq.Enumerable.from(progressbar.deactivatedStarPercentages()).select(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.ProgressbarStarView.f2)));
            var lastX = null;
            var numberOfEqualX = 1;
            for (var index = 0; index < this.stars.getCount(); index++) {
                if (Bridge.Nullable.lt(Bridge.Nullable.sub(this.stars.getItem(index).getX(), lastX), 4.94065645841247E-324)) {
                    $t = this.stars.getItem(index);
                    $t.setX($t.getX()-numberOfEqualX * (this.stars.getItem(index).getWidth() / 2));
                    numberOfEqualX++;
                }
                else  {
                    lastX = this.stars.getItem(index).getX();
                }
            }
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
        logoutButton: null,
        onLogout: null,
        badgesView: null,
        badgeInfoText: null,
        showBadges: null,
        config: {
            init: function () {
                this.playerView = new ThreeOneSevenBee.Model.UI.PlayerListView("constructor$1", 1, 1) || null;
            }
        },
        constructor: function (game) {
            ThreeOneSevenBee.Model.UI.CompositeView.prototype.$constructor.call(this, 420, 220);
    
            this.build(game);
        },
        build: function (game) {
            var $t;
            var user = game.getUser();
            var players = game.getPlayers();
            this.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
            this.welcomeText = Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView("Velkommen " + user.getPlayerName()), {
                setX: 10,
                setY: 10,
                setHeight: 50,
                setWidth: 220
            } );
    
            this.badgesView = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(220, 40), {
                setX: this.welcomeText.getX(),
                setY: this.welcomeText.getY() + this.welcomeText.getHeight()
            } );
    
            this.badgeInfoText = Bridge.merge(new ThreeOneSevenBee.Model.UI.LabelView("Badges: "), {
                setWidth: 100,
                setHeight: 20,
                setX: -10
            } );
    
            this.badgesView.add(this.badgeInfoText);
    
            if (Bridge.hasValue(user.badges)) {
                // start spacing half the width of a badge
                var spacing = -10;
                $t = Bridge.getEnumerator(user.badges);
                while ($t.moveNext()) {
                    var badge = $t.getCurrent();
                    if (this.playerView.badgeDictionary.containsKey(badge)) {
                        this.badgesView.add(Bridge.merge(new ThreeOneSevenBee.Model.UI.ImageView(this.playerView.badgeDictionary.get(badge), 25, 25), {
                            setX: this.badgeInfoText.getX() + this.badgeInfoText.getWidth() + spacing
                        } ));
                    }
                    spacing += 30;
                }
            }
    
            var playIcon = Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(0, 0, 100, 100), [
                [25, 25],
                [75, 50],
                [25, 75]
            ] );
    
            this.logoutButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView("Log ud", Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.UI.TitleView.f1)), {
                setWidth: 50,
                setHeight: 15,
                setTextColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255),
                setBackgroundColor: new ThreeOneSevenBee.Model.UI.Color("constructor$1", 193, 57, 43)
            } );
    
            playIcon.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 255, 255, 255));
            this.playButton = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(100, 100), [
                [playIcon]
            ] );
            this.playButton.setBackgroundColor(new ThreeOneSevenBee.Model.UI.Color("constructor$1", 39, 174, 97));
            this.playButton.setX(10);
            this.playButton.setY(this.badgesView.getY() + this.badgesView.getHeight());
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
            this.levelButton.setX(this.playButton.getX() + this.playButton.getWidth() + 20);
            this.levelButton.setY(this.badgesView.getY() + this.badgesView.getHeight());
    
            this.playerList = Bridge.merge(new ThreeOneSevenBee.Model.UI.PlayerListView("constructor", players, 160, 200), {
                setX: this.levelButton.getX() + this.levelButton.getWidth() + 20,
                setY: this.welcomeText.getY()
            } );
    
            this.children = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.UI.View)(), [
                [this.logoutButton],
                [this.welcomeText],
                [this.levelButton],
                [this.playerList],
                [this.playButton],
                [this.badgesView]
            ] );
        }
    });
    
    Bridge.ns("ThreeOneSevenBee.Model.UI.TitleView", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.UI.TitleView, {
        f1: function () {
            if (Bridge.hasValue(this.onLogout)) {
                this.onLogout();
            }
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
            this.getlabelView().setText(" " + value.split(String.fromCharCode(10)).join(" \n ") + " ");
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
                        [0, 11],
                        [10, 0],
                        [20, 11]
                    ] ));
                    this.getlabelView().setY(10);
                    this.getlabelView().setHeight(this.getHeight() - 10);
                    break;
                case ThreeOneSevenBee.Model.UI.Direction.right: 
                    this.setarrow(Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(this.getWidth() - 10, 0, 10, 20), [
                        [-1, 0],
                        [10, 10],
                        [-1, 20]
                    ] ));
                    this.getlabelView().setWidth(this.getWidth() - 10);
                    break;
                case ThreeOneSevenBee.Model.UI.Direction.left: 
                    this.setarrow(Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(0, 0, 10, 20), [
                        [11, 0],
                        [0, 10],
                        [11, 20]
                    ] ));
                    this.getlabelView().setX(10);
                    this.getlabelView().setWidth(this.getWidth() - 10);
                    break;
                case ThreeOneSevenBee.Model.UI.Direction.bottom: 
                    this.setarrow(Bridge.merge(new ThreeOneSevenBee.Model.UI.VectorImageView(0, this.getHeight() - 10, 20, 10), [
                        [0, -1],
                        [20, -1],
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
