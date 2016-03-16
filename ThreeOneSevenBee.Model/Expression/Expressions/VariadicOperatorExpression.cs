using System;
using System.Collections.Generic;
using System.Linq;

namespace ThreeOneSevenBee.Model.Expression.Expressions
{
    public class VariadicOperatorExpression : VariadicExpression
    {
        public VariadicOperatorExpression(OperatorType type, ExpressionBase firstExpression, ExpressionBase secondExpression, params ExpressionBase[] expressions)
            : base(type, firstExpression, secondExpression, expressions)
        { }

        private VariadicOperatorExpression(OperatorType type, params ExpressionBase[] expressions)
            : base(type, expressions)
        { }

        public override String Value
        {
            get { return this.Aggregate(this.First().Value, (s, e) => s + Symbol + e.Value); }
        }

        public override Boolean CanCalculate()
        {
            switch (Type)
            {
                case OperatorType.Add:
                case OperatorType.Multiply:
                    foreach (var expression in this)
                    {
                        if (!expression.CanCalculate())
                            return false;
                    }
                    return true;
            }
            return false;
        }

        public override Double? Calculate()
        {
            if (!this.CanCalculate())
                return null; // break out early if this cannot be calculated

            double? result = null;
            switch (Type)
            {
                case OperatorType.Add:
                    // read out the first index
                    result = this[0].Calculate();
                    // skip the first, and apply the operation
                    for (int i = 1; i < this.Count; i++)
                        result += this[i].Calculate();
                    break;
                case OperatorType.Multiply:
                    // read out the first index
                    result = this[0].Calculate();
                    // skip the first, and apply the operation
                    for (int i = 1; i < this.Count; i++)
                        result *= this[i].Calculate();
                    break;
            }
            return result;
        }

        public override ExpressionBase Clone()
        {
            return new VariadicOperatorExpression(Type, this.Select(e => e.Clone()).ToArray());
        }

        public override IEnumerable<ExpressionBase> GetNodesRecursive()
        {
            foreach (var node in this)
            {
                yield return node;
                foreach (var childNode in node.GetNodesRecursive())
                {
                    yield return childNode;
                }
            }
        }

        public override Boolean Replace(ExpressionBase old, ExpressionBase replacement)
        {
            throw new NotImplementedException();
        }

		public override string TreePrint(string indent, bool isLast)
		{
			Console.WriteLine(indent + "|-" + Symbol);
			indent += (isLast ? "  " : "| ");
			for (int i = 0; i < this.Count-1; i++) {
				this [i].TreePrint(indent);
			}
			this[this.Count-1].TreePrint(indent, true);
			return indent;
		}
    }
}
