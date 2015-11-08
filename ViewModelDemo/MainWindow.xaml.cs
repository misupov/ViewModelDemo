using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace ViewModelDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MyCustomControl.UpdateViewModel(model => model.SetPrice(100).SetQuantity(12));

            // Let's say, we have two data source threads. First one updates Price field, second one updates Quantity.
            // In this sample they do it more than million times per second.
            new Thread(PriceUpdateThread) {IsBackground = true}.Start();
            new Thread(QuantityUpdateThread) {IsBackground = true}.Start();
        }

        private void PriceUpdateThread()
        {
            var r = new Random();

            var i = 0L;

            var startNew = Stopwatch.StartNew();
            while (true)
            {
                // Some complex actions that prepare the data to be displayed.
                // For instance, in this thread we can receive, parse and process messages from another system and prepare values to be displayed.
                var delta = (decimal)((r.NextDouble() - 0.5) / 1000);

                MyCustomControl.UpdateViewModel(model => model.SetPrice(model.Price + delta));
                i++;
                if (i%1000000 == 0)
                {
                    Debug.WriteLine((i / startNew.Elapsed.TotalSeconds).ToString("N0"));
                }
            }
        }

        private void QuantityUpdateThread()
        {
            var r = new Random();

            var quantity = 10m;

            while (true)
            {
                // Some complex actions that prepare the data to be displayed.
                // For instance, in this thread we can receive, parse and process messages from another system and prepare values to be displayed.
                quantity = quantity + (decimal)((r.NextDouble() - 0.5) / 100);

                MyCustomControl.UpdateViewModel(model => model.SetQuantity(quantity));
            }
        }
    }
}
