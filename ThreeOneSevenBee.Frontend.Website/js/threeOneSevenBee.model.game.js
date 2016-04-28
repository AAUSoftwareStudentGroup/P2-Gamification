(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Game.BadgeName', {
        statics: {
            brokBadge: 0,
            masterOfAlgebra: 1,
            potensBadge: 2,
            parenthesisBadge: 3,
            tutorialBadge: 4
        },
        $enum: true
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Game.Player', {
        badges: null,
        config: {
            properties: {
                PlayerName: null,
                LastLoginTime: null
            }
        },
        constructor: function (playername) {
            this.setPlayerName(playername);
            this.badges = new Bridge.List$1(ThreeOneSevenBee.Model.Game.BadgeName)();
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Game.Game', {
        gameAPI: null,
        context: null,
        gameModel: null,
        gameView: null,
        constructor: function (context, gameAPI) {
            this.gameAPI = gameAPI;
            this.context = context;
        },
        start: function () {
            this.gameAPI.isAuthenticated(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.Game.f1));
        },
        loadGameData: function () {
            this.gameAPI.getCurrentPlayer(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.Game.f9));
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Model.Game.Game", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.Game.Game, {
        f1: function (isAuthenticated) {
            if (isAuthenticated === false) {
                var loginView = new ThreeOneSevenBee.Model.UI.LoginView(this.context.getWidth(), this.context.getHeight());
                this.context.setContentView(loginView);
                loginView.onLogin = Bridge.fn.bind(this, function (username, password) {
                    this.gameAPI.authenticate(username, password, Bridge.fn.bind(this, function (authenticateSuccess) {
                        if (authenticateSuccess) {
                            this.loadGameData();
                        }
                        else  {
                            loginView.showLoginError();
                        }
                    }));
                });
            }
            else  {
                this.loadGameData();
            }
        },
        f2: function (IsSaved) {
            console.log(IsSaved ? "Level saved" : "Could not save");
        },
        f3: function (level) {
            this.gameAPI.saveUserLevelProgress(level.levelID, level.currentExpression, level.stars, $_.ThreeOneSevenBee.Model.Game.Game.f2);
        },
        f4: function (IsAdded) {
            console.log(IsAdded ? "Badge added" : "Badge not added");
        },
        f5: function (badge) {
            this.gameAPI.userAddBadge(badge, $_.ThreeOneSevenBee.Model.Game.Game.f4);
        },
        f6: function (success) {
            console.log(success ? "Logout success" : "Logout failed");
            this.start();
        },
        f7: function () {
            this.gameAPI.logout(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.Game.f6));
        },
        f8: function () {
            this.loadGameData();
        },
        f9: function (u) {
            this.gameAPI.getPlayers(Bridge.fn.bind(this, function (p) {
                this.gameModel = Bridge.merge(new ThreeOneSevenBee.Model.Game.GameModel(u, p), {
                    onSaveLevel: Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.Game.f3),
                    onBadgeAchieved: Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.Game.f5)
                } );
    
                this.gameView = Bridge.merge(new ThreeOneSevenBee.Model.UI.GameView(this.gameModel, this.context.getWidth(), this.context.getHeight()), {
                    setOnExit: Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.Game.f7),
                    setReloadGame: Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.Game.f8)
                } );
                this.context.setContentView(this.gameView);
            }));
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Game.GameModel', {
        onChanged: null,
        onBadgeAchieved: null,
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
            players.sort($_.ThreeOneSevenBee.Model.Game.GameModel.f1);
    
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
            var startValue = serializer.deserialize(this.getUser().categories.getItem(category).getItem(level).startExpression).getSize();
            var currentValue = serializer.deserialize(this.getUser().categories.getItem(category).getItem(level).currentExpression).getSize();
            this.progressBar = new ThreeOneSevenBee.Model.Game.ProgressbarStar(startValue, endValue, currentValue);
            this.setStarExpressions(new Bridge.List$1(ThreeOneSevenBee.Model.Expression.ExpressionBase)());
    
            $t = Bridge.getEnumerator(this.getUser().getCurrentLevel().starExpressions);
            while ($t.moveNext()) {
                var starExpression = $t.getCurrent();
                var starExpressionBase = serializer.deserialize(starExpression);
                this.getStarExpressions().add(starExpressionBase);
                this.progressBar.add(starExpressionBase.getSize());
            }
            this.setExprModel(new ThreeOneSevenBee.Model.Expression.ExpressionModel("constructor", this.getUser().categories.getItem(category).getItem(level).currentExpression, Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.GameModel.f2), [Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).exponentToProductRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).productToExponentRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).variableWithNegativeExponent, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).reverseVariableWithNegativeExponent, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).exponentProduct, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).commonPowerParenthesisRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).reverseCommonPowerParenthesisRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).splittingFractions, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).productParenthesis, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).reverseProductParenthesis, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).parenthesisPowerRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).fractionToProductRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).squareRootRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).removeParenthesisRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).productOfConstantAndFraction, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).factorizeUnaryMinus, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).factorizationRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).multiplyOneRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).addFractionWithCommonDenominatorRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).removeNull, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).multiplyByNull, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).calculateVariadicRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).calculateBinaryRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).multiplyMinusRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).divisionEqualsOneRule, Bridge.get(ThreeOneSevenBee.Model.Expression.ExpressionRules.Rules).productOfFractions]));
            this.onExpressionChanged(this.getExprModel());
        },
        restartLevel: function () {
            this.getUser().getCurrentLevel().currentExpression = this.getUser().getCurrentLevel().startExpression;
            this.setLevel(this.getUser().currentLevelIndex, this.getUser().currentCategoryIndex);
        },
        onExpressionChanged: function (model) {
            var $t;
            this.progressBar.currentValue = model.getExpression().getSize();
            this.getUser().getCurrentLevel().currentExpression = model.getExpression().toString();
            if (Bridge.Linq.Enumerable.from(this.progressBar.activatedStarPercentages()).count() > this.getUser().getCurrentLevel().stars) {
                this.getUser().getCurrentLevel().stars = Bridge.Linq.Enumerable.from(this.progressBar.activatedStarPercentages()).count();
                var numberOfStars = 0;
                $t = Bridge.getEnumerator(this.getUser().categories.getItem(this.getUser().currentCategoryIndex));
                while ($t.moveNext()) {
                    var level = $t.getCurrent();
                    numberOfStars += level.stars;
                }
                if (numberOfStars === this.getUser().categories.getItem(this.getUser().currentCategoryIndex).getCount() * 3) {
                    if (Bridge.hasValue(this.onBadgeAchieved)) {
                        var achievedBadge = this.getUser().categories.getItem(this.getUser().currentCategoryIndex).getBadge();
                        if (this.getUser().badges.contains(achievedBadge) === false) {
                            this.getUser().badges.add(achievedBadge);
                            Bridge.Linq.Enumerable.from(this.getPlayers()).first(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.GameModel.f3)).badges.add(achievedBadge);
                            this.onBadgeAchieved(achievedBadge);
                        }
                    }
                }
            }
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
    
    Bridge.ns("ThreeOneSevenBee.Model.Game.GameModel", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.Game.GameModel, {
        f1: function (p1, p2) {
            return Bridge.compare(p2.badges.getCount(), p1.badges.getCount());
        },
        f2: function (m) {
            this.onExpressionChanged(m);
        },
        f3: function (p) {
            return p.getPlayerName() === this.getUser().getPlayerName();
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Game.IGameAPI');
    
    Bridge.define('ThreeOneSevenBee.Model.Game.Level', {
        startExpression: null,
        starExpressions: null,
        currentExpression: null,
        description: null,
        levelID: 0,
        levelIndex: 0,
        categoryIndex: 0,
        stars: 0,
        constructor$2: function (startExpression, currentExpression, stars, description, starExpressions) {
            ThreeOneSevenBee.Model.Game.Level.prototype.$constructor.call(this, -1, -1, -1, startExpression, stars, currentExpression, description, starExpressions);
    
        },
        constructor$1: function (levelID, startExpression, stars, currentExpression, description, starExpressions) {
            ThreeOneSevenBee.Model.Game.Level.prototype.$constructor.call(this, levelID, -1, -1, startExpression, stars, currentExpression, description, starExpressions);
    
        },
        constructor: function (levelID, levelIndex, categoryIndex, startExpression, stars, currentExpression, description, starExpressions) {
            var $t;
            this.levelID = levelID;
            this.levelIndex = levelIndex;
            this.categoryIndex = categoryIndex;
            this.startExpression = startExpression;
            this.currentExpression = currentExpression;
            this.description = description;
            this.stars = stars;
            this.starExpressions = new Bridge.List$1(String)();
            $t = Bridge.getEnumerator(starExpressions);
            while ($t.moveNext()) {
                var star = $t.getCurrent();
                this.starExpressions.add(star);
            }
            for (var n = 0; n < 3 - Bridge.Linq.Enumerable.from(starExpressions).count(); n++) {
                this.starExpressions.add(Bridge.Linq.Enumerable.from(starExpressions).last());
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
            this.stars.add(star);
        },
        remove: function (star) {
            if (this.stars.contains(star)) {
                this.stars.remove(star);
            }
        },
        activatedStarPercentages: function () {
            return Bridge.Linq.Enumerable.from(this.stars).select(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.ProgressbarStar.f1)).where(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.ProgressbarStar.f2));
        },
        deactivatedStarPercentages: function () {
            return Bridge.Linq.Enumerable.from(this.stars).select(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.ProgressbarStar.f1)).where(Bridge.fn.bind(this, $_.ThreeOneSevenBee.Model.Game.ProgressbarStar.f3));
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
            return this.calculatePercentage(s);
        },
        f2: function (p) {
            return p <= this.getPercentage();
        },
        f3: function (p) {
            return p > this.getPercentage();
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
        statics: {
            config: {
                init: function () {
                    this.categoryBadges = Bridge.merge(new Bridge.Dictionary$2(String,ThreeOneSevenBee.Model.Game.BadgeName)(), [
        ["Tutorial", ThreeOneSevenBee.Model.Game.BadgeName.tutorialBadge],
        ["Potenser", ThreeOneSevenBee.Model.Game.BadgeName.potensBadge],
        ["Brøker", ThreeOneSevenBee.Model.Game.BadgeName.brokBadge],
        ["Master of Algebra", ThreeOneSevenBee.Model.Game.BadgeName.masterOfAlgebra],
        ["Parenteser", ThreeOneSevenBee.Model.Game.BadgeName.parenthesisBadge]
    ] ) || null;
                }
            }
        },
        name: null,
        categoryIndex: 0,
        levels: null,
        constructor: function (name) {
            this.name = name;
            this.levels = new Bridge.List$1(ThreeOneSevenBee.Model.Game.Level)();
        },
        getCompleted: function () {
            return Bridge.Linq.Enumerable.from(this.levels).all($_.ThreeOneSevenBee.Model.Game.LevelCategory.f1);
        },
        getBadge: function () {
            return Bridge.get(ThreeOneSevenBee.Model.Game.LevelCategory).categoryBadges.containsKey(this.name) ? Bridge.get(ThreeOneSevenBee.Model.Game.LevelCategory).categoryBadges.get(this.name) : Bridge.getDefaultValue(ThreeOneSevenBee.Model.Game.BadgeName);
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
    
    Bridge.ns("ThreeOneSevenBee.Model.Game.LevelCategory", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Model.Game.LevelCategory, {
        f1: function (l) {
            return l.stars === l.starExpressions.getCount();
        }
    });
    
    
    
    Bridge.init();
})(this);
