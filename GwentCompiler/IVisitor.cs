namespace GwentCompiler;

// Métodos de la interface IVisitor

// Teniendo en cuenta que las expresiones retornan un valor al evaluarse y las sentencias se ejecutan sin retornar un valor,
// separé la interface IVisitor en Visitor de Expresiones y visitor de Statements para no verme en la necesidad de implementar todos los
// métodos con un mismo tipo de retorno que no iba a usar; ¿Está mal de alguna manera?


public interface IExprVisitor<out T>
{
    public T VisitBinaryExpression(BinaryExpression binaryExpression);
    public T VisitUnaryExpression(UnaryExpression unaryExpression);
    public T VisitAtom(Atom atom);
    public T VisitGroupExpression(GroupExpression groupExpression);
    public T VisitAssignmentExpression(AssignmentExpr assignmentExpression);
    public T VisitVariableExpression(Variable variable);
}

public interface IStmtVisitor<out T>
{
    public T VisitExpressionStmt(ExpressionStmt expressionStatement);
    public T VisitPrintStmt(PrintStmt printStatement);
    public T VisitBlockStmt(BlockStmt block);
    public T VisitVariableStmt(Variable variable);
    public T VisitIfStmt(IfStmt ifStatement);
    public T VisitWhileStmt(WhileStmt whileStmt);
}