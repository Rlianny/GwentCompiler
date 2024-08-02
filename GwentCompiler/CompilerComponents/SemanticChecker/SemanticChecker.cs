namespace GwentCompiler;

public class SemanticChecker : VisitorBase<bool>
{
    public bool CheckSemantic(IASTNode node)
    {
        return VisitBase(node);
    }

    

}