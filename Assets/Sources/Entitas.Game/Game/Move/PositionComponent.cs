using System;
using System.Collections.Generic;
using System.Diagnostics;
using Entitas.Generics;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

namespace Entitas.MatchLine
{


    [DebuggerDisplay("{Value} {HashCode}")]
    public sealed class PositionComponent : ValueComponent<GridPosition>, IEventComponent, IIndexedComponent<PositionComponent>, IEquatable<GridPosition> 
    {
        public bool Equals(PositionComponent x, PositionComponent y) => x != null && y != null && x.Value.Equals(y.Value);

        public int GetHashCode(PositionComponent obj) => obj.Value.GetHashCode();

        public bool Equals(GridPosition other) => other.Equals(Value);

        public override string ToString()
        {
            return $"{Value}";
        }

        //public PrimaryEntityIndex<TEntity, GridPosition> CreateIndex<TEntity>(IGenericContext<TEntity> context) where TEntity : class, IEntity, new()
        //{
        //    return new PrimaryEntityIndex<TEntity, GridPosition>(nameof(PositionComponent), context.GetGroup<PositionComponent>(), GetValue);
        //}

        //private GridPosition GetValue<TEntity>(TEntity entity, IComponent component) where TEntity : class, IEntity, new()
        //{
        //    entity.Get()
        //    return ((PositionComponent)component).value;
        //}


        //public class IndexFactory<TEntity,TComponent,TValue> where TEntity : class, IEntity, new() where TComponent : class, IComponent, new()
        //{
        //    public PrimaryEntityIndex<TEntity, TComponent> CreateIndex(IGenericContext<TEntity> context) 
        //    {
        //        return new PrimaryEntityIndex<TEntity, TValue>(nameof(TComponent), context.GetGroup<TComponent>(), GetValue);
        //    }
        //}

        //public static GameEntity GetEntityWithId<TContext,TValue>(TContext context, TValue value) where TContext : IContext
        //{
        //    return ((Entitas.PrimaryEntityIndex<GameEntity, int>)context.GetEntityIndex(Contexts.Id)).GetEntity(value);
        //}

        //game.AddEntityIndex(new Entitas.PrimaryEntityIndex<GameEntity, GridPosition>(
        //Position,
        //game.GetGroup(GameMatcher.Position),
        //(e, c) => ((PositionComponent) c).value));

        //public bool Equals(PositionComponent x, PositionComponent y) //=> x.value.Equals(y.value);
        //{
        //    var equals = x?.value.Equals(y?.value) ?? false;
        //    Debug.Log($"{x.value}/{x.HashCode} Equals({y.value}/{y.HashCode})={equals}");
        //    return equals;
        //}

        //public int GetHashCode(PositionComponent obj) => obj.value.GetHashCode();

        //public override int GetHashCode() => value.GetHashCode();

        //public bool Equals(PositionComponent other)
        //{
        //    var equals = other != null && Equals(other.value);
        //    Debug.Log($"{value}/{HashCode} Equals({other.value}/{other.HashCode})={equals}");
        //    return equals;
        //}

        //public int HashCode => GetHashCode(this);

    }
}