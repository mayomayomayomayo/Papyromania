using System;
using System.Collections.Generic;

public sealed class CardUseContext
{
    private readonly Dictionary<Type, CardEffect> _fx = new();

    public T Get<T>() where T : CardEffect, new()
    {
        if (!_fx.TryGetValue(typeof(T), out CardEffect effect))
        {
            effect = new T();
            _fx[typeof(T)] = effect;
        }
        return (T)effect;
    }
    
    public CardUseContext(CardEffect[] fx)
    {
        _fx = new();

        foreach (CardEffect effect in fx)
        {
            Type t = effect.GetType();

            if (_fx.ContainsKey(t)) 
                throw new Exception($"Duplicate CardEffects are not supported");

            _fx[t] = effect;
        }
    }
}