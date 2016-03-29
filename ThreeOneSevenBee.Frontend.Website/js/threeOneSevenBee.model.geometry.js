(function (globals) {
    "use strict";

    Bridge.define('ThreeOneSevenBee.Model.Geometry.PolygonModel', {
        config: {
            properties: {
                sides: null,
                corners: null,
                relations: null
            }
        },
        constructor: function (sides, corners, relations) {
            this.setsides(sides);
            this.setcorners(corners);
            this.setrelations(relations);
        },
        constructor$1: function (sideCount) {
            var sides = new Bridge.List$1(ThreeOneSevenBee.Model.Polygon.PolygonSide)();
            var corners = new Bridge.List$1(ThreeOneSevenBee.Model.Polygon.PolygonCorner)();
            var relations = new Bridge.List$1(ThreeOneSevenBee.Model.Polygon.PolygonRelation)();
            var side = 65;
            var corner = 97;
            var lastCorner = new ThreeOneSevenBee.Model.Polygon.PolygonCorner(corner++);
            corners.add(lastCorner);
            var thisCorner;
            for (var i = 1; i < sideCount; i++) {
                thisCorner = new ThreeOneSevenBee.Model.Polygon.PolygonCorner(corner++);
                corners.add(thisCorner);
                sides.add(new ThreeOneSevenBee.Model.Polygon.PolygonSide(lastCorner, thisCorner, side++));
                thisCorner = (i === sideCount - 1) ? sides.getItem(0).getcorner1() : lastCorner;
            }
            this.setsides(sides);
            this.setcorners(corners);
            this.setrelations(relations);
        },
        getCornerIndex: function (cornerID) {
            for (var i = 0; i < this.getcorners().getCount(); i++) {
                if (this.getcorners().getItem(i).getidentifier() === cornerID) {
                    return i;
                }
    
            }
            return -1;
        },
        cut: function (corner1c, corner2c) {
            return this.cut$1(this.getCornerIndex(corner1c), this.getCornerIndex(corner2c));
        },
        cut$1: function (corner1, corner2) {
            /* New polygons becomes this and returned object*/
            /* Part PolygonModel in 2 and copy corner and sides to new PolygonModel and create new side*/
    
            var newSides = new Bridge.List$1(ThreeOneSevenBee.Model.Polygon.PolygonSide)();
            var newCorners = new Bridge.List$1(ThreeOneSevenBee.Model.Polygon.PolygonCorner)();
    
            var newSide;
            var cutCorner1 = this.getcorners().getItem(corner1);
            var cutCorner2 = this.getcorners().getItem(corner2);
            var corner = this.getcorners().getItem(corner1);
    
            /* Part PolygonModel into 2 */
    
            while (corner !== cutCorner2) {
                // Put all encountered sides into newSides and all encountered corners into newCorners without removing
                // Find a side that connects to this corner.
                newSide = new ThreeOneSevenBee.Model.Polygon.PolygonSide(null, null, 169); // Just fuck shit up
    
                for (var i = 0; i < this.getsides().getCount(); i++) {
                    if (this.getsides().getItem(i).getcorner1() === corner || this.getsides().getItem(i).getcorner2() === corner) {
                        newSide = this.getsides().getItem(i);
                        break;
                    }
                }
                // Put side into newSides
                newSides.add(newSide);
                // Remove side from oldSides
                this.getsides().remove(newSide);
                // Put corner into newCorners
                newCorners.add(corner);
                // Remove corner from oldCorners
                this.getcorners().remove(corner);
                // Get other corner of side
                corner = newSide.getOtherCorner(corner);
                // Repeat until other cutcorner is reached
            }
            // Insert new sides
            newSides.add(new ThreeOneSevenBee.Model.Polygon.PolygonSide(cutCorner1, cutCorner2, 88));
            this.getsides().add(new ThreeOneSevenBee.Model.Polygon.PolygonSide(cutCorner1, cutCorner2, 88));
            // Insert missing corner
            newCorners.add(cutCorner2);
            this.getcorners().add(cutCorner1);
            return new ThreeOneSevenBee.Model.Geometry.PolygonModel("constructor", newSides, newCorners, this.getrelations()); // Same relations are just passed on
        }
    });
    
    
    
    Bridge.init();
})(this);
