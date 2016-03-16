using System;
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

        public override ExpressionBase Clone()
        {
            return new ConstantExpression(Type);
        }

        public override Boolean Replace(ExpressionBase old, ExpressionBase replacement)
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
                    return Math.PI;
            }

            return null;
        }
    }

    public enum ConstantType
    {
        Pi,
    }
}
