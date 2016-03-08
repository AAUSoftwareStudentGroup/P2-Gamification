(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Collections.Queue$1', function (T) { return {
        inherits: [Bridge.IEnumerable$1(T)],
        enqueue: function (value) {
            throw new Bridge.NotImplementedException();
        },
        dequeue: function () {
            throw new Bridge.NotImplementedException();
        },
        getEnumerator$1: function () {
            throw new Bridge.NotImplementedException();
        },
        getEnumerator: function () {
            throw new Bridge.NotImplementedException();
        }
    }; });
    
    Bridge.define('ThreeOneSevenBee.Model.Collections.Stack$1', function (T) { return {
        inherits: [Bridge.IEnumerable$1(T)],
        push: function (value) {
            throw new Bridge.NotImplementedException();
        },
        peek: function () {
            throw new Bridge.NotImplementedException();
        },
        pop: function () {
            throw new Bridge.NotImplementedException();
        },
        getEnumerator$1: function () {
            throw new Bridge.NotImplementedException();
        },
        getEnumerator: function () {
            throw new Bridge.NotImplementedException();
        }
    }; });
    
    
    
    Bridge.init();
})(this);
