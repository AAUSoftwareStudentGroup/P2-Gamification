using System;
using System.Collections.Generic;
using System.Linq;
using ThreeOneSevenBee.Model.Expression;
using ThreeOneSevenBee.Model.Expression.ExpressionRules;

namespace ThreeOneSevenBee.ModelConsole
{
    class Program
    {
        static void Update(ExpressionModel model)
        {
            Console.Write("Identities: ");
            foreach (ExpressionBase identity in model.Identities)
            {
                Console.Write("|" + identity + "|");
            }
            Console.WriteLine();
            Console.WriteLine("Selected: " + model.Selected);
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
            ExpressionModel model;

			/*******/
			model = new ExpressionModel("a+b*2/5", Rules.ItselfRule, Rules.CommutativeRule);

            model.OnChanged += Update;

            model.Select(model.Expression.GetNodesRecursive().ElementAt(4));
            model.Select(model.Expression.GetNodesRecursive().ElementAt(5));

			model.ApplyIdentity(model.Identities[1]);

			/*******/

			model = new ExpressionModel("a^-1", Rules.ItselfRule, Rules.InversePowerRule);

			model.OnChanged += Update;

			model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

            model.ApplyIdentity(model.Identities[1]);

			/*******/

			model = new ExpressionModel("a^0", Rules.ItselfRule, Rules.PowerZeroRule);

			model.OnChanged += Update;

			model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

			model.ApplyIdentity(model.Identities[1]);

        }
    }
}
