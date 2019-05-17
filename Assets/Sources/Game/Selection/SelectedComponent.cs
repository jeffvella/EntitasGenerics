using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

//[Game]
[Event(EventTarget.Any, EventType.Added)]
[Event(EventTarget.Any, EventType.Removed)]
public sealed class SelectedComponent : IFlagComponent
{
}