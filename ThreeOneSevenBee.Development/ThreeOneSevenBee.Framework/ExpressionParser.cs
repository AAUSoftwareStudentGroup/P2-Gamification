using System;
using System.Collections.Generic;
using System.Linq;
using ThreeOneSevenBee.Framework.Expressions;

namespace ThreeOneSevenBee.Framework
{
    public class ExpressionParser
    {
        /// <summary>
        /// The local decimal separator.
        /// </summary>
        private static char DecimalSeparator = (1.1).ToString()[1];

        /// <summary>
        /// The types of operators.
        /// </summary>
        private List<Operator> ops = new List<Operator>()
        {
            new Operator("^", 4, OperatorAssociativity.Right),
            new Operator("*", 3, OperatorAssociativity.Left),
            new Operator("/", 3, OperatorAssociativity.Left),
            new Operator("+", 2, OperatorAssociativity.Left),
            new Operator("-", 2, OperatorAssociativity.Left),
        };

        private List<string> functions = new List<string>()
        {
            "sqrt",
        };

        /// <summary>
        /// Checks if the i'th position of input is whitespace.
        /// </summary>
        private bool IsWhiteSpace(string input, ref int i)
        {
            if (i >= input.Length)
                return false;

            if (!char.IsWhiteSpace(input[i]))
                return false;
            i++;
            return true;
        }

        /// <summary>
        /// Checks if the i'th position of input is a number or a decimal point.
        /// </summary>
        private bool IsNumberOrDecimal(string input, ref int i)
        {
            if (i >= input.Length)
                return false;

            return (char.IsDigit(input[i]) || input[i] == DecimalSeparator);
        }

        /// <summary>
        /// Checks if the i'th position and onwards contains a number. The number is returned.
        /// </summary>
        private bool IsNumber(string input, ref int i, out double number)
        {
            number = double.NaN;
            if (i >= input.Length)
                return false;

            if (!IsNumberOrDecimal(input, ref i))
                return false;

            var numberString = "";
            while (IsNumberOrDecimal(input, ref i))
            {
                numberString += input[i];
                i++;
            }

            return double.TryParse(numberString, out number);
        }

        private bool IsFunction(string input, ref int i, out string function)
        {
            function = null;
            if (i >= input.Length)
                return false;

            foreach (var func in functions)
            {
                if (i + func.Length <= input.Length && input.Substring(i, func.Length).ToLower() == func)
                {
                    function = func;
                    i += func.Length;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the i'th position and onwards is pi.
        /// </summary>
        private bool IsPi(string input, ref int i)
        {
            if (i >= input.Length)
                return false;

            if (i + 1 < input.Length && input.Substring(i, 2).ToLower() == "pi")
            {
                i += 2;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the i'th position is a valid variable. The variable is returned.
        /// </summary>
        private bool IsVariable(string input, ref int i, out string variable)
        {
            variable = null;
            if (i >= input.Length)
                return false;

            const string validLetters = "abcdefghijklmnopqrstuvwz";

            foreach (var validLeter in validLetters)
            {
                if (input[i] == validLeter)
                {
                    variable = input[i].ToString();
                    i++;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the i'th postion is an operator. The operator is returned.
        /// </summary>
        private bool IsOperator(string input, ref int i, out Operator op)
        {
            op = null;
            if (i >= input.Length)
                return false;

            var opSymbol = input[i];
            op = ops.FirstOrDefault(o => o.Symbol == opSymbol.ToString());
            if (op == null)
                return false;
            i++;
            return true;
        }

        private Queue<Token> output = new Queue<Token>();

        private Stack<Token> operators = new Stack<Token>();

        public List<Token> InFixToPostFix(string inFix)
        {
            var i = 0;
            //var n = 0;
            Token lastToken = null;
            while (i < inFix.Length)
            {
                if (IsWhiteSpace(inFix, ref i))
                    continue;
                //Console.WriteLine("Step " + n++);
                //Console.WriteLine("output:    " + this.output.Aggregate("", (s, t) => s + t.Data + " "));
                //Console.WriteLine("operators: " + this.operators.Aggregate("", (s, t) => s + t.Data + " "));

                double number;
                if (IsNumber(inFix, ref i, out number))
                {
                    lastToken = Token.Number(number);
                    this.output.Enqueue(lastToken);
                    continue;
                }

                string func;
                if (IsFunction(inFix, ref i, out func))
                {
                    lastToken = Token.Function(func);
                    operators.Push(lastToken);
                    continue;
                }

                if (IsPi(inFix, ref i))
                {
                    lastToken = Token.Constant(ConstantType.Pi);
                    this.output.Enqueue(lastToken);
                    continue;
                }

                string variable;
                if (IsVariable(inFix, ref i, out variable))
                {
                    lastToken = Token.Variable(variable);
                    this.output.Enqueue(lastToken);
                    continue;
                }

                Operator op1;
                if (IsOperator(inFix, ref i, out op1))
                {
                    //Console.WriteLine("last: " + lastToken.Data);
                    // unary check
                    if (op1.Symbol == "-" && (lastToken == null || lastToken.Type == TokenType.Operator))
                    {
                        lastToken = Token.Operator(new Operator("~", 5, OperatorAssociativity.Right));
                        operators.Push(lastToken);
                    }
                    else // if not unary then it is binary
                    {
                        while (operators.Any())
                        {
                            var precedence = 0;
                            if (operators.Peek().Data is Operator)
                            {
                                precedence = (operators.Peek().Data as Operator).Precedence;
                            }
                            if ((op1.Associativity == OperatorAssociativity.Left && op1.Precedence <= precedence) ||
                                (op1.Associativity == OperatorAssociativity.Right && op1.Precedence < precedence))
                            {
                                this.output.Enqueue(operators.Pop());
                                continue;
                            }
                            else if (operators.Peek().Type == TokenType.Function)
                            {
                                this.output.Enqueue(operators.Pop());
                                continue;
                            }

                            break;
                        }
                        lastToken = Token.Operator(op1);
                        operators.Push(lastToken);
                    }

                    continue;
                }

                if (inFix[i] == '(')
                {
                    lastToken = Token.Operator(new Operator("("));
                    operators.Push(lastToken);
                    i++;
                    continue;
                }

                if (inFix[i] == ')')
                {
                    while (operators.Any())
                    {
                        var tok = operators.Pop();
                        if (tok.Type == TokenType.Operator && (tok.Data as Operator).Equals("("))
                            break;
                        this.output.Enqueue(tok);
                    }
                    lastToken = Token.Delimiter();
                    this.output.Enqueue(lastToken);
                    i++;
                    continue;
                }

                throw new InvalidOperationException("Unexpected token: " + inFix[i]);
            }

            //Console.WriteLine("Final");
            //Console.Write("output:    " + this.output.Aggregate("", (s, t) => s + t.Data + " "));
            //Console.WriteLine(this.operators.Aggregate("", (s, t) => s + t.Data + " "));

            var output = new List<Token>();
            while (this.output.Any())
                output.Add(this.output.Dequeue());
            while (operators.Any())
                output.Add(operators.Pop());

            return output;
        }

        public Expression Parse(List<Token> postFix)
        {
            var stack = new Stack<Expression>();
            var variables = new Dictionary<string, VariableExpression>();
            Expression root = null;
            foreach (var token in postFix)
            {
                switch (token.Type)
                {
                    case TokenType.Function:
                        root = new FunctionExpression(stack.Pop(), (string)token.Data);
                        stack.Push(root);
                        break;

                    case TokenType.Delimiter:
                        root = new DelimiterExpression(stack.Pop());
                        stack.Push(root);
                        break;

                    case TokenType.Number:
                        root = new NumericExpression((double)token.Data);
                        stack.Push(root);
                        break;

                    case TokenType.Constant:
                        root = new ConstantExpression((ConstantType)token.Data);
                        stack.Push(root);
                        break;

                    case TokenType.Variable:
                        VariableExpression variable;
                        var variableString = (string)token.Data;
                        if (!variables.TryGetValue(variableString, out variable))
                        {
                            variable = new VariableExpression((string)token.Data);
                            variables[variableString] = variable;
                        }
                        root = variable;
                        stack.Push(root);
                        break;

                    case TokenType.Operator:
                        switch (token.Data.ToString())
                        {
                            case "~":
                                root = new UnaryMinusExpression(stack.Pop());
                                stack.Push(root);
                                break;

                            case "+":
                                var right = stack.Pop();
                                var left = stack.Pop();
                                root = new OperatorExpression(left, right, OperatorType.Add);
                                stack.Push(root);
                                break;

                            case "-":
                                right = stack.Pop();
                                left = stack.Pop();
                                root = new OperatorExpression(left, right, OperatorType.Subtract);
                                stack.Push(root);
                                break;

                            case "*":
                                right = stack.Pop();
                                left = stack.Pop();
                                root = new OperatorExpression(left, right, OperatorType.Multiply);
                                stack.Push(root);
                                break;

                            case "/":
                                right = stack.Pop();
                                left = stack.Pop();
                                root = new OperatorExpression(left, right, OperatorType.Divide);
                                stack.Push(root);
                                break;

                            case "^":
                                right = stack.Pop();
                                left = stack.Pop();
                                root = new OperatorExpression(left, right, OperatorType.Power);
                                stack.Push(root);
                                break;
                        }

                        break;
                }
            }
            return root;
        }

        public Expression Parse(string inFix)
        {
            return Parse(InFixToPostFix(inFix));
        }
    }
}
