using Performance.Common;
using Performance.ViewModels;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Performance
{
    internal class ElementPool : IElementFactory
    {        
        private ConcurrentDictionary<string,ConcurrentStack<ElementViewModel>> _pools = new ConcurrentDictionary<string, ConcurrentStack<ElementViewModel>>();

        public ElementViewModel Create(string assetName, int assetType)
        {
            var pool = GetPool(assetName);
            if (pool.Count != 0)
            {
                if (pool.TryPop(out var element))
                    return element;
            }
            return CreateNewElement(assetName, assetType);
        }

        private ConcurrentStack<ElementViewModel> GetPool(string assetName)
        {
            return _pools.GetOrAdd(assetName, key => new ConcurrentStack<ElementViewModel>());
        }

        private static ElementViewModel CreateNewElement(string assetName, int assetType)
        {
            var element = new ElementViewModel();
            element.AddBehavior<DestroyedListener>();
            element.AddBehavior<ColorListener>();
            element.AddBehavior<PositionListener>();
            element.AddBehavior<SelectedListener>();
            element.AssetName = assetName;
            element.ActorType = (ActorType)assetType;
            return element;
        }

        public void Return(ElementViewModel element)
        {
           GetPool(element.AssetName).Push(element);           
        }

    }
}

