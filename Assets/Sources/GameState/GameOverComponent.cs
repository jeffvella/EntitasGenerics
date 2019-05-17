using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

//[GameState]
[Unique]
[Event(EventTarget.Any, EventType.Added)]
[Event(EventTarget.Any, EventType.Removed)]
public sealed class GameOverComponent : IUniqueComponent, IFlagComponent
{
}