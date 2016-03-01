using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Framework
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

        public IList<Expression> GetIdentities(Expression expression)
        {
            var identities = new List<Expression>();

            // e is always identical to itself
            identities.Add(expression);

            // check additional rules here

            return identities;
        }
    }
}