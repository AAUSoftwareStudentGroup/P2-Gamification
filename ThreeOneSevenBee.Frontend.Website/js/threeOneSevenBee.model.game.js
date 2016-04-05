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
        progressBar: null,
        config: {
            properties: {
                User: null,
                Players: null,
                ExprModel: null,
                StartExpression: null,
                StarExpressions: null
            }
        },
        constructor: function (api) {
            this.aPI = api;
            this.setUser(api.getCurrentPlayer());
            this.setPlayers(api.getPlayers());
            this.setLevel(this.getUser().currentLevel, this.getUser().currentCategory);
        },
        getCurrentExpression: function () {
            return this.getExprModel().getExpression();
        },
        getLevelCompleted: function () {
            return Bridge.Linq.Enumerable.from(this.progressBar.activatedStarPercentages()).count() >= 1;
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
            var endValue = serializer.deserialize(Bridge.Linq.Enumerable.from(this.getUser().categories.getItem(category).levels.getItem(level).starExpressions).last()).getSize();
            var currentValue = serializer.deserialize(this.getUser().categories.getItem(category).levels.getItem(level).startExpression).getSize();
            this.progressBar = new ThreeOneSevenBee.Model.Game.ProgressbarStar(currentValue, endValue, currentValue);
            this.setStarExpressions(new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)());
    
            $t = Bridge.getEnumerator(this.getUser().categories.getItem(this.getUser().currentCategory).levels.getItem(this.getUser().currentLevel).starExpressions);
            while ($t.moveNext()) {
                var starExpression = $t.getCurrent();
                var starExpressionBase = serializer.deserialize(starExpression);
                this.getStarExpressions().add(starExpressionBase);
                this.progressBar.add(starExpressionBase.getSize());
            }
    
            this.setExprModel(new ThreeOneSevenBee.Model.Expression.ExpressionModel("constructor", this.getUser().categories.getItem(category).levels.getItem(level).currentExpression, Bridge.fn.bind(this, this.onExpressionChanged), [Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).exponentToProductRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).productToExponentRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).addFractionsWithSameNumerators, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).variableWithNegativeExponent, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).reverseVariableWithNegativeExponent, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).exponentProduct, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).numericBinaryRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).numericVariadicRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).splittingFractions]));
    
            this.onExpressionChanged(this.getExprModel());
        },
        onExpressionChanged: function (model) {
            this.progressBar.currentValue = model.getExpression().getSize();
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
                    else  {
                        return;
                    }
                }
            }
            this.setLevel(this.getUser().currentLevel, this.getUser().currentCategory);
        },
        save: function () {
            this.getUser().categories.getItem(this.getUser().currentCategory).levels.getItem(this.getUser().currentLevel).currentExpression = this.getCurrentExpression().toString();
            this.aPI.updateCurrentPlayer(this.getUser());
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Game.Level', {
        startExpression: null,
        starExpressions: null,
        currentExpression: null,
        levelIndex: 0,
        categoryIndex: 0,
        constructor: function (levelIndex, categoryIndex, startExpression, currentExpression, starExpressions) {
            var $t;
            if (starExpressions === void 0) { starExpressions = []; }
            this.levelIndex = levelIndex;
            this.categoryIndex = categoryIndex;
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
        inherits: [Bridge.IEnumerable$1(Bridge.Int)],
        startValue: 0,
        endValue: 0,
        currentValue: 0,
        stars: null,
        constructor: function (startValue, endValue, currentValue, stars) {
            if (stars === void 0) { stars = []; }
            this.startValue = startValue;
            this.endValue = endValue;
            this.currentValue = currentValue;
            this.stars = new Bridge.List$1(Bridge.Int)(stars);
        },
        getPercentage: function () {
            return this.calculatePercentage(this.currentValue);
        },
        calculatePercentage: function (value) {
            return Bridge.cast((value - this.startValue), Number) / (this.endValue - this.startValue);
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
        activatedStarPercentages: function () {
            return Bridge.Linq.Enumerable.from(this.stars).where(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.ProgressbarStar.f1)).select(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.ProgressbarStar.f2));
        },
        deactivatedStarPercentages: function () {
            return Bridge.Linq.Enumerable.from(this.stars).where(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.ProgressbarStar.f3)).select(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.ProgressbarStar.f2));
        },
        getEnumerator$1: function () {
            return this.stars.getEnumerator();
        },
        getEnumerator: function () {
            return this.getEnumerator$1();
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Model.Game.ProgressbarStar", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.Game.ProgressbarStar, {
        f1: function (s) {
            return s >= this.currentValue;
        },
        f2: function (s) {
            return this.calculatePercentage(s);
        },
        f3: function (s) {
            return s < this.currentValue;
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
    
            this.currentPlayer = new ThreeOneSevenBee.Model.Game.CurrentPlayer("Morten");
            this.currentPlayer.badges = Bridge.merge(new Bridge.List$1(String)(), [
                ["FractionBadge"],
                ["ExponentBadge"]
            ] );
            this.currentPlayer.currentCategory = 0;
            this.currentPlayer.currentLevel = 0;
            this.currentPlayer.categories = Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.Game.LevelCategory)(), [
                [Bridge.merge(new ThreeOneSevenBee.Model.Game.LevelCategory("Numbers"), {
                    levels: Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.Game.Level)(), [
                        [new ThreeOneSevenBee.Model.Game.Level(0, 0, "a^2*a*a*a*a*a", "a^2*a*a*a^3", ["a^2*a^5", "a^7"])],
                        [new ThreeOneSevenBee.Model.Game.Level(1, 0, "4+5*5", "4+5*5", ["4+5^2", "4+25", "29"])],
                        [new ThreeOneSevenBee.Model.Game.Level(2, 0, "4+5*5", "4+10*5", ["4+50", "54"])],
                        [new ThreeOneSevenBee.Model.Game.Level(3, 0, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", ["{a+b+c+d}/c"])]
                    ] )
                } )],
                [Bridge.merge(new ThreeOneSevenBee.Model.Game.LevelCategory("Variables"), {
                    levels: Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.Game.Level)(), [
                        [new ThreeOneSevenBee.Model.Game.Level(0, 1, "a^2*a*a*a*a*a", "a^2*a*a*a^3", ["a^2*a^5", "a^7"])],
                        [new ThreeOneSevenBee.Model.Game.Level(1, 1, "4+5*5", "4+5*5", ["4+5^2", "4+25", "29"])],
                        [new ThreeOneSevenBee.Model.Game.Level(2, 1, "4+5*5", "4+10*5", ["4+50", "54"])],
                        [new ThreeOneSevenBee.Model.Game.Level(3, 1, "{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", ["{a+b+c+d}/c"])],
                        [new ThreeOneSevenBee.Model.Game.Level(4, 1, "a^2*a*a*a*a*a", "a^2*a*a*a^3", ["a^2*a^5", "a^7"])],
                        [new ThreeOneSevenBee.Model.Game.Level(5, 1, "4+5*5", "4+5*5", ["4+5^2", "4+25", "29"])],
                        [new ThreeOneSevenBee.Model.Game.Level(6, 1, "4+5*5", "4+10*5", ["4+50", "54"])],
                        [new ThreeOneSevenBee.Model.Game.Level(7, 1, "{a+b}/c+{c+d}/c+{e+f}/c", "{a+b}/c+{c+d}/c+{e+f}/c", ["{a+b+c+d}/c"])],
                        [new ThreeOneSevenBee.Model.Game.Level(8, 1, "a/c+b/c+d/c", "a/c+b/c+d/c", ["{a+b}/c"])],
                        [new ThreeOneSevenBee.Model.Game.Level(9, 1, "a/c+b/c-d/c-f/c", "a/c+b/c-d/c-f/c", ["{a+b}/c"])]
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
                [this.currentPlayer],
                [new ThreeOneSevenBee.Model.Game.Player("Anton")],
                [new ThreeOneSevenBee.Model.Game.Player("Christian")],
                [new ThreeOneSevenBee.Model.Game.Player("Lasse")],
                [new ThreeOneSevenBee.Model.Game.Player("Mathias P.")],
                [new ThreeOneSevenBee.Model.Game.Player("Mathias I.")],
                [new ThreeOneSevenBee.Model.Game.Player("Nikolaj")]
            ] );
        },
        updateCurrentPlayer: function (currentPlayer) {
            this.currentPlayer = currentPlayer;
        }
    });
    
    
    
    Bridge.init();
})(this);
