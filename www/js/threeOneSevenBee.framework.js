/* global Bridge */

"use strict";

/** @namespace ThreeOneSevenBee.Framework */

/**
 * Provides functionality to analyze expressions and provide identical alternatives based on {@link }.
 *
 * @public
 * @class ThreeOneSevenBee.Framework.ExpressionAnalyzer
 */
Bridge.define('ThreeOneSevenBee.Framework.ExpressionAnalyzer', {
    rules: null,
    constructor: function () {
        this.rules = new Bridge.List$1(ThreeOneSevenBee.Framework.ExpressionRule)();
    },
    add: function (rule) {
        this.rules.add(rule);
    },
    remove: function (rule) {
        this.rules.remove(rule);
    },
    getIdentities: function (expression) {
        var identities = new Bridge.List$1(ThreeOneSevenBee.Framework.Expression)();

        // e is always identical to itself
        identities.add(expression);

        // check additional rules here

        return identities;
    }
});

/**
 * A generel representation of an expression rule.
 *
 * @public
 * @class ThreeOneSevenBee.Framework.ExpressionRule
 */
Bridge.define('ThreeOneSevenBee.Framework.ExpressionRule', {
    statics: {
        fromExpression: function (expression) {
            throw new Bridge.NotImplementedException();
        },
        fromString: function (rule) {
            throw new Bridge.NotImplementedException();
        }
    }
});

Bridge.define('ThreeOneSevenBee.Framework.ExpressionSerializer', {
    serialize: function (expression) {
        throw new Bridge.NotImplementedException();
    },
    deserialize: function (expression) {
        throw new Bridge.NotImplementedException();
    }
});

Bridge.define('ThreeOneSevenBee.Framework.Game', {
    lastTotalTime: 0,
    config: {
        properties: {
            IsRunning: false,
            Canvas: null,
            Context2D: null,
            Input: null
        }
    },
    constructor: function (canvas) {
        this.setCanvas(canvas);
        this.setContext2D(canvas.getContext("2d"));
        this.setInput(new ThreeOneSevenBee.Framework.Html.ElementInput(this.getCanvas()));
    },
    run: function () {
        if (this.getIsRunning() === true) {
            throw new Bridge.InvalidOperationException("Game is already running.");
        }
        this.setIsRunning(true);

        this.initialize();

        Bridge.global.requestAnimationFrame(Bridge.fn.bind(this, this.onAnimationFrame));
    },
    onAnimationFrame: function (totalTime) {
        var deltaTime = totalTime - this.lastTotalTime;

        this.update(deltaTime, totalTime);
        this.draw(deltaTime, totalTime);

        this.lastTotalTime = totalTime;
        if (this.getIsRunning()) {
            Bridge.global.requestAnimationFrame(Bridge.fn.bind(this, this.onAnimationFrame));
        }
    },
    initialize: function () {
    },
    update: function (deltaTime, totalTime) {
    },
    draw: function (deltaTime, totalTime) {
    }
});

Bridge.define('ThreeOneSevenBee.Framework.IExpression');

Bridge.define('ThreeOneSevenBee.Framework.Expression', {
    inherits: [ThreeOneSevenBee.Framework.IExpression],
    equals$1: function (other) {
        return (Bridge.is(other, ThreeOneSevenBee.Framework.Expression)) && this.equals(Bridge.cast(other, ThreeOneSevenBee.Framework.Expression));
    },
    equals: function (other) {
        return this.toString() === other.toString();
    },
    getHashCode: function () {
        return Bridge.getHashCode(this.getValue());
    },
    toString: function () {
        return this.getValue();
    }
});

Bridge.define('ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions', {
    statics: {
        clear: function (context2D) {
            context2D.clearRect(0, 0, context2D.canvas.width, context2D.canvas.height);
        },
        clear$1: function (context2D, fillStyle) {
            context2D.save();

            context2D.fillStyle = fillStyle;
            context2D.fillRect(0, 0, context2D.canvas.width, context2D.canvas.height);

            context2D.restore();
        },
        drawLine: function (context2D, aX, aY, bX, bY, strokeStyle) {
            context2D.save();

            context2D.strokeStyle = strokeStyle;
            context2D.beginPath();
            context2D.moveTo(aX, aY);
            context2D.lineTo(bX, bY);
            context2D.closePath();
            context2D.stroke();

            context2D.restore();
        },
        drawLine$1: function (context2D, a, b, strokeStyle) {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawLine(context2D, a.x, a.y, b.x, b.y, strokeStyle);
        },
        drawCircle: function (context2D, centerX, centerY, radius, strokeStyle) {
            context2D.save();

            context2D.strokeStyle = strokeStyle;
            context2D.beginPath();
            context2D.arc(centerX, centerY, radius, 0.0, 2 * Math.PI);
            context2D.closePath();
            context2D.stroke();

            context2D.restore();
        },
        drawCircle$2: function (context2D, center, radius, strokeStyle) {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawCircle(context2D, center.x, center.y, radius, strokeStyle);
        },
        drawCircle$1: function (context2D, c, strokeStyle) {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawCircle(context2D, c.center.x, c.center.y, c.radius, strokeStyle);
        },
        fillCircle: function (context2D, centerX, centerY, radius, fillStyle) {
            context2D.save();

            context2D.fillStyle = fillStyle;
            context2D.beginPath();
            context2D.arc(centerX, centerY, radius, 0.0, 2 * Math.PI);
            context2D.closePath();
            context2D.fill();

            context2D.restore();
        },
        fillCircle$2: function (context2D, center, radius, fillStyle) {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.fillCircle(context2D, center.x, center.y, radius, fillStyle);
        },
        fillCircle$1: function (context2D, c, fillStyle) {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.fillCircle(context2D, c.center.x, c.center.y, c.radius, fillStyle);
        },
        drawRectangle: function (context2D, left, top, width, height, strokeStyle) {
            context2D.save();

            context2D.strokeStyle = strokeStyle;
            context2D.beginPath();
            context2D.rect(left, top, width, height);
            context2D.closePath();
            context2D.stroke();

            context2D.restore();
        },
        drawRectangle$2: function (context2D, leftTop, width, height, strokeStyle) {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawRectangle(context2D, leftTop.x, leftTop.y, width, height, strokeStyle);
        },
        drawRectangle$1: function (context2D, rectangle, strokeStyle) {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawRectangle(context2D, rectangle.getLeft(), rectangle.getTop(), rectangle.width, rectangle.height, strokeStyle);
        },
        fillRectangle: function (context2D, left, top, width, height, fillStyle) {
            context2D.save();

            context2D.fillStyle = fillStyle;
            context2D.beginPath();
            context2D.rect(left, top, width, height);
            context2D.closePath();
            context2D.fill();

            context2D.restore();
        },
        fillRectangle$2: function (context2D, leftTop, width, height, fillStyle) {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.fillRectangle(context2D, leftTop.x, leftTop.y, width, height, fillStyle);
        },
        fillRectangle$1: function (context2D, rectangle, fillStyle) {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.fillRectangle(context2D, rectangle.getLeft(), rectangle.getTop(), rectangle.width, rectangle.height, fillStyle);
        },
        drawString: function (context2D, left, bottom, text, font, fillStyle) {
            context2D.save();

            context2D.fillStyle = fillStyle;
            context2D.font = font;
            context2D.fillText(text, Bridge.Int.trunc(left), Bridge.Int.trunc(bottom));

            context2D.restore();
        },
        strokeString: function (context2D, left, bottom, text, font, strokeStyle) {
            context2D.save();

            context2D.strokeStyle = strokeStyle;
            context2D.font = font;
            context2D.strokeText(text, Bridge.Int.trunc(left), Bridge.Int.trunc(bottom));

            context2D.restore();
        }
    }
});



Bridge.init();