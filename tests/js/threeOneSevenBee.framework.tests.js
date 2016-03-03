(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Framework.Tests.App', {
        statics: {
            config: {
                init: function () {
                    Bridge.ready(this.main);
                }
            },
            main: function () {
                QUnit.module("Euclidean");
                QUnit.test("Constructor Vector2", $_.ThreeOneSevenBee.Framework.Tests.App.f1);
            }
        }
    });
    
    var $_ = {};
    
    Bridge.ns("ThreeOneSevenBee.Framework.Tests.App", $_)
    
    Bridge.apply($_.ThreeOneSevenBee.Framework.Tests.App, {
        f1: function (assert) {
            assert.expect(1);
    
            var vectorZero = new ThreeOneSevenBee.Framework.Euclidean.Vector2("constructor$1", 0, 0);
            assert.ok(Math.abs(vectorZero.x) < 4.94065645841247E-324 && Math.abs(vectorZero.y) < 4.94065645841247E-324, "Vector2 Zero");
        }
    });
    
    Bridge.init();
})(this);
