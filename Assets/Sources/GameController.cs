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
        contexts.Config.SetUnique<MapSizeComponent>(c => c.value = new GridSize(6, 6));
        contexts.Config.SetUnique<TypeCountComponent>(c => c.Value = 4); 
        contexts.Config.SetUnique<MaxActionCountComponent>(c => c.Value = 20);
        contexts.Config.SetUnique<MinMatchCountComponent>(c => c.value = 3);

        contexts.Config.SetUnique<ScoringTableComponent>(c =>
        {
            c.value = new List<int> {0, 10, 30, 90, 200, 500, 1200, 2500};
        });

        contexts.Config.SetUnique<ExplosiveScoringTableComponent>(c =>
        {
            c.value = new List<int> {300, 900, 1200, 2000};
        });

        contexts.Config.SetUnique<ComboDefinitionsComponent>(c =>
        {
            c.value = JsonUtility.FromJson<ComboDefinitions>(ComboDefinitions.text);
        });
    }
}