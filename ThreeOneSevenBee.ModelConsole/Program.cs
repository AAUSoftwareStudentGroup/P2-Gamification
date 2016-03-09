using System;
using System.Collections.Generic;
using System.Linq;
using ThreeOneSevenBee.Model.Expression;
using ThreeOneSevenBee.Model.Expression.ExpressionRules;

namespace ThreeOneSevenBee.ModelConsole
{
    class Program
    {
        static void UpdateIdentities(ExpressionModel model)
        {
            Console.Write("Identities: ");
            foreach (ExpressionBase identity in model.Identities)
            {
                Console.Write("|" + identity + "|");
            }
            Console.WriteLine();
        }

        static void UpdateSelection(ExpressionModel model)
        {
            Console.Write("Selection: ");
            foreach (ExpressionBase selected in model.Selection)
            {
                Console.Write("|" + selected + "|");
            }
            Console.WriteLine();
            Console.Write("Current expression: ");
            Console.WriteLine(model.Expression);
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            ExpressionModel model = new ExpressionModel("a+b*2", Rules.ItselfRule, Rules.CommunicativeRule);

            model.OnIdentitiesChanged += UpdateIdentities;
            model.OnSelectionChanged += UpdateSelection;

            model.Select(model.Expression);
            Console.ReadKey();
            model.Select(model.Expression);
            Console.ReadKey();
            model.Select(model.Expression.GetNodesRecursive().ElementAt(2));
            Console.ReadKey();
            model.Select(model.Expression.GetNodesRecursive().ElementAt(1));
            Console.ReadKey();
        }
    }
}
