using System.Collections.Generic;
using Assets.Sources.Config;
using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;
using UnityEngine;

public sealed class ComboRewardEmitterSystem : GenericReactiveSystem<GameEntity>
{

    private readonly IGenericContext<ConfigEntity> _config;
    private readonly IGenericContext<GameEntity> _game;

    public ComboRewardEmitterSystem(Contexts contexts) : base(contexts.Game, Trigger, Filter)
    {
        _game = contexts.Game;
        _config = contexts.Config;
    }

    private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
    {
        return context.GetTriggerCollector<ComboComponent>(GroupEvent.Added);
    }

    private static bool Filter(IGenericContext<GameEntity> context, GameEntity entity)
    {
        return context.Has<ComboComponent>(entity);
    }


    //protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    //{
    //    return context.CreateCollector(GameMatcher.Combo);
    //}

    //protected override bool Filter(GameEntity entity)
    //{
    //    return entity.hasCombo;
    //}

    protected override void Execute(List<GameEntity> entities)
    {
        //var definitions = _contexts.config.comboDefinitions.value;
        var definitions = _config.GetUnique<ComboDefinitionsComponent>().value;

        foreach (var entity in entities)
        {
            var combo = _game.Get<ComboComponent>(entity).value;
            var definition = definitions.Definitions[combo];
            
            //var e = _contexts.game.CreateEntity();
            //e.AddReward(defenition.Reward);

            var e = _game.CreateEntity();        
            _game.Set(e, new RewardComponent { value = definition.Reward });
        }
    }
}