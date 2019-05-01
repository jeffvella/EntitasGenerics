using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class ActionCounterSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;

    public ActionCounterSystem(Contexts contexts, GenericContexts genericContexts) : base(contexts.game)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Matched);
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        _contexts.gameState.ReplaceActionCount(_contexts.gameState.actionCount.value + 1);
    }
}