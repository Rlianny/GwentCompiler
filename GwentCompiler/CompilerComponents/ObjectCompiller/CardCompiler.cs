namespace GwentCompiler;

public partial class ObjectCompiller
{
    private CompiledCard GetCompiledCard(CardDeclaration node)
    {
        Interpreter interpreter = new Interpreter();

        string? cardName = null;
        if(node.Name != null && interpreter.Interpret(node.Name) is string stringName) cardName = stringName;
        
        string? cardType = null;
        if(node.Type != null && interpreter.Interpret(node.Type) is string stringType) cardType = stringType;

        string? cardFaction = null;
        if(node.Faction != null && interpreter.Interpret(node.Faction) is string stringFaction) cardFaction = stringFaction;

        string? effectDescription = null;
        if(node.EffectDescription != null &&interpreter.Interpret(node.EffectDescription) is string stringEffectDescription) effectDescription = stringEffectDescription;

        string? characterDescription = null;
        if(node.CharacterDescription != null && interpreter.Interpret(node.CharacterDescription) is string stringCharacterDescription) characterDescription = stringCharacterDescription;

        string? quote = null;
        if(node.Quote != null && interpreter.Interpret(node.Quote) is string stringQuote) quote = stringQuote;

        int power = 0;
        if(node.Power != null && interpreter.Interpret(node.Power) is int intPower) power = intPower;

        return new CompiledCard(cardType, cardName, cardFaction, effectDescription, power, characterDescription, quote);
        
    }

}