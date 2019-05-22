using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Performance.Common
{
    [DataContract]
    [KnownType("DerivedTypes")]
    public class NotifyBase : INotifyPropertyChanged
    {
        public NotifyBase()
        {
            ChildPropertyChanged += NotifyManager.ExecuteRouting;
        }

        public NotifyRouter NotifyManager = new NotifyRouter();

        private static Type[] DerivedTypes()
        {
            return GetDerivedTypes(typeof(NotifyBase), Assembly.GetExecutingAssembly()).ToArray();
        }

        public static IEnumerable<Type> GetDerivedTypes(Type baseType, Assembly assembly)
        {
            var types = from t in assembly.GetTypes()
                        where t.IsSubclassOf(baseType)
                        select t;

            return types;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool ThrottleChangeNotifications { get; set; }

        protected bool SetField<T>(ref T field, T value, bool routeNotifications = false, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            if (routeNotifications)
                SetNotifyRouting(ref field, value, propertyName);

            field = value;

            if (!ThrottleChangeNotifications)
                OnPropertyChanged(propertyName);

            return true;
        }

        protected bool SetField<T>(ref T field, T value, string secondaryPropertyTriggerName, bool routeNotifications = false, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            if (routeNotifications)
                SetNotifyRouting(ref field, value, propertyName);

            field = value;

            if (!ThrottleChangeNotifications)
            {
                OnPropertyChanged(propertyName);
                OnPropertyChanged(secondaryPropertyTriggerName);
            }
            return true;
        }

        private void SetNotifyRouting<T>(ref T oldValue, T newValue, string propertyName)
        {
            if (oldValue is INotifyPropertyChanged notifyOldValue)
            {
                if (NotifySubscribers.ContainsKey(propertyName))
                {
                    NotifySubscribers[propertyName].PropertyChanged -= OnChildPropertyChanged;
                }
            }
            SubscribeChildNotifications(newValue, propertyName);
        }

        private void SubscribeChildNotifications<T>(T newValue, string propertyName)
        {
            if (newValue is INotifyPropertyChanged notifyNewValue)
            {
                if (!NotifySubscribers.ContainsKey(propertyName))
                    NotifySubscribers.Add(propertyName, notifyNewValue);

                notifyNewValue.PropertyChanged += OnChildPropertyChanged;
            }
        }

        private void OnChildPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            var subscription = NotifySubscribers.FirstOrDefault(pair => pair.Value.Equals(sender));
            var args = new NotifyRouter.ChildPropertyChangedEventArgs(propertyChangedEventArgs.PropertyName)
            {
                ParentPropertyName = subscription.Key,
                Value = sender.GetType().GetProperty(propertyChangedEventArgs.PropertyName).GetValue(sender)
            };
            ChildPropertyChanged?.Invoke(sender, args);
        }

        protected static event EventHandler<NotifyRouter.ChildPropertyChangedEventArgs> ChildPropertyChanged;
        protected Dictionary<string, INotifyPropertyChanged> NotifySubscribers = new Dictionary<string, INotifyPropertyChanged>();

        public virtual void LoadDefaults()
        {
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(this))
            {
                var myAttribute = (DefaultValueAttribute)property.Attributes[typeof(DefaultValueAttribute)];
                if (myAttribute != null)
                {
                    property.SetValue(this, myAttribute.Value);
                }
            }
        }

        public static string SplitOnCapitals(string text)
        {
            if (text == null) return string.Empty;
            var regex = new Regex(@"\p{Lu}\p{Ll}*");
            return regex.Matches(text).Cast<Match>().Aggregate(string.Empty, (current, match) => current + match.Value);
        }
    }


}

