using System;
using GsCore.Database.Entities;

namespace GsCore.Api.V1.Contracts.Responses
{
    public class ArtistGetResponse
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string YearActive { get; set; }
        public string Biography { get; set; }
        public string ThumbnailTag { get; set; }
        public string SmallThumbnail { get; set; }
        public string LargeThumbnail { get; set; }

        private DateTime? DateCreated { get; set; }
        private Guid? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        private Guid? ModifiedBy { get; set; }

        public ArtistBasicInfoGetResponse BasicInfo { get; set; }
    }
}
