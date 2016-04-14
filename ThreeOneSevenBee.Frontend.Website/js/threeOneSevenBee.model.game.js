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
        constructor: function (user, players) {
            this.setUser(user);
            this.setPlayers(players);
            this.setLevel(this.getUser().currentLevel, this.getUser().currentCategory);
        },
        getCurrentExpression: function () {
            return this.getExprModel().getExpression();
        },
        getLevelCompleted: function () {
            return Bridge.Linq.Enumerable.from(this.progressBar.activatedStarPercentages()).count() >= 1;
        },
        getCategoryCompleted: function () {
            return this.getLevelCompleted() && this.getUser().currentLevel === this.getUser().categories.getItem(this.getUser().currentCategory).getCount() - 1;
        },
        getGameCompleted: function () {
            return this.getCategoryCompleted() && this.getUser().currentCategory === this.getUser().categories.getCount() - 1;
        },
        setLevel: function (level, category) {
            var $t;
            console.log(level + "; " + category);
            this.getUser().currentLevel = level;
            this.getUser().currentCategory = category;
            var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
            var endValue = serializer.deserialize(Bridge.Linq.Enumerable.from(this.getUser().categories.getItem(category).getItem(level).starExpressions).last()).getSize();
            var currentValue = serializer.deserialize(this.getUser().categories.getItem(category).getItem(level).startExpression).getSize();
            this.progressBar = new ThreeOneSevenBee.Model.Game.ProgressbarStar(currentValue, endValue, currentValue);
            this.setStarExpressions(new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)());
    
            $t = Bridge.getEnumerator(this.getUser().categories.getItem(this.getUser().currentCategory).getItem(this.getUser().currentLevel).starExpressions);
            while ($t.moveNext()) {
                var starExpression = $t.getCurrent();
                var starExpressionBase = serializer.deserialize(starExpression);
                this.getStarExpressions().add(starExpressionBase);
                this.progressBar.add(starExpressionBase.getSize());
            }
    
            this.setExprModel(new ThreeOneSevenBee.Model.Expression.ExpressionModel("constructor", this.getUser().categories.getItem(category).getItem(level).currentExpression, Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.GameModel.f1), [Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).exponentToProductRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).productToExponentRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).addFractionsWithSameNumerators, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).variableWithNegativeExponent, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).reverseVariableWithNegativeExponent, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).exponentProduct, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).numericBinaryRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).numericVariadicRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).commonPowerParenthesisRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).reverseCommonPowerParenthesisRule]));
    
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
            this.getUser().categories.getItem(this.getUser().currentCategory).getItem(this.getUser().currentLevel).currentExpression = this.getCurrentExpression().toString();
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
        levelIndex: 0,
        categoryIndex: 0,
        constructor$1: function (startExpression, currentExpression, starExpressions) {
            ThreeOneSevenBee.Model.Game.Level.prototype.$constructor.call(this, -1, -1, startExpression, currentExpression, starExpressions);
    
            if (starExpressions === void 0) { starExpressions = []; }
        },
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
        categories: null,
        constructor: function (player) {
            ThreeOneSevenBee.Model.Game.Player.prototype.$constructor.call(this, player);
    
            this.categories = new Bridge.List$1(ThreeOneSevenBee.Model.Game.LevelCategory)();
        },
        addCategory: function (category) {
            category.categoryIndex = this.categories.getCount();
    
            this.categories.add(category);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Game.LevelCategory', {
        inherits: [Bridge.IEnumerable$1(ThreeOneSevenBee.Model.Game.Level)],
        name: null,
        categoryIndex: 0,
        levels: null,
        constructor: function (name) {
            this.name = name;
            this.levels = new Bridge.List$1(ThreeOneSevenBee.Model.Game.Level)();
        },
        getItem: function (index) {
            return this.levels.getItem(index);
        },
        setItem: function (index, value) {
            this.levels.setItem(index, value);
        },
        getCount: function () {
            return this.levels.getCount();
        },
        add: function (level) {
            level.categoryIndex = this.categoryIndex;
            level.levelIndex = this.levels.getCount();
            this.levels.add(level);
        },
        getEnumerator$1: function () {
            return this.levels.getEnumerator();
        },
        getEnumerator: function () {
            return this.getEnumerator$1();
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Game.StubGameAPI', {
        inherits: [ThreeOneSevenBee.Model.Game.GameAPI],
        currentPlayer: null,
        constructor: function () {
            ThreeOneSevenBee.Model.Game.GameAPI.prototype.$constructor.call(this);
    
            this.currentPlayer = new ThreeOneSevenBee.Model.Game.CurrentPlayer("Morten");
    
            var numbers = new ThreeOneSevenBee.Model.Game.LevelCategory("Numbers");
            this.currentPlayer.addCategory(numbers);
            numbers.add(new ThreeOneSevenBee.Model.Game.Level("constructor$1", "4+(4+5)", "a*a", ["4+9", "13"]));
            numbers.add(new ThreeOneSevenBee.Model.Game.Level("constructor$1", "4*6+5", "4*6+5", ["24+5", "29"]));
    
    
            var variables = new ThreeOneSevenBee.Model.Game.LevelCategory("Variables");
            this.currentPlayer.addCategory(variables);
            variables.add(new ThreeOneSevenBee.Model.Game.Level("constructor$1", "a*a*a", "a*a*a", ["a^{2+1}", "a^3"]));
            variables.add(new ThreeOneSevenBee.Model.Game.Level("constructor$1", "a^2*a^3", "a^2*a^3", ["a^{2+3}", "a^5"]));
    
    
            //currentPlayer.AddCategory(new LevelCategory("Numbers") {
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //    });
            //currentPlayer.AddCategory(new LevelCategory("Variables") {
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("a^2*a*a*a*a*a", "a^2*a*a*a^3", "a^2*a^5", "a^7"),
            //                new Level("4+5*5", "4+5*5", "4+5^2", "4+25", "29"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("4+5*5", "4+10*5", "4+50", "54"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //                new Level("{a+b}/c+{c+d}/c", "{a+b}/c+{c+d}/c", "{a+b+c+d}/c"),
            //            });
            this.currentPlayer.badges = Bridge.merge(new Bridge.List$1(String)(), [
                ["FractionBadge"],
                ["ExponentBadge"]
            ] );
            this.currentPlayer.currentCategory = 0;
            this.currentPlayer.currentLevel = 0;
        },
        getReady: function () {
            return true;
        },
        getCurrentPlayer: function (callback) {
            callback(this.currentPlayer);
        },
        getPlayers: function (callback) {
            callback(Bridge.merge(new Bridge.List$1(ThreeOneSevenBee.Model.Game.Player)(), [
                [this.currentPlayer],
                [new ThreeOneSevenBee.Model.Game.Player("Anton")],
                [new ThreeOneSevenBee.Model.Game.Player("Christian")],
                [new ThreeOneSevenBee.Model.Game.Player("Lasse")],
                [new ThreeOneSevenBee.Model.Game.Player("Mathias P.")],
                [new ThreeOneSevenBee.Model.Game.Player("Mathias I.")],
                [new ThreeOneSevenBee.Model.Game.Player("Nikolaj")]
            ] ));
        },
        updateCurrentPlayer: function (currentPlayer) {
            this.currentPlayer = currentPlayer;
        }
    });
    
    
    
    Bridge.init();
})(this);
