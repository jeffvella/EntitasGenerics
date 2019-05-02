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
                Add<ComboDefinitionsComponent>();
                Add<ExplosiveScoringTableComponent>();
                Add<MaxActionCountComponent>();
                Add<MinMatchCountComponent>();
                Add<ScoringTableComponent>();
                Add<TypeCountComponent>();
                Add<MapSizeComponent>();
            }
        }        
    }
}
