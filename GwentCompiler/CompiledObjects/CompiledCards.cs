namespace GwentCompiler;

[Serializable]
public class CompiledCard : CompiledObject
{
    public string Type { get; private set; }
    public string Name { get; private set; }
    public string Faction { get; private set; }
    public List<string> Range { get; private set; }
    public string EffectDescription { get; private set; }
    public OnActivation OnActivation { get; private set; }
    public int Power { get; private set; }
    public string CharacterDescription { get; private set; }
    public string Quote { get; private set; }

    public CompiledCard(string type, string name, string faction, List<string> range, string effectDescription, int effectNumber, string characterDescription, string quote)
    {
        Type = type;
        Name = name;
        Faction = faction;
        EffectDescription = effectDescription;
        Power = effectNumber;
        CharacterDescription = characterDescription;
        Quote = quote;
        Range = range;
    }

    public override string ToString()
    {
        string ranges = "";
        foreach (string range in Range)
        {
            ranges += range + " ";
        }
        return $"{Type}, {Name}, {Faction}, {ranges}, {EffectDescription}, {Power}, {CharacterDescription}, {Quote}";
    }

}