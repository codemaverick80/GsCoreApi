using System;
using System.ComponentModel.DataAnnotations;

namespace GsCore.Api.V1.Contracts.Requests
{
    public abstract class TrackBaseRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public Guid AlbumId { get; set; }
        public virtual string Composer { get; set; }
        public virtual string Performer { get; set; }
        public string Featuring { get; set; }
        [MaxLength(20)]
        public virtual string Duration { get; set; }

        public virtual DateTime DateModified { get; set; }
        public bool IsDeleted { get; set; }
    }
}
