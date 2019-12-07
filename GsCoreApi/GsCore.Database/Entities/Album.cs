using System;
using System.Collections.Generic;

namespace GsCore.Database.Entities
{
    public partial class Album
    {
        public Album()
        {
            Track = new HashSet<Track>();
        }

        public Guid Id { get; set; }
        public string AlbumName { get; set; }
        public Guid ArtistId { get; set; }
        public Guid GenreId { get; set; }
        public int? Rating { get; set; }
        public int? Year { get; set; }
        public string Label { get; set; }
        public string ThumbnailTag { get; set; }
        public string SmallThumbnail { get; set; }
        public string MediumThumbnail { get; set; }
        public string LargeThumbnail { get; set; }
        public string AlbumUrl { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Inventory Inventory { get; set; }
        public virtual ICollection<Track> Track { get; set; }
    }
}
