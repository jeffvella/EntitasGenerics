using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class GameOverSystem : GenericReactiveSystem<GameStateEntity>
{
    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;

    public GameOverSystem(Contexts contexts, GenericContexts genericContexts) : base(genericContexts.GameState, Trigger)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
    }

    private static ICollector<GameStateEntity> Trigger(IGenericContext<GameStateEntity> context)
    {
        return context.GetTriggerCollector<ActionCountComponent>();
    }

    //protected override ICollector<GameStateEntity> GetTrigger(IContext<GameStateEntity> context)
    //{
    //    return context.CreateCollector(GameStateMatcher.ActionCount);
    //}

    //protected override bool Filter(GameStateEntity entity)
    //{
    //    return true;
    //}

    protected override void Execute(List<GameStateEntity> entities)
    {
        //if (_contexts.gameState.actionCount.value >= _contexts.config.maxActionCount.value)
        //{
        //    _contexts.gameState.isGameOver = true;
        //}

        var maxActions = _genericContexts.Config.GetUnique<MaxActionCountComponent>().value;

        var actionCount = _genericContexts.GameState.GetUnique<ActionCountComponent>().value;

        if (actionCount >= maxActions)
        {
            _genericContexts.GameState.SetTag<GameOverComponent>(true);
        }

        //if (_contexts.gameState.actionCount.value >= maxActions)
        //{
        //    _contexts.gameState.isGameOver = true;
        //}

    }
}