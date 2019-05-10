using Entitas;
using Entitas.CodeGeneration.Attributes;

//[Config]
[Unique]
public sealed class MinMatchCountComponent : IUniqueComponent
{
    public int value;
}