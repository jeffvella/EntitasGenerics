using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class GameRestartSystem : GenericReactiveSystem<InputEntity>
{
    private readonly Contexts _contexts;
    private readonly IGroup<GameEntity> _group;
    private readonly List<GameEntity> _buffer;

    public GameRestartSystem(Contexts contexts, GenericContexts genericContexts) 
        : base(genericContexts.Input, TriggerProducer)
    {
        _contexts = contexts;
        _group = contexts.game.GetGroup(GameMatcher.Element);
        _buffer = new List<GameEntity>();
    }

    private static ICollector<InputEntity> TriggerProducer(IGenericContext<InputEntity> context)
    {
        return context.GetTriggerCollector<RestartComponent>();
    }

    //protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    //{
    //    return context.CreateCollector(InputMatcher.Restart.Added());
    //}

    //protected override bool Filter(InputEntity entity)
    //{
    //    return true;
    //}

    protected override void Execute(List<InputEntity> entities)
    {
        foreach (var entity in _group.GetEntities(_buffer))
        {
            entity.isDestroyed = true;
        }

        var e = _contexts.game.CreateEntity();
        e.isRestartHappened = true;
    }
}