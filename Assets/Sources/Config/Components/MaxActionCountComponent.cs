using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;


[Unique]
[Event(EventTarget.Any)]
public sealed class MaxActionCountComponent : IUniqueComponent
{
    public int value;
}