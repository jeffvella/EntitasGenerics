using Entitas;
using Entitas.CodeGeneration.Attributes;

//[GameState]
[Unique]
[Event(EventTarget.Any)]
public sealed class ActionCountComponent : IUniqueComponent
{
    public int value;
}


