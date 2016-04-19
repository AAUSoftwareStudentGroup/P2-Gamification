#if BRIDGE
using Bridge.Html5;
#else
using System;
#endif
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class ConstantExpression : ExpressionBase, ILeaf
    {
        public ConstantExpression(ConstantType type)
        {
            Type = type;
        }

        public override string Value
        {
            get { return Type.ToString(); }
        }

        public ConstantType Type { get; set; }

        public override int Size
        {
            get
            {
                return 1;
            }
        }

        public override ExpressionBase Clone()
        {
            return new ConstantExpression(Type);
        }

        public override bool Replace(ExpressionBase old, ExpressionBase replacement, bool doRecursively)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield break;
        }

        public override bool CanCalculate()
        {
            switch (Type)
            {
                case ConstantType.Pi:
                    return true;
            }

            return false;
        }

        public override double? Calculate()
        {
            switch (Type)
            {
                case ConstantType.Pi:
                    return System.Math.PI;
            }

            return null;
        }

        public override bool Equals(ExpressionBase otherBase)
        {
            var other = (otherBase as ConstantExpression);

            if (other == null)
                return false;

            return this.Type == other.Type;
        }

        public override string ToString()
        {
            return Value;
        }

        public override string TreePrint(string indent, bool isLast)
        {
            Console.WriteLine(indent + "|-" + Value);
            return indent + (isLast ? "  " : "| ");
        }
    }

    public enum ConstantType
    {
        Pi,
    }
}
