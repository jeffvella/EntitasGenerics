using Performance.Common;
using Performance.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Performance.Controls
{
    public class BoardViewModel : NotifyBase
    {
        private ObservableCollection<ElementViewModel> _elements;
        private SynchronizationContext _sync;
        private InputViewModel _input;

        public BoardViewModel()
        {
            _elements = new ObservableCollection<ElementViewModel>();
            _sync = SynchronizationContext.Current;
            _input = new InputViewModel();
        }

        public InputViewModel Input
        {
            get => _input;
            set => SetField(ref _input, value);
        }

        public ObservableCollection<ElementViewModel> Elements
        {
            get => _elements;
            set => SetField(ref _elements, value);
        }

        public void AddElement(ElementViewModel element)
        {
            _sync.Post(state =>
            {
                _elements.Add(element);

            }, null);
        }

        public void RemoveElement(ElementViewModel element)
        {
            _sync.Post(state =>
            {
                _elements.Remove(element);

            }, null);
        }

        public ICommand OnMouseUpOnBoardCommand => new RelayCommand<InputViewModel>(param =>
        {
            _input.IsMouseDown = false;
        });
    }

    public class ElementDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement frameworkElement = container as FrameworkElement;
            if (frameworkElement != null && item != null && item is ElementViewModel gameElement)
            {
                switch (gameElement.ActorType)
                {
                    case ActorType.ExplosiveBlock:                       
                    case ActorType.NotMovableBlock:                      
                    case ActorType.Block:
                        return frameworkElement.FindResource("BlockElementTemplate") as DataTemplate; 

                    case ActorType.Element:
                        return frameworkElement.FindResource("DefaultElementTemplate") as DataTemplate;

                    default:
                        throw new UnknownElementTypeException($"Unknown element type with AssetName: '{gameElement.AssetName}', ActorType: '{gameElement.ActorType}'");
                }           
            }
            return null;
        }
    }

    public class UnknownElementTypeException : InvalidOperationException
    {
        public UnknownElementTypeException(string msg) : base(msg)
        {

        }
    }


}
