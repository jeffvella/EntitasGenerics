using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Performance.Common
{
    public class NotifyRouter
    {
        public List<RoutedChildEvent> RoutedEvents = new List<RoutedChildEvent>();

        public void Register<TParent, TChild>(Expression<Func<TParent>> property, Expression<Func<TParent, TChild>> childProperty, Action<TParent, ChildPropertyChangedEventArgs> routedEventHandler)
        {
            var memberExpr = property.Body as MemberExpression;
            var childExpr = childProperty.Body as MemberExpression;
            if (memberExpr == null || childExpr == null)
                return;

            RoutedEvents.Add(new RoutedChildEvent<TParent>(memberExpr.Member.Name, childExpr.Member.Name, routedEventHandler));
        }

        public void Register<T>(Expression<Func<T>> propertyExpr, Action<T, ChildPropertyChangedEventArgs> routedEventHandler)
        {
            var memberExpression = propertyExpr.Body as MemberExpression;
            if (memberExpression == null)
                return;

            RoutedEvents.Add(new RoutedChildEvent<T>(memberExpression.Member.Name, routedEventHandler));
        }

        public void ExecuteRouting(object sender, ChildPropertyChangedEventArgs e)
        {
            foreach (var notification in RoutedEvents)
            {
                if (!string.IsNullOrEmpty(notification.ParentPropertyName))
                {
                    if (!string.IsNullOrEmpty(notification.ChildPropertyName))
                    {
                        if (e.ParentPropertyName == notification.ParentPropertyName && e.PropertyName == notification.ChildPropertyName)
                            notification.Execute(sender, e);
                    }
                    else if (e.ParentPropertyName == notification.ParentPropertyName)
                    {
                        notification.Execute(sender, e);
                    }
                }
            }
        }

        public abstract class RoutedChildEvent : IRoutedChildNotification
        {
            public string ParentPropertyName { get; protected set; }
            public string ChildPropertyName { get; protected set; }
            public abstract void Execute(object o, ChildPropertyChangedEventArgs args);
        }

        public class ChildPropertyChangedEventArgs : PropertyChangedEventArgs
        {
            public object Value { get; set; }

            public string ParentPropertyName { get; set; }

            public ChildPropertyChangedEventArgs(string propertyName) : base(propertyName)
            {
            }
        }

        public interface IRoutedChildNotification
        {
            void Execute(object o, ChildPropertyChangedEventArgs args);
        }

        public class RoutedChildEvent<T> : RoutedChildEvent
        {
            public Action<T, ChildPropertyChangedEventArgs> RoutedEventHandler { get; }

            public RoutedChildEvent(string parentPropertyName, Action<T, ChildPropertyChangedEventArgs> routedEventHandler)
            {
                ParentPropertyName = parentPropertyName;
                RoutedEventHandler = routedEventHandler;
            }

            public RoutedChildEvent(string parentPropertyName, string childPropertyName, Action<T, ChildPropertyChangedEventArgs> routedEventHandler)
            {
                ParentPropertyName = parentPropertyName;
                ChildPropertyName = childPropertyName;
                RoutedEventHandler = routedEventHandler;
            }

            public override void Execute(object o, ChildPropertyChangedEventArgs args) => RoutedEventHandler?.Invoke((T)Convert.ChangeType(o, typeof(T)), args);
        }
    }
}