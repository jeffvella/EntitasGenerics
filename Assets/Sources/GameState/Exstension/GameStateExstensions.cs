using Assets.Sources.GameState;
using Entitas.Generics;
using UnityEngine.SocialPlatforms.Impl;

public static class GameStateExstensions
{
    public static void ResetState(this IGenericContext<GameStateEntity> context)
    {        
        context.SetUnique<LastSelectedComponent>(c => c.value = -1);
        context.SetUnique<ActionCountComponent>(c => c.value = 0);
        context.SetUnique<ScoreComponent>(c => c.Value = 0); 
        context.SetUnique<MaxSelectedElementComponent>(c => c.value = 0);
        context.SetFlag<GameOverComponent>(false);
    }
}