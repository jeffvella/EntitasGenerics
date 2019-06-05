using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;

namespace Entitas.Generics
{
    //[BurstCompile]
    //public unsafe struct FindEntityJob<TKey,TEntity> : IJob
    //    where TEntity : class, IEntity
    //    where TKey : struct, IComparable<TKey>
    //{
    //    public TKey Key;

    //    [NativeDisableUnsafePtrRestriction]
    //    public GCHandle* HandleResultPtr;
    //    public NativeFasterDictionary<TKey, GCHandle> Data;

    //    public void Execute()
    //    {
    //        if (Data.TryFindIndex(Key, out int findIndex))
    //        {
    //            ref GCHandle handlePtr = ref Data.GetValue(findIndex);
    //            *HandleResultPtr = *(GCHandle*)UnsafeUtility.AddressOf(ref handlePtr);
    //        }
    //    }

    //    public unsafe bool TryFindEntity(FindEntityJob<TKey, TEntity> job, NativePrimaryEntityIndex<TEntity, TKey> index, TKey key, out TEntity entity)
    //    {
    //        GCHandle resultHandle;
    //        job.Data = index._index;
    //        job.Key = key;
    //        job.HandleResultPtr = &resultHandle;

    //        //var job = new FindEntityJob<TKey,TEntity>
    //        //{
    //        //    Data = index._index,
    //        //    Key = key,
    //        //    HandleResultPtr = &resultHandle
    //        //};
    //        job.Run();
    //        if (!resultHandle.IsAllocated)
    //        {
    //            entity = default;
    //            return false;
    //        }
    //        entity = (TEntity)resultHandle.Target;
    //        return true;
    //    }
    //}

    public unsafe class NativePrimaryEntityIndex<TEntity, TKey> : IDisposable, IEntityIndex
        where TEntity : class, IEntity
        where TKey : struct, IComparable<TKey>
    {
        public readonly NativeFasterDictionary<TKey, GCHandle> _index;
        private bool _disposed;

        //public static FindEntityJob<TKey, TEntity> Instance { get; } = new FindEntityJob<TKey, TEntity>();

        public NativePrimaryEntityIndex(string name, IGroup<TEntity> group, Func<TEntity, IComponent, TKey> getKey, IEqualityComparer<TKey> comparer)
        {
            _index = new NativeFasterDictionary<TKey, GCHandle>(50, Allocator.Persistent);
            _name = name;
            _group = group;
            _getKey = getKey;
            _isSingleKey = true;
            Activate();
        }

        public void Activate()
        {
            _group.OnEntityAdded += onEntityAdded;
            _group.OnEntityRemoved += onEntityRemoved;
            indexEntities(_group);
        }

        public TEntity GetEntity(TKey key)
        {           
            _index.TryGetValue(key, out GCHandle entityHandle);
            if (!entityHandle.IsAllocated)
            {
                return default;
            }
            var entity = Unsafe.As<TEntity>(entityHandle.Target);
            if (entity == null)
            {
                throw new InvalidOperationException("Invalid entity encountered while clearing entity index");
            }
            return entity;
        }

        public bool TryGetEntity(TKey key, out TEntity entity)
        {
            //return FindEntityJob<TKey,TEntity>.TryFindEntity(this, key, out entity);

            _index.TryGetValue(key, out GCHandle entityHandle);
            if (!entityHandle.IsAllocated)
            {
                entity = default;
                return false;
            }
            entity = Unsafe.As<TEntity>(entityHandle.Target);
            if (entity == null)
            {
                throw new InvalidOperationException("Invalid entity encountered while clearing entity index");
            }
            return true;
        }

        //public bool TryGetEntity(ref FindEntityJob<TKey, TEntity> job, TKey key, out TEntity entity)
        //{
        //    return job.TryFindEntity(this, key, out entity);
        //}

        public override string ToString()
        {
            return "NativePrimaryEntityIndex(" + name + ")";
        }

        protected void clear()
        {
            var entities = _index.GetValuesArray(out int count);
            for (int i = 0; i < count; i++)
            {
                var handle = entities[i];          
                if (!handle.IsAllocated)
                {
                    continue;
                }
                if(handle.Target != null)
                {
                    var entity = Unsafe.As<TEntity>(handle.Target);
                    var safeAerc = entity.aerc as SafeAERC;
                    if (safeAerc != null)
                    {
                        if (safeAerc.owners.Contains(this))
                        {
                            entity.Release(this);
                        }
                    }
                    else
                    {
                        entity.Release(this);
                    }
                }
                handle.Free();
            }
            _index.Clear();
        }

        protected void addEntity(TKey key, TEntity entity)
        {
            if (_index.ContainsKey(key))
            {
                throw new EntityIndexException(
                    "Entity for key '" + key + "' already exists!",
                    "Only one entity for a primary key is allowed.");
            }
            var safeAerc = entity.aerc as SafeAERC;
            if (safeAerc != null)
            {
                if (!safeAerc.owners.Contains(this))
                {
                    entity.Retain(this);
                }
            }
            else
            {
                entity.Retain(this);
            }
            var handle = GCHandle.Alloc(entity, GCHandleType.Pinned);  
            try
            {
                _index.Add(key, handle);
            }
            catch(Exception ex)
            {
                Debug.LogError($"Exception {ex}");    
                handle.Free();
                throw;
            }
        }

        protected void removeEntity(TKey key, TEntity entity)
        {
            try
            {
                if (!_index.TryGetValue(key, out var value))
                {
                    Debug.LogError("Unable to find indexed entity to free allocated memory.");
                }
                if (!_index.Remove(key))
                {
                    throw new Exception("Failed properly removing an indexed entity.");
                }
                value.Free();
            }
            catch (Exception)
            {
                Debug.LogError("Failed properly removing an indexed entity, possible memory leak.");
            }
            var safeAerc = entity.aerc as SafeAERC;
            if (safeAerc != null)
            {
                if (safeAerc.owners.Contains(this))
                {
                    entity.Release(this);
                }
            }
            else
            {
                entity.Release(this);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                Deactivate();
            }
            _index.Dispose();
            _disposed = true;
        }

        ~NativePrimaryEntityIndex() => Dispose(false);

        #region Base Copy Pasta

        public string name { get { return _name; } }
        protected readonly string _name;
        protected readonly IGroup<TEntity> _group;
        protected readonly Func<TEntity, IComponent, TKey> _getKey;
        protected readonly Func<TEntity, IComponent, TKey[]> _getKeys;
        protected readonly bool _isSingleKey;

        public virtual void Deactivate()
        {
            _group.OnEntityAdded -= onEntityAdded;
            _group.OnEntityRemoved -= onEntityRemoved;
            clear();
        }

        protected void indexEntities(IGroup<TEntity> group)
        {
            foreach (var entity in group)
            {
                if (_isSingleKey)
                {
                    addEntity(_getKey(entity, null), entity);
                }
                else
                {
                    var keys = _getKeys(entity, null);
                    for (int i = 0; i < keys.Length; i++)
                    {
                        addEntity(keys[i], entity);
                    }
                }
            }
        }

        protected void onEntityAdded(IGroup<TEntity> group, TEntity entity, int index, IComponent component)
        {
            if (_isSingleKey)
            {
                addEntity(_getKey(entity, component), entity);
            }
            else
            {
                var keys = _getKeys(entity, component);
                for (int i = 0; i < keys.Length; i++)
                {
                    addEntity(keys[i], entity);
                }
            }
        }

        protected void onEntityRemoved(IGroup<TEntity> group, TEntity entity, int index, IComponent component)
        {
            if (_isSingleKey)
            {
                removeEntity(_getKey(entity, component), entity);
            }
            else
            {
                var keys = _getKeys(entity, component);
                for (int i = 0; i < keys.Length; i++)
                {
                    UnityEngine.Debug.Log($"Removed component {component.GetType().Name} from entity {entity.GetType().Name} from index {_name}");
                    removeEntity(keys[i], entity);
                }
            }
        }

        #endregion
    }
}
