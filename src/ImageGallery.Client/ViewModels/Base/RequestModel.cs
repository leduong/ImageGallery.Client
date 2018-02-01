using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Client.ViewModels.Base
{
    /// <summary>
    /// Request Model - Base
    /// </summary>
    public abstract class RequestModel
    {
        /// <summary>
        /// Sort Column
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        ///  Sort Order Direction
        /// </summary>
        public string Order { get; set; }
    }
}
