namespace GwentCompiler;

public abstract class CardComponent : IASTNode;

#region CardComponents
public abstract class GenericFieldComponent(IExpression value, Token @operator) : CardComponent
{
    public Token Operator { get; private set; } = @operator;
    public IExpression Value { get; private set; } = value;
}
public class CardRangeDeclaration(List<IExpression?> value, Token op) : CardComponent
{
    public List<IExpression?> Ranges { get; private set; } = value;
    public Token Operator { get; private set; } = op;
}
public class OnActivation(List<ActivationData> data, Token op) : CardComponent
{
    public Token Operator { get; private set; } = op;
    public List<ActivationData> Activations = data;
}
public class ActivationData(EffectInfo effect, Selector? selector, PostAction postAction) : CardComponent
{
    public EffectInfo Effect = effect;
    public Selector? Selector = selector;
    public PostAction PostAction = postAction;
}

#endregion

#region GenericFieldComponent
public class CardTypeDeclaration(IExpression value, Token @operator) : GenericFieldComponent(value, @operator);
public class CardNameDeclaration(IExpression value, Token @operator) : GenericFieldComponent(value, @operator);
public class CardFactionDeclaration(IExpression value, Token @operator) : GenericFieldComponent(value, @operator);
public class CardPowerDeclaration(IExpression value, Token @operator) : GenericFieldComponent(value, @operator);
public class CardEffectDescriptionDeclaration(IExpression value, Token @operator) : GenericFieldComponent(value, @operator);
public class CardCharacterDescriptionDeclaration(IExpression value, Token @operator) : GenericFieldComponent(value, @operator);
public class CardQuoteDeclaration(IExpression value, Token @operator) : GenericFieldComponent(value, @operator);

#endregion

#region OnActivationFieldComponents
public class EffectInfo(IExpression effectName, List<OnActivationParam> @params, Token colon)
{
    public IExpression EffectName { get; private set; } = effectName;
    public List<OnActivationParam> ActivationParams { get; private set; } = @params;
    public Token Colon { get; private set; } = colon;
}

public class Selector(SelectorSource source, SelectorSingle single, SelectorPredicate predicate)
{
    public SelectorSource Source { get; private set; } = source;
    public SelectorSingle Single { get; private set; } = single;
    public SelectorPredicate Predicate { get; private set; } = predicate;
}

public class SelectorSource(IExpression source, Token colon)
{
    public IExpression Source { get; private set; } = source;
    public Token Operator { get; private set; } = colon;
}

public class SelectorSingle(IExpression single, Token op)
{
    public IExpression Single { get; private set; } = single;
    public Token Operator { get; private set; } = op;
}

public class SelectorPredicate(LambdaExpr expression)
{
    public LambdaExpr LambdaExpression { get; private set; } = expression;
}

public class PostAction(ActivationData linkedEffect)
{
    public ActivationData LinkedEffect { get; private set; } = linkedEffect;
}

public class OnActivationParam(Variable variable, Token colon, IExpression expression)
{
    public Variable VarName { get; private set; } = variable;
    public Token Colon { get; private set; } = colon;
    public IExpression Value { get; private set; } = expression;
}

#endregion