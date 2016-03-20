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

                foreach (var descendant in expression.GetNodesRecursive())
                {
                    int i = SelectionIndex(descendant);
                    if (i != -1)
                        selection.RemoveAt(i);
                }
                selection.Add(expression);
            }
            else
            {
                selection.RemoveAt(index);
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
            Console.WriteLine(identity.ToString());
                Console.WriteLine("nice trye");
                if (Selected.Parent == null)
                {
                    expression = identity;
                    
                }
                else
                {
                    var binaryParent = Selected.Parent as BinaryOperatorExpression;
                    if (binaryParent != null)
                    {
                        if(ReferenceEquals(binaryParent.Left, Selected))
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
                }
            UnSelectAll();
        }

    }
}
