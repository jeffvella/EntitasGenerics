using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitas.Generics
{
    public interface IValueComponent : IComponent
    {
        
    }

    public interface IValueComponent<TValue> : IValueComponent
    {
        TValue Value { get; set; }
    }

}
