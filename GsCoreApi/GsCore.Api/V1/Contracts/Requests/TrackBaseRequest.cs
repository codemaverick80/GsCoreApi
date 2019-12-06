using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GsCore.Api.V1.Contracts.Requests
{
    public abstract class TrackBaseRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public int? AlbumId { get; set; }
        public string Composer { get; set; }
        public string Performer { get; set; }
        public string Featuring { get; set; }
        [MaxLength(20)]
        public string Duration { get; set; }
    }
}
