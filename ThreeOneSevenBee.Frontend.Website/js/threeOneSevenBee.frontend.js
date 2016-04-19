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
    
                var gameAPI = new ThreeOneSevenBee.Frontend.JQueryGameAPI();
    
                var gameModel;
                var gameView;
    
                gameAPI.getCurrentPlayer(function (u) {
                    u.addCategory(Bridge.merge(new ThreeOneSevenBee.Model.Game.LevelCategory("wat"), [
                        [new ThreeOneSevenBee.Model.Game.Level("constructor$2", "a*(a+a)", "a*(a+a)", ["2*a^2"])]
                    ] ));
                    gameAPI.getPlayers(function (p) {
                        gameModel = Bridge.merge(new ThreeOneSevenBee.Model.Game.GameModel(u, p), {
                            onSaveLevel: function (level) {
                                gameAPI.saveUserLevelProgress(level.levelID, level.currentExpression, $_.ThreeOneSevenBee.Frontend.App.f1);
                            }
                        } );
                        gameView = new ThreeOneSevenBee.Model.UI.GameView(gameModel, context);
                    });
                });
            }
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Frontend.App", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Frontend.App, {
        f1: function (success) {
            console.log(success);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Frontend.CanvasContext', {
        inherits: [ThreeOneSevenBee.Model.UI.Context],
        imageCache: null,
        context: null,
        constructor: function (canvas) {
            ThreeOneSevenBee.Model.UI.Context.prototype.$constructor.call(this, canvas.width, canvas.height);
    
            this.imageCache = new Bridge.Dictionary$2(String,HTMLImageElement)();
    
            this.context = canvas.getContext("2d");
            this.context.fillStyle = "#000000";
            this.context.lineWidth = 2;
            this.context.textBaseline = "middle";
    
            var canvasLeft = this.context.canvas.getBoundingClientRect().left;
            var canvasRight = this.context.canvas.getBoundingClientRect().left;
            this.context.canvas.addEventListener("mousedown", Bridge.fn.bind(this, function (e) {
                this.click(e.clientX + document.body.scrollLeft - Bridge.Int.trunc(canvasLeft), e.clientY + document.body.scrollTop - Bridge.Int.trunc(canvasRight));
            }));
        },
        colorToString: function (color) {
            return Bridge.String.format("rgba({0},{1},{2},{3})", Bridge.Int.format(color.red, 'G'), Bridge.Int.format(color.green, 'G'), Bridge.Int.format(color.blue, 'G'), Bridge.Int.format(color.alpha, 'G'));
        },
        setContentView: function (view) {
            ThreeOneSevenBee.Model.UI.Context.prototype.setContentView.call(this, view);
            this.draw();
        },
        clear: function () {
            this.context.clearRect(0, 0, Bridge.Int.trunc(this.getWidth()), Bridge.Int.trunc(this.getHeight()));
        },
        click: function (x, y) {
            this.contentView.click(x, y);
        },
        drawPolygon$1: function (path, fillColor, lineColor, lineWidth) {
            var $t;
            this.context.fillStyle = this.colorToString(fillColor);
            this.context.strokeStyle = this.colorToString(lineColor);
            this.context.lineWidth = lineWidth;
    
            this.context.beginPath();
            this.context.moveTo(path[0].x, path[0].y);
    
            $t = Bridge.getEnumerator(path);
            while ($t.moveNext()) {
                var point = $t.getCurrent();
                this.context.lineTo(point.x, point.y);
            }
            this.context.fill();
            this.context.stroke();
        },
        drawText: function (x, y, width, height, text, textColor) {
            var $t;
            var lines = text.split(String.fromCharCode(10));
    
            this.context.textBaseline = "middle";
            this.context.fillStyle = this.colorToString(textColor);
            var minFontSize = height;
    
            $t = Bridge.getEnumerator(lines);
            while ($t.moveNext()) {
                var line = $t.getCurrent();
                this.context.font = height / lines.length + "px Segoe UI";
                this.context.textAlign = "center";
                if (this.context.measureText(line).width > width) {
                    minFontSize = Math.min(minFontSize, width / this.context.measureText(line).width * (height / lines.length));
                }
            }
    
            for (var index = 0; index < lines.length; index++) {
                this.context.font = minFontSize + "px Segoe UI";
                this.context.textAlign = "center";
                this.context.fillText(lines[index], Bridge.Int.trunc((x + width / 2)), Bridge.Int.trunc((y + (index + 0.5) * (height / lines.length))));
            }
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Frontend.JQueryGameAPI', {
        inherits: [ThreeOneSevenBee.Model.Game.IGameAPI],
        getCategories: function (callback) {
            $.get("/api/?action=get_levels&debug=1", { }, function (data, textStatus, request) {
                var $t, $t1;
                var jdata = JSON.parse(Bridge.cast(data, String));
                var categories = new Bridge.List$1(ThreeOneSevenBee.Model.Game.LevelCategory)();
                var categoriesData = Bridge.as(jdata.data, Array);
                console.log(jdata);
                $t = Bridge.getEnumerator(categoriesData);
                while ($t.moveNext()) {
                    var categoryData = $t.getCurrent();
    
                    var levelCategory = new ThreeOneSevenBee.Model.Game.LevelCategory(Bridge.cast(categoryData.name, String));
                    var levelsData = Bridge.as(categoryData.levels, Array);
                    $t1 = Bridge.getEnumerator(levelsData);
                    while ($t1.moveNext()) {
                        var levelData = $t1.getCurrent();
                        var level = new ThreeOneSevenBee.Model.Game.Level("constructor$1", Bridge.Int.parseInt(Bridge.cast(levelData.id, String), -2147483648, 2147483647), Bridge.cast(levelData.initial_expression, String), Bridge.cast(levelData.initial_expression, String), Bridge.Linq.Enumerable.from((Bridge.as(levelData.star_expressions, Array))).select($_.ThreeOneSevenBee.Frontend.JQueryGameAPI.f1).toArray());
                        levelCategory.add(level);
                    }
                    categories.add(levelCategory);
                }
    
                callback(categories);
            });
        },
        getCurrentPlayer: function (callback) {
            $.get("/api/?action=get_current_user&debug=1", { }, Bridge.fn.bind(this, function (data, textStatus, request) {
                var jdata = JSON.parse(Bridge.cast(data, String));
                var currentPlayer = new ThreeOneSevenBee.Model.Game.CurrentPlayer(Bridge.cast(jdata.data.name, String));
                this.getCategories(function (categories) {
                    var $t;
                    $t = Bridge.getEnumerator(categories);
                    while ($t.moveNext()) {
                        var category = $t.getCurrent();
                        currentPlayer.addCategory(category);
                    }
                    callback(currentPlayer);
                });
                callback(currentPlayer);
            }));
        },
        getPlayers: function (callback) {
            $.get("/api/?action=get_users", { }, function (data, textStatus, request) {
                var jdata = JSON.parse(Bridge.cast(data, String));
                var result = Bridge.Linq.Enumerable.from((Bridge.as(jdata.data, Array))).select($_.ThreeOneSevenBee.Frontend.JQueryGameAPI.f2).toList(ThreeOneSevenBee.Model.Game.Player);
                callback(result);
            });
        },
        saveUserLevelProgress: function (levelID, currentExpression, callback) {
            $.post("/api/", { action: "save_user_level_progress", debug: 1, level_id: levelID, current_expression: currentExpression }, function (data, textStatus, request) {
                var jdata = JSON.parse(Bridge.cast(data, String));
                callback(Bridge.cast(jdata.success, String) === "true");
            });
        }
    });
    
    Bridge.ns("ThreeOneSevenBee.Frontend.JQueryGameAPI", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Frontend.JQueryGameAPI, {
        f1: function (o) {
            return Bridge.cast(o, String);
        },
        f2: function (s) {
            return new ThreeOneSevenBee.Model.Game.Player(Bridge.cast(s.name, String));
        }
    });
    
    Bridge.init();
})(this);
