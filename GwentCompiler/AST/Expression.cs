namespace GwentCompiler;
public interface IExpression : IASTNode;

public abstract class BinaryExpression(IExpression? left, Token op, IExpression? rigth) : object(), IExpression
{
    public IExpression? Left { get; private set; } = left;
    public IExpression? Right { get; private set; } = rigth;
    public Token Operator { get; private set; } = op;
}

public abstract class UnaryExpression(Token op, IExpression? rigth) : IExpression
{
    public IExpression? Rigth { get; private set; } = rigth;
    public Token Operator { get; private set; } = op;
}

public class GroupExpression(IExpression? expr) : IExpression
{
    public IExpression? Expression { get; private set; } = expr;
}

public abstract class Atom(Token? value) : IExpression
{
    public Token? Value { get; private set; } = value;
}

public class Variable(Token value) : IExpression
{
    public Token Value { get; private set; } = value;
}

public class AssignmentExpr(Variable? name, IExpression? value) : IExpression
{
    public Variable? Name { get; private set; } = name;
    public IExpression? Value { get; private set; } = value;
}

public class IncrementOrDecrementOperationExpr(Token op, Variable? rigth) : IExpression
{
    public Variable? Name { get; private set; } = rigth;
    public Token Operation { get; private set; } = op;
}

public abstract class ContextAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : IExpression
{
    public Token Dot { get; private set; } = dot;
    public Variable Variable { get; private set; } = variable;
    public Token Acces { get; private set; } = acces;
    public IExpression? Args { get; private set; } = args;
}

public abstract class ContextMethodsExpr(IExpression? contextAccessExpr, Token method, IExpression? args) : IExpression
{
    public IExpression? AccessExpression { get; private set; } = contextAccessExpr;
    public Token Method { get; private set; } = method;
    public IExpression? Args { get; private set; } = args;
}

#region BinaryExpressionSubtypes
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

#endregion

#region UnaryExpressionSubtypes
public class NegatedExpr(Token op, IExpression? rigth) : UnaryExpression(op, rigth);

public class Substraction(Token op, IExpression? rigth) : UnaryExpression(op, rigth);

#endregion

#region AtomicExpressionSubtypes
public class NumericLiteral(Token? value) : Atom(value);

public class StringLiteral(Token? value) : Atom(value);

public class BooleanLiteral(Token? value) : Atom(value);

# endregion

#region ContextAccessExpressionSubtypes

public class CardPropertyAccesExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class TriggerPlayerAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class BoardAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class HandOfPlayerAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class FieldOfPlayerAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class GraveyardOfPlayerAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class DeckOfPlayerAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

public class CardOwnerAccessExpr(Variable variable, Token dot, Token acces, IExpression? args) : ContextAccessExpr(variable, dot, acces, args);

#endregion

#region ContextMethodExpressionSyptypes

public class FindMethodExpr(IExpression? acces, Token method, IExpression? args) : ContextMethodsExpr(acces, method, args);

public class PushMethodExpr(IExpression? acces, Token method, IExpression? args) : ContextMethodsExpr(acces, method, args);

public class SendBottomMethodExpr(IExpression? acces, Token method, IExpression? args) : ContextMethodsExpr(acces, method, args);

public class PopMethodExpr(IExpression? acces, Token method, IExpression? args) : ContextMethodsExpr(acces, method, args);

public class RemoveMethodExpr(IExpression? acces, Token method, IExpression? args) : ContextMethodsExpr(acces, method, args);

public class ShuffleMethodExpr(IExpression? acces, Token method, IExpression? args) : ContextMethodsExpr(acces, method, args);

#endregion