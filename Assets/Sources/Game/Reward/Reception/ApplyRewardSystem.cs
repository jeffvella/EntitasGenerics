using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class ApplyRewardSystem : ReactiveSystem<GameEntity>
{

    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;

    public ApplyRewardSystem(Contexts contexts, GenericContexts genericContexts) : base(contexts.game)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Reward);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasReward;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        var score = _contexts.gameState.score.value;
        var totalReward = 0;
        
        foreach (var entity in entities)
        {
            totalReward += entity.reward.value;
            entity.isDestroyed = true;
        }
        
        _contexts.gameState.ReplaceScore(score + totalReward);


        _genericContexts.GameState.SetUnique<ScoreComponent>(new ScoreComponent
        {
            value = score + totalReward
        });
    }
}