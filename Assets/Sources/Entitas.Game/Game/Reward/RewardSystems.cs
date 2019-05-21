namespace Entitas.MatchLine
{
    public class RewardSystems : Feature
    {
        public RewardSystems(Contexts contexts, IServices services)
        {
            Add(new RewardEmitterSystem(contexts));
            Add(new ExsplosiveRewardEmitterSystem(contexts));
            Add(new ComboRewardEmitterSystem(contexts));
            Add(new ApplyRewardSystem(contexts));
        }
    }
}