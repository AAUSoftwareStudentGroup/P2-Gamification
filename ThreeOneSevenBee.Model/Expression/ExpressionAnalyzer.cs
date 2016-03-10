using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.Expression
{
    /// <summary>
    /// Provides functionality to analyze expressions and provide identical alternatives based on <see cref="ExpressionRule"/>.
    /// </summary>
    public class ExpressionAnalyzer
    {
        private List<ExpressionRule> rules;

        public ExpressionAnalyzer()
        {
            rules = new List<ExpressionRule>();
        }

        public void Add(ExpressionRule rule)
        {
            rules.Add(rule);
        }

        public void Remove(ExpressionRule rule)
        {
            rules.Remove(rule);
        }

        public ExpressionBase GetCommonParent(List<ExpressionBase> selection)
        {
            if(selection.Count == 0)
            {
                return null;
            }
            else if(selection.Count == 1)
            {
                return selection[0];
            }
            else
            {
                var intersection = selection[0].GetParentPath();

                foreach (var expression in selection)
                {
                    var path = expression.GetParentPath();
                    if (path.Count() == 0)
                    {
                        return expression;
                    }
                    intersection = intersection.Intersect(path);
                }

                return intersection.First();
            }
        }

        public List<ExpressionBase> GetIdentities(ExpressionBase expression, List<ExpressionBase> selection)
        {
            var identities = new List<ExpressionBase>();
            
            if(selection.Count() == 0)
            {
                return identities;
            }

            foreach(ExpressionRule rule in rules)
            {
                ExpressionBase commonParent = GetCommonParent(selection);
                ExpressionBase identity;
                if (rule(commonParent, selection, out identity))
                {
                    identities.Add(identity);
                }
            }

            return identities;
        }
    }
}