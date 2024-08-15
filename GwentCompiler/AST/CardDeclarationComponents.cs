namespace GwentCompiler;

public interface ICardComponent : IComponent;

#region CardComponents
public abstract class GenericFieldComponent(IExpression value, CodeLocation @operator) : ICardComponent, IEffectComponent
{
    public CodeLocation Operator { get; private set; } = @operator;
    public IExpression Value { get; private set; } = value;
}
public class CardRangeDeclaration(List<IExpression> value, CodeLocation @operator) : ICardComponent
{
    public List<IExpression> Ranges { get; private set; } = value;
    public CodeLocation Operator { get; private set; } = @operator;
}
public class OnActivation(List<ActivationData> data, CodeLocation @operator) : ICardComponent
{
    public CodeLocation Operator { get; private set; } = @operator;
    public List<ActivationData> Activations = data;
}
public class ActivationData(EffectInfo effect, Selector? selector, PostAction? postAction) : ICardComponent
{
    public EffectInfo Effect = effect;
    public Selector? Selector = selector;
    public PostAction? PostAction = postAction;
}

#endregion

#region GenericFieldComponent
public class CardTypeDeclaration(IExpression value, CodeLocation @operator) : GenericFieldComponent(value, @operator);
public class NameDeclaration(IExpression value, CodeLocation @operator) : GenericFieldComponent(value, @operator);
public class CardFactionDeclaration(IExpression value, CodeLocation @operator) : GenericFieldComponent(value, @operator);
public class CardPowerDeclaration(IExpression value, CodeLocation @operator) : GenericFieldComponent(value, @operator);
public class CardEffectDescriptionDeclaration(IExpression value, CodeLocation @operator) : GenericFieldComponent(value, @operator);
public class CardCharacterDescriptionDeclaration(IExpression value, CodeLocation @operator) : GenericFieldComponent(value, @operator);
public class CardQuoteDeclaration(IExpression value, CodeLocation @operator) : GenericFieldComponent(value, @operator);

#endregion

#region OnActivationFieldComponents
public class EffectInfo(IExpression? effectName, List<ParsedParam>? @params, CodeLocation colon)
{
    public IExpression? EffectName { get; private set; } = effectName;
    public List<ParsedParam>? ActivationParams { get; private set; } = @params;
    public CodeLocation Colon { get; private set; } = colon;
}

public class Selector(SelectorSource source, SelectorSingle? single, SelectorPredicate? predicate)
{
    public SelectorSource Source { get; private set; } = source;
    public SelectorSingle? Single { get; private set; } = single;
    public SelectorPredicate? Predicate { get; private set; } = predicate;
}

public class SelectorSource(IExpression? source, CodeLocation colon)
{
    public IExpression? Source { get; private set; } = source;
    public CodeLocation Operator { get; private set; } = colon;
}

public class SelectorSingle(IExpression? single, CodeLocation op)
{
    public IExpression? Single { get; private set; } = single;
    public CodeLocation Operator { get; private set; } = op;
}

public class SelectorPredicate(LambdaExpr expression)
{
    public LambdaExpr LambdaExpression { get; private set; } = expression;
}

public class PostAction(ActivationData linkedEffect)
{
    public ActivationData LinkedEffect { get; private set; } = linkedEffect;
}

public class ParsedParam
{
    public Variable VarName { get; private set; } 
    public CodeLocation Colon { get; private set; } 
    public IExpression? Value { get; private set; }
    public Token? Type {get; private set;}

    public ParsedParam(Variable variable, CodeLocation colon, Token type)
    {
        VarName = variable;
        Colon = colon;
        Type = type;
    }

    public ParsedParam(Variable variable, CodeLocation colon, IExpression? expression)
    {
        VarName = variable;
        Colon = colon;
        Value = expression;
    }
}

#endregion