#if BRIDGE
using Bridge.Html5;
#else
using System;
#endif
using System.Collections.Generic;

namespace threeonesevenbee.Model.Expression.Expressions
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

        public override ExpressionBase Clone()
        {
            return new ConstantExpression(Type);
        }

        public override bool Replace(ExpressionBase old, ExpressionBase replacement)
        {
            return false;
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

        public override string TreePrint(string indent, bool isLast) 
		{
			Console.WriteLine (indent + "|-" + Value);
            return indent + (isLast ? "  " : "| ");
		}
    }

    public enum ConstantType
    {
        Pi,
    }
}
