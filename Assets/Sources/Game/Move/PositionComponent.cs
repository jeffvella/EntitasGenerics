using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;
using UnityEngine;

[Event(EventTarget.Any)]
public sealed class PositionComponent : IComponent, IEquatable<GridPosition>
{
    public GridPosition value;

    public bool Equals(GridPosition other) => other.Equals(value);
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



