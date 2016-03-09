(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Collections.Queue$1', function (T) { return {
        inherits: [Bridge.IEnumerable$1(T)],
        list: null,
        constructor: function () {
            this.list = new Bridge.List$1(T)();
        },
        enqueue: function (item) {
            this.list.add(item);
        },
        dequeue: function () {
            if (this.list.getCount() === 0) {
                throw new Bridge.InvalidOperationException("The Queue is empty.");
            }
            var item = this.list.getItem(0);
            this.list.removeAt(0);
            return item;
        },
        getEnumerator$1: function () {
            return this.list.getEnumerator();
        },
        getEnumerator: function () {
            return this.list.getEnumerator();
        }
    }; });
    
    Bridge.define('ThreeOneSevenBee.Model.Collections.Stack$1', function (T) { return {
        inherits: [Bridge.IEnumerable$1(T)],
        list: null,
        constructor: function () {
            this.list = new Bridge.List$1(T)();
        },
        push: function (item) {
            this.list.add(item);
        },
        peek: function () {
            if (this.list.getCount() === 0) {
                throw new Bridge.InvalidOperationException("The Queue is empty.");
            }
    
            return this.list.getItem(this.list.getCount() - 1);
        },
        pop: function () {
            if (this.list.getCount() === 0) {
                throw new Bridge.InvalidOperationException("The Queue is empty.");
            }
    
            var item = this.list.getItem(this.list.getCount() - 1);
            this.list.removeAt(this.list.getCount() - 1);
            return item;
        },
        getEnumerator$1: function () {
            return this.list.getEnumerator();
        },
        getEnumerator: function () {
            return this.list.getEnumerator();
        }
    }; });
    
    
    
    Bridge.init();
})(this);
