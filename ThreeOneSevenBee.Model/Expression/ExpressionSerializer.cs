namespace ThreeOneSevenBee.Model.Expression
{
    public class ExpressionSerializer
    {
        private ExpressionParser parser = new ExpressionParser();

        public string Serialize(ExpressionBase expression)
        {
            return expression.ToString();
        }

        public ExpressionBase Deserialize(string expression)
        {
            return parser.Parse(expression);
        }
    }
}
