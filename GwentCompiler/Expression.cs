namespace GwentCompiler;
public interface IExpression
{
    public Object Accept(IExprVisitor<Object> visitor);
}

public abstract class BinaryExpression : IExpression
{
    public IExpression Left {get; private set;}
    public IExpression Right {get; private set;}
    public Token Operator {get; private set;}
    public BinaryExpression(IExpression left, Token op, IExpression rigth) : base()
    {
        Left = left;
        Right = rigth;
        Operator = op;
    }

    public Object Accept(IExprVisitor<Object> visitor)
    {
        return visitor.VisitBinaryExpression(this);
    }
}

public abstract class UnaryExpression : IExpression
{
    public IExpression Rigth {get; private set;}
    public Token Operator {get; private set;}
    public UnaryExpression(Token op, IExpression rigth)
    {
        Rigth = rigth;
        Operator = op;
    }

    public Object Accept(IExprVisitor<object> visitor)
    {
        return visitor.VisitUnaryExpression(this);
    }
}

public class GroupExpression : IExpression
{
    public IExpression Expression {get; private set;}
    public GroupExpression(IExpression expr)
    {
        Expression = expr;
    }

    public Object Accept(IExprVisitor<object> visitor)
    {
        return visitor.VisitGroupExpression(this);
    }
}

public abstract class Atom : IExpression
{
    public Token Value {get; private set;}
    public Atom(Token value)
    {
        Value = value;
    }

    public Object Accept(IExprVisitor<object> visitor)
    {
        return visitor.VisitAtom(this);
    }
}

public class AndExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class OrExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class EqualityExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class UnequalityExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class StringConcatenationExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class StringConcatenationSpacedExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class GreaterThanExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class GreaterThanOrEqualExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class LessThanExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class LessThanOrEqualExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class AdditionExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class SubtractionExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class MultiplicationExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class DivisionExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class PowerExpr(IExpression left, Token op, IExpression rigth) : BinaryExpression(left, op, rigth);

public class NegatedExpr(Token op, IExpression rigth) : UnaryExpression(op, rigth);

public class Substraction(Token op, IExpression rigth) : UnaryExpression(op, rigth);

public class NumericLiteral(Token value) : Atom(value);

public class StringLiteral(Token value) : Atom(value);

public class BooleanLiteral(Token value) : Atom(value);

public class Variable : IExpression
{
    public Token Value {get; private set;}

    public Variable(Token value)
    {
        Value = value;
    }
    public object Accept(IExprVisitor<object> visitor)
    {
        return visitor.VisitVariableExpression(this);
    }
}

public class AssignmentExpr : IExpression
{   
    public Variable Name {get; private set;}
    public IExpression Value {get; private set;}
    public AssignmentExpr(Variable name, IExpression value)
    {
        Name = name;
        Value = value;
    }

    public object Accept(IExprVisitor<object> visitor)
    {
        return visitor.VisitAssignmentExpression(this);
    }
}

public class PropertyAccessExpr : IExpression
{
    public Token Indexer {get; private set;}
    public IExpression Value {get; private set;}
    public IExpression Args {get; private set;}
    public PropertyAccessExpr(IExpression value, IExpression args, Token indexer)
    {
        Value = value;
        Args = args;
        Indexer = indexer;
    }

    public object Accept(IExprVisitor<object> visitor)
    {
        throw new NotImplementedException();
    }
}

public class CallToMethodExpr : IExpression
{
    public IExpression Value {get; private set;}
    public IExpression Args {get; private set;}
    public CallToMethodExpr(IExpression token, IExpression args)
    {
        Value = token;
        Args = args;
    }

    public Object Accept(IExprVisitor<object> visitor)
    {
        throw new NotImplementedException();
    }
}