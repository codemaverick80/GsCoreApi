using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GsCore.Api.V1.Contracts.Requests
{
    public class ArtistBasicInfoCreateRequest
    {

        [MaxLength(100)]
        public string Born { get; set; }
        [MaxLength(100)]
        public string Died { get; set; }
        [MaxLength(500)]
        public string AlsoKnownAs { get; set; }
    }
}
