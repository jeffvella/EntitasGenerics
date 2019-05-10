using System.Collections.Generic;
using Assets.Sources.Config;
using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;

public sealed class RewardEmitterSystem : GenericReactiveSystem<GameEntity>
{
    private readonly IGenericContext<ConfigEntity> _config;
    private readonly IGenericContext<GameEntity> _game;

    public RewardEmitterSystem(GenericContexts contexts) : base(contexts.Game, Trigger, Filter)
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
        return context.IsFlagged<MatchedComponent>(entity);
    }

    //protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    //{
    //    return context.CreateCollector(GameMatcher.Matched.Added());
    //}

    //protected override bool Filter(GameEntity entity)
    //{
    //    return entity.isMatched;
    //}

    protected override void Execute(List<GameEntity> entities)
    {
        //var table = _contexts.config.scoringTable.value;
        var table = _config.GetUnique<ScoringTableComponent>().value;

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