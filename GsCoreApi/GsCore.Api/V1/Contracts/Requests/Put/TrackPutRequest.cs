using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GsCore.Api.V1.Contracts.Requests.Put
{
    public class TrackPutRequest:TrackBaseRequest
    {
        [Required(ErrorMessage = "Composer is required.")]
        public override string Composer { get=>base.Composer; set=>base.Composer=value; }
        
        [Required(ErrorMessage = "Duration is required.")]
        public override string Duration { get=>base.Duration; set=>base.Duration=value; }
        
        [Required(ErrorMessage = "Performer is required.")]
        public override string Performer { get=>base.Performer; set=>base.Performer=value; }
    }
}
