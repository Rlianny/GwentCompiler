namespace GwentCompiler;

public partial class ObjectCompiler : VisitorBase<object>
{
    private List<IProgramNode?> nodes;
    public ObjectCompiler(List<IProgramNode?> nodes)
    {
        this.nodes = nodes;
    }  

    public List<CompiledObject> CompileObjects()
    {
        List<CompiledObject> compiledObjects = new();
        
        foreach(var node in nodes)
        {
            if(node is CardDeclaration cardDeclaration)
            {
                try
                {
                    compiledObjects.Add(GetCompiledCard(cardDeclaration));
                }
                catch (RuntimeError ex)
                {
                    hadError = true;
                    Console.WriteLine(ex.Message);
                }
            }
        }

        return compiledObjects;
    }
}