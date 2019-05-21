using System;
using Entitas;
using UnityEngine;
using Entitas.MatchLine;

public sealed class UnityViewService : Service, IViewService
{
    private Transform _root;

    public UnityViewService(Contexts contexts) : base(contexts)
    {
    }

    public void LoadAsset(Contexts contexts, IEntity entity, string assetName, int assetType)
    {
        var root = GetRoot();
        var viewObject = GetResource(assetName);

        var view = viewObject.GetComponent<IView>();
        view.InitializeView(contexts, entity);

        var eventListeners = viewObject.GetComponents<IEventListener>();
        foreach (var listener in eventListeners)
            listener.RegisterListeners(contexts, entity);
    }

    public void LoadAsset<TEntity>(Contexts contexts, TEntity entity, string assetName, int assetType) where TEntity : IEntity
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
        var viewObject = UnityEngine.Object.Instantiate(resource, _root);
        if (viewObject == null)
            throw new NullReferenceException($"Prefabs/{assetName} not found!");
        return viewObject;
    }
}