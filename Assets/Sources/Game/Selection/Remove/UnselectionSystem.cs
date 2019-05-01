using System;
using System.Collections.Generic;
using Entitas;
using Entitas.Generics;

public sealed class UnselectionSystem : GenericReactiveSystem<InputEntity>
{
    private readonly Contexts _contexts;
    private readonly GenericContexts _genericContexts;

    public UnselectionSystem(Contexts contexts, GenericContexts genericContexts) 
        : base(genericContexts.Input, Trigger)
    {
        _contexts = contexts;
        _genericContexts = genericContexts;
    }


    private static ICollector<InputEntity> Trigger(IGenericContext<InputEntity> context)
    {
        return context.GetCollector<PointerHoldingPositionComponent>();
    }

    //protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    //{
    //    return context.CreateCollector(InputMatcher.PointerHoldingPosition);
    //}

    //protected override bool Filter(InputEntity entity)
    //{
    //    return true;
    //}

    protected override void Execute(List<InputEntity> entities)
    {
        //if (!_contexts.input.isPointerHolding)
        //    return;

        if (!_genericContexts.Input.IsTagged<PointerHoldingComponent>())
            return;

        var targetSelectionId = _contexts.gameState.maxSelectedElement.value - 1;

        if (targetSelectionId < 0)
            return;

        var targetEntity = _contexts.game.GetEntityWithSelectionId(targetSelectionId);

        //var position = _contexts.input.pointerHoldingPosition.value.ToGridPosition();
        var position = _genericContexts.Input.Get<PointerHoldingPositionComponent>().value.ToGridPosition();

        if (position.Equals(targetEntity.position.value))
        {
            var lastSelectedEntity = _contexts.game.GetEntityWithId(_contexts.gameState.lastSelected.value);
            lastSelectedEntity.isSelected = false;
            lastSelectedEntity.RemoveSelectionId();
            
            _contexts.gameState.ReplaceLastSelected(targetEntity.id.value);
            _contexts.gameState.ReplaceMaxSelectedElement(targetSelectionId);
        }
    }
}