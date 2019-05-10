using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

//[Game]
public sealed class SelectionIdComponent : IComponent, IEquatable<int>
{
    [PrimaryEntityIndex] public int value;
    public bool Equals(int other)
    {
        return value ==  other;
    }
}