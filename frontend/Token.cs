using ThreeOneSevenBee.Framework.Expressions;

namespace ThreeOneSevenBee.Framework
{
    public class Token
    {
        public static Token Number(double data)
        {
            return new Token(TokenType.Number, data);
        }

        public static Token Variable(string data)
        {
            return new Token(TokenType.Variable, data);
        }

        public static Token Constant(ConstantType type)
        {
            return new Token(TokenType.Constant, type);
        }

        public static Token Operator(Operator op)
        {
            return new Token(TokenType.Operator, op);
        }

        public static Token Delimiter()
        {
            return new Token(TokenType.Delimiter, "()");
        }

        public static Token Function(string function)
        {
            return new Token(TokenType.Function, function);
        }

        public static Token LeftBracket()
        {
            return new Token(TokenType.LeftBracket, "(");
        }

        public static Token RightBracket()
        {
            return new Token(TokenType.RightBracket, ")");
        }


        public Token(TokenType type, object data)
        {
            Type = type;
            Data = data;
        }

        public TokenType Type { get; set; }

        public object Data { get; set; }
    }

    public enum TokenType
    {
        Number,
        Variable,
        Constant,
        Operator,
        Delimiter,
        Function,
        LeftBracket,
        RightBracket
    }
}