using System;
using System.Collections.Generic;
using Assets.Sources.Game;
using Assets.Sources.GameState;
using Entitas;
using Entitas.Generics;
using UnityEngine;

public sealed class UnselectionSystem : GenericReactiveSystem<InputEntity>
{
    private IGenericContext<GameStateEntity> _gameState;
    private IGenericContext<InputEntity> _input;
    private IGenericContext<GameEntity> _game;

    public UnselectionSystem(GenericContexts contexts) : base(contexts.Input, Trigger)
    {
        _gameState = contexts.GameState;
        _input = contexts.Input;
        _game = contexts.Game;
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

        if (!_input.IsFlagged<PointerHoldingComponent>())
            return;

        //var targetSelectionId = _contexts.gameState.maxSelectedElement.value - 1;

        var targetSelectionId = _gameState.GetUnique<MaxSelectedElementComponent>().value - 1;
       // Debug.Log($"Id={targetSelectionId}");
        if (targetSelectionId < 0)
            return;

        //var targetEntity = _contexts.game.GetEntityWithSelectionId(targetSelectionId);
        var targetEntity = _game.FindEntity<SelectionIdComponent,int>(targetSelectionId);

        var target = _game.GetAccessor(targetEntity);
        
        //var pos = target.Get<PositionComponent>().value;

        var targetEntityPosition = _game.Get<PositionComponent>(targetEntity).value;

        //var position = _contexts.input.pointerHoldingPosition.value.ToGridPosition();
        var pointerHoldingPosition = _input.GetUnique<PointerHoldingPositionComponent>().value.ToGridPosition();

        if (pointerHoldingPosition.Equals(targetEntityPosition))
        {
            //var lastSelectedEntity = _contexts.game.GetEntityWithId(_contexts.gameState.lastSelected.value);

            DeselectCurrentEntity();

            //lastSelectedEntity.isSelected = false;
            //lastSelectedEntity.RemoveSelectionId();

            SelectEntity(targetEntity, targetSelectionId);

            //_contexts.gameState.ReplaceLastSelected(targetEntity.id.value);
            //_contexts.gameState.ReplaceMaxSelectedElement(targetSelectionId);
        }
    }

    private void SelectEntity(GameEntity targetEntity, int targetSelectionId)
    {
        var targetEntityId = _game.Get<IdComponent>(targetEntity).value;
        _gameState.SetUnique(new LastSelectedComponent {value = targetEntityId});
        _gameState.SetUnique(new MaxSelectedElementComponent {value = targetSelectionId});
    }

    private void DeselectCurrentEntity()
    {
        var lastSelectedId = _gameState.GetUnique<LastSelectedComponent>().value;
        var lastSelectedEntity = _game.FindEntity<IdComponent, int>(lastSelectedId);
        _game.SetFlag<SelectedComponent>(lastSelectedEntity, false);
        _game.Remove<SelectionIdComponent>(lastSelectedEntity);
    }
}
