﻿using System.Collections.Generic;
using Entitas;
using Entitas.Generics;
using UnityEngine.SocialPlatforms.Impl;

public sealed class ApplyRewardSystem : GenericReactiveSystem<GameEntity>
{
    private readonly IGenericContext<ConfigEntity> _config;
    private readonly IGenericContext<GameEntity> _game;
    private IGenericContext<GameStateEntity> _gameState;

    public ApplyRewardSystem(GenericContexts contexts) : base(contexts.Game, Trigger, Filter)
    {
        _game = contexts.Game;
        _gameState = contexts.GameState;
        _config = contexts.Config;
    }

    private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
    {
        return context.GetTriggerCollector<RewardComponent>(GroupEvent.Added);
    }

    private static bool Filter(IGenericContext<GameEntity> context, GameEntity entity)
    {
        return context.IsTagged<MatchedComponent>(entity);
    }

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
            totalReward += entity.reward.value;
            //entity.isDestroyed = true;
            _game.SetTag<DestroyedComponent>(entity);
        }
        
        //_contexts.gameState.ReplaceScore(score + totalReward);

        _gameState.SetUnique(new ScoreComponent
        {
            value = score + totalReward
        });
    }
}