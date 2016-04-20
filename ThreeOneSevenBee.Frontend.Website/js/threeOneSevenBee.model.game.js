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
    
    Bridge.define('ThreeOneSevenBee.Model.Game.GameModel', {
        onChanged: null,
        onSaveLevel: null,
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
            this.setLevel(this.getUser().currentLevelIndex, this.getUser().currentCategoryIndex);
        },
        getCurrentExpression: function () {
            return this.getExprModel().getExpression();
        },
        getIsFirstLevel: function () {
            return this.getUser().currentCategoryIndex === 0 && this.getUser().currentLevelIndex === 0;
        },
        getIsLevelCompleted: function () {
            return Bridge.Linq.Enumerable.from(this.progressBar.activatedStarPercentages()).count() >= 1;
        },
        getIsCategoryCompleted: function () {
            return this.getIsLevelCompleted() && this.getUser().currentLevelIndex === this.getUser().categories.getItem(this.getUser().currentCategoryIndex).getCount() - 1;
        },
        getIsGameCompleted: function () {
            return this.getIsCategoryCompleted() && this.getUser().currentCategoryIndex === this.getUser().categories.getCount() - 1;
        },
        setLevel: function (level, category) {
            var $t;
            this.getUser().currentLevelIndex = level;
            this.getUser().currentCategoryIndex = category;
            var serializer = new ThreeOneSevenBee.Model.Expression.ExpressionSerializer();
            var endValue = serializer.deserialize(Bridge.Linq.Enumerable.from(this.getUser().categories.getItem(category).getItem(level).starExpressions).last()).getSize();
            var currentValue = serializer.deserialize(this.getUser().categories.getItem(category).getItem(level).startExpression).getSize();
            this.progressBar = new ThreeOneSevenBee.Model.Game.ProgressbarStar(currentValue, endValue, currentValue);
            this.setStarExpressions(new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)());
    
            $t = Bridge.getEnumerator(this.getUser().categories.getItem(this.getUser().currentCategoryIndex).getItem(this.getUser().currentLevelIndex).starExpressions);
            while ($t.moveNext()) {
                var starExpression = $t.getCurrent();
                var starExpressionBase = serializer.deserialize(starExpression);
                this.getStarExpressions().add(starExpressionBase);
                this.progressBar.add(starExpressionBase.getSize());
            }
            this.setExprModel(new ThreeOneSevenBee.Model.Expression.ExpressionModel("constructor", this.getUser().categories.getItem(category).getItem(level).currentExpression, Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.GameModel.f1), [Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).exponentToProductRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).productToExponentRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).addFractionsWithSameNumerators, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).variableWithNegativeExponent, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).reverseVariableWithNegativeExponent, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).exponentProduct, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).numericBinaryRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).numericVariadicRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).commonPowerParenthesisRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).reverseCommonPowerParenthesisRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).splittingFractions, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).productParenthesis, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).reverseProductParenthesis, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).parenthesisPowerRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).fractionToProductRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).squareRootRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).removeParenthesisRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).productOfConstantAndFraction]));
            this.onExpressionChanged(this.getExprModel());
        },
        onExpressionChanged: function (model) {
            this.progressBar.currentValue = model.getExpression().getSize();
            this.getUser().getCurrentLevel().currentExpression = model.getExpression().toString();
            if (Bridge.hasValue(this.onChanged)) {
                this.onChanged(this);
            }
        },
        nextLevel: function () {
            if (this.getIsGameCompleted()) {
    
            }
            else  {
                if (this.getIsCategoryCompleted()) {
                    this.getUser().currentCategoryIndex++;
                    this.getUser().currentLevelIndex = 0;
    
                }
                else  {
                    if (this.getIsLevelCompleted()) {
                        this.getUser().currentLevelIndex++;
                    }
                    else  {
                        return;
                    }
                }
            }
            this.setLevel(this.getUser().currentLevelIndex, this.getUser().currentCategoryIndex);
        },
        saveLevel: function () {
            if (Bridge.hasValue(this.onSaveLevel)) {
                this.onSaveLevel(this.getUser().getCurrentLevel());
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
    
    Bridge.define('ThreeOneSevenBee.Model.Game.IGameAPI');
    
    Bridge.define('ThreeOneSevenBee.Model.Game.Level', {
        startExpression: null,
        starExpressions: null,
        currentExpression: null,
        levelID: 0,
        levelIndex: 0,
        categoryIndex: 0,
        constructor$2: function (startExpression, currentExpression, starExpressions) {
            ThreeOneSevenBee.Model.Game.Level.prototype.$constructor.call(this, -1, -1, -1, startExpression, currentExpression, starExpressions);
    
        },
        constructor$1: function (levelID, startExpression, currentExpression, starExpressions) {
            ThreeOneSevenBee.Model.Game.Level.prototype.$constructor.call(this, levelID, -1, -1, startExpression, currentExpression, starExpressions);
    
        },
        constructor: function (levelID, levelIndex, categoryIndex, startExpression, currentExpression, starExpressions) {
            var $t;
            if (starExpressions === void 0) { starExpressions = []; }
            this.levelID = levelID;
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
        currentCategoryIndex: 0,
        currentLevelIndex: 0,
        categories: null,
        constructor: function (player) {
            ThreeOneSevenBee.Model.Game.Player.prototype.$constructor.call(this, player);
    
            this.categories = new Bridge.List$1(ThreeOneSevenBee.Model.Game.LevelCategory)();
        },
        getCurrentLevel: function () {
            return this.categories.getItem(this.currentCategoryIndex).getItem(this.currentLevelIndex);
        },
        addCategory: function (category) {
            category.setCategoryIndex(this.categories.getCount());
    
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
        getCategoryIndex: function () {
            return this.categoryIndex;
        },
        setCategoryIndex: function (value) {
            var $t;
            this.categoryIndex = value;
            $t = Bridge.getEnumerator(this.levels);
            while ($t.moveNext()) {
                var level = $t.getCurrent();
                level.categoryIndex = this.categoryIndex;
            }
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
            level.categoryIndex = this.getCategoryIndex();
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
    
    
    
    Bridge.init();
})(this);
