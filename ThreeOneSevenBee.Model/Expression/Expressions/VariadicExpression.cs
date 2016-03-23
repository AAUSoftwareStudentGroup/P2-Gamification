using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections;
using System.Collections.Generic;

namespace threeonesevenbee.Model.Expression.Expressions
{
    public abstract class VariadicExpression : OperatorExpression, ICollection<ExpressionBase>, IList<ExpressionBase>
    {
        private List<ExpressionBase> expressions;

        protected VariadicExpression(OperatorType type, ExpressionBase first, ExpressionBase second, params ExpressionBase[] expressions)
            : base(type)
        {
            
            if (type != OperatorType.Add && type != OperatorType.Multiply)
                throw new ArgumentException("Invalid Type: " + type, "type");
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            this.expressions = new List<ExpressionBase>();
            this.expressions.Add(first);
            this.expressions.Add(second);
            this.expressions.AddRange(expressions);

            foreach (var expression in this)
                expression.Parent = this;
        }

        protected VariadicExpression(OperatorType type, params ExpressionBase[] expressions)
            : base(type)
        {
            Console.WriteLine(expressions[0]);
            if (type != OperatorType.Add && type != OperatorType.Multiply)
                throw new ArgumentException("Invalid Type: " + type, "type");
            if (expressions == null)
                throw new ArgumentNullException("expressions");
            if (expressions.Length < 2)
                throw new ArgumentOutOfRangeException("Must give at least two expressions.", "expression");

            this.expressions = new List<ExpressionBase>();
            this.expressions.AddRange(expressions);

            foreach (var expression in this)
                expression.Parent = this;
        }

        public Int32 Count { get { return expressions.Count; } }

        public Boolean IsReadOnly { get { return false; } }

        public ExpressionBase this[Int32 index]
        {
            get
            {
                return expressions[index];
            }

            set
            {
                expressions[index] = value;
            }
        }

        public void Add(ExpressionBase item)
        {
            expressions.Add(item);
            item.Parent = this;
        }

        public Boolean Remove(ExpressionBase item)
        {
            return expressions.Remove(item);
        }

        public void Clear()
        {
            expressions.Clear();
        }

        public Boolean Contains(ExpressionBase item)
        {
            return expressions.Contains(item);
        }

#if !BRIDGE
        public void CopyTo(ExpressionBase[] array, Int32 arrayIndex)
        {
            expressions.CopyTo(array, arrayIndex);
        }
#endif

        public IEnumerator<ExpressionBase> GetEnumerator()
        {
            return expressions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return expressions.GetEnumerator();
        }

        public Int32 IndexOf(ExpressionBase item)
        {
            return expressions.IndexOf(item);
        }

        public void Insert(Int32 index, ExpressionBase item)
        {
            expressions.Insert(index, item);
            item.Parent = this;
        }

        public void RemoveAt(Int32 index)
        {
            expressions.RemoveAt(index);
        }

        public void RemoveReference(ExpressionBase expression)
        {
            RemoveAt(IndexOfReference(expression));
        }

        public int IndexOfReference(ExpressionBase expression)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (ReferenceEquals(this[i], expression))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
