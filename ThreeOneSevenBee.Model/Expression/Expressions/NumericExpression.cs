﻿using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class NumericExpression : ExpressionBase, ILeaf
    {
		public int Number;

        public NumericExpression(int value)
        {
            Number = value;
        }

        public override string Value
        {
            get { return Number.ToString(); }
        }

        public override int Size
        {
            get
            {
                return 1;
            }
        }

        public override ExpressionBase Clone()
        {
            return new NumericExpression(Number) { Selected = Selected };
        }

        public override Boolean Replace(ExpressionBase old, ExpressionBase replacement, bool doRecursively)
        {
            return false;
        }

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield break;
        }

        public override bool CanCalculate()
        {
            return true;
        }

        public override double? Calculate()
        {
            return Number;
		}

        public override bool Equals(ExpressionBase otherBase)
        {
            var other = (otherBase as NumericExpression);

            if (other == null)
                return false;

            return (Math.Abs(this.Number - other.Number) < double.Epsilon);
        }

        public override string ToString()
        {
            return Value;
        }

        public override string TreePrint(string indent, bool isLast)
		{
			Console.WriteLine (indent + "|-" + Value);
			return indent + (isLast ? "  " : "| ");
		}
    }
}
