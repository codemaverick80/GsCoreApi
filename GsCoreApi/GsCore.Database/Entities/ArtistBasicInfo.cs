﻿using System;

namespace GsCore.Database.Entities
{
    public partial class ArtistBasicInfo
    {
        public Guid ArtistId { get; set; }
        public string Born { get; set; }
        public string Died { get; set; }
        public string AlsoKnownAs { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Artist Artist { get; set; }
    }
}
