using System.Collections.Generic;
using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;

public sealed class RemoveMatchedSystem : GenericReactiveSystem<GameEntity>
{
    private readonly IGenericContext<GameEntity> _game;

    public RemoveMatchedSystem(Contexts contexts) : base(contexts.Game, Trigger, Filter)
    {
        _game = contexts.Game;
    }

    private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
    {
        return context.GetTriggerCollector<MatchedComponent>();
    }

    private static bool Filter(IGenericContext<GameEntity> context, GameEntity entity)
    {
        return !context.IsFlagged<DestroyedComponent>(entity);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            _game.SetFlag<DestroyedComponent>(entity);    
        }
    }
}
