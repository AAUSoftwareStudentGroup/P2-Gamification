using System;
#if BRIDGE
using Bridge.Html5;
using Bridge;
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
            return intersection[intersection.Count - 1];
        }

        public List<ExpressionBase> PathIntersection(List<ExpressionBase> first, List<ExpressionBase> second)
        {
            int secondIndex = 0;
            return first.TakeWhile(
                (expr) =>
                secondIndex == second.Count ||
                ReferenceEquals(expr, second[secondIndex++])).ToList();
        }

        public Dictionary<OperatorType, Dictionary<OperatorType, bool>> WrapInParenthesis = new Dictionary<OperatorType, Dictionary<OperatorType, bool>>()
            {
                {
                    OperatorType.Minus,
                    new Dictionary<OperatorType, bool>()
                    {
                        { OperatorType.Minus, true },
                        { OperatorType.Add, false }, //problem, therefore true as a precaution
                        { OperatorType.Divide, false },
                        { OperatorType.Multiply, true },
                        { OperatorType.Power, false } //problem, therefore true as a precaution
                    }
                },
                {
                    OperatorType.Add,
                    new Dictionary<OperatorType, bool>()
                    {
                        { OperatorType.Minus, true },
                        { OperatorType.Add, false },
                        { OperatorType.Divide, false },
                        { OperatorType.Multiply, true },
                        { OperatorType.Power, true } //problem, therefore true as a precaution
                    }
                },
                {
                    OperatorType.Divide,
                    new Dictionary<OperatorType, bool>()
                    {
                        { OperatorType.Minus, false },
                        { OperatorType.Add, false },
                        { OperatorType.Divide, false },
                        { OperatorType.Multiply, false },
                        { OperatorType.Power, false }
                    }
                },
                {
                    OperatorType.Multiply,
                    new Dictionary<OperatorType, bool>()
                    {
                        { OperatorType.Minus, false },
                        { OperatorType.Add, false },
                        { OperatorType.Divide, false },
                        { OperatorType.Multiply, false },
                        { OperatorType.Power, true } //problem, therefore true as a precaution
                    }
                },
                {
                    OperatorType.Power,
                    new Dictionary<OperatorType, bool>()
                    {
                        { OperatorType.Minus, false },
                        { OperatorType.Add, false },
                        { OperatorType.Divide, false },
                        { OperatorType.Multiply, false },
                        { OperatorType.Power, true }
                    }
                }
            };

        public ExpressionBase WrapInDelimiterIfNeccessary(OperatorExpression expression, OperatorType parentType)
        {
            bool isNeccessary = WrapInParenthesis[expression.Type][parentType];

            if (isNeccessary)
            {
                return new DelimiterExpression(expression);
            }
            else
            {
                return expression;
            }
        }

        public ExpressionBase WrapInDelimiterIfNeccessary(ExpressionBase expression, ExpressionBase parent)
        {
            if(parent == null)
            {
                return expression;
            }
            OperatorExpression operatorExpression = expression as OperatorExpression;
            OperatorExpression parentExpression = parent as OperatorExpression;

            if (operatorExpression == null || parentExpression == null)
            {
                return expression;
            }
            else
            {
                return WrapInDelimiterIfNeccessary(operatorExpression, parentExpression.Type);
            }
        }

        public List<Identity> GetIdentities(List<ExpressionBase> selection)
        {
            var identities = new List<Identity>();

            if (selection.Count == 0)
            {
                return identities;
            }

            ExpressionBase commonParent = GetCommonParent(selection);
            if (commonParent is VariadicOperatorExpression)
            {
                VariadicOperatorExpression variadicParent = commonParent as VariadicOperatorExpression;

                List<ExpressionBase> operandsLeftOfSelection = new List<ExpressionBase>();
                List<ExpressionBase> operandsRightOfSelection = new List<ExpressionBase>();
                List<ExpressionBase> selectedOperands = new List<ExpressionBase>();

                foreach (ExpressionBase operand in variadicParent)
                {
                    if (operand.Selected == false && operand.GetNodesRecursive().Any((n) => n.Selected == true) == false)
                    {
                        if (selectedOperands.Count == 0)
                        {
                            operandsLeftOfSelection.Add(operand);
                        }
                        else
                        {
                            operandsRightOfSelection.Add(operand);
                        }
                    }
                    else
                    {
                        selectedOperands.Add(operand);
                    }
                }

                VariadicOperatorExpression toBeReplaced = new VariadicOperatorExpression(variadicParent.Type, selectedOperands[0].Clone(), selectedOperands[1].Clone());
                List<ExpressionBase> toBeReplacedSelection = new List<ExpressionBase>();
                foreach (ExpressionBase operand in selectedOperands.Skip(2))
                {
                    toBeReplaced.Add(operand.Clone());
                }

                toBeReplacedSelection = toBeReplaced.GetNodesRecursive().Where((n) => n.Selected == true).ToList();
                int c = 0;
                foreach (ExpressionRule rule in rules)
                {
                   

                    var start = new Date().GetTime();

                    ExpressionBase suggestion = rule(toBeReplaced, toBeReplacedSelection);

                    var end = new Date().GetTime();
                    var time = end - start;
                    Console.WriteLine(c++ + " " + time);
                    

                    if (suggestion != null)
                    {
                        ExpressionBase result;
                        if (variadicParent.Count == selectedOperands.Count)
                        {
                            result = suggestion;
                        }
                        else
                        {
                            VariadicOperatorExpression variadicResult = new VariadicOperatorExpression(variadicParent.Type, new NumericExpression(-1), new NumericExpression(-1));
                            variadicResult.Add(operandsLeftOfSelection.Select((o) => o.Clone()).ToList());
                            variadicResult.Add(WrapInDelimiterIfNeccessary(suggestion.Clone(), variadicResult));
                            variadicResult.Add(operandsRightOfSelection.Select((o) => o.Clone()).ToList());
                            variadicResult.RemoveAt(0);
                            variadicResult.RemoveAt(0);
                            result = variadicResult;
                        }
                        identities.Add(new Identity(suggestion, WrapInDelimiterIfNeccessary(result, commonParent.Parent)));
                    }
                }
            }
            else
            {
                int c = 0;
                foreach (ExpressionRule rule in rules)
                {
                    var start = new Date().GetTime();
                    ExpressionBase suggestion = WrapInDelimiterIfNeccessary(rule(commonParent, selection), commonParent.Parent);
                    var end = new Date().GetTime();
                    var time = end - start;
                    Console.WriteLine(c++ + " " + time);
                    if (suggestion != null)
                    {
                        identities.Add(new Identity(suggestion, suggestion));
                    }
                }
            }

            return identities;
        }
    }
}