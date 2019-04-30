using System.Collections.Generic;
using EntitasGenerics;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Contexts _oldcontexts;
    private RootSystems _rootSystems;
    private Services _services;

    [SerializeField]
    private TextAsset ComboDefinitions;

    private GenericContexts _contexts;

    private void Awake()
    {
        _oldcontexts = Contexts.sharedInstance;

        _contexts = new GenericContexts();

        Configure(_contexts);
        
        //How to live without DI? 
        _services = new Services
        {
            ViewService = new UnityViewService(_oldcontexts, _contexts),
            InputService = new UnityInputService(_oldcontexts, _contexts, Camera.main),
            TimeService = new UnityTimeService(_oldcontexts, _contexts),
            ElementService = new ElementService(_oldcontexts, _contexts),
        };

        _rootSystems = new RootSystems(_oldcontexts, _contexts, _services);
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
        contexts.Config.Set(new MapSizeComponent
        {
            value = new GridSize(6, 6)
        });
        contexts.Config.Set(new TypeCountComponent
        {
            Value = 4
        }); 
        contexts.Config.Set(new MaxActionCountComponent
        {
            value = 20
        });
        contexts.Config.Set(new MinMatchCountComponent
        {
            value = 3
        }); 
        contexts.Config.Set(new ScoringTableComponent
        {
            value = new List<int> { 0, 10, 30, 90, 200, 500, 1200, 2500 }
        }); 
        contexts.Config.Set(new ExplosiveScoringTableComponent
        {
            value = new List<int> { 300, 900, 1200, 2000 }
        }); 
        contexts.Config.Set(new ComboDefinitionsComponent
        {
            value = JsonUtility.FromJson<ComboDefinitions>(ComboDefinitions.text)
        });
    }
}