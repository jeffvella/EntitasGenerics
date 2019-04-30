using Entitas;
using Entitas.CodeGeneration.Attributes;

[GameState]
[Unique]
[Event(EventTarget.Any)]
public sealed class ActionCountComponent : IComponent
{
    public int value;
}