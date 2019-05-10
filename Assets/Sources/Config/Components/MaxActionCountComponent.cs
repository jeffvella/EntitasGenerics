using Entitas;
using Entitas.CodeGeneration.Attributes;

//[Config]
[Unique]
[Event(EventTarget.Any)]
public sealed class MaxActionCountComponent : IUniqueComponent
{
    public int value;
}