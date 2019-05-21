
using Entitas.MatchLine;

namespace Entitas.MatchLine
{
    public interface IElementService : IService
    {
        void CreateRandomElement(GridPosition position);

        void CreateMovableBlock(GridPosition position);

        void CreateNotMovableBlock(GridPosition position);

        void CreateExsplosiveBlock(GridPosition position);
    }
}
