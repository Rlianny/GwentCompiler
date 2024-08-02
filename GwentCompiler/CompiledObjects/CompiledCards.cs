namespace GwentCompiler;

[Serializable]
public class CompiledCard : CompiledObject
{
    public string Type { get; private set; }
    public string Name { get; private set; }
    public string Faction { get; private set; }
    public List<string> Range { get; private set; }
    public string EffectDescription { get; private set; }
    public List<EffectActivation> OnActivation { get; private set; }
    public int Power { get; private set; }
    public string CharacterDescription { get; private set; }
    public string Quote { get; private set; }

    public CompiledCard(string type, string name, string faction, List<string> range, List<EffectActivation> onActivation, string effectDescription, int effectNumber, string characterDescription, string quote)
    {
        Type = type;
        Name = name;
        Faction = faction;
        EffectDescription = effectDescription;
        Power = effectNumber;
        CharacterDescription = characterDescription;
        Quote = quote;
        Range = range;
        OnActivation = onActivation;
    }

    public override string ToString()
    {
        string ranges = "";
        string onActivation = "";
        foreach (string range in Range)
        {
            ranges += range + " ";
        }

        foreach(var act in OnActivation)
        {
            onActivation += $"{act.EffectName}, Parameters:";

            foreach(var parm in act.Parameters)
            {
                onActivation += $"{parm.Key.Name} : {parm.Value} ;";
            }

            onActivation += $"{act.SelectorSource}, {act.SelectorSingle}, ";
        }
        return $"{Type}, {Name}, {Faction}, {ranges}, {onActivation} {EffectDescription}, {Power}, {CharacterDescription}, {Quote}";
    }

}

public class EffectActivation
{
    public string EffectName { get; private set; }
    public Dictionary<Parameter, object>? Parameters { get; private set; } = new();
    public string? SelectorSource { get; private set; }
    public bool? SelectorSingle { get; private set; }
    public Delegate? SelectorPredicate { get; private set; }
    public EffectActivation? PostAction { get; private set; }

    public EffectActivation(string name, Dictionary<Parameter, object>? @params, string? source, bool? single, EffectActivation? postAction)
    {
        EffectName = name;
        Parameters = @params;
        SelectorSource = source;
        SelectorSingle = single;
        // predicate 
        PostAction = postAction;
    }

    public void Excecute()
    {

    }
}

public class Parameter
{
    public string Name { get; private set; }
    public ValueType Type { get; private set; }

    public Parameter(string name, ValueType type)
    {
        Name = name;
        Type = type;
    }

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

public enum ValueType
{
    String, Number, Boolean, Card, CardList,
}