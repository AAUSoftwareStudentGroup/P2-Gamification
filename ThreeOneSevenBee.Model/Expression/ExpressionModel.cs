using System.Collections.Generic;
using System;
#if BRIDGE
using Bridge.Html5;
#endif
using ThreeOneSevenBee.Model.Expression.Expressions;
using System.Linq;

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

        public ExpressionModel(string expression, params ExpressionRule[] rules) : this(expression, null, rules) { }

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

        public void Select(ExpressionBase expression)
        {
            expression.Selected = expression.Selected == false;

            UpdateSelection();

            UpdateIdentities();
        }

        public void UpdateSelection()
        {
            selection = Expression.GetNodesRecursive().Where((e) => e.Selected).ToList();

            if (Expression.Selected == true)
            {
                selection.Insert(0, Expression);
            }
        }

        public void UpdateIdentities()
        {
            UpdateSelection();
            selectionParent = analyzer.GetCommonParent(selection);
            identities = analyzer.GetIdentities(selection);
            Console.WriteLine("Identities updated");
            if (OnChanged != null)
                OnChanged(this);
        }

        public void UnSelectAll()
        {
            for (int index = 0; index < selection.Count; index++)
            {
                selection[index].Selected = false;
            }
            selection.Clear();
            identities.Clear();
            selectionParent = null;
            callOnChanged();
        }

        public void ApplyIdentity(ExpressionBase identity)
        {
            ExpressionBase parent = Selected.Parent;
            if (parent == null)
            {
                expression = identity;
            }
            else
            {
                Selected.Replace(identity);
            }
            Selected.Parent = parent;
            UpdateSelection();
            UnSelectAll();
            UpdateIdentities();
        }
    }
}
