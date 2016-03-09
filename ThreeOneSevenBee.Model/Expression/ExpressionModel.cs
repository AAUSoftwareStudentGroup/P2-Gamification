using System.Collections.Generic;
using System;
using ThreeOneSevenBee.Model.Expression.Expressions;

namespace ThreeOneSevenBee.Model.Expression
{
    public class ExpressionModel
    {
        private ExpressionBase expression;
        private ExpressionBase selectionParent;
        private List<ExpressionBase> selection;
        private List<ExpressionBase> identities;
        private ExpressionAnalyzer analyzer;
        private ExpressionSerializer serializer;

        public ExpressionModel(string expression, params ExpressionRule[] rules)
        {
            selectionParent = null;
            selection = new List<ExpressionBase>();
            identities = new List<ExpressionBase>();
            serializer = new ExpressionSerializer();
            analyzer = new ExpressionAnalyzer();
            this.expression = serializer.Deserialize(expression);
            foreach (ExpressionRule rule in rules)
            {
                analyzer.Add(rule);
            }
        }

        public ExpressionBase Expression
        {
            get { return expression; }
        }

        public List<ExpressionBase> Identities
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

        public event Action<ExpressionModel> OnChanged;

        public void Select(ExpressionBase expression)
        {
            if (!selection.Contains(expression))
            {
                selection.Add(expression);
            }
            else
            {
                selection.Remove(expression);
            }
            selectionParent = analyzer.GetCommonParent(selection);
            identities = analyzer.GetIdentities(expression, selection);
            OnChanged(this);
        }

        public void UnSelectAll()
        {
            selection.Clear();
            identities.Clear();
            selectionParent = null;
            OnChanged(this);
        }

        public void ApplyIdentity(ExpressionBase identity)
        {
            if (identities.Contains(identity))
            {
                if(selectionParent.Parent == null)
                {
                    expression = identity;
                }
                else
                {
                    var parent = (selectionParent.Parent as OperatorExpression);
                    if(parent.Left == selectionParent)
                    {
                        parent.Left = identity;
                    }
                    else
                    {
                        parent.Right = identity;
                    }
                }
            }
            UnSelectAll();
        }

    }
}
