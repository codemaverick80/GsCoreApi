﻿using System;
using System.Collections.Generic;

namespace GsCore.Database.Data.Entities
{
    public partial class ArtistBasicInfo
    {
        public int ArtistId { get; set; }
        public string Born { get; set; }
        public string Died { get; set; }
        public string AlsoKnownAs { get; set; }

        public virtual Artist Artist { get; set; }
    }
}
