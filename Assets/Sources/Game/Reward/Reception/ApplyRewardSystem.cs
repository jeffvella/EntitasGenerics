using System.Collections.Generic;
using Assets.Sources.Config;
using Assets.Sources.Game;
using Assets.Sources.GameState;
using Entitas;
using Entitas.Generics;
using UnityEngine.SocialPlatforms.Impl;

public sealed class ApplyRewardSystem : GenericReactiveSystem<GameEntity>
{
    private readonly IGenericContext<ConfigEntity> _config;
    private readonly IGenericContext<GameEntity> _game;
    private IGenericContext<GameStateEntity> _gameState;

    public ApplyRewardSystem(Contexts contexts) : base(contexts.Game, Trigger)
    {
        _game = contexts.Game;
        _gameState = contexts.GameState;
        _config = contexts.Config;
    }

    private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
    {
        return context.GetTriggerCollector<RewardComponent>(GroupEvent.Added);
    }

    //private static bool Filter(IGenericContext<GameEntity> context, GameEntity entity)
    //{
    //    //return context.IsTagged<MatchedComponent>(entity);
    //}

    //protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    //{
    //    return context.CreateCollector(GameMatcher.Reward);
    //}

    //protected override bool Filter(GameEntity entity)
    //{
    //    return entity.hasReward;
    //}

    protected override void Execute(List<GameEntity> entities)
    {
        //var score = _contexts.gameState.score.value;
        var score = _gameState.GetUnique<ScoreComponent>().value;
        var totalReward = 0;
        
        foreach (var entity in entities)
        {
            totalReward += _game.Get<RewardComponent>(entity).value;
            //entity.isDestroyed = true;
            _game.SetFlag<DestroyedComponent>(entity);
        }
        
        //_contexts.gameState.ReplaceScore(score + totalReward);

        _gameState.SetUnique<ScoreComponent>(c => c.value = score + totalReward);
    }
}