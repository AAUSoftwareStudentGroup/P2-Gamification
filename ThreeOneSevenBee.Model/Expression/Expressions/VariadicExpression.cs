using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections;
using System.Collections.Generic;

namespace ThreeOneSevenBee.Model.Expression.Expressions
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
            this.Add(first);
            this.Add(second);
            foreach (var expression in expressions)
                this.Add(expression);
        }

        protected VariadicExpression(OperatorType type, params ExpressionBase[] expressions)
            : base(type)
        {
            if (type != OperatorType.Add && type != OperatorType.Multiply)
                throw new ArgumentException("Invalid Type: " + type, "type");
            if (expressions == null)
                throw new ArgumentNullException("expressions");
            if (expressions.Length < 2)
                throw new ArgumentOutOfRangeException("Must give at least two expressions.", "expression");

            this.expressions = new List<ExpressionBase>();
            foreach (var expression in expressions)
                this.Add(expression);
        }

        public override bool Replace(ExpressionBase old, ExpressionBase replacement, bool doRecursively)
        {
            var hasReplaced = false;

            ExpressionBase[] expressionArray = expressions.ToArray();

            for (int i = 0; i < expressionArray.Length; i++)
            {
                if (Object.ReferenceEquals(expressionArray[i], old))
                {
                    expressionArray[i] = replacement.Clone();
                    expressionArray[i].Parent = this;
                    hasReplaced |= true;
                }
                else if (doRecursively)
                {
                    hasReplaced |= expressionArray[i].Replace(old, replacement, true);
                }
            }

            if (hasReplaced)
            {
                expressions.Clear();
                foreach(ExpressionBase expr in expressionArray)
                {
                    Add(expr);
                }
            }

            return hasReplaced;
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
                RemoveAt(index);
                Insert(index, value);
            }
        }

        public void Add(List<ExpressionBase> items)
        {
            foreach (ExpressionBase item in items)
            {
                Add(item);
            }
        }

        public void Add(VariadicExpression item)
        {
            if (item.Type == this.Type)
            {
                foreach (var subItem in item)
                    this.Add(subItem);
            }
            else
            {
                expressions.Add(item);
                item.Parent = this;
            }
        }

        public void Add(ExpressionBase item)
        {
            if (item is VariadicExpression)
            {
                this.Add((VariadicExpression)item);
            }
            else
            {
                expressions.Add(item);
                item.Parent = this;
            }
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
            VariadicOperatorExpression variadicExpression = item as VariadicOperatorExpression;
            if(variadicExpression == null || variadicExpression.Type != Type)
            {
                expressions.Insert(index, item);
                item.Parent = this;
            }
            else
            {
                int offset = 0;
                foreach (ExpressionBase operand in variadicExpression)
                {
                    expressions.Insert(index + offset++, operand);
                    operand.Parent = this;
                }
            }
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
