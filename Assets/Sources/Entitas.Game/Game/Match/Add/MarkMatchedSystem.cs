using System.Collections.Generic;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class MarkMatchedSystem : GenericReactiveSystem<InputEntity>
    {
        private readonly IGroup<GameEntity> _selectedGroup;
        private readonly List<GameEntity> _buffer;
        private IGenericContext<GameEntity> _game;
        private IGenericContext<ConfigEntity> _config;
        private IGenericContext<InputEntity> _input;

        public MarkMatchedSystem(Contexts contexts) : base(contexts.Input, Trigger)
        {
            _selectedGroup = contexts.Game.GetGroup<SelectedComponent>();
            _input = contexts.Input;
            _config = contexts.Config;
            _game = contexts.Game;
            _buffer = new List<GameEntity>();
        }

        private static ICollector<InputEntity> Trigger(IGenericContext<InputEntity> context)
        {
            return context.GetCollector<PointerReleasedComponent>();
        }

        protected override void Execute(List<InputEntity> entities)
        {
            if (_input.Unique.IsFlagged<PointerHoldingComponent>())
                return;

            var selectedEntities = _selectedGroup.GetEntities(_buffer);
            var minMatchCount = _config.Unique.Get<MinMatchCountComponent>().Component.Value;

            if (selectedEntities.Count >= minMatchCount)
            {
                foreach (var entity in selectedEntities)
                {
                    entity.SetFlag<MatchedComponent>(true);
                }
            }
        }
    }

}