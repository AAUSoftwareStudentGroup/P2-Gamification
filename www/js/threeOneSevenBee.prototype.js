/* global Bridge */

"use strict";

Bridge.define('ThreeOneSevenBee.Prototype.PrototypeGame', {
    inherits: [ThreeOneSevenBee.Framework.Game],
    draggingRectangle: -1,
    draggingCircle: -1,
    didIt: false,
    globalCounter: 0,
    config: {
        init: function () {
            this.circleA = new ThreeOneSevenBee.Framework.Euclidean.Circle();
            this.circleB = new ThreeOneSevenBee.Framework.Euclidean.Circle();
            this.rectangleA = new ThreeOneSevenBee.Framework.Euclidean.Rectangle();
            this.rectangleB = new ThreeOneSevenBee.Framework.Euclidean.Rectangle();
            this.rectangleDragOffset = new ThreeOneSevenBee.Framework.Euclidean.Vector2();
            this.circleDragOffset = new ThreeOneSevenBee.Framework.Euclidean.Vector2();
        }
    },
    constructor: function (canvas) {
        ThreeOneSevenBee.Framework.Game.prototype.$constructor.call(this, canvas);

        this.rectangleA = Bridge.merge(new ThreeOneSevenBee.Framework.Euclidean.Rectangle(), {
            location: new ThreeOneSevenBee.Framework.Euclidean.Vector2("constructor$1", 60, 260),
            width: 100,
            height: 100
        } );

        this.rectangleB = Bridge.merge(new ThreeOneSevenBee.Framework.Euclidean.Rectangle(), {
            location: new ThreeOneSevenBee.Framework.Euclidean.Vector2("constructor$1", 10, 10),
            width: 200,
            height: 200
        } );

        this.circleA = Bridge.merge(new ThreeOneSevenBee.Framework.Euclidean.Circle(), {
            center: new ThreeOneSevenBee.Framework.Euclidean.Vector2("constructor$1", 320, 320),
            radius: 50
        } );

        this.circleB = Bridge.merge(new ThreeOneSevenBee.Framework.Euclidean.Circle(), {
            center: new ThreeOneSevenBee.Framework.Euclidean.Vector2("constructor$1", 320, 110),
            radius: 100
        } );

        this.getInput().addPointerDown(Bridge.fn.bind(this, this.input_PointerDown));
        this.getInput().addPointerUp(Bridge.fn.bind(this, this.input_PointerUp));
    },
    input_PointerDown: function (arg) {
        console.log("Input_PointerDown");
        var point = new ThreeOneSevenBee.Framework.Euclidean.Vector2("constructor$1", arg.getRelativeX(), arg.getRelativeY());
        console.log(point.$clone());

        if (this.rectangleA.contains$1(point.$clone())) {
            this.rectangleDragOffset = ThreeOneSevenBee.Framework.Euclidean.Vector2.op_Subtraction(this.rectangleA.location, point);
            this.draggingRectangle = arg.getId();
        }

        if (this.circleA.contains$1(point.$clone())) {
            this.circleDragOffset = ThreeOneSevenBee.Framework.Euclidean.Vector2.op_Subtraction(this.circleA.center, point);
            this.draggingCircle = arg.getId();
        }
    },
    input_PointerUp: function (arg) {
        console.log("Input_PointerUp");
        var point = new ThreeOneSevenBee.Framework.Euclidean.Vector2("constructor$1", arg.getRelativeX(), arg.getRelativeY());
        console.log(point.$clone());

        if (arg.getId() === this.draggingRectangle) {
            this.draggingRectangle = -1;
        }
        if (arg.getId() === this.draggingCircle) {
            this.draggingCircle = -1;
        }

        if (this.circleB.contains$1(point.$clone())) {

            $.ajax({ dataType: "json", url: "/api/?func=AddCount", cache: false, success: Bridge.fn.bind(this, function (data, textStatus, request) {
                var tempCounter = { };
                if (Bridge.Int.tryParseInt(Bridge.cast(data.counter, String), tempCounter, -2147483648, 2147483647)) {
                    this.globalCounter = tempCounter.v;
                }

            }) });
        }
    },
    update: function (deltaTime, totalTime) {


        ThreeOneSevenBee.Framework.Game.prototype.update.call(this, deltaTime, totalTime);
    },
    draw: function (deltaTime, totalTime) {
        var $t;
        ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.clear$1(this.getContext2D(), "cornflowerblue");

        if (this.draggingRectangle >= 0) {
            console.log("draggingRectangle: " + this.draggingRectangle);
            var pointer = Bridge.Linq.Enumerable.from(this.getInput().getPointers()).firstOrDefault(Bridge.fn.bind(this, function (p) {
                return p.getId() === this.draggingRectangle;
            }), Bridge.getDefaultValue(ThreeOneSevenBee.Framework.Html.Pointer));
            if (pointer !== null) {
                this.rectangleA.location = ThreeOneSevenBee.Framework.Euclidean.Vector2.op_Addition(new ThreeOneSevenBee.Framework.Euclidean.Vector2("constructor$1", pointer.getRelativeX(), pointer.getRelativeY()), this.rectangleDragOffset);
            }
            else  {
                console.log("Didn't find pointer");
            }
        }

        if (this.draggingCircle >= 0) {
            console.log("draggingCircle: " + this.draggingCircle);
            var pointer1 = Bridge.Linq.Enumerable.from(this.getInput().getPointers()).firstOrDefault(Bridge.fn.bind(this, function (p) {
                return p.getId() === this.draggingCircle;
            }), Bridge.getDefaultValue(ThreeOneSevenBee.Framework.Html.Pointer));
            if (pointer1 !== null) {
                this.circleA.center = ThreeOneSevenBee.Framework.Euclidean.Vector2.op_Addition(new ThreeOneSevenBee.Framework.Euclidean.Vector2("constructor$1", pointer1.getRelativeX(), pointer1.getRelativeY()), this.circleDragOffset);
            }
            else  {
                console.log("Didn't find pointer");
            }
        }


        if (this.circleB.contains(this.rectangleA.$clone())) {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.fillCircle$1(this.getContext2D(), this.circleB.$clone(), "lime");
        }
        else  {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.fillCircle$1(this.getContext2D(), this.circleB.$clone(), "red");
        }
        ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawCircle$1(this.getContext2D(), this.circleB.$clone(), "black");

        if (this.rectangleB.contains(this.circleA.$clone())) {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.fillRectangle$1(this.getContext2D(), this.rectangleB.$clone(), "lime");
        }
        else  {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.fillRectangle$1(this.getContext2D(), this.rectangleB.$clone(), "red");
        }
        ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawRectangle$1(this.getContext2D(), this.rectangleB.$clone(), "black");

        /* if (circleA.Contains(Input.Mouse))
                Context2D.FillCircle(circleA, HTMLColor.Lime);
            else*/
        ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.fillCircle$1(this.getContext2D(), this.circleA.$clone(), "yellow");
        ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawCircle$1(this.getContext2D(), this.circleA.$clone(), "black");

        /* if (rectangleA.Contains(Input.Mouse))
                Context2D.FillRectangle(rectangleA, HTMLColor.Lime);
            else*/
        ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.fillRectangle$1(this.getContext2D(), this.rectangleA.$clone(), "yellow");
        ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawRectangle$1(this.getContext2D(), this.rectangleA.$clone(), "black");

        ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawString(this.getContext2D(), 5, 200, "Counter: " + this.globalCounter, "20px Arial", "black");
        ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawString(this.getContext2D(), 5, 400, "Place the smaller shapes in the larger shapes of the opposite type.", "20px Arial", "black");
        if (this.circleB.contains(this.rectangleA.$clone()) && this.rectangleB.contains(this.circleA.$clone())) {
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawString(this.getContext2D(), 200, 420, "You did it!", "20px Arial", "black");
            this.didIt = true;
        }
        else  {
            if (this.didIt === true) {
                ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawString(this.getContext2D(), 200, 420, "You undid it! Tanner Helland you fuck!", "20px Arial", "black");
            }
        }

        var i = 1;
        ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawString(this.getContext2D(), 2, 2 + 20 * i++, "Touches", "20px Arial", "black");
        $t = Bridge.getEnumerator(this.getInput().getPointers());
        while ($t.moveNext()) {
            var pointer2 = $t.getCurrent();
            ThreeOneSevenBee.Framework.CanvasRenderingContext2DExtensions.drawString(this.getContext2D(), 2, 2 + 20 * i++, pointer2.getId().toString(), "20px Arial", "black");
        }

        ThreeOneSevenBee.Framework.Game.prototype.draw.call(this, deltaTime, totalTime);
    }
});

Bridge.define('ThreeOneSevenBee.Prototype.App', {
    statics: {
        config: {
            init: function () {
                Bridge.ready(this.main);
            }
        },
        main: function () {
            document.body.addEventListener("touchstart", function (e) {
                e.preventDefault();
            });
            document.body.addEventListener("touchmove", function (e) {
                e.preventDefault();
            });
            document.body.addEventListener("touchend", function (e) {
                e.preventDefault();
            });
            document.body.addEventListener("touchcancel", function (e) {
                e.preventDefault();
            });

            var canvas = document.getElementById("canvas");
            var game = new ThreeOneSevenBee.Prototype.PrototypeGame(canvas);

            game.run();
        }
    }
});

Bridge.init();