namespace GwentCompiler;

public partial class Parser
{
    #region Expressions Parsing

    // *Expression* -> Assignment;
    private IExpression? Expression()
    {
        return Assignment();
    }

    // *Assignment* -> LogicOperation ("=" Assignment)?;
    private IExpression? Assignment()
    {
        IExpression? expr = LogicOperation();

        if (Match(TokenSubtypes.Assignment))
        {
            Token equal = Previous();
            IExpression? value = Assignment();
            if (expr is Variable variable)
            {
                return new AssignmentExpr(variable, value);
            }

            GenerateError("Assignment objetive non valid.", equal.Location);
        }

        return expr;
    }

    // *LogicOperation* -> Equality ("And" Equality| "Or" Equality)*;
    private IExpression? LogicOperation()
    {
        IExpression? expr = Equality();

        while (Match(new List<TokenSubtypes>() { TokenSubtypes.AND, TokenSubtypes.OR }))
        {
            Token op = Previous();
            IExpression? rigth = Equality();

            switch (op.Subtype)
            {
                case TokenSubtypes.AND:
                    expr = new AndExpr(expr, op, rigth);
                    break;

                case TokenSubtypes.OR:
                    expr = new OrExpr(expr, op, rigth);
                    break;
            }
        }

        return expr;
    }

    // *Equality* = StringOperation ("!=" StringOperation | "==" StingOperation)*;
    private IExpression? Equality()
    {
        IExpression? expr = StringOperation();

        while (Match(new List<TokenSubtypes>() { TokenSubtypes.Inequality, TokenSubtypes.Equality }))
        {
            Token op = Previous();
            IExpression? rigth = StringOperation();

            switch (op.Subtype)
            {
                case TokenSubtypes.Equality:
                    expr = new EqualityExpr(expr, op, rigth);
                    break;

                case TokenSubtypes.Inequality:
                    expr = new InequalityExpr(expr, op, rigth);
                    break;
            }
        }

        return expr;
    }

    // *StringOperation* -> Comparison ("@" Comparison | "@@" Comparison)*;

    private IExpression? StringOperation()
    {
        IExpression? expr = Comparison();

        while (Match(new List<TokenSubtypes>() { TokenSubtypes.StringConcatenation, TokenSubtypes.StringConcatenationSpaced }))
        {
            Token op = Previous();
            IExpression? rigth = Comparison();

            switch (op.Subtype)
            {
                case TokenSubtypes.StringConcatenation:
                    expr = new StringConcatenationExpr(expr, op, rigth);
                    break;

                case TokenSubtypes.StringConcatenationSpaced:
                    expr = new StringConcatenationSpacedExpr(expr, op, rigth);
                    break;
            }
        }

        return expr;
    }

    // *Comparison* -> Term (">" Term | ">=" Term | "<" Term | "<=" Term)*;

    private IExpression? Comparison()
    {
        IExpression? expr = Term();

        while (Match(new List<TokenSubtypes>() { TokenSubtypes.GreaterThan, TokenSubtypes.GreaterThanOrEqual, TokenSubtypes.LessThan, TokenSubtypes.LessThanOrEqual }))
        {
            Token op = Previous();
            IExpression? rigth = Term();

            switch (op.Subtype)
            {
                case TokenSubtypes.GreaterThan:
                    expr = new GreaterThanExpr(expr, op, rigth);
                    break;

                case TokenSubtypes.GreaterThanOrEqual:
                    expr = new GreaterThanOrEqualExpr(expr, op, rigth);
                    break;

                case TokenSubtypes.LessThan:
                    expr = new LessThanExpr(expr, op, rigth);
                    break;

                case TokenSubtypes.LessThanOrEqual:
                    expr = new LessThanOrEqualExpr(expr, op, rigth);
                    break;
            }
        }

        return expr;
    }

    // *Term* -> Factor ("+" Factor | "-" Factor)*;
    private IExpression? Term()
    {
        IExpression? expr = Factor();

        while (Match(new List<TokenSubtypes>() { TokenSubtypes.Addition, TokenSubtypes.Subtraction }))
        {
            Token op = Previous();
            IExpression? rigth = Factor();

            switch (op.Subtype)
            {
                case TokenSubtypes.Addition:
                    expr = new AdditionExpr(expr, op, rigth);
                    break;

                case TokenSubtypes.Subtraction:
                    expr = new SubstractionExpr(expr, op, rigth);
                    break;
            }
        }

        return expr;
    }

    // *Factor* -> Power ("*" Power | "/" Power)*;
    private IExpression? Factor()
    {
        IExpression? expr = Power();

        while (Match(new List<TokenSubtypes>() { TokenSubtypes.Multiplication, TokenSubtypes.Division }))
        {
            Token op = Previous();
            IExpression? rigth = Power();

            switch (op.Subtype)
            {
                case TokenSubtypes.Multiplication:
                    expr = new MultiplicationExpr(expr, op, rigth);
                    break;

                case TokenSubtypes.Division:
                    expr = new DivisionExpr(expr, op, rigth);
                    break;
            }
        }

        return expr;
    }

    // *Power* -> Unary ("^" Power)*;
    private IExpression? Power()
    {
        IExpression? expr = Unary();

        while (Match(TokenSubtypes.Potentiation))
        {
            Token op = Previous();
            IExpression? rigth = Power();
            expr = new PowerExpr(expr, op, rigth);
        }

        return expr;
    }

    // *Unary* -> ("-" | "!")? Primary;
    private IExpression? Unary()
    {
        if (Match(new List<TokenSubtypes>() { TokenSubtypes.Subtraction, TokenSubtypes.Negation }))
        {
            Token op = Previous();
            IExpression? rigth = Primary();

            switch (op.Subtype)
            {
                case TokenSubtypes.Subtraction:
                    return new Substraction(op, rigth);

                case TokenSubtypes.Negation:
                    return new NegatedExpr(op, rigth);
            }
        }

        return Primary();
    }

    // *Primary* -> BoolLiteral | NumericLiteral | StringLiteral | Acces | MethodCalled;
    private IExpression? Primary()
    {
        if (Match(TokenSubtypes.True) || Match(TokenSubtypes.False)) return new BooleanLiteral(Previous());

        if (Match(TokenSubtypes.NumericLiteral)) return new NumericLiteral(Previous());

        if (Match(TokenSubtypes.StringLiteral)) return new StringLiteral(Previous());

        if (Match(TokenSubtypes.OpenParenthesis))
        {
            IExpression? expr = Expression();
            Token? close = Consume(TokenSubtypes.CloseParenthesis, ") expected after expression.", null);

            if (close == null) return null;

            return new GroupExpression(expr);
        }

        if (Match(TokenSubtypes.Identifier))
        {
            Variable var = new Variable(Previous());

            if (Match(new List<TokenSubtypes>() { TokenSubtypes.PostIncrement, TokenSubtypes.PostDecrement }))
            {
                Token op = Previous();
                return new IncrementOrDecrementOperationExpr(op, var);
            }

            if (Match(TokenSubtypes.Dot))
            {
                Token dot = Previous();

                if (Match(new List<TokenSubtypes>() { TokenSubtypes.TriggerPlayer, TokenSubtypes.Board, TokenSubtypes.HandOfPlayer, TokenSubtypes.FieldOfPlayer, TokenSubtypes.GraveyardOfPlayer, TokenSubtypes.DeckOfPlayer, TokenSubtypes.Owner }))
                {
                    Token acces = Previous();
                    IExpression? args = null;


                    if (acces.Subtype == TokenSubtypes.HandOfPlayer || acces.Subtype == TokenSubtypes.FieldOfPlayer || acces.Subtype == TokenSubtypes.GraveyardOfPlayer || acces.Subtype == TokenSubtypes.DeckOfPlayer)
                    {
                        if (Match(TokenSubtypes.OpenParenthesis))
                        {
                            args = Expression();
                            Consume(TokenSubtypes.CloseParenthesis, "Expect ')' after methods arguments", null);
                        }
                    }

                    ContextAccessExpr? expr = null;

                    switch (acces.Subtype)
                    {
                        case TokenSubtypes.TriggerPlayer:
                            expr = new TriggerPlayerAccessExpr(var, dot, acces, args);
                            break;

                        case TokenSubtypes.Board:
                            expr = new BoardAccessExpr(var, dot, acces, args);
                            break;

                        case TokenSubtypes.HandOfPlayer:
                            expr = new HandOfPlayerAccessExpr(var, dot, acces, args);
                            break;

                        case TokenSubtypes.FieldOfPlayer:
                            expr = new FieldOfPlayerAccessExpr(var, dot, acces, args);
                            break;

                        case TokenSubtypes.GraveyardOfPlayer:
                            expr = new GraveyardOfPlayerAccessExpr(var, dot, acces, args);
                            break;

                        case TokenSubtypes.DeckOfPlayer:
                            expr = new DeckOfPlayerAccessExpr(var, dot, acces, args);
                            break;

                        case TokenSubtypes.Owner:
                            expr = new CardOwnerAccessExpr(var, dot, acces, args);
                            break;
                    }

                    if (Match(TokenSubtypes.Dot))
                    {
                        return MethodCall(expr);
                    }

                    return expr;
                }

            }

            return var;
        }

        else GenerateError("Expression expected", Peek().Location); // Si ninguno de los casos coincide, significa que estamos sentados sobre un token que no puede iniciar una expresión
        return null;
    }

    private IExpression? MethodCall(IExpression? access)
    {

        if (Check(TokenSubtypes.Dot))
        {
            GenerateError("Invalid Access", Peek().Location);
            return null;
        }

        if (Match(TokenSubtypes.Find))
        {
            Token method = Previous();
            Consume(TokenSubtypes.OpenParenthesis, "Expected '('", new List<TokenSubtypes>{TokenSubtypes.CloseParenthesis});
            IExpression? args = Expression();
            Consume(TokenSubtypes.CloseParenthesis, "Expected ')'", null);
            return new FindMethodExpr(access, method, args);
        }

        if (Match(TokenSubtypes.Push))
        {
            Token method = Previous();
            Consume(TokenSubtypes.OpenParenthesis, "Expected '('", new List<TokenSubtypes>{TokenSubtypes.CloseParenthesis});
            IExpression? args = Expression();
            Consume(TokenSubtypes.CloseParenthesis, "Expected ')'", null);
            return new PushMethodExpr(access, method, args);
        }

        if (Match(TokenSubtypes.SendBottom))
        {
            Token method = Previous();
            Consume(TokenSubtypes.OpenParenthesis, "Expected '('", new List<TokenSubtypes>{TokenSubtypes.CloseParenthesis});
            IExpression? args = Expression();
            Consume(TokenSubtypes.CloseParenthesis, "Expected ')'", null);
            return new SendBottomMethodExpr(access, method, args);
        }

        if (Match(TokenSubtypes.Pop))
        {
            Token method = Previous();
            Consume(TokenSubtypes.OpenParenthesis, "Expected '('", new List<TokenSubtypes>{TokenSubtypes.CloseParenthesis});
            Consume(TokenSubtypes.CloseParenthesis, "Expected ')'", null);
            return new PopMethodExpr(access, method, null);
        }

        if (Match(TokenSubtypes.Remove))
        {
            Token method = Previous();
            Consume(TokenSubtypes.OpenParenthesis, "Expected '('", new List<TokenSubtypes>{TokenSubtypes.CloseParenthesis});
            IExpression? args = Expression();
            Consume(TokenSubtypes.CloseParenthesis, "Expected ')'", null);
            return new RemoveMethodExpr(access, method, args);
        }

        if (Match(TokenSubtypes.Shuffle))
        {
            Token method = Previous();
            Consume(TokenSubtypes.OpenParenthesis, "Expected '('", new List<TokenSubtypes>{TokenSubtypes.CloseParenthesis});
            Consume(TokenSubtypes.CloseParenthesis, "Expected ')'", null);
            return new ShuffleMethodExpr(access, method, null);
        }

        else GenerateError("Expression expected", Peek().Location);
        return null;
    }

    #endregion
}