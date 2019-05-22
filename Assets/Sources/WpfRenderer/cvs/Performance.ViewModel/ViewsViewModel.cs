using Performance.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Performance.ViewModels
{
    public class ViewsViewModel : NotifyBase, IEnumerable<IView>
    {
        private ObservableCollection<IView> _views;

        public ViewsViewModel()
        {
            Items = new ObservableCollection<IView>();
        }

        public ObservableCollection<IView> Items
        {
            get => _views;
            set => SetField(ref _views, value);
        }

        public IEnumerator<IView> GetEnumerator() => _views.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add<T>() where T : IView, new()
        {
            _views.Add(new T());
        }
    }
}
