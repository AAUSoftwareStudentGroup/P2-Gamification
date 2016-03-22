using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Polygon;

namespace ThreeOneSevenBee.Model.Geometry
{
    class PolygonModel
    {
        public List<PolygonSide> sides { get; private set; }
        public List<PolygonCorner> corners { get; private set; }
        public List<PolygonRelation> relations { get; private set; }

        public PolygonModel(List<PolygonSide> sides, List<PolygonCorner> corners, List<PolygonRelation> relations)
        {
            this.sides = sides;
            this.corners = corners;
            this.relations = relations;
        }

        private int GetCornerIndex(char cornerID)
        {
            for(int i = 0; i < corners.Count; i++)
            {
                if (corners[i].identifier == cornerID)
                    return i;
                    
            }
            return -1;
        }

        public PolygonModel Cut(char corner1c, char corner2c)
        {
            return Cut(GetCornerIndex(corner1c), GetCornerIndex(corner2c));
        }

        public PolygonModel Cut(int corner1, int corner2)
        {
            /*New polygons becomes this and returned object*/
            /*Part PolygonModel in 2 and copy corner and sides to new PolygonModel and create new side*/

            List<PolygonSide> newSides = new List<PolygonSide>();
            List<PolygonCorner> newCorners = new List<PolygonCorner>();

            PolygonSide newSide;
            PolygonCorner cutCorner1 = corners[corner1];
            PolygonCorner cutCorner2 = corners[corner2];
            PolygonCorner corner = corners[corner1];

            /*Part PolygonModel into 2 */

            while (corner != cutCorner2) // Loop from corner1 to corner2 around PolygonModel
            {
                // Put all encountered sides into newSides and all encountered corners into newCorners without removing
                // Find a side that connects to this corner.
                newSide = new PolygonSide(null, null, '©'); // Just fuck shit up

                for (int i = 0; i < sides.Count; i++)
                {
                    if (sides[i].corner1 == corner || sides[i].corner2 == corner)
                    {
                        newSide = sides[i];
                        break;
                    }
                }
                // Put side into newSides
                newSides.Add(newSide);
                // Remove side from oldSides
                sides.Remove(newSide);
                // Put corner into newCorners
                newCorners.Add(corner);
                // Remove corner from oldCorners
                corners.Remove(corner);
                // Get other corner of side
                corner = newSide.GetOtherCorner(corner);
                // Repeat until other cutcorner is reached
            }
            // Insert new sides
            newSides.Add(new PolygonSide(cutCorner1, cutCorner2, 'X'));
            sides.Add(new PolygonSide(cutCorner1, cutCorner2, 'X'));
            // Insert missing corner
            newCorners.Add(cutCorner2);
            corners.Add(cutCorner1);
            return new PolygonModel(newSides, newCorners, relations); // Same relations are just passed on
        }
    }
}