using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class GameOverSystem : ReactiveSystem<GameStateEntity>
{
    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;

    public GameOverSystem(Contexts contexts, GenericContexts genericContexts) : base(contexts.gameState)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
    }

    protected override ICollector<GameStateEntity> GetTrigger(IContext<GameStateEntity> context)
    {
        return context.CreateCollector(GameStateMatcher.ActionCount);
    }

    protected override bool Filter(GameStateEntity entity)
    {
        return true;
    }

    protected override void Execute(List<GameStateEntity> entities)
    {
        //if (_contexts.gameState.actionCount.value >= _contexts.config.maxActionCount.value)
        //{
        //    _contexts.gameState.isGameOver = true;
        //}

        var maxActions = _genericContexts.Config.Get<MaxActionCountComponent>().value;

        if (_contexts.gameState.actionCount.value >= maxActions)
        {
            _contexts.gameState.isGameOver = true;
        }

    }
}