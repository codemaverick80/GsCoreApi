using System;
using System.Collections.Generic;

namespace GsCore.Database.Entities
{
    public partial class Artist
    {
        public Artist()
        {
            Album = new HashSet<Album>();
        }

        public Guid Id { get; set; }
        public string ArtistName { get; set; }
        public string YearActive { get; set; }
        public string Biography { get; set; }
        public string ThumbnailTag { get; set; }
        public string SmallThumbnail { get; set; }
        public string LargeThumbnail { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ArtistBasicInfo ArtistBasicInfo { get; set; }
        public virtual ICollection<Album> Album { get; set; }
    }
}
