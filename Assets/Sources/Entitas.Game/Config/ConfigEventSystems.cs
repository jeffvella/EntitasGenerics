using Entitas;
using Entitas.Generics;
using Entitas.MatchLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitas.MatchLine
{
    public sealed class ConfigEventSystems : Feature
    {
        public ConfigEventSystems(Contexts contexts)
        {
            Add(new EventSystem<ConfigEntity, MaxActionCountComponent>(contexts.Config, GroupEvent.Added));
            Add(new EventSystem<ConfigEntity, MapSizeComponent>(contexts.Config, GroupEvent.Added));
        }
    }
}
