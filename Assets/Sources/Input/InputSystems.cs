using Entitas.Generics;

public class InputSystems : Feature
{
    public InputSystems(GenericContexts contexts, Services services)
    {
        Add(new UpdateTimeSystem(contexts, services));
        Add(new UpdateInputSystem(contexts, services));
    }
}