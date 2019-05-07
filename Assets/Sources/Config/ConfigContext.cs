using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitas;
using Entitas.Generics;

namespace Assets.Sources.Config
{
    public class ConfigContext : GenericContext<ConfigContext, ConfigEntity>
    {
        public ConfigContext() : base(new ConfigContextDefinition()) { }

        public class ConfigContextDefinition : ContextDefinition<ConfigContext, ConfigEntity>
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
