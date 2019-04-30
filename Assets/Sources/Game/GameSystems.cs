using EntitasGenerics;

public class GameSystems : Feature
{
    public GameSystems(Contexts contexts, GenericContexts genericContexts, Services services)
    {
        Add(new FillAllElementsSystem(contexts, genericContexts,  services));

        Add(new AddElementsSystem(contexts, genericContexts, services));

        Add(new ViewSystem(contexts, services));

        Add(new AddSelectionSystem(contexts, genericContexts));
        Add(new UnselectionSystem(contexts));

        Add(new MarkMatchedSystem(contexts, genericContexts));
        Add(new ExplosionSystem(contexts));

        Add(new ComboDetectionSystem(contexts, genericContexts));
        Add(new RewardSystems(contexts, genericContexts, services));

        Add(new ActionCounterSystem(contexts));
        Add(new GameOverSystem(contexts, genericContexts));

        Add(new RemoveMatchedSystem(contexts));
        Add(new DropSelectionSystem(contexts));

        Add(new MoveSystem(contexts, genericContexts));
        Add(new DropSelectionOnMoveSystem(contexts));

        Add(new GameRestartSystem(contexts));
        
        Add(new DestroyEntitySystem(contexts));
    }
}