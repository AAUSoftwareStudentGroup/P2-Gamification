(function (globals) {
    "use strict";
     
    Bridge.define('ThreeOneSevenBee.Model.Polygon.PolygonCorner', {
        config: {
            init: function () {
                Bridge.property(this, "identifier", new Bridge.Int() || new Bridge.Int());
            }
        },
        constructor: function (identifier) {
            this.setidentifier(identifier);
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Polygon.PolygonRelation', {
        statics: {
            getDefaultValue: function () { return new ThreeOneSevenBee.Model.Polygon.PolygonRelation(); }
        },
        config: {
            init: function () {
                this.corner1 = new Bridge.Int() || new Bridge.Int();
                this.corner2 = new Bridge.Int() || new Bridge.Int();
            }
        },
        constructor: function () {
        },
        getHashCode: function () {
            var hash = 17;
            hash = hash * 23 + (this.corner1 == null ? 0 : Bridge.getHashCode(this.corner1));
            hash = hash * 23 + (this.corner2 == null ? 0 : Bridge.getHashCode(this.corner2));
            return hash;
        },
        equals: function (o) {
            if (!Bridge.is(o,ThreeOneSevenBee.Model.Polygon.PolygonRelation)) {
                return false;
            }
            return Bridge.equals(this.corner1, o.corner1) && Bridge.equals(this.corner2, o.corner2);
        },
        $clone: function (to) {
            var s = to || new ThreeOneSevenBee.Model.Polygon.PolygonRelation();
            s.corner1 = this.corner1;
            s.corner2 = this.corner2;
            return s;
        }
    });
    
    Bridge.define('ThreeOneSevenBee.Model.Polygon.PolygonSide', {
        config: {
            properties: {
                corner1: null,
                corner2: null
            },
            init: function () {
                this.identifier = new Bridge.Int() || new Bridge.Int();
            }
        },
        constructor: function (corner1, corner2, identifier) {
            this.setcorner1(corner1);
            this.setcorner2(corner2);
            this.identifier = identifier;
        },
        getOtherCorner: function (corner) {
            if (corner === this.getcorner1()) {
                return this.getcorner2();
            }
            else  {
                if (corner === this.getcorner2()) {
                    return this.getcorner1();
                }
                else  {
                    return null;
                }
            }
        }
    });
    
    
    
    Bridge.init();
})(this);
