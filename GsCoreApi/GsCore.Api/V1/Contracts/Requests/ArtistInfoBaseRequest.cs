using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GsCore.Api.V1.Contracts.Requests
{
    public abstract class ArtistInfoBaseRequest
    {
        [MaxLength(100)]
        public virtual string Born { get; set; }
        [MaxLength(100)]
        public virtual string Died { get; set; }
        [MaxLength(500)]
        public virtual string AlsoKnownAs { get; set; }
    }
}
