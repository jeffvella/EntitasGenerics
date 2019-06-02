using System;
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

        public AddSelectionSystem(Contexts contexts) : base(contexts.Input, Trigger)
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

            if (!_input.Unique.IsFlagged<PointerHoldingComponent>())
                return;

            //var position = _contexts.input.pointerHoldingPosition.value.ToGridPosition();

            var position = _input.Unique.Get<PointerHoldingPositionComponent>().Component.Value;
            var mapSize = _config.Unique.Get<MapSizeComponent>().Component.Value;

            var horizontalBounded = position.x >= 0 && position.x < mapSize.x;
            var verticalBounded = position.y >= 0 && position.y < mapSize.y;

            if (horizontalBounded && verticalBounded)
            {
                if (!_game.TryFindEntity<PositionComponent, GridPosition>(position, out var entityUnderPointer))
                    return;

                if (entityUnderPointer.IsFlagged<BlockComponent>())
                    return;

                if (entityUnderPointer.IsFlagged<SelectedComponent>())
                    return;

                var entityUnderPointerId = entityUnderPointer.Get<IdComponent>().Component.Value;
                //var entityUnderPointerId = _game.Get<IdComponent>(entityUnderPointer).Value;

                var lastSelectedId = _gameState.Unique.Get<LastSelectedComponent>().Component.Value;
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
            //_game.SetFlag<SelectedComponent>(entityUnderPointer, true);
            //_game.Set(entityUnderPointer, new SelectionIdComponent { Value = 0 });
            //_gameState.SetUnique<LastSelectedComponent>(c => c.value = entityUnderPointerId);
            //_gameState.SetUnique<MaxSelectedElementComponent>(c => c.value = 0);

            entityUnderPointer.SetFlag<SelectedComponent>(true);
            entityUnderPointer.Get<SelectionIdComponent>().Apply(0);
            _gameState.Unique.Get<LastSelectedComponent>().Apply(entityUnderPointerId);
            _gameState.Unique.Get<MaxSelectedElementComponent>().Apply(0);
        }

        private void AddToExistingSelection(int lastSelectedId, GameEntity entityUnderPointer, int entityUnderPointerId)
        {
            //var lastSelected = _game.FindEntity<IdComponent, int>(lastSelectedId);

            if (!_game.TryFindEntity<IdComponent,int>(lastSelectedId, out var lastSelectedEntity))
                throw new InvalidOperationException();

            var isLastElementType = lastSelectedEntity.IsFlagged<ElementComponent>();
            var isCurrentElementType = entityUnderPointer.IsFlagged<ElementComponent>();

            if (isLastElementType && isCurrentElementType)
            {
                var lastElementType = lastSelectedEntity.Get<ElementTypeComponent>().Component.Value;
                var currentElementType = entityUnderPointer.Get<ElementTypeComponent>().Component.Value;

                if (lastElementType == currentElementType)
                {
                    var lastPosition = lastSelectedEntity.Get<PositionComponent>().Component.Value;
                    var currentPosition = entityUnderPointer.Get<PositionComponent>().Component.Value;

                    if (GridPosition.Distance(lastPosition, currentPosition) < 1.25f)
                    {
                        var selectionId = _gameState.Unique.Get<MaxSelectedElementComponent>().Component.Value;
                        selectionId++;

                        entityUnderPointer.Get<SelectionIdComponent>().Apply(selectionId);
                        entityUnderPointer.SetFlag<SelectedComponent>(true);

                        _gameState.Unique.Get<LastSelectedComponent>().Apply(entityUnderPointerId);
                        _gameState.Unique.Get<MaxSelectedElementComponent>().Apply(selectionId);
                    }
                }
            }
        }


    }
}