using EntitasGenerics;

public class RewardSystems : Feature
{
    public RewardSystems(Contexts contexts, GenericContexts genericContexts, Services services)
    {
        Add(new RewardEmitterSystem(contexts, genericContexts));
        Add(new ExsplosiveRewardEmitterSystem(contexts));
        Add(new ComboRewardEmitterSystem(contexts, genericContexts));

        Add(new ApplyRewardSystem(contexts));
    }
}