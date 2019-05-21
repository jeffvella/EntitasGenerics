using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Performance.Common.Behaviors
{
    public static class AutoScrollBehavior
    {
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.RegisterAttached("AutoScroll", typeof(bool), 
                typeof(AutoScrollBehavior), 
                new PropertyMetadata(false, 
                    AutoScrollPropertyChanged));

        private static double _height;

        private static ScrollViewer _scrollViewer;

        public static void AutoScrollPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {         
            var scrollViewer = obj as ScrollViewer;
            if (scrollViewer == null)
                return;

            if ((bool)args.NewValue)
            {
                _scrollViewer = scrollViewer;
                scrollViewer.LayoutUpdated += ScrollViewerOnLayoutUpdated;
                scrollViewer.ScrollToEnd();
            }
            else
            {
                scrollViewer.LayoutUpdated -= ScrollViewerOnLayoutUpdated;
            }
        }

        private static void ScrollViewerOnLayoutUpdated(object sender, EventArgs eventArgs)
        {
            if (_scrollViewer == null)
                return;

            if (Math.Abs(_scrollViewer.ExtentHeight - _height) > 1)
            {
                _scrollViewer.ScrollToVerticalOffset(_scrollViewer.ExtentHeight);
                _height = _scrollViewer.ExtentHeight;
            }
        }

        public static bool GetAutoScroll(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollProperty);
        }

        public static void SetAutoScroll(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollProperty, value);
        }
    }

}


