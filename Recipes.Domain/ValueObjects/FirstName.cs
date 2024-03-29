﻿using Recipes.Domain.Primitives;
using Recipes.Domain.Shared;

namespace Recipes.Domain.ValueObjects;

public sealed class FirstName : ValueObject
{
    public const int MaxLenght = 50;
    public string Value { get; }
    private FirstName(string value)
    {
        Value = value;
    }
    public static Result<FirstName> Create(string value)
    {
        return new FirstName(value);
    }
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
