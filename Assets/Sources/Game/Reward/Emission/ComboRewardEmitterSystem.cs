using System.Collections.Generic;
using Entitas;
using EntitasGenerics;
using UnityEngine;

public sealed class ComboRewardEmitterSystem : ReactiveSystem<GameEntity>
{

    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;

    public ComboRewardEmitterSystem(Contexts contexts, GenericContexts genericContexts) : base(contexts.game)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Combo);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasCombo;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        //var definitions = _contexts.config.comboDefinitions.value;
        var definitions = _genericContexts.Config.Get<ComboDefinitionsComponent>().value;

        foreach (var entity in entities)
        {
            var defenition = definitions.Definitions[entity.combo.value];
            
            var e = _contexts.game.CreateEntity();
            e.AddReward(defenition.Reward);
        }
    }
}