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
                Add<ExplosiveScoringTableComponent>();
                Add<MaxActionCountComponent>();
                Add<MinMatchCountComponent>();
                Add<ScoringTableComponent>();
                Add<TypeCountComponent>();
                Add<MapSizeComponent>();
                Add<ComboDefinitionsComponent>();
            }
        }        
    }
}
