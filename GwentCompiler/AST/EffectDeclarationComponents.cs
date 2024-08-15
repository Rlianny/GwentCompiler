namespace GwentCompiler;

public interface IEffectComponent : IComponent;

public class EffectParamsDeclaration(List<ParsedParam> parameters) : IEffectComponent
{
    public List<ParsedParam> Parameters {get; private set;} = parameters;
}

public class EffectAction(Variable targets, Variable context, BlockStmt block) : IEffectComponent
{
    public Variable TargetsId {get; private set;} = targets;
    public Variable ContextId {get; private set;} = context;
    public BlockStmt BlockStmt {get; private set;} = block;
}
