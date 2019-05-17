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

    public UnselectionSystem(Contexts contexts) : base(contexts.Input, Trigger)
    {
        _gameState = contexts.GameState;
        _input = contexts.Input;
        _game = contexts.Game;
    }

    private static ICollector<InputEntity> Trigger(IGenericContext<InputEntity> context)
    {
        return context.GetCollector<PointerHoldingPositionComponent>();
    }

    protected override void Execute(List<InputEntity> entities)
    {
        if (!_input.IsFlagged<PointerHoldingComponent>())
            return;

        var targetSelectionId = _gameState.GetUnique<MaxSelectedElementComponent>().value - 1;
        if (targetSelectionId < 0)
            return;

        var targetEntity = _game.FindEntity<SelectionIdComponent,int>(targetSelectionId);
        var targetEntityPosition = _game.Get<PositionComponent>(targetEntity).value;
        var pointerHoldingPosition = _input.GetUnique<PointerHoldingPositionComponent>().value.ToGridPosition();
        if (pointerHoldingPosition.Equals(targetEntityPosition))
        {
            DeselectCurrentEntity();
            SelectEntity(targetEntity, targetSelectionId);
        }
    }

    private void SelectEntity(GameEntity targetEntity, int targetSelectionId)
    {
        var targetEntityId = _game.Get<IdComponent>(targetEntity).value;
        _gameState.SetUnique<LastSelectedComponent>(c => c.value = targetEntityId);
        _gameState.SetUnique<MaxSelectedElementComponent>(c => c.value = targetSelectionId);
    }

    private void DeselectCurrentEntity()
    {
        var lastSelectedId = _gameState.GetUnique<LastSelectedComponent>().value;
        var lastSelectedEntity = _game.FindEntity<IdComponent, int>(lastSelectedId);
        _game.SetFlag<SelectedComponent>(lastSelectedEntity, false);
        _game.Remove<SelectionIdComponent>(lastSelectedEntity);
    }
}
