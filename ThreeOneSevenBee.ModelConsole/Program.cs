using System;
using System.Collections.Generic;
using System.Linq;
using ThreeOneSevenBee.Model.Expression;
using ThreeOneSevenBee.Model.Expression.ExpressionRules;
using ThreeOneSevenBee.Model.Expression.Expressions;

namespace ThreeOneSevenBee.ModelConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ExpressionModel model = new ExpressionModel("b^-2", null);
            model.Select(model.Expression.GetNodesRecursive().ElementAt(0));
            model.Select(model.Expression.GetNodesRecursive().ElementAt(1));
            model.Selection.ForEach((selected) => selected.PrettyPrint());
            model.Identities.ForEach((id) => id.Result.PrettyPrint());

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
