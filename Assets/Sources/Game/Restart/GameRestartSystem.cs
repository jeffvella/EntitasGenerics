using System.Collections.Generic;
using Assets.Sources.Game;
using Entitas;
using Entitas.Generics;

public sealed class GameRestartSystem : GenericReactiveSystem<InputEntity>
{
    private readonly IGroup<GameEntity> _elementGroup;
    private readonly List<GameEntity> _buffer;
    private IGenericContext<GameEntity> _game;

    public GameRestartSystem(Contexts contexts) : base(contexts.Input, Trigger)
    {
        _elementGroup = contexts.Game.GetGroup<ElementComponent>();
        _game = contexts.Game;
        //_group = contexts.game.GetGroup(GameMatcher.Element);
        _buffer = new List<GameEntity>();
    }

    private static ICollector<InputEntity> Trigger(IGenericContext<InputEntity> context)
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
        foreach (var entity in _elementGroup.GetEntities(_buffer))
        {
            _game.SetFlag<DestroyedComponent>(entity, true);
            //entity.isDestroyed = true;
        }

        var e = _game.CreateEntity();
        _game.SetFlag<RestartHappenedComponent>(e, true);

        //e.isRestartHappened = true;
    }
}