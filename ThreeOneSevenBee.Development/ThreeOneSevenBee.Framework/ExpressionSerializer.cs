namespace ThreeOneSevenBee.Framework
{
    public class ExpressionSerializer
    {
        private ExpressionParser parser = new ExpressionParser();

        public string Serialize(Expression expression)
        {
            return expression.ToString();
        }

        public Expression Deserialize(string expression)
        {
            return parser.Parse(expression);
        }
    }
}
