using System.Collections.Generic;
using Assets.Sources.Config;
using Assets.Sources.GameState;
using Entitas;
using Entitas.Generics;

public sealed class GameOverSystem : GenericReactiveSystem<GameStateEntity>
{
    private IGenericContext<ConfigEntity> _config;
    private IGenericContext<GameStateEntity> _gameState;

    public GameOverSystem(Contexts contexts) : base(contexts.GameState, Trigger)
    {
        _config = contexts.Config;
        _gameState = contexts.GameState;
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

        var maxActions = _config.GetUnique<MaxActionCountComponent>().Value;
        var actionCount = _gameState.GetUnique<ActionCountComponent>().value;

        if (actionCount >= maxActions)
        {
            _gameState.SetFlag<GameOverComponent>();
        }

        //if (_contexts.gameState.actionCount.value >= maxActions)
        //{
        //    _contexts.gameState.isGameOver = true;
        //}

    }
}