namespace GwentCompiler;

[Serializable]
public class CompiledCard : CompiledObject
{
    public string Type { get; private set; }
    public string Name { get; private set; }
    public string Faction { get; private set; }
    public string EffectDescription { get; private set; } 
    public int Power { get; private set; } 
    public string CharacterDescription { get; private set; } 
    public string Quote { get; private set; }

    public CompiledCard(string type, string name, string faction, string effectDescription, int effectNumber, string characterDescription, string quote)
    {
        Type = type;
        Name = name;
        Faction = faction;
        EffectDescription = effectDescription;
        Power = effectNumber;
        CharacterDescription = characterDescription;
        Quote = quote;
    }

    public override string ToString()
    {
        return $"{Type}, {Name}, {Faction}, {EffectDescription}, {Power}, {CharacterDescription}, {Quote}";
    }
    
}