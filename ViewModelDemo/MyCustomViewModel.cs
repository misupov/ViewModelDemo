namespace ViewModelDemo
{
    /// <summary>
    /// ViewModel should be as similar as possible to the View, because Dispatcher thread should perform only display logic. 
    /// Any calculations should be performed on another threads.
    /// ViewModel must be immutable. Any changes are to be done by invocation of SetXXX(...) methods which create new instances of view models.
    /// </summary>
    public class MyCustomViewModel
    {
        public decimal Price { get; private set; }

        public decimal Quantity { get; private set; }

        public MyCustomViewModel SetPrice(decimal price)
        {
            // This "guard" if-statement returns current view model if the value isn't changed.
            if (Price == price)
            {
                return this;
            }

            var viewModel = Clone();
            viewModel.Price = price;

            return viewModel;
        }

        public MyCustomViewModel SetQuantity(decimal quantity)
        {
            if (Quantity == quantity)
            {
                return this;
            }

            var viewModel = Clone();
            viewModel.Quantity = quantity;

            return viewModel;
        }

        private MyCustomViewModel Clone()
        {
            return (MyCustomViewModel)MemberwiseClone();
        }
    }
}