
namespace Entitas.MatchLine
{

    public interface IServices
    {
        IViewService ViewService { get; }

        IInputService InputService { get; }

        ITimeService TimeService { get; }

        IElementService ElementService { get; }
    }
}