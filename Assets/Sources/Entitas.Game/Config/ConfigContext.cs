using System;
using Entitas.Generics;

namespace Entitas.MatchLine
{
    public class ConfigContext : GenericContext<ConfigContext, ConfigEntity>
    {
        public ConfigContext() : base(new ConfigContextDefinition()) { }

        public class ConfigContextDefinition : ContextDefinition<ConfigContext, ConfigEntity>
        {
            public override Func<ConfigEntity> EntityFactory => () => new ConfigEntity();

            public ConfigContextDefinition()
            {
                AddComponent<ExplosiveScoringTableComponent>();
                AddComponent<MaxActionCountComponent>();
                AddComponent<MinMatchCountComponent>();
                AddComponent<ScoringTableComponent>();
                AddComponent<TypeCountComponent>();
                AddComponent<MapSizeComponent>();
                AddComponent<ComboDefinitionsComponent>();
            }
        }        
    }
}
