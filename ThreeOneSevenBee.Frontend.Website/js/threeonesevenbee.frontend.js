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
                var canvas = document.getElementById("canvas");
                canvas.width = document.documentElement.clientWidth;
                canvas.height = document.documentElement.clientHeight;
    
                var context = new ThreeOneSevenBee.Frontend.CanvasContext(canvas);
    
                var expressionModel = new ThreeOneSevenBee.Model.Expression.ExpressionModel("{a^3*b^4}/{b^3*a^4}", [Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).divideRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).exponentToProductRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).productToExponentRule]);
    
                var view = Bridge.merge(new ThreeOneSevenBee.Model.UI.CompositeView(canvas.width, canvas.height), [
                    [Bridge.merge(new ThreeOneSevenBee.Model.UI.ProgressbarStarView(new ThreeOneSevenBee.Model.UI.ProgressbarStar(50, 100, [30, 60, 75]), canvas.width, 20), {
                        setY: 30
                    } )],
                    [Bridge.merge(new ThreeOneSevenBee.Model.UI.ExpressionView(expressionModel, canvas.width, canvas.height - 70), {
                        setX: 0,
                        setY: 50
                    } )],
                    [Bridge.merge(new ThreeOneSevenBee.Model.UI.IdentityMenuView(expressionModel, canvas.width, 50), {
                        setY: canvas.height - 50
                    } )]
                ] );
    
                context.setContentView(view);
                expressionModel.addOnChanged(function (m) {
                    context.draw();
                });
                context.draw();
            }
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
                view.click(e.clientX + document.body.scrollLeft - Bridge.Int.trunc(canvasLeft), e.clientY + document.body.scrollTop - Bridge.Int.trunc(canvasRight));
            });
            ThreeOneSevenBee.Model.UI.Context.prototype.setContentView.call(this, view);
        },
        clear: function () {
            this.context.clearRect(0, 0, Bridge.Int.trunc(this.getWidth()), Bridge.Int.trunc(this.getHeight()));
        },
        draw$3: function (view, offsetX, offsetY) {
            this.draw$7(Bridge.as(view, ThreeOneSevenBee.Model.UI.View), offsetX, offsetY);
            this.context.fillStyle = "#000000";
            this.context.textBaseline = "middle";
            this.context.textAlign = "center";
            this.context.font = view.getHeight() + "px Cambria Math";
            this.context.fillText(view.getText(), Bridge.Int.trunc((view.getX() + offsetX + view.getWidth() / 2)), Bridge.Int.trunc((view.getY() + offsetY + view.getHeight() / 2)));
        },
        draw$4: function (view, offsetX, offsetY) {
            if (view.gettype() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.divide) {
                this.context.beginPath();
                this.context.lineCap = "round";
                this.context.lineWidth = view.getHeight() / 40;
                this.context.moveTo(view.getX() + offsetX, view.getY() + offsetY + view.getHeight() / 2);
                this.context.lineTo(view.getX() + offsetX + view.getWidth(), view.getY() + offsetY + view.getHeight() / 2);
                this.context.stroke();
            }
            else  {
                if (view.gettype() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.multiply) {
                    this.context.beginPath();
                    //context.Rect(, view.Width / 10,);
                    this.context.arc(view.getX() + offsetX + view.getWidth() / 2, view.getY() + offsetY + view.getHeight() / 2, view.getHeight() / 10, 0, 2 * Math.PI);
                    this.context.fill();
                    this.context.stroke();
                }
                else  {
                    if (view.gettype() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.add) {
                        this.context.beginPath();
                        this.context.lineWidth = view.getHeight() / 20;
                        this.context.moveTo(view.getX() + offsetX + view.getWidth() / 2, view.getY() + offsetY - view.getHeight() / 3 + view.getHeight() / 2);
                        this.context.lineTo(view.getX() + offsetX + view.getWidth() / 2, view.getY() + offsetY + view.getHeight() / 3 + view.getHeight() / 2);
                        this.context.moveTo(view.getX() + offsetX - view.getWidth() / 3 + view.getWidth() / 2, view.getY() + offsetY + view.getHeight() / 2);
                        this.context.lineTo(view.getX() + offsetX + view.getWidth() / 3 + view.getWidth() / 2, view.getY() + offsetY + view.getHeight() / 2);
                        this.context.stroke();
                        this.context.closePath();
                    }
                    else  {
                        if (view.gettype() === ThreeOneSevenBee.Model.Expression.Expressions.OperatorType.subtract) {
                            this.context.beginPath();
                            this.context.lineWidth = view.getHeight() / 20;
                            this.context.moveTo(view.getX() + offsetX - view.getWidth() / 3 + view.getWidth() / 2, view.getY() + offsetY + view.getHeight() / 2);
                            this.context.lineTo(view.getX() + offsetX + view.getWidth() / 3 + view.getWidth() / 2, view.getY() + offsetY + view.getHeight() / 2);
                            this.context.stroke();
                            this.context.closePath();
                        }
                    }
                }
            }
        },
        draw$5: function (view, offsetX, offsetY) {
            if (view.getType() === ThreeOneSevenBee.Model.UI.ParenthesisType.left) {
                this.context.beginPath();
                this.context.ellipse(view.getX() + view.getWidth() + offsetX, view.getY() + view.getHeight() / 2 + offsetY, view.getWidth(), view.getHeight() / 2, 0, Math.PI / 2, (3 * Math.PI) / 2);
                this.context.stroke();
            }
            else  {
                this.context.beginPath();
                this.context.ellipse(view.getX() + offsetX, view.getY() + view.getHeight() / 2 + offsetY, view.getWidth(), view.getHeight() / 2, 0, Math.PI / 2, (3 * Math.PI) / 2, true);
                this.context.stroke();
            }
        },
        draw$7: function (view, offsetX, offsetY) {
            this.context.fillStyle = view.getBackgroundColor();
            this.context.fillRect(Bridge.Int.trunc((view.getX() + offsetX)), Bridge.Int.trunc((view.getY() + offsetY)), Bridge.Int.trunc(view.getWidth()), Bridge.Int.trunc(view.getHeight()));
        },
        draw$2: function (view, offsetX, offsetY) {
            var img = new Image();
            img.src = "img/" + view.getImage();
            this.context.drawImage(img, view.getX() + offsetX, view.getY() + offsetY, view.getWidth(), view.getHeight());
        }
    });
    
    Bridge.init();
})(this);
