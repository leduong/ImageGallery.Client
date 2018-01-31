namespace ImageGallery.Client.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    public class OrderFrameViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderFrameViewModel"/> class.
        /// </summary>
        /// <param name="address"></param>
        public OrderFrameViewModel(string address)
        {
            Address = address;
        }

        /// <summary>
        ///
        /// </summary>
        public string Address { get; private set; }
    }
}