using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas;
using Entitas.Generics;

namespace Assets.Sources.Config
{
    public class ConfigContext : GenericContext<ConfigContext, Entity>
    {
        public ConfigContext() : base(new ConfigContextDefinition()) { }

        public class ConfigContextDefinition : ContextDefinition<ConfigContext, Entity>
        {
            public ConfigContextDefinition()
            {
                ComboDefinitions = Add<ComboDefinitionsComponent>();
                ExsplosiveScoringTable = Add<ExplosiveScoringTableComponent>();
                MaxActionCount = Add<MaxActionCountComponent>();
                MinMatchCount = Add<MinMatchCountComponent>();
                ScoringTable = Add<ScoringTableComponent>();
                TypeCount = Add<TypeCountComponent>();
                MapSize = Add<MapSizeComponent>();
            }

            public IComponentDefinition<MapSizeComponent> MapSize { get; }

            public IComponentDefinition<TypeCountComponent> TypeCount { get; }

            public IComponentDefinition<ScoringTableComponent> ScoringTable { get; }

            public IComponentDefinition<MinMatchCountComponent> MinMatchCount { get; }

            public IComponentDefinition<MaxActionCountComponent> MaxActionCount { get; }

            public IComponentDefinition<ExplosiveScoringTableComponent> ExsplosiveScoringTable { get; }

            public IComponentDefinition<ComboDefinitionsComponent> ComboDefinitions { get; }
        }        
    }
}
