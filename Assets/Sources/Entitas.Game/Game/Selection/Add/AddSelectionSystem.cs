using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class AddSelectionSystem : GenericReactiveSystem<InputEntity>
    {
        private IGenericContext<GameEntity> _game;
        private IGenericContext<GameStateEntity> _gameState;
        private IGenericContext<InputEntity> _input;
        private IGenericContext<ConfigEntity> _config;

        public AddSelectionSystem(Contexts contexts)
            : base(contexts.Input, Trigger)
        {
            _game = contexts.Game;
            _gameState = contexts.GameState;
            _input = contexts.Input;
            _config = contexts.Config;
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

            //var position = _contexts.input.pointerHoldingPosition.value.ToGridPosition();

            var position = _input.GetUnique<PointerHoldingPositionComponent>().value;
            var mapSize = _config.GetUnique<MapSizeComponent>().value;

            var horizontalBounded = position.x >= 0 && position.x < mapSize.x;
            var verticalBounded = position.y >= 0 && position.y < mapSize.y;

            if (horizontalBounded && verticalBounded)
            {
                if (!_game.TryFindEntity<PositionComponent, GridPosition>(position, out var entityUnderPointer))
                    return;

                if (_game.IsFlagged<BlockComponent>(entityUnderPointer))
                    return;

                if (_game.IsFlagged<SelectedComponent>(entityUnderPointer))
                    return;

                var entityUnderPointerId = _game.Get<IdComponent>(entityUnderPointer).value;
                var lastSelectedId = _gameState.GetUnique<LastSelectedComponent>().value;
                if (lastSelectedId == -1)
                {
                    StartNewSelection(entityUnderPointer, entityUnderPointerId);
                }
                else
                {
                    AddToExistingSelection(lastSelectedId, entityUnderPointer, entityUnderPointerId);
                }
            }
        }

        private void StartNewSelection(GameEntity entityUnderPointer, int entityUnderPointerId)
        {
            _game.SetFlag<SelectedComponent>(entityUnderPointer, true);
            _game.Set(entityUnderPointer, new SelectionIdComponent { value = 0 });
            _gameState.SetUnique<LastSelectedComponent>(c => c.value = entityUnderPointerId);
            _gameState.SetUnique<MaxSelectedElementComponent>(c => c.value = 0);
        }

        private void AddToExistingSelection(int lastSelectedId, GameEntity entityUnderPointer, int entityUnderPointerId)
        {
            var lastSelected = _game.FindEntity<IdComponent, int>(lastSelectedId);
            var isLastElementType = _game.IsFlagged<ElementComponent>(lastSelected);
            var isCurrentElementType = _game.IsFlagged<ElementComponent>(entityUnderPointer);

            if (isLastElementType && isCurrentElementType)
            {
                var lastElementType = _game.Get<ElementTypeComponent>(lastSelected).value;
                var currentElementType = _game.Get<ElementTypeComponent>(entityUnderPointer).value;

                if (lastElementType == currentElementType)
                {
                    var lastPosition = _game.Get<PositionComponent>(lastSelected).value;
                    var currentPosition = _game.Get<PositionComponent>(entityUnderPointer).value;

                    if (GridPosition.Distance(lastPosition, currentPosition) < 1.25f)
                    {
                        var selectionId = _gameState.GetUnique<MaxSelectedElementComponent>().value;
                        selectionId++;

                        _game.Set(entityUnderPointer, new SelectionIdComponent { value = selectionId });
                        _game.SetFlag<SelectedComponent>(entityUnderPointer, true);
                        _gameState.SetUnique<LastSelectedComponent>(c => c.value = entityUnderPointerId);
                        _gameState.SetUnique<MaxSelectedElementComponent>(c => c.value = selectionId);
                    }
                }
            }
        }


    }
}