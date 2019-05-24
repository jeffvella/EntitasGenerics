# EntitasGenerics

An extension for https://github.com/sschmid/Entitas-CSharp that removes the need for code generation.

This project includes an adaptation of the example: https://github.com/RomanZhu/Match-Line-Entitas-ECS

# Details #

* Designed to work on top of Entitas, without changes to the existing Entitas core.
* The Good stuff is located in: "Assets\Libs\Entitas-Generics" folder.
* Includes a WPF App (Non-Unity) version of the MatchLine game upon the same Entitas-based game core.

# Dependencies #

* Entitas v1.13.0
* Unity 2019.1

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

Components intended to be searched by value should implement IEquatableT

    public sealed class PositionComponent : IComponent, IEquatable<GridPosition>
    {
        public GridPosition Value;

        public bool Equals(GridPosition other) => other.Equals(Value);
    }

Components with events are defined in the same way as usual.

    public sealed class MaxActionCountComponent : IUniqueComponent, IEventComponent
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


##### Events #####

Event listeners can be used in a similar fashion either inline as an action...

    public class SelectedListener : MonoBehaviour, IEventListener<GameEntity>
    { 
        [SerializeField] private GameObject _selectedEffect;

        public void RegisterListeners(Contexts contexts, GameEntity entity)
        {
            entity.RegisterAddedComponentListener<SelectedComponent>(OnSelected);
            entity.RegisterRemovedComponentListener<SelectedComponent>(OnDeselected);

            _selectedEffect.SetActive(entity.IsFlagged<SelectedComponent>());
        }

        private void OnSelected((IEntity Entity, SelectedComponent Component) obj)
        {
            _selectedEffect.SetActive(true);
        }

        private void OnDeselected(IEntity entity)
        {
            _selectedEffect.SetActive(false);
        }
    }

... or by implementing an event interface (IAddedComponentListener/IRemovedComponentListener)

    public class UIScoreView : MonoBehaviour, IAddedComponentListener<GameStateEntity, ScoreComponent>
    {
        [SerializeField] private Text _label;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _triggerName;

        private int _triggerHash;

        private void Start()
        {
            Contexts.Instance.GameState.RegisterAddedComponentListener(this);

            _triggerHash = Animator.StringToHash(_triggerName);
        }

        public void OnComponentAdded(GameStateEntity entity, ScoreComponent component)
        {
            _label.text = component.Value.ToString();
            _animator.SetTrigger(_triggerHash);
        }
    }

registering via the context delivers your concrete implementation to the event handler instead of IEntity (which might be useful in the case that you have added extra functionality).

    public class UnityView : MonoBehaviour, IView<GameEntity>
    {
        public void InitializeView(Contexts contexts, GameEntity entity)
        {
            contexts.Game.RegisterAddedComponentListener<DestroyedComponent>(entity, OnEntityDestroyed));
        }

        private void OnEntityDestroyed((GameEntity Entity, DestroyedComponent Component) obj)
        {
            Destroy(gameObject);
        }
    }


##### Working with Components #####

Components can be retrieved from an entity using `Get<>()` methods via the context:

    var idComponent = _game.Get<IdComponent>(targetEntity);

or from the entity itself when deriving your entities from GenericEntity base class:

    var idComponent = targetEntity.Get<IdComponent>();
    
for changing values a lamda is used:

    entity.Set<PositionComponent>(c => c.value = position);
    
which seemed to be the cleanest approach while ensuring that Entitas' procedure for updates is respected - the component pool is used to avoid allocations and events are properly fired. In many cases the lamda will be compiled to a static method so performance isn't significantly impacted (an additional level of redirection).

`Unique` and `Flags` have their own special methods because they have special behavior and fits with Entitas' mantra of clear intent. But it would be trivial to also use the normal Set/Get methods since these are all simply `IComponent`s

    entity.SetFlag<DestroyedComponent>();
    entity.IsFlagged<DestroyedComponent>();

`Unique` components are placed on a hardcoded entity in order to get performance similar to the generated code (which also hardcodes an entity under the hood). The main difference is that while debugging in the inspector you'll notice components appear together on the same entity instead each having their own. When using the GetUnique/SetUnique methods from a context you don't have to specify the entity.

    contexts.Config.SetUnique<ComboDefinitionsComponent>(c =>
    {
        c.value = JsonUtility.FromJson<ComboDefinitions>(ComboDefinitions.text);
    });
    

# Known Issues #

* 'Indexed' Entity searches currently just loop through checking for Equality versus the dictionary-based approach in the generated code of default Entitas.
* Event 'priority' is not yet supported.
* Entitas' inspector debugging display doesn't handle generic names at all.

# The WPF Version of MatchLine?

One of the great things about Entitas (and other ECS solutions) is that the core of your game can run outside of Unity's environment. This gives you a lot more freedom, particularly in optimizing code with Line-By-Line profilers such as 'ANTS Performance Profiler' and 'DotTrace'. 

Included in the project is a functioning WPF application port of the front-end of RomanZhu's MathcLine game (https://github.com/RomanZhu/Match-Line-Entitas-ECS)

Screenshot of WPF version:

<img src="https://i.imgur.com/kr5g9RO.png" />

Analysis:

<img src="https://i.imgur.com/UBgepTY.png" />

