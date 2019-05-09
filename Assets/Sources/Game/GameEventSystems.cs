using Entitas;
using Entitas.Generics;

public sealed class GameEventSystems : Feature
{
    public GameEventSystems(GenericContexts contexts)
    {
        Add(EventSystemFactory.Create<GameEntity, ColorComponent>(contexts.Game));
        Add(EventSystemFactory.Create<GameEntity, DestroyedComponent>(contexts.Game));
        Add(EventSystemFactory.Create<GameEntity, PositionComponent>(contexts.Game));

        //Add(EventSystemFactory.Create<GameEntity, SelectedComponent>(contexts.Game, GroupEvent.AddedOrRemoved));

        Add(new GenericEventSystem<GameEntity, SelectedComponent>(contexts.Game, GroupEvent.AddedOrRemoved));

        //Add(EventSystemFactory.Create<GameEntity, SelectedComponent>(contexts.Game, GroupEvent.Removed));
    }
}