using System;

namespace ImageGallery.Model
{
    public class Album
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

    }
}
