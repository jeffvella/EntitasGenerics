using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
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
            if (!_input.Unique.IsFlagged<PointerHoldingComponent>())
                return;

            var targetSelectionId = _gameState.Unique.Get<MaxSelectedElementComponent>().Component.Value - 1;
            if (targetSelectionId < 0)
                return;

            //var targetEntity = _game.GetSearchIndex<SelectionIdComponent>().FindEntity(targetSelectionId); //.FindEntity<SelectionIdComponent, int>(targetSelectionId);

            _game.TryFindEntity<SelectionIdComponent, int>(targetSelectionId, out var targetEntity);

            var targetEntityPosition = targetEntity.Get<PositionComponent>().Component.Value;

            //var targetEntityPosition = _game.Get<PositionComponent>(targetEntity).Value;

            var pointerHoldingPosition = _input.Unique.Get<PointerHoldingPositionComponent>().Component.Value;

            if (pointerHoldingPosition.Equals(targetEntityPosition))
            {
                DeselectCurrentEntity();
                SelectEntity(targetEntity, targetSelectionId);
            }
        }

        private void SelectEntity(GameEntity targetEntity, int targetSelectionId)
        {
            var targetEntityId = targetEntity.GetComponent<IdComponent>().Value;
            _gameState.Unique.Get<LastSelectedComponent>().Apply(targetEntityId);
            _gameState.Unique.Get<MaxSelectedElementComponent>().Apply(targetSelectionId);
        }

        private void DeselectCurrentEntity()
        {
            var lastSelectedId = _gameState.Unique.Get<LastSelectedComponent>().Component.Value;
            //var lastSelectedEntity = _game.GetSearchIndex<IdComponent>().FindEntity(lastSelectedId);

            _game.TryFindEntity<IdComponent, int>(lastSelectedId, out var lastSelectedEntity);

            lastSelectedEntity.SetFlag<SelectedComponent>(false);
            lastSelectedEntity.RemoveComponent<SelectionIdComponent>();
        }
    }

}