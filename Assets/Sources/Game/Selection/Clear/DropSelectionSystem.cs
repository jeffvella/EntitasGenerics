using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class DropSelectionSystem : GenericReactiveSystem<InputEntity>
{
    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;
    private readonly IGroup<GameEntity> _group;
    private readonly List<GameEntity> _buffer;

    public DropSelectionSystem(Contexts contexts, GenericContexts genericContexts) 
        : base(genericContexts.Input, TriggerProducer)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
        _group = _contexts.game.GetGroup(GameMatcher.Selected);
        _buffer = new List<GameEntity>();
    }

    private static ICollector<InputEntity> TriggerProducer(IGenericContext<InputEntity> context)
    {
        return context.GetCollector<PointerReleasedComponent>();
    }

    //protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    //{
    //    return context.CreateCollector(InputMatcher.PointerReleased);
    //}

    //protected override bool Filter(InputEntity entity)
    //{
    //    return true;
    //}

    protected override void Execute(List<InputEntity> entities)
    {
        //if (_contexts.input.isPointerHolding)
        //    return;

        if (_genericContexts.Input.IsTagged<PointerHoldingComponent>())
            return;

        foreach (var entity in _group.GetEntities(_buffer))
        {
            entity.isSelected = false;
            entity.RemoveSelectionId();
        }

        _contexts.gameState.ReplaceLastSelected(-1);
        _contexts.gameState.ReplaceMaxSelectedElement(0);
    }
}