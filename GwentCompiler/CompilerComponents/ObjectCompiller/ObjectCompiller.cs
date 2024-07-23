namespace GwentCompiler;

public partial class ObjectCompiller : VisitorBase<object>
{
    private List<IProgramNode> nodes;
    public ObjectCompiller(List<IProgramNode> nodes)
    {
        this.nodes = nodes;
    }  

    public List<CompiledObject> CompileObjects()
    {
        List<CompiledObject> compiledObjects = new();
        
        foreach(var node in nodes)
        {
            if(node is CardDeclaration cardDeclaration)
            compiledObjects.Add(GetCompiledCard(cardDeclaration));
        }

        return compiledObjects;
    }
}