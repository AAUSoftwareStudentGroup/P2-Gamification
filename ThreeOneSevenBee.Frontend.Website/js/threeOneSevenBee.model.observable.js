(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Observable.ObservableBase$1', function (T) { return {
        inherits: [ThreeOneSevenBee.Model.ObserverPattern.IObservable$1(T)],
        data: null,
        observers: null,
        constructor: function (data) {
            this.observers = new Bridge.List$1(ThreeOneSevenBee.Model.ObserverPattern.IObserver$1(T))();
            this.data = data;
        },
        getData: function () {
            return this.data;
        },
        setData: function (value) {
            this.data = value;
            this.updateObservers();
        },
        addObserver: function (observer) {
            this.observers.add(observer);
        },
        updateObservers: function () {
            var $t;
            $t = Bridge.getEnumerator(this.observers);
            while ($t.moveNext()) {
                var observer = $t.getCurrent();
                observer.update(this.data);
            }
        }
    }; });
    
    
    
    Bridge.init();
})(this);
