# EntitasGenerics

An extension for https://github.com/sschmid/Entitas-CSharp that removes the need for code generation.

This project includes an adaptation of the example: https://github.com/RomanZhu/Match-Line-Entitas-ECS

# Goals #

* Remove all code generation from Entitas.
* Function on top of the latest official Entitas version without modifications.
* Maintain no heap allocations (outside of initialization obviously).
* Maintain performance on par with generated-code.
* Maintain all Entitas features.
* Keep usage/syntax as close as possible to the original.

# Dependencies #

* Entitas v1.13.0
* Unity 2019.1

# Usage #

#### Defining Contexts ####

A container for contexts needs to be created.

    public class Contexts
    {
        public static Contexts Instance => _instance ?? (_instance = new Contexts());
        private static Contexts _instance;
        
        public readonly IGenericContext<ConfigEntity> Config = new ConfigContext();
        public readonly IGenericContext<InputEntity> Input = new InputContext();
        public readonly IGenericContext<GameStateEntity> GameState = new GameStateContext();
        public readonly IGenericContext<GameEntity> Game = new GameContext();
    }

Each Context is created with a 'ContextDefinition', which lists the components you want to use. Indexed components for searching by value are also defined here.

    public class GameContext : GenericContext<GameContext, GameEntity>
    {
        public GameContext() : base(new GameContextDefinition())
        {
            AddIndex<IdComponent>();
            AddIndex<PositionComponent>();
        }
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

#### Defining Components ####

Components are almost the same; defining intended functionality through implementing a few different interfaces.

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

Components with values should implement `IValueComponent<T>`.

    public sealed class ColorComponent : IValueComponent<Color>, IEventComponent
    {
        public Color Value { get; set; }
    }

Components intended to be searched/indexed should implement `IEqualityComparer<T>`.

    public sealed class PositionComponent : IValueComponent<GridPosition>, IEqualityComparer<PositionComponent>, IEventComponent
    {
        public GridPosition Value { get; set; }

        public bool Equals(PositionComponent x, PositionComponent y) => x != null && y != null && x.Value.Equals(y.Value);

        public int GetHashCode(PositionComponent obj) => obj.Value.GetHashCode();
    }

Components with events... should be marked with `IEventComponent`.

    public sealed class MaxActionCountComponent : IUniqueComponent, IEventComponent
    {
        public int Value;
    }

#### Defining Entities ####

Entities are pretty straight-forward to setup; derive from a new base class `GenericEntity<T>` instead of `Entity`.

    public sealed class GameEntity : GenericEntity<GameEntity>
    {

    }


#### Defining Systems ####

Systems are set up the same way as usual...

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

... except for event systems, which need to be created like so:

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

#### Implementing Reactive Systems ####

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
            return !entity.IsFlagged<DestroyedComponent>();
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.SetFlag<DestroyedComponent>();
            }
        }
    }

You can still work with groups, triggers, collectors and partial systems using just IExecuteSystem etc.

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


#### Events ####

Event listeners can be used in a similar fashion either inline as an action...

    public class SelectedListener : MonoBehaviour, IEventListener<GameEntity>
    { 
        [SerializeField] private GameObject _selectedEffect;

        public void RegisterListeners(Contexts contexts, GameEntity entity)
        {
            entity.RegisterComponentListener<SelectedComponent>(OnSelected, GroupEvent.Added);
            entity.RegisterComponentListener<SelectedComponent>(OnDeselected, GroupEvent.Removed);

            _selectedEffect.SetActive(entity.IsFlagged<SelectedComponent>());
        }

        private void OnSelected(GameEntity entity)
        {
            _selectedEffect.SetActive(true);
        }

        private void OnDeselected(IEntity entity)
        {
            _selectedEffect.SetActive(false);
        }
    }

... or by implementing an event interface (IAddedComponentListener/IRemovedComponentListener)

    public class ColorListener : MonoBehaviour, IAddedComponentListener<GameEntity, ColorComponent>, IEventListener<GameEntity>
    {
        [SerializeField] private Renderer _renderer;

        public void RegisterListeners(Contexts contexts, GameEntity entity)
        {
            OnComponentAdded(entity);

            entity.RegisterComponentListener(this);
        }

        public void OnComponentAdded(GameEntity entity)
        {
            _renderer.material.color = entity.Get<ColorComponent>().Component.Value;
        }
    }
    

#### Working with Components ####

Entities have a full generic version of the default CRUD methods that would usually require an index:

    var size = entity.GetComponent<MapSizeComponent>().Value;

    entity.ReplaceComponent<MapSizeComponent>(newSize);

    var newComponent = entity.CreateComponent<MapSizeComponent>();

    entity.AddComponent<MapSizeComponent>(newComponent);

    entity.RemoveComponent<MapSizeComponent>();

There are a few possible ways to update the values within a component. You can of course work with the aforementioned API to manually `CreateComponent`, set the values and then `ReplaceComponent` (which is the correct procedure to ensure correct pooling and events are handled properly). But there are various helpers to wrap this process to make it easier. Currently the recommended approach is this:
    
    element.Get<PositionComponent>().Apply(targetPosition);

which relys on returning a ref struct component wrapper and the `IValueComponent<T>` interface to expose the value type and enforce error detection at compile time.


#### Using Indexes / Searchign for Entities by component value ####

The original Entitas event indexing system is used (`PrimaryEntityIndex`), and can be used through a context like this:

    if (_game.TryFindEntity<PositionComponent, GridPosition>(targetPosition, out var result))
    {
        // do something with result
    }


#### Known Limitations ####

* Event 'priority' is not yet supported.

* Only one primary key index per component is allowed.

* Entitas' inspector debugging display doesn't handle generic names at all. I have a PR submitted and waiting to be evaluated; in the meantime the fix is included in my version of the 1.13 source.

* Each type of Entity object can only be used in one context. GameEntity for GameContext, InputEntity for InputContext, etc.


#### What is the incldued WPF based version of MatchLine? ####

Ignore it for now; It needs to be updated.

