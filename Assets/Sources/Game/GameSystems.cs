using Entitas.Generics;

public class GameSystems : Feature
{
    public GameSystems(Contexts contexts, GenericContexts genericContexts, Services services)
    {
        Add(new FillAllElementsSystem(contexts, genericContexts,  services));

        Add(new AddElementsSystem(contexts, genericContexts, services));

        Add(new ViewSystem(contexts, genericContexts, services));

        Add(new AddSelectionSystem(contexts, genericContexts));
        Add(new UnselectionSystem(contexts, genericContexts));

        Add(new MarkMatchedSystem(contexts, genericContexts));
        Add(new ExplosionSystem(contexts, genericContexts));

        Add(new ComboDetectionSystem(contexts, genericContexts));
        Add(new RewardSystems(contexts, genericContexts, services));

        Add(new ActionCounterSystem(contexts, genericContexts));
        Add(new GameOverSystem(contexts, genericContexts));

        Add(new RemoveMatchedSystem(contexts, genericContexts));
        Add(new DropSelectionSystem(contexts, genericContexts));

        Add(new MoveSystem(contexts, genericContexts));
        Add(new DropSelectionOnMoveSystem(contexts, genericContexts));

        Add(new GameRestartSystem(contexts, genericContexts));
        
        Add(new DestroyEntitySystem(contexts, genericContexts));
    }
}