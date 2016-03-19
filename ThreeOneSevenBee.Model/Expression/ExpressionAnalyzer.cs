using System;
#if BRIDGE
using Bridge.Html5;
#endif
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Expression.Expressions;

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
            if (selection.Count == 0)
            {
                return null;
            }
            else if (selection.Count == 1)
            {
                return selection[0];
            }
            else
            {
                var parentPaths = new List<List<ExpressionBase>>();
                for (int index = 0; index < selection.Count; index++)
                {
                    parentPaths.Add(selection[index].GetParentPath().Reverse().ToList());
                }
                return GetCommonParent(parentPaths);
            }
        }

        public ExpressionBase GetCommonParent(List<List<ExpressionBase>> parentPaths)
        {
            List<ExpressionBase> intersection = PathIntersection(parentPaths[0], parentPaths[1]);
            for (int index = 2; index < parentPaths.Count; index++)
            {
                intersection = PathIntersection(intersection, parentPaths[index]);
            }
            return intersection[0];
        }

        public List<ExpressionBase> PathIntersection(List<ExpressionBase> first, List<ExpressionBase> second)
        {
            int secondIndex = 0;
            return first.TakeWhile(
                (expr) => 
                secondIndex == second.Count || 
                ReferenceEquals(expr, second[secondIndex++])).ToList();
        }

        public List<ExpressionBase> GetIdentities(ExpressionBase expression, List<ExpressionBase> selection)
        {
            var identities = new List<ExpressionBase>();

            if (selection.Count() == 0)
            {
                return identities;
            }

            foreach (ExpressionRule rule in rules)
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