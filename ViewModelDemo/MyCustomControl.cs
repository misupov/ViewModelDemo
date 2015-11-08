using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ViewModelDemo
{
    /// Note! WPF doesn't dispose controls, so they should be disposed manually.
    public class MyCustomControl : Control, IDisposable
    {
        private readonly object _viewModelUpdateLock = new object();
        private MyCustomViewModel _currentViewModel;

        static MyCustomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyCustomControl), new FrameworkPropertyMetadata(typeof(MyCustomControl)));
        }

        public MyCustomControl()
        {
            DataContext = _currentViewModel = new MyCustomViewModel();

            CompositionTarget.Rendering += CompositionTargetOnRendering;
        }

        public void Dispose()
        {
            CompositionTarget.Rendering -= CompositionTargetOnRendering;
        }

        /// <summary>
        /// This method should be the only way of changing the visual state of the control.
        /// </summary>
        /// <param name="updateFunc">This delegate receives current view state and returns updated state.</param>
        public void UpdateViewModel(Func<MyCustomViewModel, MyCustomViewModel> updateFunc)
        {
            lock (_viewModelUpdateLock)
            {
                var currentViewModel = _currentViewModel;
                var newViewModel = updateFunc(currentViewModel);
                if (newViewModel != currentViewModel)
                {
                    _currentViewModel = newViewModel;
                }
            }
        }

        // WPF invokes this callback each time when it wants to update the screen. 
        // By default, WPF invokes this callback 60 times/sec.
        private void CompositionTargetOnRendering(object sender, EventArgs eventArgs)
        {
            lock (_viewModelUpdateLock)
            {
                DataContext = _currentViewModel;
            }
        }
    }
}
