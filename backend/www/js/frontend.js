(function (globals) {
    "use strict";

    Bridge.define('Frontend.App', {
        statics: {
            config: {
                init: function () {
                    Bridge.ready(this.main);
                }
            },
            main: function () {
                // Simple alert() to confirm it's working
                window.alert("Success");
            }
        }
    });
    
    Bridge.init();
})(this);
