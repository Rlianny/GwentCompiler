namespace GwentCompiler;

public class Parameter(string name, ValueType type)
{
    public string Name { get; private set; } = name;
    public ValueType Type { get; private set; } = type;

    public override bool Equals(object? obj)
    {
        if (obj is Parameter param)
        {
            if (param.Name == this.Name && param.Type == this.Type) return true;
            else return false;
        }

        else return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}