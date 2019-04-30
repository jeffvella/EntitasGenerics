using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas;
using EntitasGenerics;

namespace Assets.Sources.Config
{
    public class ConfigContext : Context<ConfigContext, Entity>
    {
        public ConfigContext() : base(new ConfigContextDefinition()) { }

        public class ConfigContextDefinition : ContextDefinition<ConfigContext, Entity>
        {
            public ConfigContextDefinition()
            {
                ComboDefinitions = Register<ComboDefinitionsComponent>();
                ExsplosiveScoringTable = Register<ExsplosiveScoringTableComponent>();
                MaxActionCount = Register<MaxActionCountComponent>();
                MinMatchCount = Register<MinMatchCountComponent>();
                ScoringTable = Register<ScoringTableComponent>();
                TypeCount = Register<TypeCountComponent>();
                MapSize = Register<MapSizeComponent>();
            }

            public IComponentDefinition<MapSizeComponent> MapSize { get; set; }

            public IComponentDefinition<TypeCountComponent> TypeCount { get; set; }

            public IComponentDefinition<ScoringTableComponent> ScoringTable { get; set; }

            public IComponentDefinition<MinMatchCountComponent> MinMatchCount { get; set; }

            public IComponentDefinition<MaxActionCountComponent> MaxActionCount { get; set; }

            public IComponentDefinition<ExsplosiveScoringTableComponent> ExsplosiveScoringTable { get; set; }

            public IComponentDefinition<ComboDefinitionsComponent> ComboDefinitions { get; }
        }        
    }
}
