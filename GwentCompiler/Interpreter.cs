using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace GwentCompiler;

// A la hora de usar la interface IVisitor en los Statements el tipo de retorno debería ser void, pero C# no me permite
// pasar void como tipo. La mejor sugerencia de Phind fue declararme un struct que representara el void, y lo nombré VoidType;
// ¿Hay alguna manera más elegante de hacerlo? No me gusta demasiado.

public class Interpreter : IErrorReporter, IExprVisitor<Object>, IStmtVisitor<VoidType>
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

    private Object Evaluate(IExpression expression)
    {
        return expression.Accept(this);
    }

    private void Execute(IStatement statement)
    {
        statement.Accept(this);
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

    #region Visitor Expression

    public object VisitBinaryExpression(BinaryExpression binaryExpression)
    {
        Object left = Evaluate(binaryExpression.Left);
        Object right = Evaluate(binaryExpression.Right);

        switch (binaryExpression.Operator.Subtype)
        {
            case TokenSubtypes.AND:
                return ParseToBoolean(left) && ParseToBoolean(right);

            case TokenSubtypes.OR:
                return ParseToBoolean(left) || ParseToBoolean(right);

            case TokenSubtypes.Equality:
                return left.Equals(right);

            case TokenSubtypes.Inequality:
                return !left.Equals(right);

            case TokenSubtypes.StringConcatenation:
                return ParseToString(left) + ParseToString(right);

            case TokenSubtypes.StringConcatenationSpaced:
                return ParseToString(left) + " " + ParseToString(right);

            case TokenSubtypes.GreaterThan:
                CheckNumberOperand(binaryExpression.Operator, left, right);
                return ParseToDouble(left) > ParseToDouble(right);

            case TokenSubtypes.GreaterThanOrEqual:
                CheckNumberOperand(binaryExpression.Operator, left, right);
                return ParseToDouble(left) >= ParseToDouble(right);

            case TokenSubtypes.LessThan:
                CheckNumberOperand(binaryExpression.Operator, left, right);
                return ParseToDouble(left) < ParseToDouble(right);

            case TokenSubtypes.LessThanOrEqual:
                CheckNumberOperand(binaryExpression.Operator, left, right);
                return ParseToDouble(left) <= ParseToDouble(right);

            case TokenSubtypes.Addition:
                CheckNumberOperand(binaryExpression.Operator, left, right);
                return ParseToDouble(left) + ParseToDouble(right);

            case TokenSubtypes.Subtraction:
                CheckNumberOperand(binaryExpression.Operator, left, right);
                return ParseToDouble(left) - ParseToDouble(right);

            case TokenSubtypes.Multiplication:
                CheckNumberOperand(binaryExpression.Operator, left, right);
                return ParseToDouble(left) * ParseToDouble(right);

            case TokenSubtypes.Division:
                CheckNumberOperand(binaryExpression.Operator, left, right);
                return ParseToDouble(left) / ParseToDouble(right);

            case TokenSubtypes.Potentiation:
                CheckNumberOperand(binaryExpression.Operator, left, right);
                return Math.Pow(ParseToDouble(left), ParseToDouble(right));
        }

        return null;
    }

    public object VisitUnaryExpression(UnaryExpression unaryExpression)
    {
        Object right = Evaluate(unaryExpression.Rigth);

        switch (unaryExpression.Operator.Subtype)
        {
            case TokenSubtypes.Subtraction:
                CheckNumberOperand(unaryExpression.Operator, right);
                return -ParseToDouble(right);

            case TokenSubtypes.Negation:
                return !ParseToBoolean(right);
        }

        return null;
    }

    public object VisitAtom(Atom atom)
    {
        return atom.Value.Lexeme;
    }

    public object VisitGroupExpression(GroupExpression groupExpression)
    {
        return Evaluate(groupExpression.Expression);
    }

    public object VisitAssignmentExpression(AssignmentExpr assignmentExpression)
    {
        Object value = Evaluate(assignmentExpression.Value);
        environment.Assign(assignmentExpression.Name.Value.Lexeme, value);
        return value;
    }

    public object VisitVariableExpression(Variable variable)
    {
        return environment.Get(variable.Value);
    }

    # endregion

    #region Visitor Statements

    public VoidType VisitExpressionStmt(ExpressionStmt expressionStatement)
    {
        Evaluate(expressionStatement.Expression);

        return ReturnVoid();
    }

    public VoidType VisitPrintStmt(PrintStmt printStatement)
    {
        Object value = Evaluate(printStatement.Expression);
        System.Console.WriteLine(Stringify(value));
        return ReturnVoid();
    }


    public VoidType VisitBlockStmt(BlockStmt block)
    {
        ExecuteBlock(block, new Environment(environment));
        return ReturnVoid();
    }

    public VoidType VisitVariableStmt(Variable variable)
    {
        Object value = Evaluate(variable);
        environment.Assign(variable.Value.Lexeme, value);
        return ReturnVoid();
    }

    public VoidType VisitIfStmt(IfStmt ifStatement)
    {
        if (IsTruthy(Evaluate(ifStatement.Condition))) Execute(ifStatement.ThenBranch);
        else if (ifStatement != null) Execute(ifStatement.ElseBranch);
        return ReturnVoid();
    }

    public VoidType VisitWhileStmt(WhileStmt whileStmt)
    {
        while(IsTruthy(Evaluate(whileStmt.Condition)))
        {
            Execute(whileStmt.Body);
        }

        return ReturnVoid();
    }

    #endregion

    #region Auxiliar Methods

    private void CheckNumberOperand(Token operation, Object right)
    {
        if (double.TryParse(right.ToString(), out double result)) return;
        throw new RuntimeError("The operand must to be a number", operation.Location);
    }

    private void CheckNumberOperand(Token operation, Object left, Object right)
    {
        if (double.TryParse(right.ToString(), out double result) && double.TryParse(left.ToString(), out double result1)) return;
        throw new RuntimeError("The operands must to be a number", operation.Location);
    }

    private string Stringify(Object obj)
    {
        if (obj == null) return "null";
        string text = obj.ToString();
        if (text.EndsWith(".0")) text = text[..(text.Length - 2)];
        if (text[0] == '"' && text[text.Length - 1] == '"') text = text[1..(text.Length - 1)];
        return text;
    }

    public void GenerateError(string message, CodeLocation errorLocation)
    {
        SemanticError newError = new SemanticError(message, errorLocation);
        Error.AllErrors.Add(newError);
        Report(newError);
    }

    public void Report(Error error)
    {
        System.Console.WriteLine(error.ToString());
    }

    private double ParseToDouble(object obj)
    {
        return double.Parse(obj.ToString());
    }

    private bool ParseToBoolean(Object obj)
    {
        return bool.Parse(obj.ToString());
    }

    private string ParseToString(Object obj)
    {
        return obj.ToString()[1..(obj.ToString().Length - 1)];
    }

    private bool IsTruthy(Object obj)
    {
        if (obj == null) return false;
        if (bool.TryParse(obj.ToString(), out bool value)) return value;
        return true;
    }


    private VoidType ReturnVoid()
    {
        VoidType voidType;
        return voidType;
    }

    #endregion
}