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

    /// <summary>
    /// A component with a single value.
    /// </summary>
    public interface IValueComponent<TValue> : IValueComponent
    {
        TValue Value { get; set; }
    }

}
