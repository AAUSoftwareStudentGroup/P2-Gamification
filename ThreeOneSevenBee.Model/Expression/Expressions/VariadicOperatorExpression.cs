#if BRIDGE
using Bridge.Html5;
#endif
using System;
using System.Collections.Generic;
using System.Linq;

namespace threeonesevenbee.Model.Expression.Expressions
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
            get { return this.Skip(1).Aggregate(this.First().Value, (s, e) => s + Symbol + e.Value); }
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

        //Removes element from two clones if an element is in both lists. If both lists are 0 at the end, returns true.
        public override bool Equals(ExpressionBase otherBase)
        {
            var other = (otherBase as VariadicOperatorExpression);

            if (other == null)
                return false;

            other = (VariadicOperatorExpression)other.Clone();
            var thisClone = (VariadicOperatorExpression)this.Clone();

            for (int i = 0; i < thisClone.Count; i++)
            {
                for (int j = 0; j < other.Count; j++)
                {
                    if (thisClone[i].Equals(other[j]))
                    {
                        thisClone.RemoveAt(i);
                        i--;
                        other.RemoveAt(j);
                        break;
                    }
                }
            }

            if (other.Count != 0 && thisClone.Count != 0)
                return false;

            return true;
        }

        public override ExpressionBase Clone()
        {
            var expression = new VariadicOperatorExpression(Type, this[0].Clone(), this[1].Clone());
            foreach (var expr in this.Skip(2))
            {
                expression.Add(expr.Clone());
            }
            return expression;
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
				this [i].TreePrint(indent, false);
			}
			this[this.Count-1].TreePrint(indent, true);
			return indent;
		}

    }
}
