namespace GwentCompiler;

public partial class Parser
{
    private IProgramNode? Card()
    {
        CardDeclaration parsedCard = new();

        Consume(TokenSubtypes.OpenBrace, "Card must declare a body", new List<TokenSubtypes> { TokenSubtypes.CloseBrace });
        while (!Match(TokenSubtypes.CloseBrace))
        {
            if (Match(TokenSubtypes.Type))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a type", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card type has been defined before", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    break;
                }
                continue;
            }

            if (Match(TokenSubtypes.Name))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a name", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    return null;
                }

                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card name has been defined before", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    break;
                }
                continue;
            }

            if (Match(TokenSubtypes.Faction))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a faction", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card faction has been defined before", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    break;
                }
                continue;
            }

            if (Match(TokenSubtypes.Range))
            {
                CardComponent? value = CardRange();
                if (value == null)
                {
                    GenerateError("The card must to declare a range", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card range has been defined before", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    break;
                }
                continue;
            }

            if (Match(TokenSubtypes.Power))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a power", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card power has been defined before", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    break;
                }
                continue;
            }

            if (Match(TokenSubtypes.OnActivation))
            {
                CardComponent? value = OnActivation();
                if (value == null)
                {
                    GenerateError("The card must to declare a OnActivation field", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.CloseBracket });
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card OnActivation field has been defined before", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.CloseBracket });
                    break;
                }
                continue;
            }

            if (Match(TokenSubtypes.EffectDescription))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a effect description", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The effect description has been defined before", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    break;
                }
                continue;
            }

            if (Match(TokenSubtypes.CharacterDescription))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a character description", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The character description has been defined before", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    break;
                }
                continue;
            }

            if (Match(TokenSubtypes.Quote))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a quote", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card quote has been defined before", Previous().Location);
                    Synchronize(new List<TokenSubtypes> { TokenSubtypes.Comma });
                    break;
                }
                continue;
            }

            Synchronize(new List<TokenSubtypes> { TokenSubtypes.card, TokenSubtypes.effect });
            break;
        }

        //Consume(TokenSubtypes.OpenBrace, "Expect '}' after card body");
        return parsedCard;

    }

    private CardComponent? GenericField()
    {
        var component = Previous();
        Token? colon = Consume(TokenSubtypes.Colon, "Expected ':'", new List<TokenSubtypes> { TokenSubtypes.Comma });
        if (colon == null) return null;
        IExpression? value = Expression();

        if (value == null) return null;

        Consume(TokenSubtypes.Comma, "Expected ',' after field declaration", null);

        switch (component.Subtype)
        {
            case TokenSubtypes.Name:
                return new CardNameDeclaration(value, colon);
            case TokenSubtypes.Faction:
                return new CardFactionDeclaration(value, colon);
            case TokenSubtypes.Power:
                return new CardPowerDeclaration(value, colon);
            case TokenSubtypes.EffectDescription:
                return new CardEffectDescriptionDeclaration(value, colon);
            case TokenSubtypes.CharacterDescription:
                return new CardCharacterDescriptionDeclaration(value, colon);
            case TokenSubtypes.Quote:
                return new CardQuoteDeclaration(value, colon);
            case TokenSubtypes.Type:
                return new CardTypeDeclaration(value, colon);
        }

        return null;
    }

    private CardComponent? CardRange()
    {
        List<IExpression?> ranges = new();
        Token? colon = Consume(TokenSubtypes.Colon, "Expected ':'", null);
        Consume(TokenSubtypes.OpenBracket, "Expect '['", new List<TokenSubtypes> { TokenSubtypes.CloseBracket });
        while (!Match(TokenSubtypes.CloseBracket))
        {
            ranges.Add(Expression());
            if (!Check(TokenSubtypes.CloseBracket))
            {
                Consume(TokenSubtypes.Comma, "Expect ','", new List<TokenSubtypes> { TokenSubtypes.CloseBracket });
            }
        }
        Consume(TokenSubtypes.Comma, "Expect ','", null);
        return new CardRangeDeclaration(ranges, colon);
    }

    private CardComponent? OnActivation()
    {
        Token? colon = Consume(TokenSubtypes.Colon, "Expected ':'", null);
        Consume(TokenSubtypes.OpenBracket, "Expect '['", new List<TokenSubtypes> { TokenSubtypes.CloseBracket });
        List<ActivationData> data = new();
        while (!Match(TokenSubtypes.CloseBracket))
        {
            data.Add(ActivationData());
        }
        return new OnActivation(data, colon);

    }

    private ActivationData ActivationData()
    {
        Consume(TokenSubtypes.OpenBrace, "Was expected '{'", null);
        EffectInfo effectInfo = EffectInfo();
        Selector selector = Selector();
        PostAction postAction = PostAction();
        Consume(TokenSubtypes.CloseBrace, "Was expected '}'", null);

        return new ActivationData(effectInfo, selector, postAction);
    }

    private EffectInfo EffectInfo()
    {
        Consume(TokenSubtypes.Effect, "Effect information declaration expected", new List<TokenSubtypes> { TokenSubtypes.CloseBrace });
        Token? colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
        IExpression? name = null;
        List<OnActivationParam?>? @params = new();
        if (Match(TokenSubtypes.OpenBrace))
        {
            Consume(TokenSubtypes.Name, "Effect name expected", new List<TokenSubtypes> { TokenSubtypes.Comma });
            Consume(TokenSubtypes.Colon, "Was expected ':'", null);
            name = Expression();
            Consume(TokenSubtypes.Comma, "Was expected ','", null);
            while (!Match(TokenSubtypes.CloseBrace))
            {
                @params.Add(OnActivationParam());
            }
            Consume(TokenSubtypes.Comma, "Was expected ','", null);
        }
        else name = Expression();

        return new EffectInfo(name, @params, colon);

    }

    private OnActivationParam? OnActivationParam()
    {
        Token token = Peek();
        var variable = Expression();
        if (variable is Variable var)
        {
            Token? colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
            IExpression? value = Expression();
            Consume(TokenSubtypes.Comma, "Was expected ','", null);
            return new OnActivationParam(var, colon, value);
        }
        GenerateError("Was expected a variable", token.Location);
        Synchronize(new List<TokenSubtypes>() { TokenSubtypes.CloseBrace });
        return null;
    }

    private Selector Selector()
    {
        Consume(TokenSubtypes.Selector, "Effect selector declaration expected", new List<TokenSubtypes> { TokenSubtypes.CloseBrace });
        Token? colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
        Consume(TokenSubtypes.OpenBrace, "Was expected '{'", null);
        SelectorSource source = Source();
        SelectorSingle? single = null;
        if (Match(TokenSubtypes.Single))
        {
            single = Single();
        }
        SelectorPredicate predicate = Predicate();
        Consume(TokenSubtypes.CloseBrace, "Was expected '}'", null);
        Consume(TokenSubtypes.Comma, "Was expect ','", null);
        return new Selector(source, single, predicate);
    }

    private SelectorSource Source()
    {
        Consume(TokenSubtypes.Source, "Source declaration expected", new List<TokenSubtypes> { TokenSubtypes.CloseBrace });
        Token? colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
        IExpression source = Expression();
        Consume(TokenSubtypes.Comma, "Was expected ','", null);
        return new SelectorSource(source, colon);
    }

    public SelectorSingle Single()
    {
        Token? colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
        IExpression single = Expression();
        Consume(TokenSubtypes.Comma, "Was expected ','", null);
        return new SelectorSingle(single, colon);
    }

    public SelectorPredicate? Predicate()
    {
        Consume(TokenSubtypes.Predicate, "Predicate declaration expected", new List<TokenSubtypes> { TokenSubtypes.CloseBrace });
        Consume(TokenSubtypes.Colon, "Was expected ':'", null);
        var parenthesis = Consume(TokenSubtypes.OpenParenthesis, "Was expected '('", null);
        var var = Expression();
        if (var is Variable variable)
        {
            Consume(TokenSubtypes.CloseParenthesis, "Was expected ')'", null);
            var lambda = Consume(TokenSubtypes.Lambda, "Was expected '=>'", null);
            IExpression? expression = Expression();
            return new SelectorPredicate(variable, lambda, expression);
        }
        GenerateError("Was expect a variable", parenthesis.Location);
        Synchronize(new List<TokenSubtypes> { TokenSubtypes.CloseBrace });
        return null;
    }

    private PostAction PostAction()
    {
        if (Match(TokenSubtypes.PostAction))
        {
            Consume(TokenSubtypes.OpenBrace, "Was expected '{'", null);
            Consume(TokenSubtypes.Type, "Effect name declaration expected", new List<TokenSubtypes> { TokenSubtypes.CloseBrace });
            Token? colon = Consume(TokenSubtypes.Colon, "Was expected ':'", null);
            IExpression? name = Expression();
            EffectInfo info = new EffectInfo(name, null, colon);
            Selector selector = null;
            if (Match(TokenSubtypes.Selector))
            {
                selector = Selector();
            }
            PostAction postAction = PostAction();
            Consume(TokenSubtypes.CloseBrace, "Was expected '}'", null);
            return new PostAction(new ActivationData(info, selector, postAction));
        }
        else return null;
    }


}