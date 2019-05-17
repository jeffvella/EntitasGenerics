# EntitasGenerics

An extension for EntitasCSharp that removes the need for code generation.

This project includes a conversion of the example: https://github.com/RomanZhu/Match-Line-Entitas-ECS

# Details #

* Designed to work on top of Entitas, without changes to the existing Entitas core.
* Currently using Entitas version 1.13.0
* The Good stuff is located in: "Assets\Libs\Entitas-Generics" folder.

# Differences from Standard Entitas #

##### Defining Contexts #####

A container for contexts should be created.

    public class Contexts
    {
        public static Contexts Instance => _instance ?? (_instance = new Contexts());
        private static Contexts _instance;
        
        public readonly IGenericContext<ConfigEntity> Config = new ConfigContext();
        public readonly IGenericContext<InputEntity> Input = new InputContext();
        public readonly IGenericContext<GameStateEntity> GameState = new GameStateContext();
        public readonly IGenericContext<GameEntity> Game = new GameContext();
    }

Each Context is created from a 'ContextDefinition', which lists the components you want to use.

    public class GameContext : GenericContext<GameContext, GameEntity>
    {
        public GameContext() : base(new GameContextDefinition()) { }
    }

    public class GameContextDefinition : ContextDefinition<GameContext, GameEntity>
    {
        public GameContextDefinition()
        {
            Add<AssetComponent>();
            Add<AssetLoadedComponent>();
            Add<BlockComponent>();
            Add<ColorComponent>();
            Add<ComboComponent>();
            ...
        }
    }

##### Defining Components #####

Components are almost the same, 

    public sealed class AssetComponent : IComponent
    {
        public string Value;
    }

Uniques would implement an interface instead of an attribute.

    public sealed class ScoreComponent : IUniqueComponent 
    {
        public int Value;
    }
    
Components intended to contain no data ('Flags' in entitas) are explicitly marked as such:

    public sealed class BlockComponent : IFlagComponent
    {
    }

Components intended to searched by value should implment IEquatable<TValue>
  
    [Event(EventTarget.Any)]
    public sealed class PositionComponent : IComponent, IEquatable<GridPosition>
    {
        public GridPosition Value;

        public bool Equals(GridPosition other) => other.Equals(Value);
    }

Components with events are defined in the same way as usual.

    [Event(EventTarget.Any)]
    public sealed class MaxActionCountComponent : IUniqueComponent
    {
        public int Value;
    }

##### Defining Systems #####

Systems are setup the same way as usual...

    public class RootSystems : Feature
    {
        public RootSystems(Contexts contexts, Services services)
        {
            Add(new InputSystems(contexts, services));
            Add(new GameStateSystems(contexts, services));
            Add(new GameStateEventSystems(contexts));
            Add(new GameSystems(contexts, services));
            Add(new GameEventSystems(contexts));
        }
    }

... except for event systems, which need to be created when added to a feature.

    public sealed class GameStateEventSystems : Feature
    {
        public GameStateEventSystems(Contexts contexts)
        {
            Add(new EventSystem<GameStateEntity, ScoreComponent>(contexts.GameState, GroupEvent.Added));
            Add(new EventSystem<GameStateEntity, GameOverComponent>(contexts.GameState, GroupEvent.Added));
            Add(new EventSystem<GameStateEntity, GameOverComponent>(contexts.GameState, GroupEvent.Removed));
            Add(new EventSystem<GameStateEntity, ActionCountComponent>(contexts.GameState, GroupEvent.Added));
        }
    }

##### Implementing Reactive Systems #####

Currently working with systems is pretty much the same.

    public sealed class RemoveMatchedSystem : GenericReactiveSystem<GameEntity>
    {
        private readonly IGenericContext<GameEntity> _game;

        public RemoveMatchedSystem(Contexts contexts) : base(contexts.Game, Trigger, Filter)
        {
            _game = contexts.Game;
        }

        private static ICollector<GameEntity> Trigger(IGenericContext<GameEntity> context)
        {
            return context.GetTriggerCollector<MatchedComponent>();
        }

        private static bool Filter(IGenericContext<GameEntity> context, GameEntity entity)
        {
            return !context.IsFlagged<DestroyedComponent>(entity);
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                _game.SetFlag<DestroyedComponent>(entity);    
            }
        }
    }

You can still work with groups, trigger, collectors and partial systems using just IExecuteSystem etc.

    public sealed class DestroyEntitySystem : ICleanupSystem
    {
        private readonly IGroup<GameEntity> _gameGroup;
        private readonly List<GameEntity> _gameBuffer;

        private readonly IGroup<InputEntity> _inputGroup;
        private readonly List<InputEntity> _inputBuffer;

        public DestroyEntitySystem(Contexts contexts)
        {
            _gameGroup = contexts.Game.GetGroup<DestroyedComponent>();
            _gameBuffer = new List<GameEntity>();

            _inputGroup = contexts.Input.GetGroup<DestroyedComponent>();
            _inputBuffer = new List<InputEntity>();
        }

        public void Cleanup()
        {
            foreach (GameEntity e in _gameGroup.GetEntities(_gameBuffer))
            {
                e.Destroy();
            }

            foreach (var e in _inputGroup.GetEntities(_inputBuffer))
            {
                e.Destroy();
            }
        }
    }

# Known Issues #

* 'Indexed' Entity searches currently loops through for Equality versus the dictionary-based generated code in default Entitas.
* Event 'target' and 'priority' are not yet supported.
* Entitas' inspector debugging display doesn't handle generic names at all.

