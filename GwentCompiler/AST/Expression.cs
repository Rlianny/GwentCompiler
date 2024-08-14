namespace GwentCompiler;
public interface IExpression : IASTNode;

public abstract class BinaryExpression(IExpression? left, Token op, IExpression? right) : object(), IExpression
{
    public IExpression? Left { get; private set; } = left;
    public IExpression? Right { get; private set; } = right;
    public Token Operator { get; private set; } = op;
}

public abstract class UnaryExpression(Token op, IExpression? right) : IExpression
{
    public IExpression? Right { get; private set; } = right;
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

public class IncrementOrDecrementOperationExpr(Token op, Variable? right) : IExpression
{
    public Variable? Name { get; private set; } = right;
    public Token Operation { get; private set; } = op;
}

public abstract class ContextAccessExpr(Variable variable, Token dot, Token access, IExpression? args) : IExpression
{
    public Token Dot { get; private set; } = dot;
    public Variable Variable { get; private set; } = variable;
    public Token Access { get; private set; } = access;
    public IExpression? Args { get; private set; } = args;
}

public abstract class ContextMethodsExpr(IExpression? contextAccessExpr, Token method, IExpression? args) : IExpression
{
    public IExpression? AccessExpression { get; private set; } = contextAccessExpr;
    public Token Method { get; private set; } = method;
    public IExpression? Args { get; private set; } = args;
}

public class LambdaExpr(Variable variable, Token lambda, IExpression expression) : IExpression
{
    public Variable Variable {get; private set;} = variable;
    public Token Lambda {get; private set;} = lambda;
    public IExpression? Filter {get; private set;} = expression;
}

#region BinaryExpressionSubtypes
public abstract class ArithmeticBinaryExpression(IExpression? left, Token op, IExpression? right) : BinaryExpression(left, op, right);

public abstract class BooleanOperationBinaryExpression(IExpression? left, Token op, IExpression? right) : BinaryExpression(left, op, right);

public abstract class StringOperationBinaryExpression(IExpression? left, Token op, IExpression? right) : BinaryExpression(left, op, right);

public class AndExpr(IExpression? left, Token op, IExpression? right) : BooleanOperationBinaryExpression(left, op, right);

public class OrExpr(IExpression? left, Token op, IExpression? right) : BooleanOperationBinaryExpression(left, op, right);

public class EqualityExpr(IExpression? left, Token op, IExpression? right) : BooleanOperationBinaryExpression(left, op, right);

public class InequalityExpr(IExpression? left, Token op, IExpression? right) : BooleanOperationBinaryExpression(left, op, right);

public class StringConcatenationExpr(IExpression? left, Token op, IExpression? right) : StringOperationBinaryExpression(left, op, right);

public class StringConcatenationSpacedExpr(IExpression? left, Token op, IExpression? right) : StringOperationBinaryExpression(left, op, right);

public class GreaterThanExpr(IExpression? left, Token op, IExpression? right) : BooleanOperationBinaryExpression(left, op, right);

public class GreaterThanOrEqualExpr(IExpression? left, Token op, IExpression? right) : BooleanOperationBinaryExpression(left, op, right);

public class LessThanExpr(IExpression? left, Token op, IExpression? right) : BooleanOperationBinaryExpression(left, op, right);

public class LessThanOrEqualExpr(IExpression? left, Token op, IExpression? right) : BooleanOperationBinaryExpression(left, op, right);

public class AdditionExpr(IExpression? left, Token op, IExpression? right) : ArithmeticBinaryExpression(left, op, right);

public class SubtractionExpr(IExpression? left, Token op, IExpression? right) : ArithmeticBinaryExpression(left, op, right);

public class MultiplicationExpr(IExpression? left, Token op, IExpression? right) : ArithmeticBinaryExpression(left, op, right);

public class DivisionExpr(IExpression? left, Token op, IExpression? right) : ArithmeticBinaryExpression(left, op, right);

public class PowerExpr(IExpression? left, Token op, IExpression? right) : ArithmeticBinaryExpression(left, op, right);

#endregion

#region UnaryExpressionSubtypes
public class NegatedExpr(Token op, IExpression? right) : UnaryExpression(op, right);

public class Subtraction(Token op, IExpression? right) : UnaryExpression(op, right);

#endregion

#region AtomicExpressionSubtypes
public class NumericLiteral(Token? value) : Atom(value);

public class StringLiteral(Token? value) : Atom(value);

public class BooleanLiteral(Token? value) : Atom(value);

# endregion

#region ContextAccessExpressionSubtypes

public class CardPropertyAccessExpr(Variable variable, Token dot, Token access, IExpression? args) : ContextAccessExpr(variable, dot, access, args);

public class TriggerPlayerAccessExpr(Variable variable, Token dot, Token access, IExpression? args) : ContextAccessExpr(variable, dot, access, args);

public class BoardAccessExpr(Variable variable, Token dot, Token access, IExpression? args) : ContextAccessExpr(variable, dot, access, args);

public class HandOfPlayerAccessExpr(Variable variable, Token dot, Token access, IExpression? args) : ContextAccessExpr(variable, dot, access, args);

public class FieldOfPlayerAccessExpr(Variable variable, Token dot, Token access, IExpression? args) : ContextAccessExpr(variable, dot, access, args);

public class GraveyardOfPlayerAccessExpr(Variable variable, Token dot, Token access, IExpression? args) : ContextAccessExpr(variable, dot, access, args);

public class DeckOfPlayerAccessExpr(Variable variable, Token dot, Token access, IExpression? args) : ContextAccessExpr(variable, dot, access, args);

public class CardOwnerAccessExpr(Variable variable, Token dot, Token access, IExpression? args) : ContextAccessExpr(variable, dot, access, args);

#endregion

#region ContextMethodExpressionSubtypes

public class FindMethodExpr(IExpression? access, Token method, IExpression? args) : ContextMethodsExpr(access, method, args);

public class PushMethodExpr(IExpression? access, Token method, IExpression? args) : ContextMethodsExpr(access, method, args);

public class SendBottomMethodExpr(IExpression? access, Token method, IExpression? args) : ContextMethodsExpr(access, method, args);

public class PopMethodExpr(IExpression? access, Token method, IExpression? args) : ContextMethodsExpr(access, method, args);

public class RemoveMethodExpr(IExpression? access, Token method, IExpression? args) : ContextMethodsExpr(access, method, args);

public class ShuffleMethodExpr(IExpression? access, Token method, IExpression? args) : ContextMethodsExpr(access, method, args);

#endregion