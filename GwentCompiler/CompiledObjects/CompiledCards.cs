namespace GwentCompiler;

[Serializable]
public class CompiledCard(string type, string name, string faction, List<string> range, List<EffectActivation> onActivation, string effectDescription, int effectNumber, string characterDescription, string quote) : CompiledObject
{
    public string Type { get; private set; } = type;
    public string Name { get; private set; } = name;
    public string Faction { get; private set; } = faction;
    public List<string> Range { get; private set; } = range;
    public string EffectDescription { get; private set; } = effectDescription;
    public List<EffectActivation> OnActivation { get; private set; } = onActivation;
    public int Power { get; private set; } = effectNumber;
    public string CharacterDescription { get; private set; } = characterDescription;
    public string Quote { get; private set; } = quote;

    public override string ToString()
    {
        string ranges = "";
        string onActivation = "";
        foreach (string range in Range)
        {
            ranges += range + " ";
        }

        foreach (var act in OnActivation)
        {
            onActivation += $"{act.EffectName}, Parameters:";

            if (act.Parameters != null)
                foreach (var parm in act.Parameters)
                {
                    onActivation += $"{parm.Key.Name} : {parm.Value} ;";
                }

            onActivation += $"{act.SelectorSource}, {act.SelectorSingle}, ";
        }
        return $"{Type}, {Name}, {Faction}, {ranges}, {onActivation} {EffectDescription}, {Power}, {CharacterDescription}, {Quote}";
    }

}