using Entitas;
using Entitas.CodeGeneration.Attributes;

//[Game]
[Event(EventTarget.Any, EventType.Added)]
[Event(EventTarget.Any, EventType.Removed)]
public sealed class SelectedComponent : IFlagComponent
{
}