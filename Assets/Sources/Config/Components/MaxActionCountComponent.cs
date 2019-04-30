using Entitas;
using Entitas.CodeGeneration.Attributes;

[Config]
[Unique]
[Event(EventTarget.Any)]
public sealed class MaxActionCountComponent : IComponent
{
    public int value;
}