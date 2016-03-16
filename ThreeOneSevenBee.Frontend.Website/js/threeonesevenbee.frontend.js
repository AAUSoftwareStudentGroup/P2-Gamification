(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Frontend.App', {
        statics: {
            config: {
                init: function () {
                    Bridge.ready(this.main);
                }
            },
            main: function () {
                var t = new ThreeOneSevenBee.Model.Template();
                console.log(t.toString());
    
                var canvas = document.getElementById("canvas");
    
                var context = new ThreeOneSevenBee.Frontend.CanvasContext(canvas);
    
                var model = new ThreeOneSevenBee.Model.Expression.ExpressionModel("b+a*a", [Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).itselfRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).commutativeRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).fractionVariableMultiplyRule]);
    
                var view = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(600, 400), [
                    [Bridge.merge(new ThreeOneSevenBee.Model.UI.IdentityMenuView(model, 600, 20), {
                        setY: 60
                    } )],
                    [Bridge.merge(new ThreeOneSevenBee.Model.UI.ExpressionView(model, 220, 100), {
                        setX: 20,
                        setY: 20
                    } )],
                    [Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView("Hello", $_.ThreeOneSevenBee.Frontend.App.f1), {
                        setX: 100,
                        setY: 100,
                        setWidth: 40,
                        setHeight: 20
                    } )],
                    [Bridge.merge(new ThreeOneSevenBee.Model.UI.ButtonView("World", $_.ThreeOneSevenBee.Frontend.App.f2), {
                        setX: 200,
                        setY: 100,
                        setWidth: 40,
                        setHeight: 20
                    } )]
                ] );
    
                console.log(view);
    
                context.setContentView(view);
                model.addOnChanged(function (m) {
                    context.draw();
                });
                context.draw();
            }
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Frontend.App", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Frontend.App, {
        f1: function () {
            Bridge.global.alert("Hello");
        },
        f2: function () {
            Bridge.global.alert("World");
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Frontend.CanvasContext', {
        inherits: [ThreeOneSevenBee.Model.UI.Context],
        context: null,
        constructor: function (canvas) {
            ThreeOneSevenBee.Model.UI.Context.prototype.$constructor.call(this, canvas.width, canvas.height);
    
            this.context = canvas.getContext("2d");
            this.context.font = "12px Arial Black";
        },
        setContentView: function (view) {
            var canvasLeft = this.context.canvas.getBoundingClientRect().left;
            var canvasRight = this.context.canvas.getBoundingClientRect().left;
            this.context.canvas.addEventListener("mousedown", function (e) {
                view.click(e.clientX - Bridge.Int.trunc(canvasLeft), e.clientY - Bridge.Int.trunc(canvasRight));
            });
            ThreeOneSevenBee.Model.UI.Context.prototype.setContentView.call(this, view);
        },
        clear: function () {
            this.context.clearRect(0, 0, Bridge.Int.trunc(this.getWidth()), Bridge.Int.trunc(this.getHeight()));
        },
        draw$3: function (view, offsetX, offsetY) {
            this.context.beginPath();
            this.context.rect(view.getX() + offsetX, view.getY() + offsetY, view.getWidth(), view.getHeight());
            this.context.rect(view.getX() + offsetX, view.getY() + offsetY, view.getWidth() * view.progressbar.getPercentage(), view.getHeight());
            this.context.closePath();
            this.context.stroke();
        },
        draw$2: function (view, offsetX, offsetY) {
            this.context.textBaseline = "middle";
            this.context.textAlign = "center";
            this.context.fillText(view.getText(), Bridge.Int.trunc((view.getX() + offsetX + view.getWidth() / 2)), Bridge.Int.trunc((view.getY() + offsetY + view.getHeight() / 2)));
        },
        draw$1: function (view, offsetX, offsetY) {
            this.context.fillStyle = view.getSelected() ? "skyblue" : "white";
            this.context.fillRect(Bridge.Int.trunc((view.getX() + offsetX)), Bridge.Int.trunc((view.getY() + offsetY)), Bridge.Int.trunc(view.getWidth()), Bridge.Int.trunc(view.getHeight()));
            this.context.fillStyle = "black";
            this.draw$2(Bridge.as(view, ThreeOneSevenBee.Model.UI.LabelView), offsetX, offsetY);
        }
    });
    
    Bridge.init();
})(this);
