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
            ExpressionModel model = new ExpressionModel("b^-2");
            model.Select(model.Expression.GetNodesRecursive().ElementAt(0));
            model.Select(model.Expression.GetNodesRecursive().ElementAt(1));
            model.Selection.ForEach((selected) => selected.PrettyPrint());
            model.Identities.ForEach((id) => id.Result.PrettyPrint());


            // Template for testing Size
            ExpressionModel model2 = new ExpressionModel("a*(1-b)");
            model2.Expression.PrettyPrint();
            Console.WriteLine(model2.Expression.Size);
            //

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
