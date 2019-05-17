using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

[Unique]
public sealed class MinMatchCountComponent : IUniqueComponent
{
    public int value;
}