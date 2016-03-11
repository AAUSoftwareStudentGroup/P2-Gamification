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

			model.ApplyIdentity(model.Identities[model.Identities.Count-1]);
			Console.WriteLine(model.Expression.Value.Equals("a+2*b/5")+"\n-----\n");

			/*******/

			model = new ExpressionModel("a^-1", Rules.ItselfRule, Rules.InversePowerRule);

			model.OnChanged += Update;

			model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

			model.ApplyIdentity(model.Identities[model.Identities.Count-1]);
			Console.WriteLine(model.Expression.Value.Equals("1/a^1")+"\n-----\n");

			/*******/

			model = new ExpressionModel("a^0", Rules.ItselfRule, Rules.PowerZeroRule);

			model.OnChanged += Update;

			model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

			model.ApplyIdentity(model.Identities[model.Identities.Count-1]);
			Console.WriteLine(model.Expression.Value.Equals("1")+"\n-----\n");

			/*******/

			model = new ExpressionModel("a/c+b/c", Rules.ItselfRule, Rules.FractionAddRule);

			model.OnChanged += Update;

			model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

			model.ApplyIdentity(model.Identities[model.Identities.Count-1]);
			Console.WriteLine(model.Expression.Value.Equals("a+b/c")+"\n-----\n");

			/*******/

			model = new ExpressionModel("a/b*c/d", Rules.ItselfRule, Rules.FractionMultiplyRule);

			model.OnChanged += Update;

			model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

			model.ApplyIdentity(model.Identities[model.Identities.Count-1]);
			Console.WriteLine(model.Expression.Value.Equals("a*c/b*d")+"\n-----\n");

			/*******/

			model = new ExpressionModel("a*b/c", Rules.ItselfRule, Rules.FractionVariableMultiplyRule);

			model.OnChanged += Update;

			model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

			model.ApplyIdentity(model.Identities[model.Identities.Count-1]);
			Console.WriteLine(model.Expression.Value.Equals("a*b/c")+"\n-----\n");

			/*******/

			model = new ExpressionModel("a^n*a^p", Rules.ItselfRule, Rules.SameVariableDifferentExpMultiplyRule);

			model.OnChanged += Update;

			model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

			model.ApplyIdentity(model.Identities[model.Identities.Count-1]);
			Console.WriteLine(model.Expression.Value.Equals("a^n+p")+"\n-----\n");

			/*******/

            model = new ExpressionModel("1*a+1*b", Rules.ItselfRule, Rules.MultiplyingWithOneRule);

            model.OnChanged += Update;

            model.Select(model.Expression.GetNodesRecursive().ElementAt(1));

            model.ApplyIdentity(model.Identities[model.Identities.Count - 1]);
            Console.WriteLine(model.Expression.Value.Equals("a+1*b") + "\n-----\n");

            /*******/

            model = new ExpressionModel("b/1", Rules.ItselfRule, Rules.DenumeratorIsOneRule);

			model.OnChanged += Update;

			model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

			model.ApplyIdentity(model.Identities[model.Identities.Count-1]);
			Console.WriteLine(model.Expression.Value.Equals("b")+"\n-----\n");

            /*******/

            model = new ExpressionModel("0/b", Rules.ItselfRule, Rules.NumeratorIsZero);

            model.OnChanged += Update;

            model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

            model.ApplyIdentity(model.Identities[model.Identities.Count - 1]);
            Console.WriteLine(model.Expression.Value.Equals("0") + "\n-----\n");

            /*******/

            model = new ExpressionModel("-a/-b", Rules.ItselfRule, Rules.RemovingUnaryMinusInDivisionRule);

            model.OnChanged += Update;

            model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

            model.ApplyIdentity(model.Identities[model.Identities.Count - 1]);
            Console.WriteLine(model.Expression.Value.Equals("a/b") + "\n-----\n");

            /*******/

            model = new ExpressionModel("--a", Rules.ItselfRule, Rules.DoubleMinusEqualsPlus);

            model.OnChanged += Update;

            model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

            model.ApplyIdentity(model.Identities[model.Identities.Count - 1]);
            Console.WriteLine(model.Expression.Value.Equals("a") + "\n-----\n");

            /*******/

            model = new ExpressionModel("sqrt(a^2)", Rules.ItselfRule, Rules.SquareRootAndPowerRule);

            model.OnChanged += Update;

            model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

            model.ApplyIdentity(model.Identities[model.Identities.Count - 1]);
            Console.WriteLine(model.Expression.Value.Equals("a") + "\n-----\n");


            /*******/

            model = new ExpressionModel("a^n^n", Rules.ItselfRule, Rules.VariableWithTwoExponent);

            model.OnChanged += Update;

            model.Select(model.Expression.GetNodesRecursive().ElementAt(0));

            model.ApplyIdentity(model.Identities[model.Identities.Count - 1]);
            Console.WriteLine(model.Expression.Value.Equals("a^n*n") + "\n-----\n");

            Console.ReadKey();

        }
    }
}
