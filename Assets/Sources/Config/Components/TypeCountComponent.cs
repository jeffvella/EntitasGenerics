using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

//[Config]
[Unique]
public sealed class TypeCountComponent : IUniqueComponent
{
    public int Value;
}
