using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

[Game]
public sealed class IdComponent : IComponent, IEquatable<int>
{
    [PrimaryEntityIndex] public int value;

    public bool Equals(int other) => value.Equals(other);
}
