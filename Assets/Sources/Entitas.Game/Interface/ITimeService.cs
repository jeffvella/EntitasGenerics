
namespace Entitas.MatchLine
{
    public interface ITimeService : IService
    {
        float DeltaTime();
        float RealtimeSinceStartup();
    }
}