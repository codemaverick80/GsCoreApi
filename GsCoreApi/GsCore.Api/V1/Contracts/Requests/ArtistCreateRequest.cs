using System;

namespace GsCore.Api.V1.Contracts.Requests
{
    public class ArtistCreateRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string YearActive { get; set; }
        public string Biography { get; set; }
        public string ThumbnailTag { get; set; }
        public string SmallThumbnail { get; set; }
        public string LargeThumbnail { get; set; }
        public ArtistBasicInfoCreateRequest BasicInfo { get; set; }
        
    }
}
