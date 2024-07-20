using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace GwentCompiler;

// A la hora de usar la interface IVisitor en los Statements el tipo de retorno debería ser void, pero C# no me permite
// pasar void como tipo. La mejor sugerencia de Phind fue declararme un struct que representara el void, y lo nombré VoidType;
// ¿Hay alguna manera más elegante de hacerlo? No me gusta demasiado.

public class Interpreter : VisitorBase<Object>
{
    private Environment environment = new Environment();

    #region Principal Methods

    public void Interpret(List<IStatement> statements)
    {
        try
        {
            foreach (var statement in statements)
            {
                Execute(statement);
            }
        }
        catch (RuntimeError ex)
        {
            GenerateError(ex.Message, ex.CodeLocation);
        }
    }

    private Object? Evaluate(IExpression expression)
    {
        return VisitBase(expression);
    }

    private void Execute(IStatement statement)
    {
        VisitBase(statement);
    }

    public void ExecuteBlock(BlockStmt block, Environment environment)
    {
        Environment previous = this.environment;
        try
        {
            this.environment = environment;
            foreach (var stmt in block.Statements)
            {
                Execute(stmt);
            }
        }
        finally
        {
            this.environment = previous;
        }
    }
    #endregion

    #region ExpressionVisitor

    public object? Visit(AndExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is bool leftBool && right is bool rigthBool) return leftBool && rigthBool;
        else throw new RuntimeError("The operands must to be a boolean", expr.Operator.Location);
    }

    public object? Visit(OrExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is bool leftBool && right is bool rigthBool) return leftBool || rigthBool;
        else throw new RuntimeError("The operands must to be a boolean", expr.Operator.Location);
    }

    public object? Visit(EqualityExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        return left?.Equals(right);
    }

    public object? Visit(InequalityExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        return !left?.Equals(right);
    }

    public object? Visit(StringConcatenationExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is string leftString && right is string rigthString) return leftString + rigthString;
        else throw new RuntimeError("The operands must to be a string", expr.Operator.Location);
    }

    public object? Visit(StringConcatenationSpacedExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is string leftString && right is string rigthString) return leftString + " " + rigthString;
        else throw new RuntimeError("The operands must to be a string", expr.Operator.Location);
    }

    public object? Visit(GreaterThanExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is int leftInt && right is int rigthInt) return leftInt > rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(GreaterThanOrEqualExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is int leftInt && right is int rigthInt) return leftInt >= rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(LessThanExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is int leftInt && right is int rigthInt) return leftInt < rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(LessThanOrEqualExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is int leftInt && right is int rigthInt) return leftInt <= rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(AdditionExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is int leftInt && right is int rigthInt) return leftInt + rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(SubstractionExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is int leftInt && right is int rigthInt) return leftInt - rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(MultiplicationExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is int leftInt && right is int rigthInt) return leftInt * rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(DivisionExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is int leftInt && right is int rigthInt) return leftInt / rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(PowerExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is int leftInt && right is int rigthInt) return Math.Pow(leftInt, rigthInt);
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(NegatedExpr expr)
    {
        var right = Evaluate(expr.Rigth);

        if (right is bool rightBool) return !rightBool;
        else throw new RuntimeError("The operand must to be a boolean", expr.Operator.Location);
    }

    public object? Visit(Substraction substraction)
    {
        var right = Evaluate(substraction.Rigth);

        if (right is int rightInt) return -rightInt;
        else throw new RuntimeError("The operand must to be a number", substraction.Operator.Location);
    }

    public object Visit(NumericLiteral literal)
    {
        return int.Parse(literal.Value.Lexeme);
    }

    public object Visit(StringLiteral literal)
    {
        return Stringify(literal.Value.Lexeme);
    }

    public object Visit(BooleanLiteral literal)
    {
        return bool.Parse(literal.Value.Lexeme);
    }

    public object Visit(Variable variable)
    {
        return environment.Get(variable.Value);
    }

    public object? Visit(GroupExpression group)
    {
        return Evaluate(group.Expression);
    }

    public object? Visit(AssignmentExpr expr)
    {
        var value = Evaluate(expr.Value);
        environment.Assign(expr.Name.Value.Lexeme, value);
        return value;
    }

    public object Visit(PropertyAccessExpr expr)
    {
        throw new NotImplementedException();
    }

    public object Visit(CallToMethodExpr expr)
    {
        throw new NotImplementedException();
    }

    public object Visit(IncrementOrDecrementOperationExpr expr)
    {
        var value = environment.Get(expr.Name.Value);

        if (value is int valueInt)
        {
            if (expr.Operation.Subtype == TokenSubtypes.PostDecrement) valueInt--;
            else valueInt++;
            environment.Assign(expr.Name.Value.Lexeme, valueInt);
            return valueInt;
        }
        else throw new RuntimeError("The operand must to be a number", expr.Operation.Location);
    }

    #endregion
    #region StatementsVisitor;

    public object? Visit(ExpressionStmt expressionStatement)
    {
        Evaluate(expressionStatement.Expression);
        return null;
    }

    public object? Visit(PrintStmt printStatement)
    {
        Object value = Evaluate(printStatement.Expression);
        System.Console.WriteLine(Stringify(value));
        return null;
    }

    public object? Visit(BlockStmt block)
    {
        ExecuteBlock(block, new Environment(environment));
        return null;
    }

    public object? Visit(IfStmt ifStatement)
    {
        if (IsTruthy(Evaluate(ifStatement.Condition))) Execute(ifStatement.ThenBranch);
        else if (ifStatement.ElseBranch != null) Execute(ifStatement.ElseBranch);
        return null;
    }

    public object? Visit(WhileStmt whileStmt)
    {
        while (IsTruthy(Evaluate(whileStmt.Condition)))
        {
            Execute(whileStmt.Body);
        }

        return null;
    }

    #endregion

    #region Auxiliar Methods

    private string Stringify(Object obj)
    {
        if (obj == null) return "null";
        string? text = obj.ToString();
        if (text.EndsWith(".0")) text = text[..(text.Length - 2)];
        if (text[0] == '"' && text[text.Length - 1] == '"') text = text[1..(text.Length - 1)];
        return text;
    }

    private bool IsTruthy(Object obj)
    {
        if (obj == null) return false;
        if (bool.TryParse(obj.ToString(), out bool value)) return value;
        return true;
    }

    #endregion
}