using System.Collections.Generic;
using Entitas;
using EntitasGenerics;

public sealed class MarkMatchedSystem : ReactiveSystem<InputEntity>
{
    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;
    private readonly IGroup<GameEntity> _group;
    private readonly List<GameEntity> _buffer;

    public MarkMatchedSystem(Contexts contexts, GenericContexts genericContexts) : base(contexts.input)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
        _group = _contexts.game.GetGroup(GameMatcher.Selected);
        _buffer = new List<GameEntity>();
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector(InputMatcher.PointerReleased);
    }

    protected override bool Filter(InputEntity entity)
    {
        return true;
    }

    protected override void Execute(List<InputEntity> entities)
    {
        if (_contexts.input.isPointerHolding)
            return;

        var selectedEntities = _group.GetEntities(_buffer);
        var minMatchCount = _genericContexts.Config.Get<MinMatchCountComponent>().value;

        if (selectedEntities.Count >= minMatchCount)
        {
            foreach (var entity in selectedEntities)
            {
                entity.isMatched = true;
            }
        }
    }
}