using System;
using Bridge;
using Bridge.Html5;
using Bridge.QUnit;
using ThreeOneSevenBee.Framework.Euclidean;

namespace ThreeOneSevenBee.Framework.Tests
{
    public class App
    {
        [Ready]
        public static void Main()
        {
            QUnit.Module("Euclidean");
            QUnit.Test("Constructor Vector2", (assert) =>
            {
                assert.Expect(1);

                var vectorZero = new Vector2(0, 0);
                assert.Ok(Math.Abs(vectorZero.X) < double.Epsilon && Math.Abs(vectorZero.Y) < double.Epsilon, "Vector2 Zero");
            });
        }
    }
}