﻿using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class RemoveMatchedSystem : GenericReactiveSystem<GameEntity>
{
    private readonly IGenericContext<GameEntity> _game;

    public RemoveMatchedSystem(GenericContexts contexts) : base(contexts.Game, Trigger, Filter)
    {
        _game = contexts.Game;
    }

    private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
    {
        return context.GetTriggerCollector<MatchedComponent>();
    }

    private static bool Filter(IGenericContext<GameEntity> context, GameEntity entity)
    {
        return !context.IsTagged<DestroyedComponent>(entity);
    }

    //protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    //{
    //    return context.CreateCollector(GameMatcher.Matched.Added());
    //}

    //protected override bool Filter(GameEntity entity)
    //{
    //    return !entity.isDestroyed;
    //}

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var entity in entities)
        {
            _game.SetTag<DestroyedComponent>(entity);

            //entity.SetTag<DestroyedComponent>(true);
            //entity.isDestroyed = true;           
        }
    }
}
