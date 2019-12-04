using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GsCore.Api.V1.Contracts.Requests.Put
{
    public class ArtistInfoPutRequest:ArtistInfoBaseRequest
    {
        [Required(ErrorMessage = "Artist born info must be provided while updating.")]
        public override string Born { get=>base.Born; set=>base.Born=value; }

        [Required(ErrorMessage = "Artist deceased info must be provided while updating.")]
        public override string Died { get=>base.Died; set=>base.Born=value; }

        [Required(ErrorMessage = "Artist AKA info must be provided while updating.")]
        public override string AlsoKnownAs { get=>base.AlsoKnownAs; set=>base.AlsoKnownAs=value; }
    }
}
