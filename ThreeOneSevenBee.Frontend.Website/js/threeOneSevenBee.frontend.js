﻿(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Frontend.App', {
        statics: {
            config: {
                init: function () {
                    Bridge.ready(this.main);
                }
            },
            main: function () {
                document.addEventListener("touchmove", $_.ThreeOneSevenBee.Frontend.App.f1);
    
    
                var canvas = document.getElementById("canvas");
                canvas.width = document.documentElement.clientWidth;
                canvas.height = document.documentElement.clientHeight;
                var input = document.getElementById("input");
    
                var context = new ThreeOneSevenBee.Frontend.CanvasContext(canvas, input);
    
                var gameAPI = new ThreeOneSevenBee.Frontend.JQueryGameAPI();
    
                var game = new ThreeOneSevenBee.Model.Game.Game(context, gameAPI);
    
                game.start();
            }
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Frontend.App", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Frontend.App, {
        f1: function (e) {
            e.preventDefault();
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Frontend.CanvasContext', {
        inherits: [ThreeOneSevenBee.Model.UI.Context],
        imageCache: null,
        context: null,
        input: null,
        cleared: false,
        constructor: function (canvas, input) {
            ThreeOneSevenBee.Model.UI.Context.prototype.$constructor.call(this, canvas.width, canvas.height);
    
            this.imageCache = new Bridge.Dictionary$2(String,HTMLImageElement)();
    
            this.input = input;
            input.type = "text";
            input.value = "A";
            input.oninput = Bridge.fn.bind(this, function (e) {
                if (input.value === "") {
                    this.keyPressed("Back");
                }
                this.keyPressed(input.value.substr(Math.max(0, input.value.length - 1), input.value.length));
                input.value = "A";
            });
    
            this.context = canvas.getContext("2d");
            this.context.fillStyle = "#000000";
            this.context.lineWidth = 2;
            this.context.textBaseline = "middle";
    
            var canvasLeft = this.context.canvas.getBoundingClientRect().left;
            var canvasRight = this.context.canvas.getBoundingClientRect().left;
    
            this.context.canvas.addEventListener("click", Bridge.fn.bind(this, function (e) {
                this.click(e.clientX + document.body.scrollLeft - Bridge.Int.trunc(canvasLeft), e.clientY + document.body.scrollTop - Bridge.Int.trunc(canvasRight));
                if (this.getContentView$1().getActive() === true) {
                    input.focus();
                }
            }));
    
            window.onresize = Bridge.fn.bind(this, $_.ThreeOneSevenBee.Frontend.CanvasContext.f1);
        },
        resizeContent: function () {
            this.context.canvas.width = document.documentElement.clientWidth;
            this.context.canvas.height = document.documentElement.clientHeight;
            this.setWidth(this.context.canvas.width);
            this.setHeight(this.context.canvas.height);
            this.getContentView$1().setWidth(this.getWidth());
            this.getContentView$1().setHeight(this.getHeight());
            this.getContentView$1().update();
            this.draw();
        },
        colorToString: function (color) {
            return Bridge.String.format("rgba({0},{1},{2},{3})", Bridge.Int.format(color.red, 'G'), Bridge.Int.format(color.green, 'G'), Bridge.Int.format(color.blue, 'G'), Bridge.Int.format(color.alpha, 'G'));
        },
        setContentView: function (view) {
            ThreeOneSevenBee.Model.UI.Context.prototype.setContentView.call(this, view);
            this.draw();
        },
        clear: function () {
            this.cleared = true;
            this.context.clearRect(0, 0, Bridge.Int.trunc(this.getWidth()), Bridge.Int.trunc(this.getHeight()));
        },
        click: function (x, y) {
            this.getContentView$1().click(x, y, this);
            this.draw();
        },
        keyPressed: function (text) {
            this.getContentView$1().keyPressed(text, this);
            this.draw();
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
        drawText: function (x, y, width, height, text, textColor, alignment) {
            var $t;
            var lines = text.split(String.fromCharCode(10));
            this.context.textBaseline = "middle";
            this.context.fillStyle = this.colorToString(textColor);
            var minFontSize = height / lines.length;
    
            $t = Bridge.getEnumerator(lines);
            while ($t.moveNext()) {
                var line = $t.getCurrent();
                this.context.font = height / lines.length + "px Arial";
                this.context.textAlign = alignment === ThreeOneSevenBee.Model.UI.TextAlignment.centered ? "center" : alignment === ThreeOneSevenBee.Model.UI.TextAlignment.left ? "left" : "right";
                if (this.context.measureText(line).width > width) {
                    minFontSize = Math.min(minFontSize, width / this.context.measureText(line).width * (height / lines.length));
                }
            }
    
            for (var index = 0; index < lines.length; index++) {
                this.context.font = minFontSize + "px Arial";
                this.context.textAlign = alignment === ThreeOneSevenBee.Model.UI.TextAlignment.centered ? "center" : alignment === ThreeOneSevenBee.Model.UI.TextAlignment.left ? "left" : "right";
                this.context.fillText(lines[index], Bridge.Int.trunc((x + (alignment === ThreeOneSevenBee.Model.UI.TextAlignment.centered ? width / 2 : 0))), Bridge.Int.trunc((y + (index + 0.5) * (height / lines.length))));
            }
        },
        drawPNGImage: function (fileName, x, y, width, height) {
    
            if (this.imageCache.containsKey(fileName)) {
                this.context.fillStyle = "transparent";
                this.context.drawImage(this.imageCache.get(fileName), x, y, width, height);
                this.context.fillStyle = "#000000";
            }
            else  {
                var img = new Image();
                this.cleared = false;
                img.onload = Bridge.fn.bind(this, function (e) {
                    if (this.cleared === false) {
                        this.context.fillStyle = "transparent";
                        this.context.drawImage(img, x, y, width, height);
                        this.context.fillStyle = "#000000";
                    }
                    this.imageCache.set(fileName, img);
                });
                img.src = "img/" + fileName;
            }
        },
        getTextDimensions: function (text, maxWidth, maxHeight) {
            var minFontSize = maxHeight;
            this.context.font = maxHeight + "px Arial";
            this.context.textAlign = "left";
            this.context.font = minFontSize + "px Arial";
            if (this.context.measureText(text).width > maxWidth) {
                minFontSize = Math.min(minFontSize, maxWidth / this.context.measureText(text).width * maxHeight);
            }
            return new ThreeOneSevenBee.Model.Euclidean.Vector2("constructor$1", this.context.measureText(text).width, minFontSize);
        }
    });
    
    Bridge.ns("ThreeOneSevenBee.Frontend.CanvasContext", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Frontend.CanvasContext, {
        f1: function (e) {
            this.resizeContent();
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Frontend.JQueryGameAPI', {
        inherits: [ThreeOneSevenBee.Model.Game.IGameAPI],
        token: null,
        getCategories: function (callback) {
            $.post("/api/", { action: "get_levels", token: this.token }, function (data, textStatus, request) {
                var $t, $t1, $t2;
                var jdata = JSON.parse(Bridge.cast(data, String));
                var categories = new Bridge.List$1(ThreeOneSevenBee.Model.Game.LevelCategory)();
                var categoriesData = Bridge.as(jdata.data, Array);
                $t = Bridge.getEnumerator(categoriesData);
                while ($t.moveNext()) {
                    var categoryData = $t.getCurrent();
    
                    var levelCategory = new ThreeOneSevenBee.Model.Game.LevelCategory(Bridge.cast(categoryData.name, String));
                    var levelsData = Bridge.as(categoryData.levels, Array);
    
                    $t1 = Bridge.getEnumerator(levelsData);
                    while ($t1.moveNext()) {
                        var levelData = $t1.getCurrent();
                        var level = new ThreeOneSevenBee.Model.Game.Level("constructor$1", Bridge.Int.parseInt(Bridge.cast(levelData.id, String), -2147483648, 2147483647), Bridge.cast(levelData.initial_expression, String), Bridge.Int.parseInt(($t2 = Bridge.cast(levelData.stars, String), Bridge.hasValue($t2) ? $t2 : "0"), -2147483648, 2147483647), Bridge.cast(levelData.current_expression, String), Bridge.Linq.Enumerable.from((Bridge.as(levelData.star_expressions, Array))).select($_.ThreeOneSevenBee.Frontend.JQueryGameAPI.f1).toArray());
                        levelCategory.add(level);
                    }
                    categories.add(levelCategory);
                }
    
                callback(categories);
            });
        },
        getCurrentPlayer: function (callback) {
            $.post("/api/", { action: "get_current_user", token: this.token }, Bridge.fn.bind(this, function (data, textStatus, request) {
                var jdata = JSON.parse(Bridge.cast(data, String));
                var currentPlayer = new ThreeOneSevenBee.Model.Game.CurrentPlayer(Bridge.cast(jdata.data.name, String));
                currentPlayer.badges = Bridge.Linq.Enumerable.from((Bridge.cast(jdata.data.badges, Array))).where($_.ThreeOneSevenBee.Frontend.JQueryGameAPI.f2).select($_.ThreeOneSevenBee.Frontend.JQueryGameAPI.f3).toList(ThreeOneSevenBee.Model.Game.BadgeName);
                this.getCategories(function (categories) {
                    var $t;
                    $t = Bridge.getEnumerator(categories);
                    while ($t.moveNext()) {
                        var category = $t.getCurrent();
                        currentPlayer.addCategory(category);
                    }
                    callback(currentPlayer);
                });
            }));
        },
        getPlayers: function (callback) {
            $.post("/api/", { action: "get_users", token: this.token }, function (data, textStatus, request) {
                var jdata = JSON.parse(Bridge.cast(data, String));
                var result = Bridge.Linq.Enumerable.from((Bridge.as(jdata.data, Array))).select($_.ThreeOneSevenBee.Frontend.JQueryGameAPI.f4).toList(ThreeOneSevenBee.Model.Game.Player);
                callback(result);
            });
        },
        saveUserLevelProgress: function (levelID, currentExpression, stars, callback) {
            $.post("/api/", { action: "save_user_level_progress", level_id: levelID, current_expression: currentExpression, stars: stars, token: this.token }, function (data, textStatus, request) {
                var jdata = JSON.parse(Bridge.cast(data, String));
                callback(Bridge.cast(jdata.success, String) === "true");
            });
        },
        isAuthenticated: function (callback) {
            $.get("/api/?action=is_authenticated", { }, Bridge.fn.bind(this, function (data, textStatus, request) {
                var jdata = JSON.parse(Bridge.cast(data, String));
                var success = Bridge.cast(jdata.success, String) === "true";
                this.token = Bridge.cast(jdata.success, String) === "true" ? Bridge.cast(jdata.data.token, String) : null;
                callback(success);
            }));
        },
        authenticate: function (username, password, callback) {
            $.post("/api/", { action: "user_login", username: username, password: password }, Bridge.fn.bind(this, function (data, textStatus, request) {
                var jdata = JSON.parse(Bridge.cast(data, String));
                var success = Bridge.cast(jdata.success, String) === "true";
                this.token = Bridge.cast(jdata.success, String) === "true" ? Bridge.cast(jdata.data.token, String) : null;
                callback(success);
            }));
        },
        userAddBadge: function (badge, callback) {
            $.post("/api/", { action: "user_add_badge", token: this.token, badge_id: Bridge.cast(badge, Bridge.Int) }, function (data, textStatus, request) {
                var jdata = JSON.parse(Bridge.cast(data, String));
                callback(Bridge.cast(jdata.success, String) === "true");
            });
        },
        logout: function (callback) {
            $.post("/api/", { action: "user_logout", token: this.token }, function (data, textStatus, request) {
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
        f2: function (b) {
            return b !== "";
        },
        f3: function (b) {
            return Bridge.Int.parseInt(b, -2147483648, 2147483647);
        },
        f4: function (s) {
            return Bridge.merge(new ThreeOneSevenBee.Model.Game.Player(Bridge.cast(s.name, String)), {
                badges: Bridge.Linq.Enumerable.from((Bridge.cast(s.badges, Array))).where($_.ThreeOneSevenBee.Frontend.JQueryGameAPI.f2).select($_.ThreeOneSevenBee.Frontend.JQueryGameAPI.f3).toList(ThreeOneSevenBee.Model.Game.BadgeName)
            } );
        }
    });
    
    Bridge.init();
})(this);
