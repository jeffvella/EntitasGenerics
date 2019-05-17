using System;
using System.Runtime.Remoting.Messaging;
using Entitas;
using Entitas.Generics;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class UnityViewService : Service, IViewService
{
    private Transform _root;

    public UnityViewService(Contexts contexts) : base(contexts)
    {
    }

    public void LoadAsset(Contexts contexts, IEntity entity, string assetName)
    {
        var root = GetRoot();
        var viewObject = GetResource(assetName);

        var view = viewObject.GetComponent<IView>();
        view.InitializeView(contexts, entity);

        var eventListeners = viewObject.GetComponents<IEventListener>();
        foreach (var listener in eventListeners)
            listener.RegisterListeners(contexts, entity);
    }

    public void LoadAsset<TEntity>(Contexts contexts, TEntity entity, string assetName) where TEntity : IEntity
    {
        var root = GetRoot();
        var viewObject = GetResource(assetName);

        foreach (var view in viewObject.GetComponents<IView>())
            view.InitializeView(contexts, entity);

        foreach (var view in viewObject.GetComponents<IView<TEntity>>())
            view.InitializeView(contexts, entity);

        foreach (var listener in viewObject.GetComponents<IEventListener>())
            listener.RegisterListeners(contexts, entity);

        foreach (var listener in viewObject.GetComponents<IEventListener<TEntity>>())
            listener.RegisterListeners(contexts, entity);
    }

    private Transform GetRoot()
    {
        if (_root == null)
        {
            _root = new GameObject("ViewRoot").transform;                 
        }
        return _root;
    }

    private GameObject GetResource(string assetName)
    {
        var resource = Resources.Load<GameObject>($"Prefabs/{assetName}");
        var viewObject = Object.Instantiate(resource, _root);
        if (viewObject == null)
            throw new NullReferenceException($"Prefabs/{assetName} not found!");
        return viewObject;
    }

    //public IViewService<TEntity> CreateFor<TEntity>() where TEntity : IEntity 
    //    => new UnityViewService<TEntity>(_contexts, _root);
}

//public sealed class UnityViewService<TEntity> : Service, IViewService<TEntity> where TEntity : IEntity
//{
//    private Transform _root;

//    public UnityViewService(Contexts contexts, Transform root) : base(contexts)
//    {
//        _root = root;
//    }

//    public void LoadAsset(Contexts contexts, TEntity entity, string asset)
//    {
//        if (_root == null)
//        {
//            _root = new GameObject("ViewRoot").transform;
//        }

//        var resource = Resources.Load<GameObject>($"Prefabs/{asset}");
//        var viewObject = Object.Instantiate(resource, _root);
//        if (viewObject == null)
//            throw new NullReferenceException($"Prefabs/{asset} not found!");

//        var view = viewObject.GetComponent<IView<TEntity>>();
//        view.InitializeView(contexts, entity);

//        var eventListeners = viewObject.GetComponents<IEventListener<TEntity>>();
//        foreach (var listener in eventListeners)
//            listener.RegisterListeners(contexts, entity);
//    }
//}
