using Entitas.Generics;

namespace Entitas.MatchLine
{
    public static class GameStateExstensions
    {
        public static void ResetState(this IGenericContext<GameStateEntity> context)
        {
            context.Unique.Get<LastSelectedComponent>().Apply(-1);
            context.Unique.Get<ActionCountComponent>().Apply(0);
            context.Unique.Get<ScoreComponent>().Apply(0);
            context.Unique.Get<MaxSelectedElementComponent>().Apply(0);
            context.Unique.SetFlag<GameOverComponent>(false);
        }
    }
}