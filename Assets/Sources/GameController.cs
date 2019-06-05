using Entitas.MatchLine;
using System.Collections.Generic;
using Entitas.Generics;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static Contexts Contexts { get; private set; }

    private RootSystems _rootSystems;
    private IServices _services;

    [SerializeField]
    private TextAsset ComboDefinitions;

    private void Awake()
    {
        Contexts = Contexts.Instance;

        Contexts.EnableVisualDebugging();

        Configure(Contexts);

        _services = new Services
        {
            ViewService = new UnityViewService(Contexts),
            InputService = new UnityInputService(Contexts, Camera.main),
            TimeService = new UnityTimeService(Contexts),
            ElementService = new UnityElementService(Contexts),
        };

        _rootSystems = new RootSystems(Contexts, _services);
        _rootSystems.Initialize();
    }

    private void Update()
    {
        _rootSystems.Execute();
        _rootSystems.Cleanup();
    }

    private void OnDestroy()
    {
        _rootSystems.DeactivateReactiveSystems();
        _rootSystems.ClearReactiveSystems();
        _rootSystems.TearDown();
    }

    private void Configure(Contexts contexts)
    {
        contexts.Config.Unique.GetAccessor<MapSizeComponent>().Set(c => c.Value = new GridSize(6, 6));
        contexts.Config.Unique.GetAccessor<TypeCountComponent>().Set(c => c.Value = 4);
        contexts.Config.Unique.GetAccessor<MaxActionCountComponent>().Set(c => c.Value = 20);
        contexts.Config.Unique.GetAccessor<MinMatchCountComponent>().Set(c => c.Value = 3);

        contexts.Config.Unique.GetAccessor<ScoringTableComponent>().Set(c =>
        {
            c.Value = new List<int> {0, 10, 30, 90, 200, 500, 1200, 2500};
        });

        contexts.Config.Unique.GetAccessor<ScoringTableComponent>().Set(c =>
        {
            c.Value = new List<int> { 300, 900, 1200, 2000 };
        });

        contexts.Config.Unique.GetAccessor<ComboDefinitionsComponent>().Set(c =>
        {
            c.Value = JsonUtility.FromJson<ComboDefinitions>(ComboDefinitions.text);
        });
    }
}