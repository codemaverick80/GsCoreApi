﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GsCore.Api.V1.Contracts.Responses
{
    public class ArtistBasicInfoGetResponse
    {
        public string Born { get; set; }
        public string Died { get; set; }
        public string AlsoKnownAs { get; set; }
    }
}
