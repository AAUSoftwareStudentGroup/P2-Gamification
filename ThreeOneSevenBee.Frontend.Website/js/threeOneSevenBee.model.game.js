(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Game.Player', {
        config: {
            properties: {
                PlayerName: null,
                LastLoginTime: null
            },
            init: function () {
                this.badges = new Bridge.List$1(String)() || null;
            }
        },
        constructor: function (playername) {
            this.setPlayerName(playername);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Game.GameAPI');
    
    Bridge.define('ThreeOneSevenBee.Model.Game.GameModel', {
        aPI: null,
        progress: null,
        currentCategory: 0,
        currentLevel: 0,
        config: {
            events: {
                OnChanged: null
            },
            properties: {
                Player: null,
                ExprModel: null,
                StartExpression: null,
                StarExpressions: null
            }
        },
        constructor: function (api) {
            this.aPI = api;
            this.setPlayer(api.getCurrentPlayer());
            this.currentLevel = -1;
            this.currentCategory = -1;
        },
        getCurrentExpression: function () {
            return this.getExprModel().getExpression();
        },
        getLevelCompleted: function () {
            if (this.currentLevel === -1) {
                return false;
            }
            return this.progress.getStars() > 0;
        },
        getCategoryCompleted: function () {
            if (this.currentCategory === -1) {
                return false;
            }
            return this.getLevelCompleted() && this.currentLevel === this.getPlayer().categories.getItem(this.currentCategory).levels.getCount() - 1;
        },
        getGameCompleted: function () {
            return this.getCategoryCompleted() && this.currentCategory === this.getPlayer().categories.getCount() - 1;
        },
        setLevel: function (level, category) {
            var $t;
            this.currentLevel = level;
            this.currentCategory = category;
            this.setExprModel(new ThreeOneSevenBee.Model.Expression.ExpressionModel(this.getPlayer().categories.getItem(category).levels.getItem(level).currentExpression, Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.GameModel.f1)));
            this.progress = new ThreeOneSevenBee.Model.Game.ProgressbarStar(this.getExprModel().getExpression().getSize(), this.getExprModel().getExpression().getSize());
            var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
            $t = Bridge.getEnumerator(this.getPlayer().categories.getItem(category).levels.getItem(level).starExpressions);
            while ($t.moveNext()) {
                var starExpression = $t.getCurrent();
                var starExpressionBase = serializer.deserialize(starExpression);
                this.getStarExpressions().add(starExpressionBase);
                this.progress.add(starExpressionBase.getSize());
            }
        },
        onExpressionChanged: function (model) {
            this.progress.setProgress(model.getExpression().getSize());
            this.OnChanged(this);
        },
        nextLevel: function () {
            if (this.getGameCompleted()) {
    
            }
            else  {
                if (this.getCategoryCompleted()) {
                    this.currentCategory++;
                    this.currentLevel = 0;
    
                }
                else  {
                    if (this.getLevelCompleted()) {
                        this.currentLevel++;
                    }
                }
            }
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Model.Game.GameModel", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.Game.GameModel, {
        f1: function (m) {
            this.onExpressionChanged(m);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Game.Level', {
        startExpression: null,
        starExpressions: null,
        currentExpression: null,
        constructor: function (startExpression, currentExpression, starExpressions) {
            var $t;
            if (starExpressions === void 0) { starExpressions = []; }
            this.startExpression = startExpression;
            this.currentExpression = currentExpression;
            $t = Bridge.getEnumerator(starExpressions);
            while ($t.moveNext()) {
                var star = $t.getCurrent();
                this.starExpressions.add(star);
            }
    }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Game.LevelCategory', {
        name: null,
        levels: null,
        constructor: function (name) {
            this.name = name;
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Game.ProgressbarStar', {
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
    
    Bridge.define('ThreeOneSevenBee.Model.Game.CurrentPlayer', {
        inherits: [ThreeOneSevenBee.Model.Game.Player],
        config: {
            init: function () {
                this.categories = new Bridge.List$1(ThreeOneSevenBee.Model.Game.LevelCategory)() || null;
            }
        },
        constructor: function (player) {
            ThreeOneSevenBee.Model.Game.Player.prototype.$constructor.call(this, player);
    
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Game.StubGameAPI', {
        inherits: [ThreeOneSevenBee.Model.Game.GameAPI],
        currentPlayer: null,
        constructor: function () {
            ThreeOneSevenBee.Model.Game.GameAPI.prototype.$constructor.call(this);
    
            this.currentPlayer = new ThreeOneSevenBee.Model.Game.CurrentPlayer("morten");
            this.currentPlayer.badges = Bridge.merge(new Bridge.List$1(String)(), [
                ["FractionBadge"],
                ["ExponentBadge"]
            ] );
            this.currentPlayer.categories = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.Game.LevelCategory)(), [
                [Bridge.merge(new ThreeOneSevenBee.Model.Game.LevelCategory("Numbers"), {
                    levels: Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.Game.Level)(), [
                        [new ThreeOneSevenBee.Model.Game.Level("5*5*5", "5*5*5", ["5^2*5", "5^3", "125"])]
                    ] )
                } )]
            ] );
        },
        getReady: function () {
            return true;
        },
        getCurrentPlayer: function () {
            return this.currentPlayer;
        },
        getPlayers: function () {
            return Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.Game.Player)(), [
                [this.currentPlayer]
            ] );
        },
        updateCurrentPlayer: function (currentPlayer) {
            this.currentPlayer = currentPlayer;
        }
    });
    
    
    
    Bridge.init();
})(this);
