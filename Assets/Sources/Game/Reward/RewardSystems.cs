using Entitas.Generics;

public class RewardSystems : Feature
{
    public RewardSystems(Contexts contexts, GenericContexts genericContexts, Services services)
    {
        Add(new RewardEmitterSystem(contexts, genericContexts));
        Add(new ExsplosiveRewardEmitterSystem(contexts, genericContexts));
        Add(new ComboRewardEmitterSystem(contexts, genericContexts));
        Add(new ApplyRewardSystem(contexts, genericContexts));
    }
}