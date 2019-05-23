using System.Collections.Generic;
using System.Threading;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public sealed class FillAllElementsSystem : GenericReactiveSystem<GameEntity>, IInitializeSystem
    {
        private readonly IElementService _elementService;
        private readonly IGenericContext<ConfigEntity> _config;
        private readonly IGenericContext<GameEntity> _game;

        public FillAllElementsSystem(Contexts contexts, IServices services) : base(contexts.Game, Trigger)
        {
            _game = contexts.Game;
            _config = contexts.Config;
            _elementService = services.ElementService;
        }

        private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
        {
            return context.GetTriggerCollector<RestartHappenedComponent>();
        }

        protected override void Execute(List<GameEntity> entities)
        {
            Fill();

            foreach (var entity in entities)
            {
                _game.SetFlag<DestroyedComponent>(entity, true);
            }
        }

        public void Initialize()
        {
            Fill();
        }

        private static readonly ThreadLocal<System.Random> Random
            = new ThreadLocal<System.Random>(() => new System.Random());

        private void Fill()
        {
            var size = _config.GetUnique<MapSizeComponent>().Value;

            for (int row = 0; row < size.y; row++)
            {
                for (int column = 0; column < size.x; column++)
                {
                    float random = (float)Random.Value.NextDouble();
                    //float random = Random.Range(0f, 1f);

                    if (random < 0.1f)
                    {
                        if (random < 0.05f)
                        {
                            if (random < 0.005f)
                            {
                                _elementService.CreateExsplosiveBlock(new GridPosition(column, row));
                            }
                            else
                            {
                                _elementService.CreateNotMovableBlock(new GridPosition(column, row));
                            }
                        }
                        else
                        {
                            _elementService.CreateMovableBlock(new GridPosition(column, row));
                        }
                    }
                    else
                    {
                        _elementService.CreateRandomElement(new GridPosition(column, row));
                    }
                }
            }
        }
    }
}