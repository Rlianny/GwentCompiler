namespace GwentCompiler;

public partial class Interpreter : VisitorBase<Object>
{
    public object? Visit(CardTypeDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if(value is string stringType)
        {
            if(stringType == "Oro" || stringType == "Plata" || stringType == "Líder") return value;
            else
            {
                throw new RuntimeError("Invalid type declaration, only the following types are accepted: 'Oro', 'Plata', 'Líder'", declaration.Operator.Location);
            }
        }

        else
        {
            throw new RuntimeError("The type must to be a string value", declaration.Operator.Location);
        }
    }

    public object? Visit(CardNameDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if(value != null && value is string stringName)
        {
            return "Morty " + stringName;
        }
        else
        {
            throw new RuntimeError("The name must to be a string value", declaration.Operator.Location);
        }
    }

    public object? Visit(CardFactionDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if(value is string stringFaction)
        {
            return stringFaction;
        }
        else
        {
            throw new RuntimeError("The faction must to be a string value", declaration.Operator.Location);
        }
    }

    public object? Visit(CardPowerDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if(value is int intPower)
        {
            return intPower;
        }
        else
        {
            throw new RuntimeError("The power must to be a integer value", declaration.Operator.Location);
        }
    }

    public object? Visit(CardEffectDescriptionDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if(value is string stringValue)
        {
            return stringValue;
        }
        else
        {
            throw new RuntimeError("The effect description must to be a string value", declaration.Operator.Location);
        }
    }

    public object? Visit(CardCharacterDescriptionDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if(value is string stringValue)
        {
            return stringValue;
        }
        else
        {
            throw new RuntimeError("The character description must to be a string value", declaration.Operator.Location);
        }
    }

    public object? Visit(CardQuoteDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if(value is string stringValue)
        {
            return stringValue;
        }
        else
        {
            throw new RuntimeError("The quote must to be a string value", declaration.Operator.Location);
        }
    }
}