using System;
using System.Collections.Generic;

namespace Entitas.Generics
{

    public static class ComponentHelper<TEntity, TComponent> 
        where TEntity : IGenericEntity 
        where TComponent : IComponent
    {
        public static int Index { get; private set; } = -1;

        public static bool IsInitialized { get; private set; }

        public static void Initialize(int componentIndex)
        {
            Index = componentIndex;     
            IsInitialized = true;
        }
    }

    public static class ComponentHelper<TComponent> where TComponent : IComponent, new()
    {
        public static TComponent Default { get; } = new TComponent();

        //public static EqualityComparer<TComponent> Default { get; } = new EqualityComparer<TComponent>();

        public static bool IsEvent { get; } = ComponentHelper.IsEventComponent<TComponent>();

        public static bool IsUnique { get; } = ComponentHelper.IsUniqueComponent<TComponent>();

        public static bool IsFlag { get; } = ComponentHelper.IsFlagComponent<TComponent>();
    }

    //public class ValueComaprers<TComponentData> : IEqualityComparer<TComponentData>
    //{
    //    public bool Equals(TComponentData x, TComponentData y)
    //    {
    //        x.
    //    }

    //    public int GetHashCode(TComponentData obj)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public static class ComponentHelper
    {

        public static bool IsUniqueComponent<T>()
        {
            return typeof(IUniqueComponent).IsAssignableFrom(typeof(T));
        }

        public static bool IsEventComponent<T>()
        {
            return typeof(IEventComponent).IsAssignableFrom(typeof(T));
        }

        public static bool IsFlagComponent<T>()
        {
            return typeof(T).GetMembers().Length == 0 || typeof(IEventComponent).IsAssignableFrom(typeof(T));
        }

        public static T Cast<T>(IComponent component)
        {
            if (!(component is T result))
            {
                throw new InvalidCastException();
            }
            return result;
        }

    }
}
