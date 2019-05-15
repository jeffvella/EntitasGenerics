using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;
using UnityEngine;

[Event(EventTarget.Any)]
public sealed class PositionComponent : IComponent, IEquatable<GridPosition> //, IValueComponent<GridPosition>, IEntityLinkedComponent
//ValueComponent<PositionComponent, GridPosition>
{
    //private ComponentAccessor<PositionComponent, GridPosition> _accessor;

    public GridPosition value;

    public bool Equals(GridPosition other) => other.Equals(value);

    //public GridPosition value { get; set; }

    ////private GridPosition _value;

    ////public GridPosition value
    ////{
    ////    get => _value;
    ////    set => _accessor.Value = value;
    ////}

    //public void DirectSetValue(GridPosition v)
    //{
    //    value = v;
    //}

    //public void Link(IEntityContext context, IEntity entity)
    //{
    //    _accessor = new ComponentAccessor<PositionComponent, GridPosition>(context, entity);
    //}
}


//[Game]
//[Event(EventTarget.Any)]
//public sealed class PositionComponent : ValueComponent<PositionComponent, GridPosition>
//{

//}

//public class ValueComponent<TComponent, TValue> : IEquatable<TValue>, IValueComponent<TValue>, IEntityLinkedComponent
//    where TComponent : IValueComponent<TValue>, new()
//{
//    private TValue _value;
//    private ComponentAccessor<TComponent, TValue> _accessor;

//    public void Link(IEntityContext context, IEntity entity)
//    {
//        _accessor = new ComponentAccessor<TComponent, TValue>(context, entity);
//    }

//    public TValue value
//    {
//        get => _value;
//        set => _accessor.Value = value;
//    }

//    public void SetValue(TValue v)
//    {
//        _value = value;
//    }

//    void IValueComponent<TValue>.DirectSetValue(TValue newValue) => _value = newValue;

//    public bool Equals(TValue other) => other?.Equals(value) ?? false;
//}


//public interface IEntityLinkedComponent
//{
//    void Link(IEntityContext context, IEntity entity);
//}



