namespace GwentCompiler;

public partial class Interpreter : VisitorBase<Object>
{
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

        if (left is double leftInt && right is double rigthInt) return leftInt > rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(GreaterThanOrEqualExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rigthInt) return leftInt >= rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(LessThanExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rigthInt) return leftInt < rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(LessThanOrEqualExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rigthInt) return leftInt <= rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(AdditionExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rigthInt) return leftInt + rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(SubstractionExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rigthInt) return leftInt - rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(MultiplicationExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rigthInt) return leftInt * rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(DivisionExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rigthInt) return leftInt / rigthInt;
        else throw new RuntimeError("The operands must to be a number", expr.Operator.Location);
    }

    public object? Visit(PowerExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rigthInt) return Math.Pow(leftInt, rigthInt);
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

        if (right is double rightInt) return -rightInt;
        else throw new RuntimeError("The operand must to be a number", substraction.Operator.Location);
    }

    public object Visit(NumericLiteral literal)
    {
        return double.Parse(literal.Value.Lexeme);
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

    public object Visit(ContextAccessExpr expr)
    {
        throw new NotImplementedException();
    }

    public object Visit(ContextMethodsExpr expr)
    {
        throw new NotImplementedException();
    }

    public object Visit(IncrementOrDecrementOperationExpr expr)
    {
        var value = environment.Get(expr.Name.Value);

        if (value is double valueInt)
        {
            if (expr.Operation.Subtype == TokenSubtypes.PostDecrement) valueInt--;
            else valueInt++;
            environment.Assign(expr.Name.Value.Lexeme, valueInt);
            return valueInt;
        }
        else throw new RuntimeError("The operand must to be a number", expr.Operation.Location);
    }
}