namespace GwentCompiler;
public interface IExpression : IASTNode
{

}

public abstract class BinaryExpression : IExpression
{
    public IExpression? Left { get; private set; }
    public IExpression? Right { get; private set; }
    public Token Operator { get; private set; }
    public BinaryExpression(IExpression? left, Token op, IExpression? rigth) : base()
    {
        Left = left;
        Right = rigth;
        Operator = op;
    }
}

public abstract class UnaryExpression : IExpression
{
    public IExpression? Rigth { get; private set; }
    public Token Operator { get; private set; }
    public UnaryExpression(Token op, IExpression? rigth)
    {
        Rigth = rigth;
        Operator = op;
    }
}

public class GroupExpression : IExpression
{
    public IExpression? Expression { get; private set; }
    public GroupExpression(IExpression? expr)
    {
        Expression = expr;
    }
}

public abstract class Atom : IExpression
{
    public Token? Value { get; private set; }
    public Atom(Token? value)
    {
        Value = value;
    }
}

public abstract class ArithmeticBinaryExpression(IExpression? left, Token op, IExpression? rigth) : BinaryExpression(left, op, rigth);

public abstract class BooleanOperationBinaryExpression(IExpression? left, Token op, IExpression? rigth) : BinaryExpression(left, op, rigth);

public abstract class StringOperationBinaryExpression(IExpression? left, Token op, IExpression? rigth) : BinaryExpression(left, op, rigth);

public class AndExpr(IExpression? left, Token op, IExpression? rigth) : BooleanOperationBinaryExpression(left, op, rigth);

public class OrExpr(IExpression? left, Token op, IExpression? rigth) : BooleanOperationBinaryExpression(left, op, rigth);

public class EqualityExpr(IExpression? left, Token op, IExpression? rigth) : BooleanOperationBinaryExpression(left, op, rigth);

public class InequalityExpr(IExpression? left, Token op, IExpression? rigth) : BooleanOperationBinaryExpression(left, op, rigth);

public class StringConcatenationExpr(IExpression? left, Token op, IExpression? rigth) : StringOperationBinaryExpression(left, op, rigth);

public class StringConcatenationSpacedExpr(IExpression? left, Token op, IExpression? rigth) : StringOperationBinaryExpression(left, op, rigth);

public class GreaterThanExpr(IExpression? left, Token op, IExpression? rigth) : BooleanOperationBinaryExpression(left, op, rigth);

public class GreaterThanOrEqualExpr(IExpression? left, Token op, IExpression? rigth) : BooleanOperationBinaryExpression(left, op, rigth);

public class LessThanExpr(IExpression? left, Token op, IExpression? rigth) : BooleanOperationBinaryExpression(left, op, rigth);

public class LessThanOrEqualExpr(IExpression? left, Token op, IExpression? rigth) : BooleanOperationBinaryExpression(left, op, rigth);

public class AdditionExpr(IExpression? left, Token op, IExpression? rigth) : ArithmeticBinaryExpression(left, op, rigth);

public class SubstractionExpr(IExpression? left, Token op, IExpression? rigth) : ArithmeticBinaryExpression(left, op, rigth);

public class MultiplicationExpr(IExpression? left, Token op, IExpression? rigth) : ArithmeticBinaryExpression(left, op, rigth);

public class DivisionExpr(IExpression? left, Token op, IExpression? rigth) : ArithmeticBinaryExpression(left, op, rigth);

public class PowerExpr(IExpression? left, Token op, IExpression? rigth) : ArithmeticBinaryExpression(left, op, rigth);

public class NegatedExpr(Token op, IExpression? rigth) : UnaryExpression(op, rigth);

public class Substraction(Token op, IExpression? rigth) : UnaryExpression(op, rigth);

public class NumericLiteral(Token? value) : Atom(value);

public class StringLiteral(Token? value) : Atom(value);

public class BooleanLiteral(Token? value) : Atom(value);

public class Variable : IExpression
{
    public Token? Value { get; private set; }

    public Variable(Token? value)
    {
        Value = value;
    }
}

public class AssignmentExpr : IExpression
{
    public Variable? Name { get; private set; }
    public IExpression? Value { get; private set; }
    public AssignmentExpr(Variable? name, IExpression? value)
    {
        Name = name;
        Value = value;
    }
}

public class IncrementOrDecrementOperationExpr : IExpression
{
    public Variable? Name {get; private set;}
    public Token Operation {get; private set;} 

    public IncrementOrDecrementOperationExpr(Token op, Variable? rigth)
    {
        Operation = op;
        Name = rigth;
    }
}

public abstract class ContextAccessExpr : IExpression
{
    public Token Dot { get; private set; }
    public Variable Variable { get; private set; }
    public Token Acces { get; private set;}
    public IExpression? Args{get; private set;}
    public ContextAccessExpr(Variable variable, Token dot, Token acces, IExpression? args)
    {
        Variable = variable;
        Dot = dot;
        Acces = acces;
        Args = args;
    }
}

public class TriggerPlayerAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class BoardAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class HandOfPlayerAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class FieldOfPlayerAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class GraveyardOfPlayerAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class DeckOfPlayerAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class CardOwnerAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class ContextMethodsExpr : IExpression
{
    public IExpression? AccessExpression { get; private set; }
    public Token Method { get; private set; }
    public IExpression? Args{get; private set;}
    public ContextMethodsExpr(IExpression? contextAccessExpr, Token method, IExpression? args)
    {
        AccessExpression = contextAccessExpr;
        Method = method;
        Args = args;
    }
}

public class FindMethodExpr(IExpression? acces, Token method, IExpression? args) : ContextMethodsExpr(acces, method, args);

public class PushMethodExpr(IExpression? acces, Token method, IExpression? args) : ContextMethodsExpr(acces, method, args);

public class SendBottomMethodExpr(IExpression? acces, Token method, IExpression? args) : ContextMethodsExpr(acces, method, args);

public class PopMethodExpr(IExpression? acces, Token method, IExpression? args) : ContextMethodsExpr(acces, method, args);

public class RemoveMethodExpr(IExpression? acces, Token method, IExpression? args) : ContextMethodsExpr(acces, method, args);

public class ShuffleMethodExpr(IExpression? acces, Token method, IExpression? args) : ContextMethodsExpr(acces, method, args);