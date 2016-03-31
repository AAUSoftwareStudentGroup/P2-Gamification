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
        onChanged: null,
        progress: null,
        config: {
            properties: {
                User: null,
                ExprModel: null,
                StartExpression: null,
                StarExpressions: null
            }
        },
        constructor$1: function (api, onChanged) {
            this.onChanged = onChanged;
            this.aPI = api;
            this.setUser(api.getCurrentPlayer());
            this.setLevel(this.getUser().currentLevel, this.getUser().currentCategory);
        },
        constructor: function (api) {
            ThreeOneSevenBee.Model.Game.GameModel.prototype.constructor$1.call(this, api, null);
    
        },
        getCurrentExpression: function () {
            return this.getExprModel().getExpression();
        },
        getLevelCompleted: function () {
            return this.progress.getStars() > 0;
        },
        getCategoryCompleted: function () {
            return this.getLevelCompleted() && this.getUser().currentLevel === this.getUser().categories.getItem(this.getUser().currentCategory).levels.getCount() - 1;
        },
        getGameCompleted: function () {
            return this.getCategoryCompleted() && this.getUser().currentCategory === this.getUser().categories.getCount() - 1;
        },
        setLevel: function (level, category) {
            var $t;
            this.getUser().currentLevel = level;
            this.getUser().currentCategory = category;
            var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
            this.progress = new ThreeOneSevenBee.Model.Game.ProgressbarStar(serializer.deserialize(Bridge.Linq.Enumerable.from(this.getUser().categories.getItem(category).levels.getItem(level).starExpressions).last()).getSize(), serializer.deserialize(this.getUser().categories.getItem(category).levels.getItem(level).startExpression).getSize());
            this.setStarExpressions(new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)());
            $t = Bridge.getEnumerator(this.getUser().categories.getItem(this.getUser().currentCategory).levels.getItem(this.getUser().currentLevel).starExpressions);
            while ($t.moveNext()) {
                var starExpression = $t.getCurrent();
                var starExpressionBase = serializer.deserialize(starExpression);
                this.getStarExpressions().add(starExpressionBase);
                this.progress.add(starExpressionBase.getSize());
            }
            this.setExprModel(new ThreeOneSevenBee.Model.Expression.ExpressionModel(this.getUser().categories.getItem(category).levels.getItem(level).currentExpression, Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.GameModel.f1), [Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).exponentToProductRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).productToExponentRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).addFractionsWithSameNumerators, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).variableWithNegativeExponent, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).reverseVariableWithNegativeExponent, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).exponentProduct]));
            this.onExpressionChanged(this.getExprModel());
        },
        onExpressionChanged: function (model) {
            this.progress.setProgress(model.getExpression().getSize());
            if (Bridge.hasValue(this.onChanged)) {
                this.onChanged(this);
            }
        },
        nextLevel: function () {
            if (this.getGameCompleted()) {
    
            }
            else  {
                if (this.getCategoryCompleted()) {
                    this.getUser().currentCategory++;
                    this.getUser().currentLevel = 0;
    
                }
                else  {
                    if (this.getLevelCompleted()) {
                        this.getUser().currentLevel++;
                    }
                }
            }
            this.setLevel(this.getUser().currentLevel, this.getUser().currentCategory);
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
            this.starExpressions = new Bridge.List$1(String)();
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
        maxProgress: 0,
        currentProgress: 0,
        stars: null,
        constructor: function (progress, maxValue, stars) {
            if (stars === void 0) { stars = []; }
            this.currentProgress = progress;
            this.maxProgress = maxValue;
            this.stars = new Bridge.List$1(Bridge.Int)(stars);
            this.getStars();
        },
        getProgress: function () {
            return this.currentProgress;
        },
        setProgress: function (value) {
            this.currentProgress = value;
        },
        getPercentage: function () {
            return Bridge.cast(this.currentProgress, Number) / this.maxProgress;
        },
        getMaxProgress: function () {
            return this.maxProgress;
        },
        setMaxProgress: function (value) {
            this.maxProgress = value;
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
                if (i <= this.currentProgress) {
                    starsCount++;
                }
            }
            // Returns amount of reached stars.
            return starsCount;
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Game.CurrentPlayer', {
        inherits: [ThreeOneSevenBee.Model.Game.Player],
        currentCategory: 0,
        currentLevel: 0,
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
            this.currentPlayer.currentCategory = 0;
            this.currentPlayer.currentLevel = 0;
            this.currentPlayer.categories = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.Game.LevelCategory)(), [
                [Bridge.merge(new ThreeOneSevenBee.Model.Game.LevelCategory("Numbers"), {
                    levels: Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.Game.Level)(), [
                        [new ThreeOneSevenBee.Model.Game.Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", ["a^2*a^5", "a^7"])],
                        [new ThreeOneSevenBee.Model.Game.Level("4+5*5", "4+5*5", ["4+25", "25"])],
                        [new ThreeOneSevenBee.Model.Game.Level("4+5*5", "4+10*5", ["4+50", "54"])]
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
