namespace GwentCompiler;

public partial class Interpreter : VisitorBase<Object>
{
    public object? Visit(CardTypeDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value is string stringType)
        {
            if (stringType == "Oro" || stringType == "Plata" || stringType == "Líder") return value;
            else
            {
                throw new RuntimeError("Invalid type declaration, only the following types are accepted: 'Oro', 'Plata', 'Líder'", declaration.Operator.Location);
            }
        }

        else
        {
            throw new RuntimeError("The type must to be a string value", declaration.Operator.Location);
        }
    }

    public object? Visit(CardNameDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value != null && value is string stringName)
        {
            return "Morty " + stringName;
        }
        else
        {
            throw new RuntimeError("The name must to be a string value", declaration.Operator.Location);
        }
    }

    public object? Visit(CardFactionDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value is string stringFaction)
        {
            return stringFaction;
        }
        else
        {
            throw new RuntimeError("The faction must to be a string value", declaration.Operator.Location);
        }
    }

    public object? Visit(CardPowerDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value is double intPower)
        {
            if (intPower > 0)
                return intPower;

            else throw new RuntimeError("The power must to be a positive integer value", declaration.Operator.Location);
        }
        else
        {
            throw new RuntimeError("The power must to be a integer value", declaration.Operator.Location);
        }
    }

    public object? Visit(CardEffectDescriptionDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value is string stringValue)
        {
            return stringValue;
        }
        else
        {
            throw new RuntimeError("The effect description must to be a string value", declaration.Operator.Location);
        }
    }

    public object? Visit(CardCharacterDescriptionDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value is string stringValue)
        {
            return stringValue;
        }
        else
        {
            throw new RuntimeError("The character description must to be a string value", declaration.Operator.Location);
        }
    }

    public object? Visit(CardQuoteDeclaration declaration)
    {
        var value = Evaluate(declaration.Value);

        if (value is string stringValue)
        {
            return stringValue;
        }
        else
        {
            throw new RuntimeError("The quote must be a string value", declaration.Operator.Location);
        }
    }

    public object? Visit(CardRangeDeclaration declaration)
    {
        List<string> ranges = new();

        bool melee = false, ranged = false, siege = false;

        foreach (IExpression expression in declaration.Ranges)
        {
            var value = Evaluate(expression);

            if (value is string stringValue)
            {
                if (stringValue == "Melee")
                {
                    if (melee) throw new RuntimeError("The range 'Melee' has already been declared", declaration.Operator.Location);
                    melee = true;
                    ranges.Add(stringValue);
                    continue;
                }

                if (stringValue == "Ranged")
                {
                    if (ranged) throw new RuntimeError("The range 'Ranged' has already been declared", declaration.Operator.Location);
                    ranged = true;
                    ranges.Add(stringValue);
                    continue;
                }

                if (stringValue == "Siege")
                {
                    if (siege) throw new RuntimeError("The range 'Siege' has already been declared", declaration.Operator.Location);
                    siege = true;
                    ranges.Add(stringValue);
                    continue;
                }

                throw new RuntimeError("The range is not valid, only the ranks 'melee', 'siege' and 'ranged' are accepted", declaration.Operator.Location);
            }
            else
            {
                throw new RuntimeError("The range must be a string value", declaration.Operator.Location);
            }
        }

        return ranges;
    }

    public object? Visit(OnActivation onActivation)
    {
        List<EffectActivation> effects = new();

        foreach (var activation in onActivation.Activations)
        {
            effects.Add((EffectActivation)Visit(activation, true));
        }

        return effects;
    }

    public object? Visit(ActivationData data, bool isRoot)
    {
        string? effectName = null;
        Dictionary<Parameter, object>? Params = null;
        string? selectorSource = null;
        bool? selectorSingle = null;
        EffectActivation? postAction = null;

        var name = Evaluate(data.Effect.EffectName);
        if (name is string stringName) effectName = stringName;
        else throw new RuntimeError("The name must be a string value", data.Effect.Colon.Location);

        if (Effect.AllEffects.ContainsKey(stringName))
        {
            Dictionary<Parameter, object> parameter = new();

            if (data.Effect.ActivationParams != null)
            {
                foreach (var activationParam in data.Effect.ActivationParams)
                {
                    object? paramValue = Evaluate(activationParam.Value);
                    string paramName = activationParam.VarName.Value.Lexeme;
                    Parameter? newParam = null;

                    if (paramValue is string stringValue)
                    {
                        newParam = new Parameter(paramName, ValueType.String);
                        paramValue = stringValue;
                    }

                    else if (paramValue is bool booleanValue)
                    {
                        newParam = new Parameter(paramName, ValueType.Boolean);
                        paramValue = booleanValue;
                    }

                    else if (paramValue is double numericValue)
                    {
                        newParam = new Parameter(paramName, ValueType.Number);
                        paramValue = numericValue;
                    }

                    else throw new RuntimeError("The parameter value must be string, number or boolean", activationParam.Colon.Location);

                    if (Effect.AllEffects[stringName].Parameters.Contains(newParam)) parameter.Add(newParam, paramValue);

                    else throw new RuntimeError("Invalid parameter declaration, the effect does not contains this parameter", activationParam.VarName.Value.Location);
                }

                if(Effect.AllEffects[stringName].Parameters.Count != parameter.Count) throw new RuntimeError("Invalid parameter declaration, faltan parametros", data.Effect.ActivationParams[0].Colon.Location);
                Params = parameter;
            }

            else if (Effect.AllEffects[stringName].Parameters == null) Params = null;

            else throw new RuntimeError("Params must be declared", data.Effect.Colon.Location);
        }

        else throw new RuntimeError("The effect must be declared before", data.Effect.Colon.Location);

        if (data.Selector != null)
        {
            var source = Evaluate(data.Selector.Source.Source);

            if (source is string stringSource)
            {
                if (stringSource == "board" || stringSource == "hand" || stringSource == "otherHand" || stringSource == "deck" || stringSource == "otherDeck" || stringSource == "field" || stringSource == "parent" && isRoot == false) selectorSource = stringSource;

                else throw new RuntimeError("Inavlid source declaration", data.Selector.Source.Operator.Location);
            }
            else throw new RuntimeError("The source must be a string value", data.Selector.Source.Operator.Location);

            var single = Evaluate(data.Selector.Single.Single);

            if (single is bool booleanSingle) selectorSingle = booleanSingle;

            else throw new RuntimeError("The single must to be a boolean value", data.Selector.Single.Operator.Location);

            //comprobación de predicate
        }

        else if (isRoot == false)
        {
            selectorSource = null;
            selectorSingle = null;
        }

        else throw new RuntimeError("The Selector must be declared", data.Effect.Colon.Location);

        if (data.PostAction != null)
        {
            postAction = (EffectActivation)Visit(data.PostAction.LinkedEffect, false);
        }

        return new EffectActivation(effectName, Params, selectorSource, selectorSingle, postAction);
    }
}