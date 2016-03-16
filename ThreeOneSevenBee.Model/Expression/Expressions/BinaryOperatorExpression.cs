using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class BinaryOperatorExpression : BinaryExpression
    {
        public BinaryOperatorExpression(ExpressionBase left, ExpressionBase right, OperatorType type)
            : base(left, right, type)
        { }

        public override String Value
        {
            get { return Left.Value + Symbol + Right.Value; }
        }

        public override Boolean CanCalculate()
        {
            switch (Type)
            {
                case OperatorType.Add:
                case OperatorType.Subtract:
                case OperatorType.Divide:
                case OperatorType.Multiply:
                case OperatorType.Power:
                    return Left.CanCalculate() && Right.CanCalculate();
            }
            return false;
        }

        public override Double? Calculate()
        {
            switch (Type)
            {
                case OperatorType.Add:
                    return Left.Calculate() + Right.Calculate();
                case OperatorType.Subtract:
                    return Left.Calculate() - Right.Calculate();
                case OperatorType.Divide:
                    return Left.Calculate() / Right.Calculate();
                case OperatorType.Multiply:
                    return Left.Calculate() * Right.Calculate();
                case OperatorType.Power:
                    return Math.Pow(Left.Calculate().Value, Right.Calculate().Value);
            }
            return null;
        }

        public override ExpressionBase Clone()
        {
            return new BinaryOperatorExpression(Left.Clone(), Right.Clone(), Type);
        }

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            yield return Left;
            foreach (var node in Left.GetNodesRecursive())
                yield return node;
            yield return Right;
            foreach (var node in Right.GetNodesRecursive())
                yield return node;
        }

        public override Boolean Replace(ExpressionBase old, ExpressionBase replacement)
        {
            throw new NotImplementedException();
        }

        public override string TreePrint(string indent, bool isLast)
        {
            Console.WriteLine(indent + "|-" + Symbol);
            indent += (isLast ? "  " : "| ");
            Left.TreePrint(indent, false);
            Right.TreePrint(indent, true);
            return indent;
        }
    }
}
