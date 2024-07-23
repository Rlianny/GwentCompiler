namespace GwentCompiler;

public partial class Parser
{
    #region Statements Parsing

    // *Statement* -> IfStmt | PrintStmt | WhileStmt | Block| ExpressionStmt;
    private IStatement? Statement()
    {
        if (Match(TokenSubtypes.If))
        {
            return IfStatement();
        }

        if (Match(TokenSubtypes.Print))
        {
            return PrintStatement();
        }

        if (Match(TokenSubtypes.While))
        {
            return WhileStatement();
        }

        if (Match(TokenSubtypes.For))
        {
            return ForStatement();
        }

        if (Match(TokenSubtypes.OpenBrace))
        {
            return new BlockStmt(Block());
        }

        return ExpressionStatement();
    }

    // *ExpressionStmt* -> Expression ";";
    private IStatement ExpressionStatement()
    {
        IExpression? expression = Expression();
        Consume(TokenSubtypes.Semicolon, "Expect ; after expression;");
        return new ExpressionStmt(expression);
    }

    // *PrintStmt* -> "print" Expression ";";
    private IStatement PrintStatement()
    {
        IExpression? expression = Expression();
        Consume(TokenSubtypes.Semicolon, "Expect ';' after value.");
        return new PrintStmt(expression);
    }

    // *Block* -> "{" (Statements)* "}";
    private List<IStatement?> Block()
    {
        List<IStatement?> statements = new();

        while (!Check(TokenSubtypes.CloseBrace) && !IsAtEnd())
        {
            statements.Add(Statement());
        }

        Consume(TokenSubtypes.CloseBrace, "Expect } after block");
        return statements;
    }

    // *IfStmt* -> "if" "(" Expression ")" Statement ("else" Statement)?;
    private IStatement IfStatement()
    {
        Consume(TokenSubtypes.OpenParenthesis, "Expect '(' after 'if'.");
        IExpression? condition = Expression();
        Consume(TokenSubtypes.CloseParenthesis, "Expect ')' after condition");
        IStatement? thenStatement = Statement();

        IStatement? elseStatement = null;

        if (Match(TokenSubtypes.Else))
        {
            elseStatement = Statement();
        }

        return new IfStmt(condition, thenStatement, elseStatement);
    }

    // *WhileStmt* -> "while" "(" Expression ")" Estatement;
    private IStatement? WhileStatement()
    {
        Consume(TokenSubtypes.OpenParenthesis, "Expect '(' after 'while'.");
        IExpression? condition = Expression();
        Consume(TokenSubtypes.CloseParenthesis, "Expect ')' after condition");
        IStatement? body = Statement();
        return new WhileStmt(condition, body);

    }

    // *ForStmt* -> "for" "(" Variable "in" Expression ")" Block;
    private IStatement? ForStatement()
    {
        Consume(TokenSubtypes.OpenParenthesis, "Expect '(' after for.");
        Variable variable = new Variable(Consume(TokenSubtypes.Identifier, "Expect identifier in for statement"));
        Consume(TokenSubtypes.In, "Expect 'in' in for statement");
        IExpression? collection = Expression();
        Consume(TokenSubtypes.CloseParenthesis, "Expect ')' in for statement.");
        List<IStatement?>? body = null;
        if (Match(TokenSubtypes.OpenBrace))
        {
            body = Block();
        }
        return new ForStmt(variable, collection, body);
    }

    #endregion
}