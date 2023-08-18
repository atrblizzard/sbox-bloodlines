using System.Collections.Generic;

namespace Vampire
{
    public enum OperatorType
    {
        And,
        Or,
        Not,
        Equals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        NotEquals
    }

    public abstract class Expression {}

    public class BinaryExpression : Expression
    {
        public Expression Left { get; set; }
        public OperatorType Operator { get; set; }
        public Expression Right { get; set; }
    }

    public class UnaryExpression : Expression
    {
        public OperatorType Operator { get; set; }
        public Expression Operand { get; set; }
    }

    public class LiteralExpression : Expression
    {
        public string Literal { get; set; }
    }

    public class FunctionExpression : Expression
    {
        public string FunctionName { get; set; }
        public List<Expression> Arguments { get; set; }
    }
}
