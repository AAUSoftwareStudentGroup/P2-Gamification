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
                var t = new ThreeOneSevenBee.Model.Template();
                console.log(t.toString());
            }
        }
    });
    
    Bridge.init();
})(this);
