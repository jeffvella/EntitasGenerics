using Entitas;
using Entitas.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitas.Generics
{
    public interface IContextLinkedEntity : IEntity
    {
        IEntityContext Context { get; set; }
    }

    public class LinkedEntity : Entity, IContextLinkedEntity
    {
        IEntityContext IContextLinkedEntity.Context { get; set; }
    }
}
