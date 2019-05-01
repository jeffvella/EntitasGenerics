using Entitas.Generics;

public class InputSystems : Feature
{
    public InputSystems(Contexts contexts, GenericContexts genericContexts, Services services)
    {
        Add(new UpdateTimeSystem(contexts, genericContexts, services));
        Add(new UpdateInputSystem(contexts, genericContexts, services));
    }
}