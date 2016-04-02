using System.Collections.Generic;
using System;
#if BRIDGE
using Bridge.Html5;
#endif
using ThreeOneSevenBee.Model.Expression.Expressions;

namespace ThreeOneSevenBee.Model.Expression
{
    public class ExpressionModel
    {
        private ExpressionBase expression;
        private ExpressionBase selectionParent;
        private List<ExpressionBase> selection;
        private List<Identity> identities;
        private ExpressionAnalyzer analyzer;
        private ExpressionSerializer serializer;

        public ExpressionModel(string expression, Action<ExpressionModel> onChange, params ExpressionRule[] rules)
        {
            selectionParent = null;
            selection = new List<ExpressionBase>();
            identities = new List<Identity>();
            serializer = new ExpressionSerializer();
            analyzer = new ExpressionAnalyzer();
            this.expression = serializer.Deserialize(expression);
            foreach (ExpressionRule rule in rules)
            {
                analyzer.Add(rule);
            }
            OnChanged = onChange;
            callOnChanged();
        }

        public ExpressionBase Expression
        {
            get { return expression; }
        }

        public List<Identity> Identities
        {
            get { return identities; }
        }

        public List<ExpressionBase> Selection
        {
            get { return selection; }
        }

        public ExpressionBase Selected
        {
            get { return selectionParent; }
        }

        public Action<ExpressionModel> OnChanged;

        private void callOnChanged()
        {
            if (OnChanged != null)
            {
                OnChanged(this);
            }
        }

        public int SelectionIndex(ExpressionBase expression)
        {
            for (int i = 0; i < selection.Count; i++)
            {
                if (ReferenceEquals(selection[i], expression))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Select(ExpressionBase expression)
        {
            int index = SelectionIndex(expression);
            if (index == -1)
            {
                selection.Add(expression);
            }
            else
            {
                selection.RemoveAt(index);
            }
            
            selectionParent = analyzer.GetCommonParent(selection);
         
            identities = analyzer.GetIdentities(expression, selection);
            if(OnChanged != null)
                OnChanged(this);
        }

        public void UnSelectAll()
        {
            selection.Clear();
            identities.Clear();
            selectionParent = null;
            callOnChanged();
        }



        public void ApplyIdentity(ExpressionBase identity)
        {
            if (Selected.Parent == null)
            {
                expression = identity;

            }
            else
            {
                var binaryParent = Selected.Parent as BinaryOperatorExpression;
                if (binaryParent != null)
                {
                    if (ReferenceEquals(binaryParent.Left, Selected))
                    {
                        binaryParent.Left = identity;
                    }
                    else
                    {
                        binaryParent.Right = identity;
                    }
                    identity.Parent = binaryParent;
                }
                var variadicParent = Selected.Parent as VariadicOperatorExpression;
                if (variadicParent != null)
                {
                    var temp = identity as VariadicOperatorExpression;
                    int selectedIndex = -1;
                    for (int index = 0; index < variadicParent.Count; index++)
                    {
                        if (ReferenceEquals(variadicParent[index], Selected))
                        {
                            selectedIndex = index;
                        }
                    }
                    if (temp != null && temp.Type == variadicParent.Type)
                    {
                        variadicParent.RemoveAt(selectedIndex);
                        foreach (var operand in temp)
                        {
                            variadicParent.Insert(selectedIndex, operand);
                        }
                    }
                    else
                    {
                        variadicParent[selectedIndex] = identity;
                        identity.Parent = variadicParent;
                    }
                }
                var minusParent = Selected.Parent as UnaryMinusExpression;
                if (minusParent != null)
                {
                    minusParent.Expression = identity;
                    identity.Parent = minusParent;
                }
                var delimiterParent = Selected.Parent as DelimiterExpression;
                if (delimiterParent != null)
                {
                    delimiterParent.Expression = identity;
                    identity.Parent = delimiterParent;
                }
                var functionExpression = Selected.Parent as FunctionExpression;
                if (functionExpression != null)
                {
                    functionExpression.Expression = identity;
                    identity.Parent = functionExpression;
                }
            }
            UnSelectAll();
        }

    }
}
