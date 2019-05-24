using Entitas.Generics;

namespace Entitas.MatchLine
{
    public class InputSystems : Feature
    {
        public InputSystems(Contexts contexts, IServices services)
        {
            Add(new UpdateTimeSystem(contexts, services));
            Add(new UpdateInputSystem(contexts, services));
        }
    }
}