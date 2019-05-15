using System;
using System.Xml;
using Assets.Sources.Game;
using Assets.Sources.GameState;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Entitas.Generics;

//[GameState]
[Unique]
[Event(EventTarget.Any)]
public sealed class ScoreComponent : IUniqueComponent //ValueComponent<GameStateContext, GameStateEntity, ScoreComponent, int>, IUniqueComponent
//: IUniqueComponent, IValueComponent<int>
{
    public int value;
    //private IValueComponent<int> _valueComponentImplementation;
    //private IValueAccessor<int> _accessor;


    ////private Action<ScoreComponent> _updater;

    //public ScoreComponent()
    //{
    //   // _updater = component => ;
    //}

    //public void Initialize()
    //{
    //    //_accessor = new GenericContext<GameContext,GameEntity>.EntityAccessor.ComponentAccessor<ScoreComponent,int>();

    //    _accessor = GenericContext<GameContext, GameEntity>.EntityAccessor.CreateComponentAccessor<ScoreComponent, int>();
    //}

    //public int value { get; set; }

    //public void Set(int v)
    //{
    //    _accessor.Value = v;
    //}

    ////public static void Set(GenericContext<GameContext, GameEntity>.EntityAccessor accessor, int newValue)
    ////{
    ////    // global context access
    ////    // pass entity arg
    ////    // 

    ////    //var a = new EntityAccessor<TEntity>.ComponentAccessor<ScoreComponent, int>(accessor.Context, accessor.Entity);

    ////    accessor.Update();

    ////    a.Value = 5;
    ////}

    ////public static Action<ScoreComponent> ValueSetter(int value)
    ////{
    ////    return obj => obj.value = value;
    ////}


}

//public interface IContextLinkedComponent<TEntity>
//{
//    void Initialize(IGenericContext<TEntity> context, TEntity entity)
//}

public interface IContextInitializedComponent<TEntity> where TEntity : class, IEntity
{
    void Initialize(IGenericContext<TEntity> context, TEntity entity);
}

//public class ValueComponent<TContext, TEntity, TComponent, TValue> : IValueComponent<TValue>, IContextInitializedComponent<TEntity>
//    where TContext : GenericContext<TContext,TEntity>
//    where TEntity : class, IEntity, new()
//    where TComponent : IValueComponent<TValue>, new() 
//    where TValue : unmanaged
//{
//    private static IValueAccessor<TValue> _accessor; 

//    public void Initialize(IGenericContext<TEntity> context, TEntity entity)
//    {
//        _accessor = new GenericContext<TContext,TEntity>.EntityAccessor.ComponentAccessor<TComponent,TValue>(context, entity);
//    }

//    private TValue _value;

//    public TValue Value
//    {
//        get => _accessor?.Value ?? _value;
//        set
//        {
//            if (_accessor == null)
//            {
//                _value = value;
//            }
//            else
//            {
//                _accessor.Value = value;
//            }
//        }
//    }

//    TValue IValueComponent<TValue>.GetValue() =>_value;

//    void IValueComponent<TValue>.SetValue(TValue newValue) =>  _value = newValue;   
//}

//public interface IValueComponent : IComponent
//{

//}

//public interface IAccessibleComponent<TComponent>
//{
//    IValueAccessor<TComponent> Accessor { get; }
//}

//public interface IValueComponent<T> : IValueComponent
//{
//    T value { get; set; }

//    //T GetValue();

//    void DirectSetValue(T value);
//}

//public class ValueComponent<T> : IValueComponent<T> where T : unmanaged
//{
//    public static EntityAccessor<TEntity>.ComponentAccessor<TComponent, T> 
//        CreateAccessor<TEntity, TComponent>(IGenericContext<TEntity> context, TEntity entity) 
//        where TEntity : class, IEntity 
//        where TComponent : IValueComponent<T>, IComponent, new() 
//        => new EntityAccessor<TEntity>.ComponentAccessor<TComponent, T>(context, entity);

//    public T value { get; set; }

//    public void SetValue(T value)
//    {

//    }

//    public T GetValue()
//    {
//        return default;
//    }
//}

//public class ComponentUpdater<TComponent, TValue>
//    where TComponent : IValueComponent<TValue>, IComponent, new()
//{
//    static GenericMatcher()
//    {
//        Func = Updater;
//    }

//    private static void Updater(TComponent obj)
//    {
//        obj.value = value;

//    }

//    public static Action<TComponent> Func { get } 
//}
