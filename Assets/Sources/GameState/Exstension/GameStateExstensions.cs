using Assets.Sources.GameState;
using Entitas.Generics;

public static class GameStateExstensions
{
    //public static void ResetState(this GameStateContext context)
    //{
    //    context.ReplaceLastSelected(-1);
    //    context.ReplaceActionCount(0);
    //    context.ReplaceScore(0);
    //    context.ReplaceMaxSelectedElement(0);
    //    context.isGameOver = false;
    //}

    public static void ResetState(this IGenericContext<GameStateEntity> context)
    {
        context.SetUnique(new LastSelectedComponent { value = -1 });
        context.SetUnique(new ActionCountComponent { value = 0 });
        context.SetUnique(new ScoreComponent { value = 0 });
        context.SetUnique(new MaxSelectedElementComponent { value = 0 });
        context.SetUniqueFlag<GameOverComponent>(false);
    }
}