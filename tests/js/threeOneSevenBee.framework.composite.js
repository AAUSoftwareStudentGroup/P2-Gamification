(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Framework.Composite.IContext');
    
    Bridge.define('ThreeOneSevenBee.Framework.Composite.IDrawable');
    
    Bridge.define('ThreeOneSevenBee.Framework.Composite.IControllable', {
        inherits: [ThreeOneSevenBee.Framework.Composite.IDrawable]
    });
    
    Bridge.define('ThreeOneSevenBee.Framework.Composite.View$1', function (T) { return {
        inherits: [ThreeOneSevenBee.Framework.Composite.IDrawable],
        _model: null,
        _context: null,
        config: {
            properties: {
                Width: 0,
                Height: 0,
                X: 0,
                Y: 0
            }
        },
        constructor: function (context, model) {
            this._model = model;
            this._context = context;
        }
    }; });
    
    Bridge.define('ThreeOneSevenBee.Framework.Composite.ViewContainer', {
        inherits: [ThreeOneSevenBee.Framework.Composite.View$1(Bridge.List$1(ThreeOneSevenBee.Framework.Composite.IControllable))],
        constructor: function (context) {
            ThreeOneSevenBee.Framework.Composite.View$1(Bridge.List$1(ThreeOneSevenBee.Framework.Composite.IControllable)).prototype.$constructor.call(this, context, new Bridge.List$1(ThreeOneSevenBee.Framework.Composite.IControllable)());
    
        },
        draw: function () {
            var $t;
            $t = Bridge.getEnumerator(this._model);
            while ($t.moveNext()) {
                var child = $t.getCurrent();
                child.draw();
            }
        },
        click: function (x, y) {
            var $t;
            $t = Bridge.getEnumerator(this._model);
            while ($t.moveNext()) {
                var child = $t.getCurrent();
                if (child.getX() <= x && x < child.getX() + child.getWidth() && child.getY() <= y && y < child.getY() + child.getHeight()) {
                    child.click(x, y);
                }
            }
        }
    });
    
    Bridge.init();
})(this);
