namespace GwentCompiler;

public partial class Interpreter : VisitorBase<Object>
{
    public object? Visit(AndExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is bool leftBool && right is bool rightBool) return leftBool && rightBool;
        else throw new RuntimeError("The operands must be boolean values", expr.Operator.Location);
    }

    public object? Visit(OrExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is bool leftBool && right is bool rightBool) return leftBool || rightBool;
        else throw new RuntimeError("The operands must be boolean values", expr.Operator.Location);
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

        if (left is string leftString && right is string rightString) return leftString + rightString;
        else throw new RuntimeError("The operands must be string values", expr.Operator.Location);
    }

    public object? Visit(StringConcatenationSpacedExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is string leftString && right is string rightString) return leftString + " " + rightString;
        else throw new RuntimeError("The operands must be string values", expr.Operator.Location);
    }

    public object? Visit(GreaterThanExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt > rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object? Visit(GreaterThanOrEqualExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt >= rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object? Visit(LessThanExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt < rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object? Visit(LessThanOrEqualExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt <= rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object? Visit(AdditionExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt + rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object? Visit(SubtractionExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt - rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object? Visit(MultiplicationExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt * rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object? Visit(DivisionExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return leftInt / rightInt;
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object? Visit(PowerExpr expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        if (left is double leftInt && right is double rightInt) return Math.Pow(leftInt, rightInt);
        else throw new RuntimeError("The operands must be numeric values", expr.Operator.Location);
    }

    public object? Visit(NegatedExpr expr)
    {
        var right = Evaluate(expr.Right);

        if (right is bool rightBool) return !rightBool;
        else throw new RuntimeError("The operand must be boolean value", expr.Operator.Location);
    }

    public object? Visit(Subtraction subtraction)
    {
        var right = Evaluate(subtraction.Right);

        if (right is double rightInt) return -rightInt;
        else throw new RuntimeError("The operand must be a numeric value", subtraction.Operator.Location);
    }

    public object? Visit(NumericLiteral literal)
    {
        if(literal.Value == null) return null;
        return double.Parse(literal.Value.Lexeme);
    }

    public object? Visit(StringLiteral literal)
    {
        if(literal.Value == null) return null;
        return Stringify(literal.Value.Lexeme);
    }

    public object? Visit(BooleanLiteral literal)
    {
        if(literal.Value == null) return null;
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
        if(value == null || expr.Name == null) return null;
        environment.Assign(expr.Name.Value.Lexeme, value);
        return value;
    }

    public object? Visit(ContextAccessExpr expr)
    {
        throw new NotImplementedException();
    }

    public object? Visit(ContextMethodsExpr expr)
    {
        throw new NotImplementedException();
    }

    public object? Visit(IncrementOrDecrementOperationExpr expr)
    {
        if(expr.Name == null) return null;
        var value = environment.Get(expr.Name.Value);

        if (value is double valueInt)
        {
            if (expr.Operation.Subtype == TokenSubtypes.PostDecrement) valueInt--;
            else valueInt++;
            environment.Assign(expr.Name.Value.Lexeme, valueInt);
            return valueInt;
        }
        else throw new RuntimeError("The operand must be a numeric value", expr.Operation.Location);
    }
}