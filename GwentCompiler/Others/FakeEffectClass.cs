namespace GwentCompiler;

public class Effect
{
    public static Dictionary<string, Effect> AllEffects = new();

    public List<Parameter> Parameters = new(); 
    public string Name { get; set; }

    public Effect(string name, List<Parameter> parameters)
    {
        Name = name;
        Parameters = parameters;
    }
}