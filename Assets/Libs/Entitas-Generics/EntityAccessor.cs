using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using Entitas.CodeGeneration.Attributes;
using TreeEditor;
using UnityEditorInternal;
using UnityEngine.Experimental.PlayerLoop;

namespace Entitas.Generics
{
    //public readonly ref struct EntityAccessor<TEntity> where TEntity : class, IContextLinkedEntity
    //{
    //    public readonly IGenericContext<TEntity> Context;
    //    public readonly TEntity Entity;

    //    public EntityAccessor(IGenericContext<TEntity> context, TEntity entity)
    //    {
    //        Context = context;
    //        Entity = entity;
    //    }

    //    public ComponentAccessor<TComponent> Get<TComponent>() where TComponent : class, IComponent, new()
    //    {
    //        return new ComponentAccessor<TComponent>(Entity, Context);
    //    }

    //    public void Set<TComponent, TValue>(TValue value) where TComponent : class, IValueComponent<TValue>, new()
    //    {
    //        Get<TComponent>().Apply(value);
    //    }

    //    public void SetFlag<TComponent>(bool value = true) where TComponent : class, IFlagComponent, new()
    //    {
    //        Get<TComponent>().SetFlag(value);
    //    }

    //    public void IsFlagged<TComponent>(bool value = true) where TComponent : class, IFlagComponent, new()
    //    {
    //        Get<TComponent>().IsFlagged(value);
    //    }

    //    public static implicit operator TEntity(EntityAccessor<TEntity> accessor) => accessor.Entity;

    //    //public PersistentEntityAccessor<TEntity> ToPersistent()
    //    //{
    //    //    return new PersistentEntityAccessor<TEntity>(Context, Entity);
    //    //}
    //}

    //public sealed class PersistentEntityAccessor<TEntity> where TEntity : class, IEntity
    //{
    //    public readonly IGenericContext<TEntity> Context;
    //    public readonly TEntity Entity;

    //    public PersistentEntityAccessor(IGenericContext<TEntity> context, TEntity entity)
    //    {
    //        Context = context;
    //        Entity = entity;
    //    }

    //    public ComponentAccessor<TComponent> Get<TComponent>() where TComponent : class, IComponent, new()
    //    {
    //        return new ComponentAccessor<TComponent>(Entity, Context);
    //    }

    //    public void Set<TComponent, TValue>(TValue value) where TComponent : class, IValueComponent<TValue>, new()
    //    {
    //        Get<TComponent>().Apply(value);
    //    }

    //    public void SetFlag<TComponent>(bool value = true) where TComponent : class, IFlagComponent, new()
    //    {
    //        Get<TComponent>().SetFlag(value);
    //    }

    //    public void IsFlagged<TComponent>(bool value = true) where TComponent : class, IFlagComponent, new()
    //    {
    //        Get<TComponent>().IsFlagged(value);
    //    }

    //    public static implicit operator TEntity(PersistentEntityAccessor<TEntity> accessor) => accessor.Entity;
    //}

    //public class PersistentEntityAccessor<TEntity> where TEntity : class, IEntity
    //{
    //    public readonly IGenericContext<TEntity> Context;
    //    public readonly TEntity Entity;

    //    public PersistentEntityAccessor(IGenericContext<TEntity> context, TEntity entity)
    //    {
    //        Context = context;
    //        Entity = entity;
    //        //_accessors = new IComponentAccessor[context.totalComponents];
    //    }

    //    public bool IsValid => Entity.isEnabled;

    //    //private IComponentAccessor[] _accessors;

    //    private static class Accessors<TComponent>
    //    {
    //        public ComponentAccessor<TComponent> Accessor;
    //    }

    //    private class ComponentData
    //    {
    //        public int Index;
    //    }

    //    private GetOrCreateComponentAccessor<TComponent>() where TComponent : class, IComponent, new()
    //    {
    //        var accessor = _accessors[index];
    //        if(_ac)
    //    }

    //    public ComponentAccessor<TComponent> Get<TComponent>() where TComponent : class, IComponent, new()
    //    {
    //        return new ComponentAccessor<TComponent>(Entity, Context);
    //    }

    //    public void Set<TComponent, TValue>(TValue value) where TComponent : class, IValueComponent<TValue>, new()
    //    {
    //        new ComponentAccessor<TComponent>(Entity, Context).Apply(value);
    //    }

    //    public void SetFlag<TComponent>(bool value = true) where TComponent : class, IFlagComponent, new()
    //    {
    //        new ComponentAccessor<TComponent>(Entity, Context).SetFlag(value);
    //    }

    //    public void IsFlagged<TComponent>(bool value = true) where TComponent : class, IFlagComponent, new()
    //    {
    //        new ComponentAccessor<TComponent>(Entity, Context).IsFlagged(value);
    //    }

    //    public static implicit operator TEntity(EntityAccessor<TEntity> accessor) => accessor.Entity;
    //}

}