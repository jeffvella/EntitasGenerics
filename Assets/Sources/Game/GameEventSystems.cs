using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;

public sealed class GameEventSystems : Feature
{
    public GameEventSystems(Contexts contexts)
    {
        Add(new EventSystem<GameEntity, ColorComponent>(contexts.Game, GroupEvent.Added));
        Add(new EventSystem<GameEntity, DestroyedComponent>(contexts.Game, GroupEvent.Added));
        Add(new EventSystem<GameEntity, PositionComponent>(contexts.Game, GroupEvent.Added));
        Add(new EventSystem<GameEntity, SelectedComponent>(contexts.Game, GroupEvent.AddedOrRemoved));
    }
}