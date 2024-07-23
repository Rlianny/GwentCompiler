using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace GwentCompiler;

// A la hora de usar la interface IVisitor en los Statements el tipo de retorno debería ser void, pero C# no me permite
// pasar void como tipo. La mejor sugerencia de Phind fue declararme un struct que representara el void, y lo nombré VoidType;
// ¿Hay alguna manera más elegante de hacerlo? No me gusta demasiado.

public partial class Interpreter : VisitorBase<Object>
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

    public object? Interpret(CardComponent cardComponent)
    {
        try
        {
            return VisitBase(cardComponent);
        }
        catch (RuntimeError ex)
        {
            GenerateError(ex.Message, ex.CodeLocation);
        }
        return null;
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

    #region Auxiliar Methods

    private string Stringify(Object? obj)
    {
        if (obj == null) return "null";
        string? text = obj.ToString();
        if (text.EndsWith(".0")) text = text[..(text.Length - 2)];
        if (text[0] == '"' && text[text.Length - 1] == '"') text = text[1..(text.Length - 1)];
        return text;
    }

    private bool IsTruthy(Object? obj)
    {
        if (obj == null) return false;
        if (bool.TryParse(obj.ToString(), out bool value)) return value;
        return true;
    }

    #endregion
}