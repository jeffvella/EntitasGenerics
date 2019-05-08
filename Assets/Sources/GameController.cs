using System.Collections.Generic;
using Entitas.Generics;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GenericContexts Contexts { get; private set; }

    private RootSystems _rootSystems;
    private Services _services;

    [SerializeField]
    private TextAsset ComboDefinitions;

    //private GenericContexts _contexts;

    private void Awake()
    {
        //Contexts = Contexts.sharedInstance;
        Contexts = GenericContexts.Instance;
        //Contexts.GenericTemp = _contexts;

        Configure(Contexts);
        
        //How to live without DI? 
        _services = new Services
        {
            ViewService = new UnityViewService(Contexts),
            InputService = new UnityInputService(Contexts, Camera.main),
            TimeService = new UnityTimeService(Contexts),
            ElementService = new ElementService(Contexts),
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

    private void Configure(GenericContexts contexts)
    {
        contexts.Config.SetUnique(new MapSizeComponent
        {
            value = new GridSize(6, 6)
        });
        contexts.Config.SetUnique(new TypeCountComponent
        {
            Value = 4
        }); 
        contexts.Config.SetUnique(new MaxActionCountComponent
        {
            value = 20
        });
        contexts.Config.SetUnique(new MinMatchCountComponent
        {
            value = 3
        }); 
        contexts.Config.SetUnique(new ScoringTableComponent
        {
            value = new List<int> { 0, 10, 30, 90, 200, 500, 1200, 2500 }
        }); 
        contexts.Config.SetUnique(new ExplosiveScoringTableComponent
        {
            value = new List<int> { 300, 900, 1200, 2000 }
        }); 
        contexts.Config.SetUnique(new ComboDefinitionsComponent
        {
            value = JsonUtility.FromJson<ComboDefinitions>(ComboDefinitions.text)
        });
    }
}