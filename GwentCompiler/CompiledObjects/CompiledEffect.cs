namespace GwentCompiler;

[Serializable]
public class CompiledEffect(string name, List<Parameter>? parameters, BlockStmt block) : CompiledObject
{
    public string Name {get; private set;} = name;
    public List<Parameter>? Parameters {get; private set;} = parameters;
    public BlockStmt Block {get; private set;} = block;

    public override string ToString()
    {
        string @params = "";

        if(Parameters != null)
        {
            foreach(var param in Parameters)
            {
                @params += $"{param.Name} : {param.Type}; ";
            }

        }

        return $"{Name}, {@params}";
    }
}
