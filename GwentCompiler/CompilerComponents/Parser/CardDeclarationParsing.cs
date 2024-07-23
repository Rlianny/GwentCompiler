namespace GwentCompiler;

public partial class Parser
{
    private IProgramNode? Card()
    {
        CardDeclaration parsedCard = new();

        Consume(TokenSubtypes.OpenBrace, "Card must declare a body");
        while (!Match(TokenSubtypes.CloseBrace))
        {
            if (Match(TokenSubtypes.Type))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a type", Previous().Location);
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card type has been defined before", Previous().Location);
                    break;
                }
            }

            if (Match(TokenSubtypes.Name))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a name", Previous().Location);
                    return null;
                }

                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card name has been defined before", Previous().Location);
                    break;
                }
            }

            if (Match(TokenSubtypes.Faction))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a faction", Previous().Location);
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card faction has been defined before", Previous().Location);
                    break;
                }
            }

            if (Match(TokenSubtypes.Range))
            {
                CardComponent? value = CardRange();
                if (value == null)
                {
                    GenerateError("The card must to declare a range", Previous().Location);
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card range has been defined before", Previous().Location);
                    break;
                }
            }

            if (Match(TokenSubtypes.Power))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a power", Previous().Location);
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card power has been defined before", Previous().Location);
                    break;
                }
            }

            if (Match(TokenSubtypes.OnActivation))
            {
                CardComponent? value = OnActivation();
                if (value == null)
                {
                    GenerateError("The card must to declare a OnActivation field", Previous().Location);
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card OnActivation field has been defined before", Previous().Location);
                    break;
                }
            }

            if (Match(TokenSubtypes.EffectDescription))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a effect description", Previous().Location);
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The effect description has been defined before", Previous().Location);
                    break;
                }
            }

            if (Match(TokenSubtypes.CharacterDescription))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a character description", Previous().Location);
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The character description has been defined before", Previous().Location);
                    break;
                }
            }

            if (Match(TokenSubtypes.Quote))
            {
                CardComponent? value = GenericField();
                if (value == null)
                {
                    GenerateError("The card must to declare a quote", Previous().Location);
                    return null;
                }
                if (!parsedCard.SetComponent(value))
                {
                    GenerateError("The card quote has been defined before", Previous().Location);
                    break;
                }
            }
        }

        //Consume(TokenSubtypes.OpenBrace, "Expect '}' after card body");
        return parsedCard;

    }

    private CardComponent? GenericField()
    {
        var component = Previous();
        Token? colon = Consume(TokenSubtypes.Colon, "Expected ':'");
        if (colon == null) return null;
        IExpression? value = Expression();

        if (value == null) return null;

        Consume(TokenSubtypes.Comma, "Expected ',' after field declaration");

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
        Consume(TokenSubtypes.OpenBrace, "Expect '['");
        while (!Match(TokenSubtypes.CloseBrace))
        {
            ranges.Add(Expression());
            if (!Check(TokenSubtypes.CloseBrace))
            {
                Consume(TokenSubtypes.Comma, "Expect ','");
            }
        }
        return new CardRangeDeclaration(ranges);
    }

    private CardComponent? OnActivation()
    {
        return null;
    }
}