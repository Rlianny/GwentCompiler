namespace GwentCompiler;

public interface IStatement
{
    public VoidType Accept(IStmtVisitor<VoidType> visitor);
}

public class ExpressionStmt : IStatement
{
    public IExpression Expression;

    public ExpressionStmt(IExpression expression)
    {
        Expression = expression;
    }

    public VoidType Accept(IStmtVisitor<VoidType> visitor)
    {
        return visitor.VisitExpressionStmt(this);
    }
}

public class PrintStmt : IStatement
{
    public IExpression Expression;

    public PrintStmt(IExpression expression)
    {
        Expression = expression;
    }

    public VoidType Accept(IStmtVisitor<VoidType> visitor)
    {
        return visitor.VisitPrintStmt(this);
    }
}

public class BlockStmt : IStatement
{
    public List<IStatement> Statements;
    public BlockStmt(List<IStatement> statements)
    {
        Statements = statements;
    }

    public VoidType Accept(IStmtVisitor<VoidType> visitor)
    {
        return visitor.VisitBlockStmt(this);
    }
}

public class IfStmt : IStatement
{
    public IExpression Condition;
    public IStatement ThenBranch;
    public IStatement ElseBranch;

    public IfStmt(IExpression condition, IStatement thenBranch, IStatement elseBranch)
    {
        Condition = condition;
        ThenBranch = thenBranch;
        ElseBranch = elseBranch;
    }

    public VoidType Accept(IStmtVisitor<VoidType> visitor)
    {
        return visitor.VisitIfStmt(this);
    }
}

public class WhileStmt : IStatement
{
    public IExpression Condition;
    public IStatement Body;

    public WhileStmt(IExpression condition, IStatement body)
    {
        Condition = condition;
        Body = body;
    }

    public VoidType Accept(IStmtVisitor<VoidType> visitor)
    {
        return visitor.VisitWhileStmt(this);
    }
}

