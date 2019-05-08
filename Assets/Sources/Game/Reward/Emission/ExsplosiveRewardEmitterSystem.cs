using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class ExsplosiveRewardEmitterSystem : GenericReactiveSystem<GameEntity>
{
    private readonly IGenericContext<ConfigEntity> _config;
    private readonly IGenericContext<GameEntity> _game;

    public ExsplosiveRewardEmitterSystem(GenericContexts contexts) : base(contexts.Game, Trigger, Filter)
    {
        _game = contexts.Game;
        _config = contexts.Config;
    }

    private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
    {
        return context.GetTriggerCollector<MatchedComponent>(GroupEvent.Added);
    }

    private static bool Filter(IGenericContext<GameEntity> context, GameEntity entity)
    {
        return context.IsTagged<MatchedComponent>(entity) && context.IsTagged<ExplosiveComponent>();
    }


    //protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    //{
    //    return context.CreateCollector(GameMatcher.Matched.Added());
    //}

    //protected override bool Filter(GameEntity entity)
    //{
    //    return entity.isMatched && entity.isExsplosive;
    //}

    protected override void Execute(List<GameEntity> entities)
    {
        //var table = _contexts.config.ExplosiveScoringTable.value;
        var table = _config.GetUnique<ExplosiveScoringTableComponent>().value;

        var scoreId = entities.Count;
        scoreId--;

        if (scoreId >= table.Count)
            scoreId = table.Count - 1;

        var reward = table[scoreId];

        var e = _game.CreateEntity();
        //e.AddReward(reward);
        _game.Set(e, new RewardComponent { value = reward });
    }
}