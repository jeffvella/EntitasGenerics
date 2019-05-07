﻿using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class GameStateRestartSystem : GenericReactiveSystem<InputEntity>
{
    private readonly Contexts _contexts;

    public GameStateRestartSystem(Contexts contexts) : base(contexts.GenericTemp.Input, Trigger)
    {
        _contexts = contexts;
    }

    private static ICollector<InputEntity> Trigger(IGenericContext<InputEntity> context)
    {
        return context.GetTriggerCollector<RestartComponent>();
    }

    //protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    //{
    //    return context.CreateCollector(InputMatcher.Restart.Added());
    //}

    //protected override bool Filter(InputEntity entity)
    //{
    //    return true;
    //}

    protected override void Execute(List<InputEntity> entities)
    {
        _contexts.gameState.ResetState();

        _contexts.GenericTemp.GameState.ResetState();

    }

}
