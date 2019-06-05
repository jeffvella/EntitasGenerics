using Entitas.Generics;

namespace Entitas.MatchLine
{
    public static class GameStateExstensions
    {
        public static void ResetState(this IGenericContext<GameStateEntity> context)
        {
            context.Unique.GetAccessor<LastSelectedComponent>().Apply(-1);
            context.Unique.GetAccessor<ActionCountComponent>().Apply(0);
            context.Unique.GetAccessor<ScoreComponent>().Apply(0);
            context.Unique.GetAccessor<MaxSelectedElementComponent>().Apply(0);
            context.Unique.SetFlag<GameOverComponent>(false);
        }
    }
}