namespace GwentCompiler;
public class CardDeclaration : IProgramNode
{
    public CardTypeDeclaration? Type { get; private set; }
    public CardNameDeclaration? Name { get; private set; }
    public CardFactionDeclaration? Faction { get; private set; }
    public CardRangeDeclaration? Range { get; private set; }
    public CardPowerDeclaration? Power { get; private set; }
    public CardEffectDescriptionDeclaration? EffectDescription { get; private set; }
    public OnActivation? OnActivationField { get; private set; }
    public CardCharacterDescriptionDeclaration? CharacterDescription { get; private set; }
    public CardQuoteDeclaration? Quote { get; private set; }

    public bool SetComponent(CardComponent component)
    {
        switch (component.GetType().Name)
        {
            case "CardTypeDeclaration":

                if (Type == null)
                {
                    Type = (CardTypeDeclaration)component;
                    return true;
                }
                return false;

            case "CardNameDeclaration":

                if (Name == null)
                {
                    Name = (CardNameDeclaration)component;
                    return true;
                }
                return false;

            case "CardFactionDeclaration":

                if (Faction == null)
                {
                    Faction = (CardFactionDeclaration)component;
                    return true;
                }
                return false;

            case "CardRangeDeclaration":

                if (Range == null)
                {
                    Range = (CardRangeDeclaration)component;
                    return true;
                }
                return false;

            case "CardPowerDeclaration":

                if (Power == null)
                {
                    Power = (CardPowerDeclaration)component;
                    return true;
                }
                return false;

            case "CardEffectDescriptionDeclaration":

                if (EffectDescription == null)
                {
                    EffectDescription = (CardEffectDescriptionDeclaration)component;
                    return true;
                }
                return false;

            case "OnActivation":

                if (OnActivationField == null)
                {
                    OnActivationField = (OnActivation)component;
                    return true;
                }
                return false;

            case "CardCharacterDescriptionDeclaration":

                if (CharacterDescription == null)
                {
                    CharacterDescription = (CardCharacterDescriptionDeclaration)component;
                    return true;
                }
                return false;

            case "CardQuoteDeclaration":

                if (Quote == null)
                {
                    Quote = (CardQuoteDeclaration)component;
                    return true;
                }
                return false;

        }
        return false;
    }
}

public abstract class CardComponent : IASTNode;
public abstract class StringFieldComponent : CardComponent
{
    public Token Operator {get; private set;}
    public IExpression Value { get; private set; }
    public StringFieldComponent(IExpression value, Token @operator)
    {
        Value = value;
        Operator = @operator;
    }
}
public class CardTypeDeclaration(IExpression value, Token @operator) : StringFieldComponent(value, @operator);
public class CardNameDeclaration(IExpression value, Token @operator) : StringFieldComponent(value, @operator);
public class CardFactionDeclaration(IExpression value, Token @operator) : StringFieldComponent(value, @operator);
public class CardRangeDeclaration : CardComponent
{
    public List<IExpression?> Ranges {get; private set;}

    public CardRangeDeclaration(List<IExpression?> value)
    {
        Ranges = value;
    }
}
public class CardPowerDeclaration(IExpression value, Token @operator) : StringFieldComponent(value, @operator);
public class CardEffectDescriptionDeclaration(IExpression value, Token @operator) : StringFieldComponent(value, @operator);
public class CardCharacterDescriptionDeclaration(IExpression value, Token @operator) : StringFieldComponent(value, @operator);
public class CardQuoteDeclaration(IExpression value, Token @operator) : StringFieldComponent(value, @operator);
public class OnActivation : CardComponent;
