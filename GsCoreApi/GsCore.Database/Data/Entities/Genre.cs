﻿using System;
using System.Collections.Generic;

namespace GsCore.Database.Data.Entities
{
    public partial class Genre
    {
        public Genre()
        {
            Album = new HashSet<Album>();
        }

        public int Id { get; set; }
        public string GenreName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Album> Album { get; set; }
    }
}
