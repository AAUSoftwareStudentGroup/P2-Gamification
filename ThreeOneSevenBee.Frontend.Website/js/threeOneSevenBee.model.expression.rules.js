(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Expression.Rules.ItselfRule', {
        inherits: [ThreeOneSevenBee.Model.Expression.ExpressionRule],
        tryGetIdentity: function (expression, selection, identity) {
            identity.v = null;
            return true;
        }
    });
    
    
    
    Bridge.init();
})(this);
